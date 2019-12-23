using System.Collections;
using System.Collections.Generic;
using Game.Event;
using Game.General;
using UnityEngine;

public class CombatController : MonoBehaviour
{

    public CardHolderView CardHolder;
    
    private DeckController deckController;
    
    void Start()
    {
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
        if (tryUseCardEvent.CardView.Config.ManaCost <= HeroStatus.Instance.CurrentMana)
        {
            EventSystem.Instance.Raise(new UsedCardEvent()
            {
                CardView = tryUseCardEvent.CardView
            });
            
            HeroStatus.Instance.SpendMana(tryUseCardEvent.CardView.Config.ManaCost);
        }
    } 
}
