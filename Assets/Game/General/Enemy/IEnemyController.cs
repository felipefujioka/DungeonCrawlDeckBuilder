using System.Collections;

namespace Game.General
{
    public interface IEnemyController
    {
        ActionConfig GetCurrentAction();
        void ChangeCurrentAction();

        IEnumerator AnimateAction();
    }
}