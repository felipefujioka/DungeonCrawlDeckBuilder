using Game.General;

namespace Game.Event
{
    public class OnLifeDataChanged : DDBEvent
    {
        public int ChangedValue;
        public int CurrentLife;
        public int MaxLife;
        public bool Heal;
    }
}