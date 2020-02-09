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
        
        EventSystem.Instance.AddListener<ChoseQuestNodeEvent>(OnChoseQuestNode);
        EventSystem.Instance.AddListener<RoomFinishedEvent>(OnRoomCleared);
    }

    private void OnRoomCleared(RoomFinishedEvent e)
    {
        onQuest = false;
    }

    private void OnChoseQuestNode(ChoseQuestNodeEvent e)
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
