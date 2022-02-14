using System;
using System.Collections;
using System.Collections.Generic;
using Game.Event;
using Game.General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestBuilder : MonoBehaviour
{
    public QuestRow QuestRowPrefab;
    public BossQuestRow BossRowPrefab;
    public VerticalLayoutGroup Content;
    public TextMeshProUGUI HP;

    List<QuestRow> QuestRows = new List<QuestRow>();

    public int CurrentRow = 0;
    
    IEnumerator Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var questRow = Instantiate(QuestRowPrefab, Content.transform).GetComponent<QuestRow>();
            yield return null;
            questRow.SetInteractable(CurrentRow == i);
            questRow.transform.SetAsFirstSibling();
            QuestRows.Add(questRow);
        }
        
        var bossRow = Instantiate(BossRowPrefab, Content.transform).GetComponent<BossQuestRow>();
        bossRow.SetInteractable(CurrentRow == 10);
        bossRow.transform.SetAsFirstSibling();
        QuestRows.Add(bossRow);
        
        EventSystem.Instance.AddListener<OnLifeDataChanged>(OnLifeChanged);
    }

    void OnLifeChanged(OnLifeDataChanged e)
    {
        HP.text = $"HP: {HeroStatus.Instance.CurrentHp}/{HeroStatus.Instance.MaxHp}";
    }

    void OnEnable()
    {
        HP.text = $"HP: {HeroStatus.Instance.CurrentHp}/{HeroStatus.Instance.MaxHp}";
    }

    public void CompleteCurrentRow()
    {
        QuestRows[CurrentRow].CompleteRow();
        CurrentRow++;
        if (CurrentRow < 10)
        {
            QuestRows[CurrentRow].SetInteractable(true);
        }
    }
}
