using System.Collections;

namespace Game.General
{
    public interface IEnemyController
    {
        ActionConfig GetCurrentAction();
        void ChangeCurrentAction();
        bool IsAlive();
        IEnumerator AnimateAction();

        IEnumerator TakeDamage(int damage);

        IEnumerator ActivateStatusesOnUpkeep();

        IEnumerator ActivateStatusesOnEndPhase();
    }
}