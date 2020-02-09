using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBuilder : MonoBehaviour
{
    public GameObject QuestRowPrefab;
    public VerticalLayoutGroup Content;

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var questRow = Instantiate(QuestRowPrefab, Content.transform).GetComponent<QuestRow>();
            
        }
    }
}
