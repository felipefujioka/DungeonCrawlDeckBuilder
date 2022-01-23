using System.Collections;
using Game.General.Character;

namespace Game.General.Turn.Enemy
{
    public class EnemyActionPhase : IEnemyTurnPhase
    {
        public IEnumerator BeginPhase(IEnemyController character, HeroCharacter hero, DeckController deckController,
            CardHolderView cardHolderView)
        {
            var action = character.GetCurrentAction();

            yield return character.AnimateAction();
            
            if (action.Type == ActionType.ATTACK)
            {
               yield return hero.SufferDamage(action.Magnitude);
            }
        }

        public IEnumerator EndPhase(IEnemyController character, HeroCharacter hero, DeckController deckController,
            CardHolderView cardHolderView)
        {
            if (character.IsAlive())
            {
                character.ChangeCurrentAction();   
            }

            yield return null;
        }
    }
}