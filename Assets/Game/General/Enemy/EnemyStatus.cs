using System.Collections;

namespace Game.General
{
    public abstract class EnemyStatus : IEnemyStatus
    {

        public EnemyStatusType StatusType;
        protected int magnitude = 0;

        public int GetMagnitude()
        {
            return magnitude;
        }
        
        public EnemyStatus(EnemyStatusType type, int magnitude)
        {
            StatusType = type;
            this.magnitude = magnitude;
        }

        public EnemyStatusType GetStatusType()
        {
            return StatusType;
        }

        public abstract IEnumerator OnBeginTurn(IEnemyController enemyController);

        public abstract IEnumerator OnEndTurn(IEnemyController enemyController);

        public abstract string GetTooltip();

        public virtual float DamageTakenModifier()
        {
            return 1;
        }

        public virtual float DamageDealtModifier()
        {
            return 1;
        }

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