using System.Collections;
using Game.General.Character;

namespace Game.General.Turn.Enemy
{
    public interface IEnemyTurnPhase
    {
        IEnumerator BeginPhase(IEnemyController character, HeroCharacter hero, DeckController deckController, CardHolderView cardHolderView);
        IEnumerator EndPhase(IEnemyController character, HeroCharacter hero, DeckController deckController, CardHolderView cardHolderView);
    }
}