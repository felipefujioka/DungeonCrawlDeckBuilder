using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance;
    
    public TextMeshProUGUI TooltipText;

    float hideTimeout = 0;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        gameObject.SetActive(false);
    }

    public void ShowTooltip(string text)
    {
        TooltipText.text = text;
        gameObject.SetActive(true);
        hideTimeout = 0f;
    }

    public void Hide()
    {
        if (hideTimeout > Time.deltaTime)
        {
            gameObject.SetActive(false);    
        }
    }
    
    void Update()
    {
        hideTimeout += Time.deltaTime;
        transform.position = Input.mousePosition;
    }
}
