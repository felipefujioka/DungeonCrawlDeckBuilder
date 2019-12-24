using System.Collections;
using DG.Tweening;
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

        public void Init(EnemyConfig config)
        {
            EnemyImage.sprite = config.Sprite;
            LifeLabel.text = config.MaxHP + " / " + config.MaxHP;
            NameLabel.text = config.Name;
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