using UnityEngine;

namespace Game.General
{
    public enum ActionType
    {
        ATTACK = 0,
        DEFENSE = 1,
        POISON = 2,
        REFLECT = 3,
        HEAL = 4,
        BUFF = 5,
        MANA = 6,
        REPEAT = 7
    }

    public enum BuffType
    {
        STRENGTH = 0,
        WEAKNESS = 1        
    }

    public enum TargetType
    {
        SELF = 0,
        ENEMY = 1,
        ALL_ENEMIES = 2
    }

    [CreateAssetMenu(menuName = "DungeonDB/Action Config")]
    public class ActionConfig : ScriptableObject
    {
        public ActionType Type;
        public BuffType BuffType;
        public TargetType Target;
        public int Magnitude;
    }
}