using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using DG.Tweening;
using Game.Event;
using Game.General;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class LifeIndicatorView : MonoBehaviour
    {
        public TextMeshProUGUI CurrentLifeLabel;
        public TextMeshProUGUI MaxLifeLabel;

        public TextMeshProUGUI IncreaseLifeLabel;
        public List<TextMeshProUGUI> DecreaseLifeLabels;

        private int labelsCount;
        private int labelIndex;
        
        private void Start()
        {
            EventSystem.Instance.AddListener<OnLifeDataChanged>(OnLifeChanged);
            labelsCount = DecreaseLifeLabels.Count;
        }

        private void OnDestroy()
        {
            EventSystem.Instance.RemoveListener<OnLifeDataChanged>(OnLifeChanged);
        }

        private void OnLifeChanged(OnLifeDataChanged e)
        {
            labelIndex = labelIndex < labelsCount - 1 ? labelIndex + 1 : 0;  
            
            CoroutineManager.Instance.StartCoroutine(Animate(e));

        }

        private IEnumerator Animate(OnLifeDataChanged e)
        {
            MaxLifeLabel.text = e.MaxLife.ToString();
            
            CurrentLifeLabel.text = e.CurrentLife.ToString();

            if (e.Heal)
            {
                yield return AnimateChange(IncreaseLifeLabel, e.ChangedValue);
            }
            else
            {
                yield return AnimateChange(DecreaseLifeLabels[labelIndex], e.ChangedValue);
            }
        }

        private IEnumerator AnimateChange(TextMeshProUGUI label, int eChangedValue)
        {
            var finished = false;
            label.gameObject.SetActive(true);
            label.transform.position = CurrentLifeLabel.transform.position;
            label.alpha = 1f;

            var tween = label.transform.DOLocalJump(
                new Vector3(50, 0, 0),
                140,
                1,
                0.7f
            );

            var fadeTween = label.DOFade(0, 0.3f);

            var seq = DOTween.Sequence();
            seq.Append(tween);
            seq.Insert(0.4f, fadeTween);

            seq.Play();

            label.text = eChangedValue.ToString();

            tween.OnComplete(() =>
            {
                finished = true;
                label.gameObject.SetActive(false);
            });

            yield return new WaitUntil(() => finished);
        }
    }
}