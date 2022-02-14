using System;
using System.Collections;
using System.Collections.Generic;
using Game.General;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusDisplayView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image StatusIcon;
    public TextMeshProUGUI MagnitudeText;

    public Sprite PoisonIcon;
    public Sprite VulnerableIcon;
    public Sprite WeakenedIcon;

    IEnemyStatus status;

    public void SetStatus(IEnemyStatus status)
    {
        this.status = status;
        SetIcon(this.status);
    }
    
    void SetIcon(IEnemyStatus status)
    {
        switch (status.GetStatusType())
        {
            case EnemyStatusType.POISON:
                StatusIcon.sprite = PoisonIcon;
                break;
            case EnemyStatusType.VULNERABLE:
                StatusIcon.sprite = VulnerableIcon;
                break;
            case EnemyStatusType.WEAKENED:
                StatusIcon.sprite = WeakenedIcon;
                break;
            default:
                throw new Exception();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Instance.ShowTooltip(status.GetTooltip());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Instance.Hide();
    }
}
