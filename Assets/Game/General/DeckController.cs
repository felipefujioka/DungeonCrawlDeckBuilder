using System;
using System.Collections.Generic;
using System.Linq;
using Game.Event;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Game.General
{
    public class DeckController : IDisposable
    {
        List<CardConfig> deck;

        List<CardConfig> discardPile;

        List<CardConfig> hand;
        
        public DeckController(List<CardConfig> deckCards)
        {
            deck = new List<CardConfig>(deckCards);
            discardPile = new List<CardConfig>();
            hand = new List<CardConfig>();
            
            ShuffleDeck();
            
            EventSystem.Instance.AddListener<UsedCardDdbEvent>(OnUseCardEvent);
        }

        void OnUseCardEvent(UsedCardDdbEvent e)
        {
            RemoveCardFromHand(e.CardView.Config);
            DiscardCard(e.CardView.Config);
        }

        void ShuffleDeck()
        {
            for (int i = 0; i < deck.Count; i++)
            {
                var targetPosition = Random.Range(0, deck.Count);

                var buffer = deck[i];
                deck[i] = deck[targetPosition];
                deck[targetPosition] = buffer;
            }
        }

        public List<CardConfig> GetHand()
        {
            return hand;
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
            if (deck.Count == 0 && discardPile.Count > 0)
            {
                deck = new List<CardConfig>(discardPile);
                discardPile = new List<CardConfig>();
                ShuffleDeck();
            }

            if (deck.Count == 0)
            {
                return null;
            }
            
            var card = deck[0];
            deck.RemoveAt(0);
            
            hand.Add(card);

            return card;
        }

        public void Dispose()
        {
            EventSystem.Instance.RemoveListener<UsedCardDdbEvent>(OnUseCardEvent);
        }
    }
}