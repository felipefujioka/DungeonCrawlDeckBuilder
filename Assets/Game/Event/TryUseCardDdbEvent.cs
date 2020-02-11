using Game.General;

namespace Game.Event
{
    public enum Position
    {
        LEFT = 0,
        MIDDLE = 1,
        RIGHT = 2
    }

    public class TryUseCardDdbEvent : General.DDBEvent
    {
        public CardView CardView;
        public Position Position;
    }
}