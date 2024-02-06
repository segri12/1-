using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ещё_один_бой_сука    // тут ты думаешь и делаешь  её ты исправил 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Work();
        }
    }

    class Game
    {
        private Random _random = new Random();
        private List<Fighter> _fighters = new List<Fighter> { };

        private Fighter _fighter1;
        private Fighter _fighter2;

        public Game()
        {
            CreateFighters();
        }

        public void Work() 
        {
            const string CommandShowAllFighters = "1";
            const string CommandChooseFighters = "2";
            const string CommandStartFight = "3"; 
            const string CommandExit = "4";

            bool isWork = true;  

            while (isWork)  
            {
                Console.WriteLine($"{CommandShowAllFighters} - показать всех бойцов");
                Console.WriteLine($"{CommandChooseFighters} - выбрать бойцов");
                Console.WriteLine($"{CommandStartFight} - начать бой между выбранными бойцами");
                Console.WriteLine($"{CommandExit} - выйти");
                Console.Write("\nВыберите пункт меню: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandShowAllFighters:
                        ShowAllСharacteristicsFighters();
                        break;

                    case CommandChooseFighters:
                        ChooseFighters(); 
                        break;

                    case CommandStartFight:
                        Fight(_fighter1, _fighter2);
                        break;

                    case CommandExit:
                        isWork = false; 
                        break;

                    default:
                        Console.WriteLine("\nНет такой команды");
                        break;
                }

                Console.ReadKey();
                Console.Clear();
            }
        }

        private void CreateFighters()  
        {
            _fighters.Add(new CrazyMax("Безумный макс", 120, 20, 20));  
            _fighters.Add(new TrevorPhillips("Тревор филипс", 100, 20, 10));  
            _fighters.Add(new WilliamBlaskowitz("Уильям бласковиц", 110, 30, 15));  
            _fighters.Add(new Kratos("Кратос", 130, 20, 15)); 
            _fighters.Add(new Batman("Бетман", 140, 30, 10)); 
        }

        

        private bool TryGetFigher(out Fighter fighter)
        {
            fighter = null;

            ShowFighters();

            Console.Write("\nВыберите бойца: ");

            if (int.TryParse(Console.ReadLine(), out int index) == true)
            {
                index--;

                if (index >= 0 && index < _fighters.Count && _fighters[index].Health > 0)
                {
                    fighter = _fighters[index];

                    return true;
                }
                else
                {
                    Console.WriteLine("\nНет такого бойца");
                }
            }

            return false;
        }


        private void ChooseFighters()  
        {
            _fighter1 = GetFighter();

            if (_fighter1 != null)
            {
                Console.WriteLine("\nПервый боец: ");
                _fighter1.ShowInfo();

                _fighter2 = GetFighter(_fighter1);

                if (_fighter2 != null)
                {
                    Console.WriteLine("\nВторой боец: ");
                    _fighter2.ShowInfo();
                }
                else
                {
                    Console.WriteLine("\nОшибка, повторите выбор");
                }
            }
        }

        private Fighter GetFighter(Fighter excludeFighter = null)  
        {
            while (true)
            {
                if (TryGetFigher(out Fighter fighter) && (excludeFighter == null || fighter != excludeFighter))
                {
                    return fighter;
                }              
            }
        }

        private void Fight(Fighter fighter1, Fighter fighter2)
        {
            _fighter1 = null;
            _fighter2 = null;

            if (fighter1 != null && fighter2 != null)
            {
                Console.WriteLine("\nБитва началась!\n");

                while (fighter1.IsDead == false && fighter2.IsDead == false)
                {
                    if (ChooseAttackerFirst(fighter1, fighter2) == fighter1) 
                    {
                        Attack(fighter1, fighter2);
                        Attack(fighter2, fighter1);
                    }
                    else
                    {
                        Attack(fighter2, fighter1);
                        Attack(fighter1, fighter2);
                    }
                }

                Console.WriteLine("\nБитва окончена!");
            }
            else
            {
                Console.WriteLine("\nВыберите бойцов");
            }
        }

        private Fighter ChooseAttackerFirst(Fighter fighter1, Fighter fighter2)
        {
            if (fighter1.RollDice(_random) >= fighter2.RollDice(_random)) 
            {
                return fighter1;
            }

            return fighter2;
        }

        private void Attack(Fighter attacker, Fighter defender) 
        {
            if (attacker.IsDead == false)
            {
                defender.TakeDamage(attacker.Attak()); 

                Console.WriteLine($"{attacker.Name} (HP:{attacker.Health}) нанес урон {defender.Name} (HP:{defender.Health})");

                defender.AddSKill();
                attacker.AddSKill();

                if (defender.Health <= 0)
                {
                    Console.WriteLine($"\n{defender.Name} убит!");
                }
            }
        }

        private void ShowAllСharacteristicsFighters() 
        {
            int number = 0;

            Console.Write("\n");

            foreach (var fighter in _fighters)
            {
                number++;

                Console.Write(number + " - ");
                fighter.ShowInfo();
            }
        }

        private void ShowFighters() 
        {
            int number = 0;

            Console.Write("\n");

            foreach (var fighter in _fighters)
            {
                number++;

                if (fighter.Health > 0)
                {
                    Console.WriteLine($"{number} - {fighter.Name}");
                }
            }
        }
    }

    class Fighter
    {
        protected bool IsSkill = false;

        public Fighter(string name, int health, int armor, int damage)
        {
            Name = name;
            Health = health;
            Armor = armor;
            Damage = damage;
        }

        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int Armor { get; protected set; }
        public int Damage { get; protected set; }
        public bool IsDead => Health <= 0;

        public int RollDice(Random random)
        {
            int maxValue = 105;

            return random.Next(0, maxValue);
        }

        public void ShowInfo()
        {
            if (Health <= 0)
            {
                Console.WriteLine($"Убит");
                Console.WriteLine("----------------------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine($"Класс бойца: {Name} \nЗдоровье: {Health} \nБроня: {Armor} \nУрон: {Damage}");
                ShowSKill();
                Console.WriteLine("----------------------------------------------------------------------------");
            }
        }

        public int TakeDamage(int damage)  
        {
            int divider = 100; 

            Health -= damage - damage * Armor / divider;

            return Health;
        }

        public virtual int Attak() 
        {
            return Damage;
        }

        public virtual void AddSKill() 
        {
        }

        protected virtual void ShowSKill() 
        {
        }
    }

    class CrazyMax : Fighter  
    {
        private const int minimumHealthToUpgradeArmor = 30;    
        private const int maximumArmor = 50;  

        public CrazyMax(string name, int health, int armor, int damage) : base(name, health, armor, damage) { }

        public override void AddSKill() 
        {              
            if (Health < minimumHealthToUpgradeArmor && IsSkill == false)
            {
                IsSkill = true;
                Armor = maximumArmor;
                Console.WriteLine($"У {Name} сработало умение - броня увеличилась до {Armor}");
            }
        }

        protected override void ShowSKill() 
        {
            Console.Write("Умение: Если у бойца здоровье < 30, то получаемый урон уменьшается на 50%\n");
        }
    }

    class TrevorPhillips : Fighter
    {
        private const int minimumHealthToImproveAttack = 30;
        private const int increaseInNumber = 2; //увеличение количества

        public TrevorPhillips(string name, int health, int armor, int damage) : base(name, health, armor, damage) { }

        public override void AddSKill() 
        {                        
            if (Health < minimumHealthToImproveAttack && IsSkill == false)
            {
                IsSkill = true; 
                Damage *= increaseInNumber;

                Console.WriteLine($"У {Name} сработало умение - урон увеличился до {Damage}");
            }
        }

        protected override void ShowSKill() 
        {
            Console.Write("Умение: Если у бойца  здоровье < 30, то урон увеличивается в 2 раза\n");
        }
    }

    class WilliamBlaskowitz : Fighter
    {
        private const int minimumHealthToHit = 40;
        private const int increaseInNumber = 5;
        private const int devide = 2;
        
        public WilliamBlaskowitz(string name, int health, int armor, int damage) : base(name, health, armor, damage) { }

        public override int Attak()
        {
            int sumDamage = 0;

            if (Health < minimumHealthToHit && IsSkill == false)
            {
                IsSkill = true;

                for (int i = 0; i < increaseInNumber; i++)
                {
                    sumDamage += Attak() / devide;
                }

                Console.WriteLine($"У {Name} сработало умение - боец применил серию ударов");

                return sumDamage;
            }
            else
            {
                return base.Attak();
            }

        }
        protected override void ShowSKill() 
        {
            Console.Write("Умение: Если у бойца  здоровье < 40, то боец наносит серию ударов\n");
        }
    }

    class Kratos : Fighter
    {
        public Kratos(string name, int health, int armor, int damage) : base(name, health, armor, damage) { }

        public override void AddSKill() 
        {
            if (Health < 0 && IsSkill == false)
            {
                IsSkill = true;

                Health = 30;

                Console.WriteLine($"У {Name} сработало умение - Вы воскресли после нанесения смертельного удара");
            }
        }

        protected override void ShowSKill() 
        {
            Console.Write("Умение: Воскрешение\n");
        }
    }

    class Batman : Fighter
    {
        private const int minimumHealthIncreases = 20;
        private const int increaseInNumber = 3;

        public Batman(string name, int health, int armor, int damage) : base(name, health, armor, damage) { }

        public override void AddSKill()
        {
            
            if (Health > 0 && Health < minimumHealthIncreases && IsSkill == false)
            {
                IsSkill = true;

                Health *= increaseInNumber;

                Console.Write($"У {Name} сработало умение - восстановилась часть здоровья\n");
            }
        }

        protected override void ShowSKill()
        {
            Console.Write("Умение: Если у бойца  здоровье < 20, то здоровье бойца увеличивается в 3 раза\n");
        }
    }
}
    



