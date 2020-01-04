using System.Collections;
using Game.Event;
using Game.General.Character;
using UnityEngine;

namespace Game.General
{
    public class HeroPlayPhase : IHeroTurnPhase
    {

        private EncounterController encounterController;
        
        private bool endPhase;
        
        private bool resolvingCard = false;

        public HeroPlayPhase(EncounterController encounterController)
        {
            this.encounterController = encounterController;
            
            EventSystem.Instance.AddListener<EndTurnEvent>(OnEndTurn);
            EventSystem.Instance.AddListener<TryUseCardEvent>(OnTryUseCard);
        }
        
        private void OnTryUseCard(TryUseCardEvent tryUseCardEvent)
        {
            if (!resolvingCard && tryUseCardEvent.CardView.Config.ManaCost <= HeroStatus.Instance.CurrentMana)
            {
                EventSystem.Instance.Raise(new UsedCardEvent()
                {
                    CardView = tryUseCardEvent.CardView,
                    Position = tryUseCardEvent.Position
                });
            
                CoroutineManager.Instance.StartCoroutine(UseCard(tryUseCardEvent.CardView.Config, tryUseCardEvent.Position));
            
                HeroStatus.Instance.SpendMana(tryUseCardEvent.CardView.Config.ManaCost);
            }
        }
        
        private IEnumerator UseCard(CardConfig card, Position position)
        {
            resolvingCard = true;
        
            foreach (var action in card.Actions)
            {
                yield return UseAction(action, position);
            }

            resolvingCard = false;
        }
        
        private IEnumerator UseAction(ActionConfig action, Position position)
        {
            if (action.Type == ActionType.ATTACK)
            {
                yield return encounterController.DealDamage(action.Magnitude, (int) position);
            }
        }

        private void OnEndTurn(EndTurnEvent e)
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
            EventSystem.Instance.RemoveListener<EndTurnEvent>(OnEndTurn);
            EventSystem.Instance.RemoveListener<TryUseCardEvent>(OnTryUseCard);
        }
    }
}