using Game.General;

namespace Game.Event
{
    public class UsedCardDdbEvent : General.DDBEvent
    {
        public CardView CardView;
        public Position Position;
    }
}