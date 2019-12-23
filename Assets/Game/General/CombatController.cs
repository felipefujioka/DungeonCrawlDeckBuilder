using System.Collections;
using System.Collections.Generic;
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
    }
}
