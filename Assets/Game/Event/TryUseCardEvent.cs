using Game.General;

namespace Game.Event
{
    public enum Position
    {
        LEFT = 0,
        MIDDLE = 1,
        RIGHT = 2
    }

    public class TryUseCardEvent : General.Event
    {
        public CardView CardView;
        public Position Position;
    }
}