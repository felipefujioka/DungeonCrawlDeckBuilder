using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Event;

namespace Game.General
{
    public class EncounterController : IDisposable
    {
        private EncounterView view;

        private int ticketCount = 0;

        private Dictionary<int, EnemyController> enemies;
        private Dictionary<int, int> enemiesPositions;

        public EncounterController(EncounterView view, EncounterConfig config)
        {
            this.view = view;
            
            enemies = new Dictionary<int, EnemyController>();
            enemiesPositions = new Dictionary<int, int>();

            foreach (var enemy in config.Enemies)
            {
                var position = -1;
                var enemyView = this.view.AddEnemy(enemy, out position);
                
                var controller = new EnemyController(enemyView, enemy, ticketCount, position);
                
                enemiesPositions.Add(position, controller.GetTicket());

                enemies.Add(controller.GetTicket(), controller);
                
                ticketCount++;
            }
            
            EventSystem.Instance.AddListener<EnemyDiedDdbEvent>(OnEnemyDied);
        }

        public List<EnemyController> GetEnemies()
        {
            var ticketsInOrder = enemiesPositions.OrderBy(pair => pair.Key).Select(pair => pair.Value);
            
            var list = new List<EnemyController>();

            foreach (var ticket in ticketsInOrder)
            {
                list.Add(enemies[ticket]);
            }

            return list;
        }

        private void OnEnemyDied(EnemyDiedDdbEvent e)
        {
            var controller = enemies[e.Ticket];
            
            enemies.Remove(e.Ticket);
            enemiesPositions.Remove(e.Position);

            controller.Die();

            if (enemies.Count == 0)
            {
                EventSystem.Instance.Raise(new RoomFinishedDdbEvent());
            }
        }

        public IEnumerator DealDamage(int damage, int position)
        {
            var ticket = 0;
            if (enemiesPositions.ContainsKey(position))
            {
                ticket = enemiesPositions[position];
                
                var controller = enemies[ticket];

                yield return controller.SufferDamage(damage);
            }
            else
            {
                if (position == 2)
                {
                    position = 1;
                } 
                else if (position == 1)
                {
                    position = 0;
                }
                else if(position == 0)
                {
                    position = 2;
                }

                yield return DealDamage(damage, position);
            }
        }

        public void Dispose()
        {
            EventSystem.Instance.RemoveListener<EnemyDiedDdbEvent>(OnEnemyDied);
        }
    }
}