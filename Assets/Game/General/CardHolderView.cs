using System.Collections.Generic;
using UnityEngine;

namespace Game.General
{
    public class CardHolderView : MonoBehaviour
    {

        public RectTransform HandHolder;

        public CardView CardViewPrefab;

        private Dictionary<int, CardView> cardsInHand;

        private List<int> orderedCards;

        private void Start()
        {
            cardsInHand = new Dictionary<int, CardView>();
            orderedCards = new List<int>();
        }

        public void AddCardInHand(CardConfig config)
        {
            var cardViewInstance = Instantiate(CardViewPrefab, HandHolder);
            
            cardViewInstance.Init(config);

            cardsInHand.Add(cardViewInstance.GetHashCode(), cardViewInstance);
            
            orderedCards.Add(cardViewInstance.GetHashCode());
            
            ArrangeCards();
        }

        public void RemoveCardInHand(CardView card)
        {
            Destroy(card.gameObject);

            cardsInHand.Remove(card.GetHashCode());
            orderedCards.Remove(card.GetHashCode());
            
            ArrangeCards();
        }

        private void ArrangeCards()
        {
            var cardCount = orderedCards.Count;
            var cardSize = 300;
            var initialPos = Screen.width /2f - cardSize * (cardCount - 1) / 2f;

            for (int i = 0; i < cardCount; i++)
            {
                var code = orderedCards[i];
                var view = cardsInHand[code];
                
                view.transform.position = new Vector3(
                        initialPos + i * cardSize,
                        0,
                        view.transform.position.z
                    );
            }
        }
        
    }
}