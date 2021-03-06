using System.Collections;
using Game.General.Character;

namespace Game.General.Turn.Enemy
{
    public class EnemyFinishTurnPhase : IEnemyTurnPhase
    {
        public IEnumerator BeginPhase(IEnemyController character, HeroCharacter hero, DeckController deckController,
            CardHolderView cardHolderView)
        {
            yield return character.ActivateStatusesOnEndPhase();
        }

        public IEnumerator EndPhase(IEnemyController character, HeroCharacter hero, DeckController deckController,
            CardHolderView cardHolderView)
        {
            yield return null;
        }
    }
}