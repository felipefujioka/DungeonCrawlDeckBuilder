using System.Collections;
using System.Collections.Generic;
using Game.Event;
using Game.General;
using Game.General.Character;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public GameObject FinishRoomPopup;
    
    public EncounterConfig Encounter;

    public CardHolderView CardHolder;
    public EncounterView EncounterView;
    
    private DeckController deckController;
    private EncounterController encounterController;

    private bool resolvingCard = false;

    private List<IHeroTurnPhase> heroTurnPhases;
    private HeroCharacter hero;
    
    void Start()
    {
        encounterController = new EncounterController(EncounterView, Encounter);
        
        var deck = Resources.Load<DeckConfig>("InitialDeck");

        foreach (var card in deck.Cards)
        {
            HeroStatus.Instance.AddCardToDeck(card);
        }
        
        deckController = new DeckController(HeroStatus.Instance.CardsInDeck);
        
        EventSystem.Instance.AddListener<RoomFinishedEvent>(OnFinishRoom);
        
        heroTurnPhases = new List<IHeroTurnPhase>
        {
            new HeroDrawPhase(),
            new HeroUpkeepPhase(),
            new HeroPlayPhase(encounterController),
            new HeroDiscardPhase()
        };

        StartCoroutine(RunCombat());
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
        }
    }

    private void OnFinishRoom(RoomFinishedEvent e)
    {
        FinishRoomPopup.SetActive(true);   
    }
}
