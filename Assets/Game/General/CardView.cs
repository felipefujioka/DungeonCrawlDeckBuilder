using TMPro;
using UnityEngine;

namespace Game.General
{
    public class CardView : MonoBehaviour
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public TextMeshProUGUI Cost;

        public CardConfig Config;
        
        public void Init(CardConfig config)
        {
            Config = config;

            Title.text = config.Title;
            Description.text = config.Description;
            Cost.text = config.ManaCost.ToString();
        }
    }
}