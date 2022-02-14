using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Event;
using Game.General;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoomClearedView : MonoBehaviour
{

    public SpoilView SpoilViewPrefab;
    public HorizontalLayoutGroup SpoilHolder;

    List<SpoilView> spoils;

    CardConfig selectedCard;

    static List<CardConfig> CommonCards;
    static List<CardConfig> UncommonCards;
    static List<CardConfig> RareCards;

    void Start()
    {
        if (CommonCards == null)
        {
            var cardList = Resources.LoadAll<CardConfig>("Cards");

            CommonCards = cardList.Where(card => card.Rarity == CardConfig.RarityValue.COMMON).ToList();
            UncommonCards = cardList.Where(card => card.Rarity == CardConfig.RarityValue.UNCOMMON).ToList();
            RareCards = cardList.Where(card => card.Rarity == CardConfig.RarityValue.RARE).ToList();
        }

        spoils = new List<SpoilView>();
        HashSet<CardConfig> cards = new HashSet<CardConfig>();

        for (int i = 0; i < 3; i++)
        {
            var spoil = Instantiate(SpoilViewPrefab, SpoilHolder.transform);

            var candidate = GetCard();

            while (cards.Contains(candidate))
            {
                candidate = GetCard();
            }

            cards.Add(candidate);

            spoil.CardView.Init(candidate);

            spoils.Add(spoil);
            
            var button = spoil.GetComponent<Button>();
            
            button.onClick.AddListener(OnSelectSpoil(spoil));
        }
    }

    CardConfig GetCard()
    {
        var random = Random.value;

        if (random < 0.15f)
        {
            return RareCards[Random.Range(0, RareCards.Count)];
        }
        
        if (random < 0.33f)
        {
            return UncommonCards[Random.Range(0, UncommonCards.Count)];
        }

        return CommonCards[Random.Range(0, CommonCards.Count)];
    }

    private UnityAction OnSelectSpoil(SpoilView spoil)
    {
        return () =>
        {
            selectedCard = spoil.CardView.Config;
            ResetSpoilViews();
            spoil.Select(true);
        };
    }

    public void OnChoseSpoil()
    {
        if (selectedCard != null)
        {
            EventSystem.Instance.Raise(new SpoilsGottenDdbEvent()
            {
                card = selectedCard
            });
            
            HeroStatus.Instance.AddCardToDeck(selectedCard);
        }
    }

    private void ResetSpoilViews()
    {
        foreach (var spoil in spoils)
        {
            spoil.Select(false);
        }
    }

    private void OnDestroy()
    {
        foreach (var spoil in spoils)
        {
            spoil.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}
