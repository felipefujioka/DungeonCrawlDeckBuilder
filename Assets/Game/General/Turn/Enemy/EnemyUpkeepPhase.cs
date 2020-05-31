using System.Collections;
using Game.General.Character;

namespace Game.General.Turn.Enemy
{
    public class EnemyUpkeepPhase : IEnemyTurnPhase
    {
        public IEnumerator BeginPhase(IEnemyController character, HeroCharacter hero, DeckController deckController,
            CardHolderView cardHolderView)
        {
            yield return null;
        }

        public IEnumerator EndPhase(IEnemyController character, HeroCharacter hero, DeckController deckController,
            CardHolderView cardHolderView)
        {
            yield return null;
        }
    }
}