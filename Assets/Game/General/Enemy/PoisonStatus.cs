using System.Collections;

namespace Game.General
{
    public class PoisonStatus : EnemyStatus
    {

        public override IEnumerator OnBeginTurn(IEnemyController enemyController)
        {
            yield return enemyController.TakeDamage(magnitude);
        }

        public override IEnumerator OnEndTurn(IEnemyController enemyController)
        {
            yield return DecreaseStatus(1);
        }

        public PoisonStatus(int magnitude) : base(EnemyStatusType.POISON, magnitude)
        {
        }
    }
}