using System;
using System.Collections;
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

        public IEnumerator AnimateAction()
        {
            yield return enemyView.Act();
        }
    }
}