using System.Collections;
using Game.Event;
using Game.General;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestController : MonoBehaviour
{
    public QuestBuilder QuestMap;
    bool questFinished = false;
    bool onQuest = false;
    bool onBoss = false;
    bool rest = false;

    void Start()
    {
        StartCoroutine(RunQuest());
        
        EventSystem.Instance.AddListener<ChoseQuestNodeDdbEvent>(OnChoseQuestNode);
        EventSystem.Instance.AddListener<SpoilsGottenDdbEvent>(OnRoomCleared);
        EventSystem.Instance.AddListener<RestDdbEvent>(OnRest);
    }

    void OnDestroy()
    {
        EventSystem.Instance.RemoveListener<ChoseQuestNodeDdbEvent>(OnChoseQuestNode);
        EventSystem.Instance.RemoveListener<SpoilsGottenDdbEvent>(OnRoomCleared);
        EventSystem.Instance.RemoveListener<RestDdbEvent>(OnRest);
    }
    
    void OnRest(RestDdbEvent e)
    {
        rest = true;
    }

    void OnRoomCleared(SpoilsGottenDdbEvent e)
    {
        onQuest = false;
    }

    void OnChoseQuestNode(ChoseQuestNodeDdbEvent e)
    {
        onQuest = true;
    }

    IEnumerator RunQuest()
    {
        CombatController.Level = 0;
        
        while (!questFinished)
        {
            yield return new WaitUntil(() => onQuest || rest);

            if (rest)
            {
                HeroStatus.Instance.Heal((int)(0.33f * HeroStatus.Instance.MaxHp));
                QuestMap.CompleteCurrentRow();
                rest = false;
                continue;
            }
            
            QuestMap.gameObject.SetActive(false);

            SceneManager.LoadSceneAsync("CombatScene", LoadSceneMode.Additive);
            
            yield return new WaitUntil(() => !onQuest);

            SceneManager.UnloadSceneAsync("CombatScene");
            
            QuestMap.gameObject.SetActive(true);
            
            QuestMap.CompleteCurrentRow();

            CombatController.Level++;
        }
    }
}
