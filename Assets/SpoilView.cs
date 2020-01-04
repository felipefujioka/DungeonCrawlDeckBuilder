using System.Collections;
using System.Collections.Generic;
using Game.General;
using UnityEngine;

public class SpoilView : MonoBehaviour
{
    public CardView CardView;

    public void Init(CardConfig config)
    {
        CardView.Init(config);
    }
}
