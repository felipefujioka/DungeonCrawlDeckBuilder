using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Game.General
{
    [CreateAssetMenu(menuName = "DungeonDB/Card Config")]
    public class CardConfig : ScriptableObject
    {
        public enum RarityValue
        {
            BASIC = 0,
            COMMON = 1,
            UNCOMMON = 2,
            RARE = 3
        }
        
        [ReorderableList]
        public List<ActionConfig> Actions;

        public RarityValue Rarity;

        public string Title;
        public string Description;
        public int ManaCost;
    }
}