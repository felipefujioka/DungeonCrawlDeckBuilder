using System.Collections;
using System.Collections.Generic;
using Game.General.Condition;

namespace Game.General.Character
{
    public class HeroCharacter : BaseCharacter
    {
        public override void UpkeepReset()
        {
            HeroStatus.Instance.RecoverMana(HeroStatus.Instance.MaxMana);
        }

        public override IEnumerator SufferDamage(int damage)
        {
            HeroStatus.Instance.TakeDamage(damage);

            //TODO: add damage animation
            yield return null;
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