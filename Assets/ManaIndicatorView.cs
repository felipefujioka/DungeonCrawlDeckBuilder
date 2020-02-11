using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using Game.General;
using TMPro;
using UnityEngine;

public class ManaIndicatorView : MonoBehaviour
{
    public TextMeshProUGUI ManaLabel;
    
    void Start()
    {
        EventSystem.Instance.AddListener<OnManaDataChangeDdbEvent>(OnManaDataChangeHandler);       
    }

    private void OnManaDataChangeHandler(OnManaDataChangeDdbEvent e)
    {
        ManaLabel.text = $"{e.newCurrentManaValue}/{e.newMaxManaValue}";

        ManaLabel.transform.DOKill(true);
        
        ManaLabel.transform.DOPunchScale(
            Vector3.one * 1.2f, 
            0.5f,
            2,
            0.3f
            ).Play();
    }
}
