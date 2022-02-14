using System.Collections;

namespace Game.General
{
    public class VulnerableStatus : EnemyStatus
    {
        public VulnerableStatus(int magnitude) : base(EnemyStatusType.VULNERABLE, magnitude)
        {
        }

        public override string GetTooltip()
        {
            return "Vulnerable enemies take 50% more damage. Lasts for " + magnitude + " turns.";
        }

        public override float DamageTakenModifier()
        {
            return 1.50f;
        }

        public override IEnumerator OnBeginTurn(IEnemyController enemyController)
        {
            yield return null;
        }

        public override IEnumerator OnEndTurn(IEnemyController enemyController)
        {
            yield return DecreaseStatus(1);
        }
    }
}