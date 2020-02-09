using System.Collections;
using System.Collections.Generic;
using Game.General;
using UnityEngine;
using UnityEngine.UI;

public class SpoilView : MonoBehaviour
{
    public Outline SelectView;
    public CardView CardView;

    public void Init(CardConfig config)
    {
        CardView.Init(config);
    }

    public void Select(bool select)
    {
        SelectView.enabled = select;
    }
}
