using System.Collections;
using System.Collections.Generic;
using Game.Event;
using Game.General;
using UnityEngine;
using UnityEngine.UI;

public class QuestNode : MonoBehaviour
{
    public bool Occupied;
    public bool RestSite;
    public bool IsBoss;

    public Image OccupiedImage;
    public Image RestSiteImage;
    public Image BossImage;
    public Button QuestButton;
    

    void Start()
    {
        QuestButton.onClick.AddListener(OnQuestButtonPressed);
        if (Occupied)
        {
            OccupiedImage.enabled = true;
        }
        else if (RestSite)
        {
            RestSiteImage.enabled = true;
        }

        if (IsBoss)
        {
            BossImage.enabled = true;
        }
    }

    void OnQuestButtonPressed()
    {
        if (Occupied)
        {
            EventSystem.Instance.Raise(new ChoseQuestNodeDdbEvent()
            {
                Node = this
            });    
        } 
        else if (RestSite)
        {
            EventSystem.Instance.Raise(new RestDdbEvent());
        } 
        else if (IsBoss)
        {
            EventSystem.Instance.Raise(new ChoseQuestNodeDdbEvent()
            {
                Node = this
            });    
        }
         
    }

    public void SetInteractable(bool interactable)
    {
        QuestButton.enabled = interactable;
        OccupiedImage.color = new Color(1, 1, 1, interactable ? 1 : 0.5f);
        RestSiteImage.color = new Color(1, 1, 1, interactable ? 1 : 0.5f);
    }
}
