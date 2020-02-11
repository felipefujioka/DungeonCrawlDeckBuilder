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
            
            EventSystem.Instance.AddListener<EndTurnDdbEvent>(OnEndTurn);
            EventSystem.Instance.AddListener<TryUseCardDdbEvent>(OnTryUseCard);
        }
        
        private void OnTryUseCard(TryUseCardDdbEvent tryUseCardDdbEvent)
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

        private void OnEndTurn(EndTurnDdbEvent e)
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