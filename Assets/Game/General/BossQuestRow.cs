using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Game.General
{
    public class BossQuestRow : QuestRow
    {
        public override void Start()
        {
            Row = new List<QuestNode>();
        
            for (int i = 0; i < 3 ; i++)
            {
                var node = Instantiate(QuestNodePrefab, transform);
                node.IsBoss = true;
                
                Row.Add(node);
            }
        }
    }
}