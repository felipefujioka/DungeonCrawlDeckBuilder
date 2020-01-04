using System.Collections;
using Game.General.Character;

namespace Game.General.Condition
{
    public interface ICondition
    {
        IEnumerator OnUpkeepBeginning(ICharacter character);
        IEnumerator OnUpkeepEnd(ICharacter character);
        int GetMagnitude();
        void SetMagnitude(int value);
        void IncreaseMagnitude(int increment);
        int GetConditionId();
    }
}