using System.Collections;
using Game.General.Character;

namespace Game.General
{
    public interface IHeroTurnPhase
    {
        IEnumerator BeginPhase(HeroCharacter character, DeckController deckController, CardHolderView cardHolderView);
        IEnumerator WaitEndTurnCondition(ICharacter character);
        IEnumerator EndPhase(HeroCharacter character, DeckController deckController, CardHolderView cardHolderView);

        void OnDestroy();
    }
}