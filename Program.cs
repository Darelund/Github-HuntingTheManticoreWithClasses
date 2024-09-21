namespace Github_HuntingTheManticoreWithClasses
{
    //I wasn't to sure on where to put everything, like:
    //If I wanted the user to decide min and max Distance, where should I put that?
    //Should I have seperated player and city? I probably should have, I mean player is the player and the city is just the city.
    //GameManager does a lot of things and I don't think it should handle this much. Like displaying? Probably not, but at the same time its a GameManager(I am not enterily sure what a game manager really is)
    //So shouldn't it handle the status of the game?
    
    
    
    
    //Classes


    //First 3 are the objects of the game or whatever you call them, the things that interact the most with the "game world"

    //Manticore
    public class Manticore
    {
        public const int minDisancet = 0;
        public const int maxDistance = 100;

        public int Health { get; private set; }
        public int Damage { get; private set; }
        public int Distance { get; private set; }
        public Manticore(int health, int damage, int distance)
        {
            Health = health;
            Damage = damage;
            Distance = distance;
        }
        public void SetDistance(int newDistance)
        {
            if (newDistance > minDisancet && newDistance < maxDistance)
            {
                Distance = newDistance;
            }
        }
        public void takeDamage(int damage)
        {
            Health -= damage;
        }
    }

    //City
    public class City
    {
        public int Health { get; private set; }
        public City(int health)
        {
            Health = health;
        }
        public void takeDamage(int damage)
        {
            Health -= damage;
        }
    }

    //Player
    public class Player
    {
        public int Damage { get; private set; } = 0;
        public int Distance { get; private set; } = 0;

        public void ChangeDamage(int newdDamage)
        {
            Damage = newdDamage;
        }
        public void UpdateDistance(int newdDistance)
        {
            Distance = newdDistance;
        }
    }

    //Helper class for getting numbers
    public static class NumberInputHelper
    {
        public static int GetNumber(string text)
        {
            int number;
            while (true)
            {
                Console.Write(text);
                if (!int.TryParse(Console.ReadLine(), out number))
                {
                    Console.WriteLine("HEY! That's not a number?");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
                else
                {
                    return number;
                }
            }
        }
        public static int GetNumberWithinRange(string text, int minRange, int maxRange)
        {

            while (true)
            {
                int number = GetNumber(text);
                if (minRange < 0 || maxRange > 100)
                {
                    Console.WriteLine("Number need to be: ");
                    Console.WriteLine("A whole number");
                    Console.WriteLine("More than 0");
                    Console.WriteLine("Less than or equal to 100");
                    Thread.Sleep(2000);
                    Console.Clear();
                }
                else return number;
            }
        }
    }

    //A GameManager, I am not really sure here. It take cares of a lot of things
    public class GameManager
    {
        public static int round = 1; //Should this be here?

        public static void DisplayStatus(int round, int cityHealth, int manticoreHealth)
        {
            Console.WriteLine($"STATUS: Round: {round} City: {cityHealth}/15 Manticore: {manticoreHealth}/10");
        }
        public static int DamageForRound(int round)
        {
            if (round % 3 == 0 && round % 5 == 0) return 10;
            else if (round % 3 == 0 || round % 5 == 0) return 3;
            else return 1;
        }
        public static void DisplayHitResults(int manticoreDistance, int cannonRange)
        {
            if (cannonRange > manticoreDistance) Console.WriteLine("You overshot");
            else if (cannonRange < manticoreDistance) Console.WriteLine("You undershot");
            else Console.WriteLine("Hit");
        }
        public static bool CalculateHit(int manticoreRange, int cannonRange)
        {
            if (cannonRange == manticoreRange)
                return true;
            else
                return false;
        }
        public static bool IsGameOver(City city, Manticore manticore)
        {
            if (city.Health > 0 && manticore.Health > 0) return false;
            else return true;

        }
        public static void DisplayWinOrLose(Manticore manticore)
        {
            if (manticore.Health > 0)
            {
                Console.WriteLine("The Manticore could not be stopped and destroyed everything in its path. Not a single human survived");
            }
            else
            {
                Console.WriteLine("The Manticore was destroyed! The City was saved!");
            }
        }
    }


    //The game loop and stuff for the game to run
    public class Game
    {
        private City city;
        private Manticore manticore;
        private Player player;
        public Game(City city, Manticore manticore, Player player)
        {
            this.city = city;
            this.manticore = manticore;
            this.player = player;
        }


        public void RunGame()
        {
            while (!GameManager.IsGameOver(city, manticore))
            {
                //Display status
                Console.WriteLine("------------------------------------------------------------------");
                GameManager.DisplayStatus(GameManager.round, city.Health, manticore.Health);

                //Damage this round
                player.ChangeDamage(GameManager.DamageForRound(GameManager.round));
                Console.WriteLine($"The cannon is expected to deal {player.Damage} damage this round");

                //Get distance from player
                player.UpdateDistance(NumberInputHelper.GetNumber("Enter desired cannon range: "));

                //Display outcome
                GameManager.DisplayHitResults(manticore.Distance, player.Distance);
                if (GameManager.CalculateHit(manticore.Distance, player.Distance)) manticore.takeDamage(player.Damage);


                //Update each round based on game status
                if (manticore.Health > 0) city.takeDamage(manticore.Damage);
                if (manticore.Health > 0 && city.Health > 0) GameManager.round++;
            }
            GameManager.DisplayWinOrLose(manticore);
        }

    }




    //And last is the main method to run the game with additional stuff to get it ready to run
    internal class Program
    {
        static void Main(string[] args)
        {
            int cityHealth = 15;
            int manticoreHealth = 10;
            int manticoreDefaultDamage = 1;

            City city;
            Manticore manticore;
            Player player;
            Game game;

            city = new City(cityHealth);
            manticore = new Manticore(manticoreHealth, manticoreDefaultDamage, NumberInputHelper.GetNumberWithinRange("Player 1, " +
                 "how far away from the city do you want to station the Manticore? ", Manticore.minDisancet, Manticore.maxDistance));
            Console.Clear();
            player = new Player();

            game = new Game(city, manticore, player);
            game.RunGame();
        }
    }



}
