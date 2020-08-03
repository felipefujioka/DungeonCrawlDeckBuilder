using System.Collections;

namespace Game.General
{
    public abstract class EnemyStatus : IEnemyStatus
    {

        public EnemyStatusType StatusType;
        protected int magnitude = 0;

        public EnemyStatus(EnemyStatusType type, int magnitude)
        {
            StatusType = type;
            this.magnitude = magnitude;
        }

        public abstract IEnumerator OnBeginTurn(IEnemyController enemyController);

        public abstract IEnumerator OnEndTurn(IEnemyController enemyController);
        
        public IEnumerator IncreaseStatus(int increment)
        {
            magnitude += increment;
            yield return null;
        }

        public IEnumerator DecreaseStatus(int decrement)
        {
            magnitude -= decrement;
            yield return null;
        }
    }
}