using System.Collections.Generic;
using UnityEngine;

namespace Game.General
{
    [CreateAssetMenu(menuName = "DungeonDB/Encounter")]
    public class EncounterConfig : ScriptableObject
    {
        public enum Difficulty
        {
            EASY = 0,
            MEDIUM = 1,
            HARD = 3,
            BOSS = 4
        }

        public Difficulty EncounterDifficulty;
        public List<EnemyConfig> Enemies;
    }
}