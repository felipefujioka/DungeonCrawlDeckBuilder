using System.Collections;

namespace Game.General
{
    public interface IEnemyStatus
    {
        IEnumerator OnBeginTurn(IEnemyController enemyController);
        IEnumerator OnEndTurn(IEnemyController enemyController);

        IEnumerator IncreaseStatus(int increment);

        IEnumerator DecreaseStatus(int decrement);

    }
}