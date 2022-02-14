using System;
using System.Collections;
using Game.Event;
using Game.General.Character;
using UnityEngine;
using UnityEngine.UIElements;
using Position = Game.Event.Position;

namespace Game.General
{
    public class HeroPlayPhase : IHeroTurnPhase
    {
        EncounterController encounterController;
        DeckController deckController;
        CardHolderView cardHolderView;

        ICharacter hero;

        bool endPhase;

        bool resolvingCard = false;
        bool repeatNext = false;

        public HeroPlayPhase(EncounterController encounterController, ICharacter hero, DeckController deckController, CardHolderView cardHolderView)
        {
            this.hero = hero;
            this.encounterController = encounterController;
            this.deckController = deckController;
            this.cardHolderView = cardHolderView;
            
            EventSystem.Instance.AddListener<EndTurnDdbEvent>(OnEndTurn);
            EventSystem.Instance.AddListener<TryUseCardDdbEvent>(OnTryUseCard);
        }

        void OnTryUseCard(TryUseCardDdbEvent tryUseCardDdbEvent)
        {
            if (!resolvingCard && tryUseCardDdbEvent.CardView.Config.ManaCost <= HeroStatus.Instance.CurrentMana)
            {
                EventSystem.Instance.Raise(new UsedCardDdbEvent()
                {
                    CardView = tryUseCardDdbEvent.CardView,
                    Position = tryUseCardDdbEvent.Position
                });
            
                CoroutineManager.Instance.StartCoroutine(UseCard(tryUseCardDdbEvent.CardView.Config, tryUseCardDdbEvent.Position));
            
                HeroStatus.Instance.SpendMana(tryUseCardDdbEvent.CardView.Config.ManaCost);
            }
        }

        IEnumerator UseCard(CardConfig card, Position position)
        {
            resolvingCard = true;
            repeatNext = false;
        
            foreach (var action in card.Actions)
            {
                if (repeatNext)
                {
                    foreach (var pos in Enum.GetValues(typeof(Position)))
                    {
                        yield return UseAction(action, (Position) pos);
                    }
                }
                else
                {
                    yield return UseAction(action, position);    
                }
                
            }

            resolvingCard = false;
        }

        IEnumerator UseAction(ActionConfig action, Position position)
        {
            switch (action.Type)
            {
                case ActionType.ATTACK:
                    yield return encounterController.HeroDealDamage(action.Magnitude, (int) position);
                    break;
                case ActionType.POISON:
                    yield return encounterController.CauseStatusOnEnemy(action.Magnitude, EnemyStatusType.POISON, (int) position);
                    break;
                case ActionType.VULNERABLE:
                    yield return encounterController.CauseStatusOnEnemy(action.Magnitude, EnemyStatusType.VULNERABLE, (int) position);
                    break;
                case ActionType.WEAKENED:
                    yield return encounterController.CauseStatusOnEnemy(action.Magnitude, EnemyStatusType.WEAKENED, (int) position);
                    break;
                case ActionType.DEFENSE:
                    yield return hero.GainBlock(action.Magnitude);
                    break;
                case ActionType.REPEAT:
                    repeatNext = true;
                    break;
                case ActionType.HEAL:
                    HeroStatus.Instance.Heal(action.Magnitude);
                    break;
                case ActionType.MANA:
                    HeroStatus.Instance.GainMana(action.Magnitude);
                    break;
                case ActionType.DRAW:
                    for (int i = 0; i < action.Magnitude; i++)
                    {
                        yield return cardHolderView.AddCardInHand(deckController.DrawCard());    
                    }
                    break;
                default:
                    yield return null;
                    break;
            }
        }

        void OnEndTurn(EndTurnDdbEvent e)
        {
            endPhase = true;
        }   

        public IEnumerator BeginPhase(HeroCharacter character, DeckController deckController, CardHolderView cardHolderView)
        {
            endPhase = false;
            
            yield return null;
        }

        public IEnumerator WaitEndTurnCondition(ICharacter character)
        {
            yield return new WaitUntil(() => endPhase);
        }

        public IEnumerator EndPhase(HeroCharacter character, DeckController deckController, CardHolderView cardHolderView)
        {
            yield return null;
        }

        public void OnDestroy()
        {
            EventSystem.Instance.RemoveListener<EndTurnDdbEvent>(OnEndTurn);
            EventSystem.Instance.RemoveListener<TryUseCardDdbEvent>(OnTryUseCard);
        }
    }
}