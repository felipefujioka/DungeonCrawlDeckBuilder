using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuestRow : MonoBehaviour
{
    public QuestNode QuestNodePrefab;

    public List<QuestNode> Row;

    public List<int> Indexes = new List<int>() {0, 1, 2, 3, 4, 5, 6, 7, 8};

    public List<QuestNode> OccupiedNodes;
    private void Start()
    {
        OccupiedNodes = new List<QuestNode>();
        Row = new List<QuestNode>();
        
        for (int i = 0; i < Indexes.Count ; i++)
        {
            var original = Indexes[i];
            var targetIndex = Random.Range(0, Indexes.Count);
            Indexes[i] = Indexes[targetIndex];
            Indexes[targetIndex] = original;
        }
        
        var questNodes = new List<QuestNode>();
        
        for (int i = 0; i < 3; i++)
        {
            var questNode = Instantiate(QuestNodePrefab, transform);
            questNode.Occupied = true;
            var index = Indexes[0];
            Indexes.RemoveAt(0);
            questNode.Index = index; 
            OccupiedNodes.Add(questNode);
            Row.Add(questNode);
        }

        for (int i = 0; i < 6; i++)
        {
            var questNode = Instantiate(QuestNodePrefab, transform);
            questNode.Occupied = false;
            var index = Indexes[0];
            Indexes.RemoveAt(0);
            questNode.Index = index; 
            Row.Add(questNode);
        }

        foreach (var item in Row)
        {
            item.transform.SetSiblingIndex(item.Index);    
        }
    }
}
