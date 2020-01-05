using System.Collections.Generic;
using Game.General.Condition;

namespace Game.General.Character
{
    public abstract class BaseCharacter : ICharacter
    {
        private readonly Dictionary<int, ICondition> conditions = new Dictionary<int, ICondition>();

        public abstract void UpkeepReset();

        public abstract void SufferDamage(int damage);

        public void SufferCondition(ICondition condition)
        {
            if (conditions.ContainsKey(condition.GetConditionId()))
            {
                conditions[condition.GetConditionId()].IncreaseMagnitude(condition.GetMagnitude());
            }
            else
            {
                conditions.Add(condition.GetConditionId(), condition);
            }
        }

        public abstract bool ShouldDraw();

        public abstract int AmountToDraw();

        public Dictionary<int, ICondition> GetConditions()
        {
            return conditions;
        }
    }
}