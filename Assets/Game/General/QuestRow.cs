using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class QuestRow : MonoBehaviour
{
    public QuestNode QuestNodePrefab;
    public Image CheckImage;

    public List<QuestNode> Row;

    bool HasRestSite;
    public virtual void Start()
    {
        Row = new List<QuestNode>();
        
        for (int i = 0; i < 3 ; i++)
        {
            var node = Instantiate(QuestNodePrefab, transform);
            var randomRoll = Random.value;

            if (randomRoll < 0.1f && !HasRestSite)
            {
                node.RestSite = true;
                HasRestSite = true;
            }
            else
            {
                node.Occupied = true;
            }
            Row.Add(node);
        }
    }

    public void SetInteractable(bool interactable)
    {
        foreach (QuestNode node in Row)
        {
            node.SetInteractable(interactable);
        }
    }

    public void CompleteRow()
    {
        SetInteractable(false);
        CheckImage.gameObject.SetActive(true);
    }
}
