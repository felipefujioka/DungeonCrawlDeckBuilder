using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Game.Event;
using UnityEngine;

namespace Game.General
{
    public class CardHolderView : MonoBehaviour
    {

        public RectTransform HandHolder;

        public CardView CardViewPrefab;

        private Dictionary<int, CardView> cardsInHand;

        private List<int> orderedCards;

        private void Awake()
        {
            cardsInHand = new Dictionary<int, CardView>();
            orderedCards = new List<int>();
            
            EventSystem.Instance.AddListener<UsedCardEvent>(OnUseCardHandler);
        }

        private void OnDestroy()
        {
            EventSystem.Instance.RemoveListener<UsedCardEvent>(OnUseCardHandler);
        }

        private void OnUseCardHandler(UsedCardEvent e)
        {
            RemoveCardInHand(e.CardView);
        }

        public Dictionary<int, CardView> GetCardsInHand()
        {
            return cardsInHand;
        }
        
        public IEnumerator DiscardCard(CardConfig cardConfig)
        {
            CardView card = null;

            foreach (var cardInHand in cardsInHand)
            {
                if (cardInHand.Value.Config == cardConfig)
                {
                    card = cardInHand.Value;
                    break;
                }
            }
            
            RemoveCardInHand(card);

            yield return null;
            
            ArrangeCards();
        }

        public IEnumerator AddCardInHand(CardConfig config)
        {
            if (config == null)
            {
                yield break;
            }
            
            var cardViewInstance = Instantiate(CardViewPrefab, HandHolder);
            
            cardViewInstance.Init(config);

            cardsInHand.Add(cardViewInstance.GetHashCode(), cardViewInstance);
            
            orderedCards.Add(cardViewInstance.GetHashCode());
            
            ArrangeCards();

            yield return null;
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