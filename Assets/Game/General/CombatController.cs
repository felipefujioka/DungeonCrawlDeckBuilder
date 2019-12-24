using System.Collections;
using System.Collections.Generic;
using Game.Event;
using Game.General;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public EncounterConfig Encounter;

    public CardHolderView CardHolder;
    public EncounterView EncounterView;
    
    private DeckController deckController;
    private EncounterController encounterController;

    private bool resolvingCard = false;
    
    void Start()
    {
        encounterController = new EncounterController(EncounterView, Encounter);
        
        var deck = Resources.Load<DeckConfig>("InitialDeck");

        foreach (var card in deck.Cards)
        {
            HeroStatus.Instance.AddCardToDeck(card);
        }
        
        deckController = new DeckController(HeroStatus.Instance.CardsInDeck);
        
        for (int i = 0; i < 5; i++)
        {
            CardHolder.AddCardInHand(deckController.DrawCard());
        }
        
        EventSystem.Instance.AddListener<TryUseCardEvent>(OnTryUseCard);
        EventSystem.Instance.AddListener<EndTurnEvent>(OnEndTurn);
    }

    public void EndTurn()
    {
        EventSystem.Instance.Raise(new EndTurnEvent());
    }

    private void OnEndTurn(EndTurnEvent e)
    {
        HeroStatus.Instance.RestoreMana();
        for (int i = 0; i < HeroStatus.Instance.DrawsPerTurn; i++)
        {
            CardHolder.AddCardInHand(deckController.DrawCard());
        }
    }

    private void OnTryUseCard(TryUseCardEvent tryUseCardEvent)
    {
        if (!resolvingCard && tryUseCardEvent.CardView.Config.ManaCost <= HeroStatus.Instance.CurrentMana)
        {
            EventSystem.Instance.Raise(new UsedCardEvent()
            {
                CardView = tryUseCardEvent.CardView,
                Position = tryUseCardEvent.Position
            });
            
            StartCoroutine(UseCard(tryUseCardEvent.CardView.Config, tryUseCardEvent.Position));
            
            HeroStatus.Instance.SpendMana(tryUseCardEvent.CardView.Config.ManaCost);
        }
    }

    private IEnumerator UseCard(CardConfig card, Position position)
    {
        resolvingCard = true;
        
        foreach (var action in card.Actions)
        {
            yield return UseAction(action, position);
        }

        resolvingCard = false;
    }

    private IEnumerator UseAction(ActionConfig action, Position position)
    {
        if (action.Type == ActionType.ATTACK)
        {
            yield return encounterController.DealDamage(action.Magnitude, (int) position);
        }
    }
}
