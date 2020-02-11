using System.Collections;
using System.Collections.Generic;
using Game.Event;
using Game.General;
using UnityEngine;
using UnityEngine.UI;

public class QuestNode : MonoBehaviour
{
    public List<QuestNode> NextNodes;
    public int Index;
    public bool Occupied;

    public Image OccupiedImage;
    public Button QuestButton;

    private void Start()
    {
        if (Occupied)
        {
            OccupiedImage.enabled = true;
            QuestButton.onClick.AddListener(OnQuestButtonPressed);
        }
    }

    private void OnQuestButtonPressed()
    {
        EventSystem.Instance.Raise(new ChoseQuestNodeDdbEvent()
        {
            Node = this
        });
    }
}
