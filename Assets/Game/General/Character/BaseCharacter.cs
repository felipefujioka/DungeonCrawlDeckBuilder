using System.Collections;
using System.Collections.Generic;
using Game.General.Condition;

namespace Game.General.Character
{
    public abstract class BaseCharacter : ICharacter
    {
        private readonly Dictionary<int, ICondition> conditions = new Dictionary<int, ICondition>();

        public abstract void UpkeepReset();

        public abstract IEnumerator SufferDamage(int damage);

        public IEnumerator SufferCondition(ICondition condition)
        {
            if (conditions.ContainsKey(condition.GetConditionId()))
            {
                conditions[condition.GetConditionId()].IncreaseMagnitude(condition.GetMagnitude());
            }
            else
            {
                conditions.Add(condition.GetConditionId(), condition);
            }

            // TODO: add condition animation
            yield return null;
        }

        public abstract bool ShouldDraw();

        public abstract int AmountToDraw();

        public Dictionary<int, ICondition> GetConditions()
        {
            return conditions;
        }
    }
}