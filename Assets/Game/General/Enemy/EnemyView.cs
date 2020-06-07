using System.Collections;
using DG.Tweening;
using Game.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.General
{
    public class EnemyView : MonoBehaviour
    {
        public RectTransform LifeBarFill;
        public TextMeshProUGUI LifeLabel;
        public TextMeshProUGUI NameLabel;
        public Image EnemyImage;
        public Animator Animator;
        private static readonly int Act1 = Animator.StringToHash("Act");

        public void Init(EnemyConfig config)
        {
            EnemyImage.sprite = config.Sprite;
            LifeLabel.text = config.MaxHP + " / " + config.MaxHP;
            NameLabel.text = config.Name;
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
            
            yield return new WaitUntil(() => finished);
        }

        public IEnumerator Die()
        {
            var finished = false;

            EnemyImage.DOFade(0f, 0.5f).OnComplete(() => { finished = true; }).Play();
            
            yield return new WaitUntil(() => finished);
            
            Destroy(gameObject);
        }
    }
}