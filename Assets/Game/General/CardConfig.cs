using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Game.General
{
    [CreateAssetMenu(menuName = "DungeonDB/Card Config")]
    public class CardConfig : ScriptableObject
    {
        [ReorderableList]
        public List<ActionConfig> Actions;

        public string Title;
        public string Description;
        public int ManaCost;
    }
}