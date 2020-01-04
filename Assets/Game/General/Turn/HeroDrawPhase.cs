using System.Collections;
using Game.General.Character;

namespace Game.General
{
    public class HeroDrawPhase : IHeroTurnPhase
    {
        public IEnumerator BeginPhase(HeroCharacter character, DeckController deckController, CardHolderView cardHolderView)
        {
            if (character.ShouldDraw())
            {
                for (int i = 0; i < character.AmountToDraw(); i++)
                {
                    yield return cardHolderView.AddCardInHand(deckController.DrawCard());       
                }
            }
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