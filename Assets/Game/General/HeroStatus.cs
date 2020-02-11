using System.Collections.Generic;
using System.Dynamic;
using DefaultNamespace;

namespace Game.General
{
    public class HeroStatus
    {
        private static HeroStatus instance;

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

        private int maxHP;
        private int currentHP;
        private int maxMana;
        private int currentMana;

        private int drawsPerTurn;
        
        private List<CardConfig> cardsInDeck;

        private HeroStatus()
        {
            cardsInDeck = new List<CardConfig>();
            SetMaxMana(3);
            RestoreMana();
            
            SetMaxHP(80);
            FullHeal();
            
            SetDrawsPerTurn(5);
        }
        
        public int MaxHp => maxHP;

        public int CurrentHp => currentHP;

        public int MaxMana => maxMana;

        public int CurrentMana => currentMana;

        public int DrawsPerTurn => drawsPerTurn;

        public List<CardConfig> CardsInDeck => cardsInDeck;

        public void SetDrawsPerTurn(int value)
        {
            drawsPerTurn = value;
        }
        
        public void SetMaxHP(int value)
        {
            maxHP = value;
        }

        public void SetMaxMana(int value)
        {
            maxMana = value;
            
            NotifyManaDataChanged();
        }

        public void TakeDamage(int damage)
        {
            currentHP -= damage;

            currentHP = CurrentHp < 0 ? 0 : CurrentHp;
        }

        public void Heal(int value)
        {
            currentHP += value;

            currentHP = CurrentHp > MaxHp ? MaxHp : CurrentHp;
        }

        public void FullHeal()
        {
            currentHP = MaxHp;
        }

        public void RecoverMana(int value)
        {
            currentMana += value;

            currentMana = CurrentMana > MaxMana ? MaxMana : CurrentMana;
            
            NotifyManaDataChanged();
        }
        
        public void RestoreMana()
        {
            currentMana = MaxMana;
            
            NotifyManaDataChanged();
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

        private void NotifyManaDataChanged()
        {
            EventSystem.Instance.Raise(new OnManaDataChangeDdbEvent()
            {
                newMaxManaValue = MaxMana,
                newCurrentManaValue = CurrentMana
            });
        }
    }
}