using System.Collections.Generic;
using System.Dynamic;
using DefaultNamespace;
using Game.Event;
using TMPro;

namespace Game.General
{
    public class HeroStatus
    {
        static HeroStatus instance;

        public static HeroStatus Instance {
            get
            {
                if (instance == null)
                {
                    instance = new HeroStatus();
                }

                return instance;
            }
        }

        int maxHP;
        int currentHP;
        int maxMana;
        int currentMana;

        int block;

        int drawsPerTurn;

        List<CardConfig> cardsInDeck;

        HeroStatus()
        {
            cardsInDeck = new List<CardConfig>();
            SetMaxMana(3, false);
            RestoreMana(false);
            
            SetMaxHP(80, false);
            FullHeal(false);
            
            SetDrawsPerTurn(5);
        }
        
        public int MaxHp => maxHP;

        public int CurrentHp => currentHP;

        public int MaxMana => maxMana;

        public int CurrentMana => currentMana;

        public int DrawsPerTurn => drawsPerTurn;

        public int Block => block;

        public List<CardConfig> CardsInDeck => cardsInDeck;

        public void SetDrawsPerTurn(int value)
        {
            drawsPerTurn = value;
        }
        
        public void SetMaxHP(int value, bool notify = true)
        {
            maxHP = value;

            if (notify)
            {
                NotifyLifeChanged(CurrentHp, maxHP, false, 0);
            }
        }

        public void SetMaxMana(int value, bool notify = true)
        {
            maxMana = value;

            if (notify)
            {
                NotifyManaDataChanged();
            }
            
        }

        public void GainBlock(int block)
        {
            this.block += block;
            
            NotifyBlockChanged(0, this.block);
        }

        public void ResetBlock()
        {
            block = 0;
            
            NotifyBlockChanged(0, 0);
        }

        public void TakeDamage(int damage)
        {
            if (Block > 0)
            {
                var blockedDamage = block > damage ? damage : block;
                block -= blockedDamage;
                damage -= blockedDamage;
                NotifyBlockChanged(blockedDamage, block);
            }
            
            currentHP -= damage;

            currentHP = CurrentHp < 0 ? 0 : CurrentHp;
            
            NotifyLifeChanged(currentHP, maxHP, false, damage);

            if (currentHP == 0)
            {
                EventSystem.Instance.Raise(new OnHeroDiedDdbEvent()); 
            }
        }

        public void Heal(int value)
        {
            currentHP += value;

            currentHP = CurrentHp > MaxHp ? MaxHp : CurrentHp;
            
            NotifyLifeChanged(currentHP, maxHP, true, value);
        }

        public void FullHeal(bool notify = true)
        {
            var value = maxHP - currentHP;
            
            currentHP = MaxHp;

            if (notify)
            {
                NotifyLifeChanged(currentHP, maxHP, true, value);    
            }
        }

        public void RecoverMana(int value)
        {
            currentMana += value;

            currentMana = CurrentMana > MaxMana ? MaxMana : CurrentMana;
            
            NotifyManaDataChanged();
        }
        
        public void RestoreMana(bool notify = true)
        {
            currentMana = MaxMana;

            if (notify)
            {
                NotifyManaDataChanged();    
            }
        }

        public void SpendMana(int value)
        {
            currentMana -= value;

            currentMana = CurrentMana < 0 ? 0 : CurrentMana;
            
            NotifyManaDataChanged();
        }

        public void AddCardToDeck(CardConfig card)
        {
            cardsInDeck.Add(card);
        }

        void NotifyManaDataChanged()
        {
            EventSystem.Instance.Raise(new OnManaDataChangeDdbEvent()
            {
                newMaxManaValue = MaxMana,
                newCurrentManaValue = CurrentMana
            });
        }

        void NotifyLifeChanged(int currentLife, int maxLife, bool heal, int changed)
        {
            EventSystem.Instance.Raise(new OnLifeDataChanged()
            {
                CurrentLife = currentLife,
                MaxLife = maxLife,
                Heal = heal,
                ChangedValue = changed
            });
        }

        void NotifyBlockChanged(int blocked, int block) 
        {
            EventSystem.Instance.Raise(new OnBlockDataChanged()
            {
                Blocked = blocked,
                Block = block
            });
        }

        public void Reset()
        {
            instance = null;
        }

        public void GainMana(int actionMagnitude)
        {
            currentMana += actionMagnitude;
            
            NotifyManaDataChanged();
        }
    }
}