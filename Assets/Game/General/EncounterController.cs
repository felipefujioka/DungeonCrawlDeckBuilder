using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Event;
using Game.General.Character;

namespace Game.General
{
    public class EncounterController : IDisposable
    {
        EncounterView view;

        int ticketCount = 0;

        Dictionary<int, EnemyController> enemies;
        Dictionary<int, int> enemiesPositions;

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

        void OnEnemyDied(EnemyDiedDdbEvent e)
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

        EnemyController GetEnemy(int position)
        {
            var ticket = 0;
            if (enemiesPositions.ContainsKey(position))
            {
                ticket = enemiesPositions[position];
                
                var controller = enemies[ticket];

                return controller;
            }

            if (enemiesPositions.ContainsKey(2))
            {
                return GetEnemy(2);
            }
            
            if (enemiesPositions.ContainsKey(1))
            {
                return GetEnemy(1);
            }
            
            if (enemiesPositions.ContainsKey(0))
            {
                return GetEnemy(0);
            }

            return null;
        }
        
        public IEnumerator HeroDealDamage(int damage, int position)
        {
            if (GetEnemy(position) != null)
            {
                yield return GetEnemy(position).TakeDamage(damage);    
            }
        }

        public void Dispose()
        {
            EventSystem.Instance.RemoveListener<EnemyDiedDdbEvent>(OnEnemyDied);
        }

        public IEnumerator CauseStatusOnEnemy(int actionMagnitude, EnemyStatusType status, int position)
        {
            var enemy = GetEnemy(position);
            if (enemy != null)
            {
                yield return enemy.AddStatus(actionMagnitude, status);    
            }
        }
    }
}