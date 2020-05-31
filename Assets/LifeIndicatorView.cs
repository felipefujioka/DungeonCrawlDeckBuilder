using System.Collections;
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
        public TextMeshProUGUI DecreaseLifeLabel;
        
        private void Start()
        {
            EventSystem.Instance.AddListener<OnLifeDataChanged>(OnLifeChanged);
        }

        private void OnLifeChanged(OnLifeDataChanged e)
        {
            CoroutineManager.Instance.StartCoroutine(Animate(e));

        }

        private IEnumerator Animate(OnLifeDataChanged e)
        {
            MaxLifeLabel.text = e.MaxLife.ToString();

            if (e.Heal)
            {
                yield return AnimateChange(IncreaseLifeLabel, e.ChangedValue);
            }
            else
            {
                yield return AnimateChange(DecreaseLifeLabel, e.ChangedValue);
            }

            CurrentLifeLabel.text = e.CurrentLife.ToString();
        }

        private IEnumerator AnimateChange(TextMeshProUGUI label, int eChangedValue)
        {
            var finished = false;
            label.gameObject.SetActive(true);
            label.transform.position = CurrentLifeLabel.transform.position;

            var tween = label.transform.DOJump(
                label.transform.position + new Vector3(50, 200, 0),
                200,
                1,
                0.7f
            ).Play();

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