using System.Collections;
using Game.Event;
using Game.General;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestController : MonoBehaviour
{
    public GameObject QuestMap;
    private bool questFinished = false;
    private bool onQuest = false;

    private void Start()
    {
        StartCoroutine(RunQuest());
        
        EventSystem.Instance.AddListener<ChoseQuestNodeDdbEvent>(OnChoseQuestNode);
        EventSystem.Instance.AddListener<SpoilsGottenDdbEvent>(OnRoomCleared);
    }

    private void OnRoomCleared(SpoilsGottenDdbEvent e)
    {
        onQuest = false;
    }

    private void OnChoseQuestNode(ChoseQuestNodeDdbEvent e)
    {
        onQuest = true;
    }

    private IEnumerator RunQuest()
    {
        while (!questFinished)
        {
            yield return new WaitUntil(() => onQuest);
            
            QuestMap.SetActive(false);

            SceneManager.LoadSceneAsync("CombatScene", LoadSceneMode.Additive);
            
            yield return new WaitUntil(() => !onQuest);

            SceneManager.UnloadSceneAsync("CombatScene");
            
            QuestMap.SetActive(true);
        }
    }
}
