using System;
using System.Collections;
using System.Collections.Generic;
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

    private List<SpoilView> spoils;
    
    private CardConfig selectedCard;
    
    void Start()
    {
        var cardList = Resources.LoadAll<CardConfig>("Cards");

        spoils = new List<SpoilView>();
        
        for (int i = 0; i < 3; i++)
        {
            var spoil = Instantiate(SpoilViewPrefab, SpoilHolder.transform);
            spoil.CardView.Init(cardList[Random.Range(0, cardList.Length)]);

            spoils.Add(spoil);
            
            var button = spoil.GetComponent<Button>();
            
            button.onClick.AddListener(OnSelectSpoil(spoil));
        }
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
