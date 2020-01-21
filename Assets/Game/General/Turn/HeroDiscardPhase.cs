using System;
using System.Collections;
using Game.General.Character;

namespace Game.General
{
    public class HeroDiscardPhase : IHeroTurnPhase
    {
        public IEnumerator BeginPhase(HeroCharacter character, DeckController deckController, CardHolderView cardHolderView)
        {
            var cardsCount = deckController.GetHand().Count;
            for (int i = cardsCount - 1; i >= 0; i--)
            {
                var cardInHand = deckController.GetHand()[i];
                deckController.DiscardCard(cardInHand);
                deckController.RemoveCardFromHand(cardInHand);
                CoroutineManager.Instance.StartCoroutine(cardHolderView.DiscardCard(cardInHand));
            }

            yield return null;
        }

        public IEnumerator WaitEndTurnCondition(ICharacter character)
        {
            yield return null;
        }

        public IEnumerator EndPhase(HeroCharacter character, DeckController deckController, CardHolderView cardHolderView)
        {
            yield return null;
        }

        public void OnDestroy()
        {
            
        }
    }
}