using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.General
{
    [CreateAssetMenu(menuName = "DungeonDB/Enemy")]
    public class EnemyConfig : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;

        public int MaxHP;
        public List<ActionConfig> Actions;
        public List<ActionConfig> FixedActions;
    }
}