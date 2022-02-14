using System.Collections;

namespace Game.General
{
    public interface IEnemyStatus
    {
        EnemyStatusType GetStatusType();
        int GetMagnitude();

        string GetTooltip();
        
        IEnumerator OnBeginTurn(IEnemyController enemyController);
        IEnumerator OnEndTurn(IEnemyController enemyController);

        float DamageTakenModifier();
        float DamageDealtModifier();

        IEnumerator IncreaseStatus(int increment);

        IEnumerator DecreaseStatus(int decrement);

    }
}