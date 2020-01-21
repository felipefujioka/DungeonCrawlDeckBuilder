using System.Collections;
using Game.General.Character;

namespace Game.General
{
    public class HeroUpkeepPhase : IHeroTurnPhase
    {
        public IEnumerator BeginPhase(HeroCharacter character, DeckController deckController, CardHolderView cardHolderView)
        {
            foreach (var condition in character.GetConditions())
            {
                yield return condition.Value.OnUpkeepBeginning(character);
            }
            
            HeroStatus.Instance.RestoreMana();
        }

        public IEnumerator WaitEndTurnCondition(ICharacter character)
        {
            yield return null;
        }

        public IEnumerator EndPhase(HeroCharacter character, DeckController deckController, CardHolderView cardHolderView)
        {
            foreach (var condition in character.GetConditions())
            {
                yield return condition.Value.OnUpkeepEnd(character);
            }
        }

        public void OnDestroy()
        {
            throw new System.NotImplementedException();
        }
    }
}