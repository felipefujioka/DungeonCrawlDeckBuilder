using System;
using Game.Event;
using Game.General;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class BlockIndicatorView : MonoBehaviour
    {
        public GameObject ViewGroup;
        public TextMeshProUGUI ValueLabel;
        
        private void Start()
        {
            EventSystem.Instance.AddListener<OnBlockDataChanged>(OnBlockDataChanged);
        }

        private void OnDestroy()
        {
            EventSystem.Instance.RemoveListener<OnBlockDataChanged>(OnBlockDataChanged); 
        }

        private void OnBlockDataChanged(OnBlockDataChanged e)
        {
            ViewGroup.SetActive(e.Block > 0);
            ValueLabel.text = e.Block.ToString();
        }
    }
}