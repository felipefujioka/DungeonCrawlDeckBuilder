using System.Collections;
using System.Collections.Generic;
using Game.General.Condition;

namespace Game.General.Character
{
    public interface ICharacter
    {
        void UpkeepReset();
        IEnumerator SufferDamage(int damage);
        
        IEnumerator SufferCondition(ICondition condition);

        bool ShouldDraw();

        int AmountToDraw();

        Dictionary<int, ICondition> GetConditions();
        IEnumerator GainBlock(int actionMagnitude);

        IEnumerator ResetBlock();
    }
}