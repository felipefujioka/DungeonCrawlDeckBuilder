using System.Collections.Generic;
using Game.General.Condition;

namespace Game.General.Character
{
    public interface ICharacter
    {
        void UpkeepReset();
        void SufferDamage(int damage);
        
        void SufferCondition(ICondition condition);

        bool ShouldDraw();

        int AmountToDraw();

        Dictionary<int, ICondition> GetConditions();
    }
}