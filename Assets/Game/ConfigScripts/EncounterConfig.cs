using System.Collections.Generic;
using UnityEngine;

namespace Game.General
{
    [CreateAssetMenu(menuName = "DungeonDB/Encounter")]
    public class EncounterConfig : ScriptableObject
    {
        public List<EnemyConfig> Enemies;
    }
}