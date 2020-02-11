using Game.General;

namespace DefaultNamespace
{
    public class OnManaDataChangeDdbEvent : DDBEvent
    {
        public int newCurrentManaValue;
        public int newMaxManaValue;
    }
}