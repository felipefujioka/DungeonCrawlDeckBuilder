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
    
    public EncounterConfig Encounter;

    public CardHolderView CardHolder;
    public EncounterView EncounterView;
    
    private DeckController deckController;
    private EncounterController encounterController;

    private bool resolvingCard = false;

    private List<IHeroTurnPhase> heroTurnPhases;
    private HeroCharacter hero;

    private List<IEnemyTurnPhase> enemyTurnPhases;
    
    void Start()
    {
        encounterController = new EncounterController(EncounterView, Encounter);
        
        var deck = Resources.Load<DeckConfig>("InitialDeck");

        foreach (var card in deck.Cards)
        {
            HeroStatus.Instance.AddCardToDeck(card);
        }

        var copyDeck = HeroStatus.Instance.CardsInDeck.Select(Instantiate).ToList();
        
        deckController = new DeckController(copyDeck);
        
        EventSystem.Instance.AddListener<RoomFinishedDdbEvent>(OnFinishRoom);
        EventSystem.Instance.AddListener<OnHeroDiedDdbEvent>(OnHeroDied);
        
        heroTurnPhases = new List<IHeroTurnPhase>
        {
            new HeroDrawPhase(),
            new HeroUpkeepPhase(),
            new HeroPlayPhase(encounterController),
            new HeroDiscardPhase()
        };
        
        enemyTurnPhases = new List<IEnemyTurnPhase>()
        {
            new EnemyUpkeepPhase(),
            new EnemyActionPhase(),
            new EnemyFinishTurnPhase()
        };
        
        hero = new HeroCharacter();

        StartCoroutine(RunCombat());
    }

    private void OnHeroDied(OnHeroDiedDdbEvent e)
    {
        GameOverScreen.SetActive(true);
    }

    private IEnumerator RunCombat()
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
                    yield return enemyPhase.BeginPhase(enemy, hero, deckController, CardHolder);
                    yield return enemyPhase.EndPhase(enemy, hero, deckController, CardHolder);
                }
            }
        }
    }

    private void OnFinishRoom(RoomFinishedDdbEvent e)
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

    private void OnDestroy()
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
