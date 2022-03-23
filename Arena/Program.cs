using System;
using System.Collections.Generic;

namespace Arena
{
    class Program
    {
        static void Main()
        {
            Ninja ninja = new Ninja("Ниндзя", 100, 30, 5, 2);
            Vampire vampire = new Vampire("Вампир", 150, 30, 0, 10);
            Viking viking = new Viking("Викинг", 120, 40, 10, 20);
            Knight knight = new Knight("Рыцарь", 100, 30, 30, 30);
            Warrior warrior = new Warrior("Воин", 170, 20, 20, 2, 30);
            //Gladiator[] gladiators = { ninja, vampire, viking, knight, warrior };

            List<Gladiator> gladiators = new List<Gladiator>() { ninja, vampire, viking, knight, warrior };

            Arena arena = new Arena(gladiators);
            arena.Start();
        }

        class Arena
        {
            private Gladiator _gladiatorOne;
            private Gladiator _gladiatorSecond;
            //private Gladiator[] _gladiators;
            private List<Gladiator> _gladiators;

            public void Start()
            {
                ShowAllGladiators();
                MakeChoice(ref _gladiatorOne, 1);
                ShowAllGladiators();
                MakeChoice(ref _gladiatorSecond, 2);
                Fight();
            }

            public Arena(/*Gladiator[] gladiators*/ List<Gladiator> gladiators)
            {
                _gladiators = gladiators;
            }

            public void MakeChoice(ref Gladiator gladiator, int number)
            {
                int indexFighter;

                do
                {
                    Console.Write("\nВыберите номер " + number + " бойца ");
                }
                while (Int32.TryParse(Console.ReadLine(), out indexFighter) == false || indexFighter < 1 || indexFighter > _gladiators.Count);

                gladiator = _gladiators[indexFighter - 1];
                _gladiators.Remove(gladiator);
            }

            public void ShowAllGladiators()
            {
                for (int i = 0; i < _gladiators.Count; i++)
                {
                    Console.Write((i + 1) + " ");
                    _gladiators[i].ShowInfo();
                }
            }

            private void CheckWin()
            {
                if (_gladiatorOne.Health <= 0 && _gladiatorSecond.Health <= 0)
                {
                    Console.WriteLine("\nНичья");
                }
                else if (_gladiatorOne.Health <= 0)
                {
                    Console.WriteLine("\nПОБЕДИЛ - " + _gladiatorSecond.Name);
                }
                else if (_gladiatorSecond.Health <= 0)
                {
                    Console.WriteLine("\nПОБЕДИЛ - " + _gladiatorOne.Name);
                }
            }

            public void Fight()
            {
                if (_gladiatorOne != null && _gladiatorSecond != null)
                {
                    while (_gladiatorOne.Health > 0 && _gladiatorSecond.Health > 0)
                    {
                        Console.WriteLine();
                        _gladiatorOne.TakeDamage(_gladiatorSecond.Damage);
                        _gladiatorSecond.TakeDamage(_gladiatorOne.Damage);
                        Console.WriteLine();
                        _gladiatorOne.ShowInfo();
                        _gladiatorSecond.ShowInfo();
                        CheckWin();

                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Вы не выбрали бойцов");
                }
            }
        }

        class Gladiator
        {
            private Random _random = new Random();

            protected string _name;
            protected int _health;
            protected int _damage;
            protected int _armor;
            protected int _saveDamage;
            protected int _saveArmor;

            public int Health => _health;
            public int Damage => _damage;
            public string Name => _name;
            public int Armor => _armor;

            public Gladiator(string name, int health, int damage, int armor)
            {
                _health = health;
                _damage = damage;
                _armor = armor;
                _name = name;
                _saveDamage = damage;
                _saveArmor = armor;
            }

            public Gladiator(Gladiator gladiator)
            {
                _health = gladiator.Health;
                _damage = gladiator.Damage;
                _armor = gladiator.Armor;
                _name = gladiator.Name;
                _saveDamage = gladiator.Damage;
                _saveArmor = gladiator.Armor;
            }

            public void TakeDamage(int damage)
            {
                int maxProbability = 100;
                int probability = 50;
                int maxHealth = 100;

                if (_random.Next(maxProbability) > probability)
                {
                    UseSpecialAttack();
                    Console.WriteLine($"{_name} Выполнил специальную атаку");
                }

                _health -= damage - _armor;
                _health = Clamp(_health, 0, maxHealth);
                CancelBonus();
            }

            private int Clamp(int value, int min, int max)
            {
                if (value > max)
                    return max;

                if (value < min)
                    return min;

                return value;
            }

            public void ShowInfo()
            {
                Console.WriteLine($"{_name} здоровье {_health} урон {_damage} броня {_armor}");
            }

            private void CancelBonus()
            {
                _damage = _saveDamage;
                _armor = _saveArmor;
            }

            protected virtual void UseSpecialAttack()
            {

            }
        }

        class Warrior : Gladiator
        {
            private int _berserkDamage;
            private int _fine;

            public Warrior(string name, int health, int damage, int armor, int berserkDamage, int fine) : base(name, health, damage, armor)
            {
                _berserkDamage = berserkDamage;
                _fine = fine;
            }

            protected override void UseSpecialAttack()
            {
                _damage *= _berserkDamage;
                _health -= _fine;
            }
        }

        class Ninja : Gladiator
        {
            private int _speedDamage;

            public Ninja(string name, int health, int damage, int armor, int speedDamage) : base(name, health, damage, armor)
            {
                _speedDamage = speedDamage;
            }

            protected override void UseSpecialAttack()
            {
                _damage *= _speedDamage;
                _armor = 0;
            }
        }

        class Viking : Gladiator
        {
            private int _battleCry;

            public Viking(string name, int health, int damage, int armor, int battleCry) : base(name, health, damage, armor)
            {
                _battleCry = battleCry;
            }

            protected override void UseSpecialAttack()
            {
                _damage += _battleCry;
            }
        }

        class Vampire : Gladiator
        {
            private int _amountHpRegeneration;

            public Vampire(string name, int health, int damage, int armor, int amountHpRegeneration) : base(name, health, damage, armor)
            {
                _amountHpRegeneration = amountHpRegeneration;
            }

            protected override void UseSpecialAttack()
            {
                _health += _amountHpRegeneration;
            }
        }

        class Knight : Gladiator
        {
            private int _defense;

            public Knight(string name, int health, int damage, int armor, int defense) : base(name, health, damage, armor)
            {
                _defense = defense;
            }

            protected override void UseSpecialAttack()
            {
                _armor += _defense;
            }
        }
    }
}