using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Event;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.General
{
    public class EnemyView : MonoBehaviour
    {
        public RectTransform LifeBarFill;
        public TextMeshProUGUI LifeLabel;
        public TextMeshProUGUI NameLabel;

        public CanvasGroup DamageCanvas;
        public TextMeshProUGUI DamageLabel;

        public Sprite AttackActionIcon;
        public Sprite DefenseActionIcon;
        public Sprite MagicActionIcon;
        public Sprite UnknownActionIcon;
        
        public Image IntentionIcon;
        public TextMeshProUGUI IntentLabel;

        public GameObject BlockGroup;
        public TextMeshProUGUI BlockLabel;
        
        public Image EnemyImage;
        public Animator Animator;
        static readonly int Act1 = Animator.StringToHash("Act");

        public Transform StatusHolder;
        public StatusDisplayView StatusPrefab;
        public Dictionary<EnemyStatusType, StatusDisplayView> StatusMap;

        public void Init(EnemyConfig config)
        {
            EnemyImage.sprite = config.Sprite;
            LifeLabel.text = config.MaxHP + " / " + config.MaxHP;
            NameLabel.text = config.Name;
            StatusMap = new Dictionary<EnemyStatusType, StatusDisplayView>();
        }

        public IEnumerator Act()
        {
            var finished = false;
            
            if (Animator != null)
            {
                Animator.SetTrigger(Act1);
            }

            var moveTween = EnemyImage.transform.DOLocalJump(Vector3.zero, 30, 1, 0.5f).Play().OnComplete(() =>
            {
                finished = true;
            });
            
            yield return new WaitUntil(() => finished);
            
            if (Animator != null)
            {
                yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).IsName("Act"));
            }
        } 
        
        public IEnumerator TakeDamage(int damage, int currentLife, int maxLife)
        {
            var life = currentLife;
            var finished = false;
            
            DOTween.To(() => life, (value) =>
                {
                    life = value;
                    LifeLabel.text = life + " / " + maxLife;
                }, currentLife - damage, 0.3f).Play();
            
            var targetAnchorMax = new Vector2((currentLife - damage) / (float) maxLife, LifeBarFill.anchorMax.y);

            LifeBarFill.DOAnchorMax(targetAnchorMax, 0.3f).OnComplete(() => { finished = true; }).Play();

            yield return AnimateDamage(damage);
            
            yield return new WaitUntil(() => finished);
        }

        IEnumerator AnimateDamage(int damage)
        {
            DamageLabel.text = damage.ToString();

            DamageCanvas.transform.localPosition = Vector3.zero;
            
            DamageCanvas.transform.DOLocalJump(new Vector3(100, 200, 0), 100, 1, 0.5f);

            DamageCanvas.alpha = 1f;

            yield return new WaitForSecondsRealtime(0.25f);

            yield return DamageCanvas.DOFade(0, 0.25f).SetEase(Ease.OutQuad).WaitForCompletion();
        }

        public IEnumerator Die()
        {
            var finished = false;

            EnemyImage.DOFade(0f, 0.5f).OnComplete(() => { finished = true; }).Play();
            
            yield return new WaitUntil(() => finished);
            
            Destroy(gameObject);
        }

        public IEnumerator ChangeBlock(int value)
        {
            if (value > 0)
            {
                BlockGroup.SetActive(false);
            }
            
            BlockLabel.text = value.ToString();

            yield return null;
        }

        public IEnumerator ShowIntent(ActionConfig action)
        {
            IntentLabel.text = "";
            
            switch (action.Type)
            {
                case ActionType.ATTACK:
                    IntentionIcon.sprite = AttackActionIcon;
                    IntentLabel.text = action.Magnitude.ToString();
                    break;
            }

            yield return null;
        }

        public void UpdateStatusView(IEnemyStatus status)
        {
            StatusDisplayView statusView;
            if (StatusMap.TryGetValue(status.GetStatusType(), out statusView))
            {
                if (status.GetMagnitude() > 0)
                {
                    statusView.MagnitudeText.text = status.GetMagnitude().ToString();    
                }
                else
                {
                    Destroy(statusView.gameObject);
                }
            }
            else
            {
                StatusDisplayView newStatusView = Instantiate(StatusPrefab, StatusHolder);
                StatusMap.Add(status.GetStatusType(), newStatusView);
                newStatusView.SetStatus(status);
                newStatusView.MagnitudeText.text = status.GetMagnitude().ToString();
            }
        }
    }
}