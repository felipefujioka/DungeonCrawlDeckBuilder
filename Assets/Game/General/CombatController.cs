using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Event;
using Game.General;
using Game.General.Character;
using Game.General.Turn.Enemy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatController : MonoBehaviour
{
    public RoomClearedView FinishRoomPopup;
    public GameObject GameOverScreen;
    
    public List<EncounterConfig> Encounters;

    public CardHolderView CardHolder;
    public EncounterView EncounterView;
    
    DeckController deckController;
    EncounterController encounterController;

    bool resolvingCard = false;

    List<IHeroTurnPhase> heroTurnPhases;
    HeroCharacter hero;

    List<IEnemyTurnPhase> enemyTurnPhases;

    public static int Level = 0;

    void Start()
    {
        EncounterConfig.Difficulty difficulty = Level < 4 ? 
            EncounterConfig.Difficulty.EASY :
            Level < 7 ? EncounterConfig.Difficulty.MEDIUM :
            Level <= 9 ? EncounterConfig.Difficulty.HARD : EncounterConfig.Difficulty.BOSS;

        List<EncounterConfig> candidates = Encounters.Where(encounter => encounter.EncounterDifficulty == difficulty).ToList();

        encounterController = new EncounterController(EncounterView, candidates[Random.Range(0, candidates.Count)]);
        
        var deck = Resources.Load<DeckConfig>("InitialDeck");

        foreach (var card in deck.Cards)
        {
            HeroStatus.Instance.AddCardToDeck(card);
        }

        var copyDeck = HeroStatus.Instance.CardsInDeck.Select(Instantiate).ToList();
        
        deckController = new DeckController(copyDeck);
        
        EventSystem.Instance.AddListener<RoomFinishedDdbEvent>(OnFinishRoom);
        EventSystem.Instance.AddListener<OnHeroDiedDdbEvent>(OnHeroDied);
        
        hero = new HeroCharacter();
        
        heroTurnPhases = new List<IHeroTurnPhase>
        {
            new HeroDrawPhase(),
            new HeroUpkeepPhase(),
            new HeroPlayPhase(encounterController, hero, deckController, CardHolder),
            new HeroDiscardPhase()
        };
        
        enemyTurnPhases = new List<IEnemyTurnPhase>()
        {
            new EnemyUpkeepPhase(),
            new EnemyActionPhase(),
            new EnemyFinishTurnPhase()
        };

        StartCoroutine(RunCombat());
    }

    void OnHeroDied(OnHeroDiedDdbEvent e)
    {
        GameOverScreen.SetActive(true);
    }

    IEnumerator RunCombat()
    {
        while (true)
        {
            foreach (var heroTurnPhase in heroTurnPhases)
            {
                yield return heroTurnPhase.BeginPhase(hero, deckController, CardHolder);
                yield return heroTurnPhase.WaitEndTurnCondition(hero);
                yield return heroTurnPhase.EndPhase(hero, deckController, CardHolder);
            }

            foreach (var enemy in encounterController.GetEnemies())
            {
                foreach (var enemyPhase in enemyTurnPhases)
                {
                    if (enemy.IsAlive())
                    {
                        yield return enemyPhase.BeginPhase(enemy, hero, deckController, CardHolder);
                        yield return enemyPhase.EndPhase(enemy, hero, deckController, CardHolder);    
                    }
                }
            }
        }
    }

    void OnFinishRoom(RoomFinishedDdbEvent e)
    {
        FinishRoomPopup.gameObject.SetActive(true);   
    }

    public void SendOnFinishTurnEvent()
    {
        EventSystem.Instance.Raise(new EndTurnDdbEvent());
    }

    public void ResetGame()
    {
        HeroStatus.Instance.Reset();
        SceneManager.LoadScene("QuestScene");
    }

    void OnDestroy()
    {
        deckController.Dispose();
        encounterController.Dispose();
        
        EventSystem.Instance.RemoveListener<RoomFinishedDdbEvent>(OnFinishRoom);
        EventSystem.Instance.RemoveListener<OnHeroDiedDdbEvent>(OnHeroDied);

        foreach (var phase in heroTurnPhases)
        {
            phase.OnDestroy();
        }
    }
}
