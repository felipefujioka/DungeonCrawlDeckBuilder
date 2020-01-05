using System.Collections.Generic;
using UnityEngine;

namespace Game.General
{
    public class EncounterView : MonoBehaviour
    {
        public EnemyView EnemyViewPrefab;
        
        public RectTransform LeftSlot;
        public RectTransform MiddleSlot;
        public RectTransform RightSlot;

        private List<RectTransform> Slots;

        private void Awake()
        {
            Slots = new List<RectTransform> {LeftSlot, MiddleSlot, RightSlot};
        }

        private int GetEmptySlot()
        {
            var emptySlot = -1;

            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots[i].childCount == 0)
                {
                    emptySlot = i;
                    break;
                }
            }

            return emptySlot;
        }
        
        public EnemyView AddEnemy(EnemyConfig enemy, out int position)
        {
            
            var emptySlot = GetEmptySlot();

            if (emptySlot != -1)
            {
                position = emptySlot;
                var enemyView = Instantiate(EnemyViewPrefab, Slots[emptySlot]);
                enemyView.transform.localPosition = Vector3.zero;

                return enemyView;
            }

            position = emptySlot;
            return null;
        }
    }
}