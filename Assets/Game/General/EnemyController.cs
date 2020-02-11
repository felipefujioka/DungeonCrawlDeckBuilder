using System;
using System.Collections;
using Game.Event;

namespace Game.General
{
    public class EnemyController
    {
        private int ticket;
        private int position;
        
        private int maxHP;
        private int currentHP;
        
        private EnemyView enemyView;
        private EnemyConfig config;

        public int GetTicket()
        {
            return ticket;
        }
        
        public EnemyController(EnemyView view, EnemyConfig config, int ticket, int position)
        {
            this.ticket = ticket;
            this.position = position;
            
            enemyView = view;
            this.config = config;
            
            enemyView.Init(config);
            maxHP = config.MaxHP;
            currentHP = maxHP;
        }

        public IEnumerator TakeDamage(int damage)
        {
            yield return enemyView.TakeDamage(damage, currentHP, maxHP);

            currentHP -= damage;

            if (currentHP <= 0)
            {
                EventSystem.Instance.Raise(new EnemyDiedDdbEvent()
                {
                    Ticket = GetTicket(),
                    Position = position
                });
            }
        }

        public void Die()
        {
            enemyView.StartCoroutine(enemyView.Die());
        }
    }
}