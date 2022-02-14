using System.Collections;
using Game.General.Character;
using UnityEngine;

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
                float modifier = 1;
                foreach (var status in character.GetStatuses())
                {
                    modifier *= status.DamageDealtModifier();
                }
                yield return hero.SufferDamage(Mathf.FloorToInt(action.Magnitude * modifier));
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