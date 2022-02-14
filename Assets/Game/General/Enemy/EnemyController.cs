using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Event;
using Game.General.Character;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.General
{
    public class EnemyController : BaseCharacter, IEnemyController
    {
        ActionConfig currentAction;

        int ticket;
        int position;

        int maxHP;
        int currentHP;

        int block;

        EnemyView enemyView;
        EnemyConfig config;

        int fixedActionIndex = 0;

        Dictionary<EnemyStatusType, IEnemyStatus> statuses;

        public int GetTicket()
        {
            return ticket;
        }
        
        public EnemyController(EnemyView view, EnemyConfig config, int ticket, int position)
        {
            this.ticket = ticket;
            this.position = position;
            
            statuses = new Dictionary<EnemyStatusType, IEnemyStatus>();
            
            enemyView = view;
            this.config = config;
            
            enemyView.Init(config);
            maxHP = config.MaxHP;
            currentHP = maxHP;
            
            ChangeCurrentAction();
        }

        public void Die()
        {
            enemyView.StartCoroutine(enemyView.Die());
        }

        public override void UpkeepReset()
        {
        }

        public override IEnumerator SufferDamage(int damage)
        {
            if (block > 0)
            {
                var blockedDamage = block > damage ? damage : block;
                block -= blockedDamage;
                damage -= blockedDamage;
                enemyView.ChangeBlock(block);
            }
            
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

        public override bool ShouldDraw()
        {
            return false;
        }

        public override int AmountToDraw()
        {
            return 0;
        }

        public override IEnumerator GainBlock(int actionMagnitude)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator ResetBlock()
        {
            block = 0;

            yield return null;
        }

        public ActionConfig GetCurrentAction()
        {
            return currentAction;
        }

        public void ChangeCurrentAction()
        {
            if (fixedActionIndex < config.FixedActions.Count)
            {
                currentAction = config.FixedActions[fixedActionIndex];
                fixedActionIndex++;
            }
            else
            {
                currentAction = config.Actions[Random.Range(0, config.Actions.Count)];    
            }
            
            enemyView.StartCoroutine(enemyView.ShowIntent(currentAction));
        }

        public bool IsAlive()
        {
            return currentHP > 0;
        }

        public IEnumerator AnimateAction()
        {
            yield return enemyView.Act();
        }

        public IEnumerator TakeDamage(int damage)
        {
            float modifier = 1;
            foreach (var status in statuses.Values)
            {
                modifier *= status.DamageTakenModifier();
            }
            yield return SufferDamage(Mathf.FloorToInt(modifier * damage));
        }

        public IEnumerator ActivateStatusesOnUpkeep()
        {
            foreach (var status in statuses.Values)
            {
                yield return status.OnBeginTurn(this);
                enemyView.UpdateStatusView(status);
            }
        }

        public IEnumerator ActivateStatusesOnEndPhase()
        {
            foreach (var status in statuses.Values)
            {
                yield return status.OnEndTurn(this);
            }
        }

        public List<IEnemyStatus> GetStatuses()
        {
            return statuses.Values.ToList();
        }

        public IEnumerator AddStatus(int actionMagnitude, EnemyStatusType type)
        {
            IEnemyStatus status;
            if (statuses.TryGetValue(type, out status))
            {
                yield return status.IncreaseStatus(actionMagnitude);
                enemyView.UpdateStatusView(status);
            }
            else
            {
                switch (type)
                {
                    case EnemyStatusType.POISON:
                        status = new PoisonStatus(actionMagnitude);
                        break;
                    case EnemyStatusType.VULNERABLE:
                        status = new VulnerableStatus(actionMagnitude);
                        break;
                    case EnemyStatusType.WEAKENED:
                        status = new WeakenedStatus(actionMagnitude);
                        break;
                }
                statuses.Add(type, status);
                enemyView.UpdateStatusView(status);
                
                yield return null;
            }
        }
    }
}