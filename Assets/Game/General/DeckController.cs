using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.General
{
    public class DeckController
    {

        private List<CardConfig> deck;

        private List<CardConfig> discardPile;

        private List<CardConfig> hand;
        
        public DeckController(List<CardConfig> deckCards)
        {
            deck = new List<CardConfig>(deckCards);
        }

        private void ShuffleDeck()
        {
            for (int i = 0; i < deck.Count; i++)
            {
                var targetPosition = Random.Range(0, deck.Count);

                var buffer = deck[i];
                deck[i] = deck[targetPosition];
                deck[targetPosition] = buffer;
            }
        }

        public void RemoveCardFromHand(CardConfig card)
        {
            var indexToRemove = hand.IndexOf(card);
            
            hand.RemoveAt(indexToRemove);
        }
        
        public void DiscardCard(CardConfig card)
        {
            discardPile.Add(card);
        }

        public CardConfig DrawCard()
        {
            if (deck.Count == 0)
            {
                deck = new List<CardConfig>(discardPile);
                discardPile = new List<CardConfig>();
                ShuffleDeck();
            }
            
            var card = deck[0];
            deck.RemoveAt(0);

            return card;
        }
    }
}