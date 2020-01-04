using System.Collections.Generic;
using Game.General.Condition;

namespace Game.General.Character
{
    public class HeroCharacter : BaseCharacter
    {
        public override void SufferDamage(int damage)
        {
            HeroStatus.Instance.TakeDamage(damage);
        }

        public override bool ShouldDraw()
        {
            return true;
        }

        public override int AmountToDraw()
        {
            return HeroStatus.Instance.DrawsPerTurn;
        }
    }
}