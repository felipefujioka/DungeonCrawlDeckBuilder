using System.Collections;

namespace Game.General
{
    public class WeakenedStatus : EnemyStatus
    {
        public WeakenedStatus(int magnitude) : base(EnemyStatusType.WEAKENED, magnitude)
        {
        }

        public override float DamageDealtModifier()
        {
            return 0.75f;
        }

        public override string GetTooltip()
        {
            return "Weakened enemies deal 25% less damage. Lasts for " + magnitude + " turns";
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