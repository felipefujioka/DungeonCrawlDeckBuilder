using System;
using System.Collections;
using System.Collections.Generic;
using Game.Event;
using Game.General.Character;
using Random = UnityEngine.Random;

namespace Game.General
{
    public class EnemyController : BaseCharacter, IEnemyController
    {
        private ActionConfig currentAction;
        
        private int ticket;
        private int position;
        
        private int maxHP;
        private int currentHP;

        private int block;

        private EnemyView enemyView;
        private EnemyConfig config;

        private Dictionary<EnemyStatusType, EnemyStatus> statuses;

        public int GetTicket()
        {
            return ticket;
        }
        
        public EnemyController(EnemyView view, EnemyConfig config, int ticket, int position)
        {
            this.ticket = ticket;
            this.position = position;
            
            statuses = new Dictionary<EnemyStatusType, EnemyStatus>();
            
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
            currentAction = config.Actions[Random.Range(0, config.Actions.Count)];
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
            yield return SufferDamage(damage);
        }

        public IEnumerator ActivateStatusesOnUpkeep()
        {
            foreach (var status in statuses.Values)
            {
                yield return status.OnBeginTurn(this);
            }
        }

        public IEnumerator ActivateStatusesOnEndPhase()
        {
            foreach (var status in statuses.Values)
            {
                yield return status.OnEndTurn(this);
            }
        }

        public IEnumerator AddStatus(int actionMagnitude, EnemyStatusType status)
        {
            switch (status)
            {
                case EnemyStatusType.POISON: 
                    EnemyStatus poison;
                    if (statuses.TryGetValue(EnemyStatusType.POISON, out poison))
                    {
                        yield return poison.IncreaseStatus(actionMagnitude);
                    }
                    else
                    {
                        var poisonStatus = new PoisonStatus(actionMagnitude);
                        statuses.Add(EnemyStatusType.POISON, poisonStatus);

                        yield return null;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}