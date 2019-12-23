using System.Collections.Generic;
using UnityEngine;

namespace Game.General
{
    [CreateAssetMenu(menuName = "DungeonDB/Deck")]
    public class DeckConfig : ScriptableObject
    {
        public List<CardConfig> Cards;
    }
}