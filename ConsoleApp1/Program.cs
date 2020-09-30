﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(106,33);

            Random random = new Random();
            var town = CreateTown(random);
            var numberInPrison = 0;

            while (true)
            {
                
                PrintOutPersons(town);

                Collision(town, numberInPrison, random);

                CountPrisonTime(town, numberInPrison);

                numberInPrison = CountPrisonerAndMove(town, numberInPrison);

                WriteOutInfo(town, numberInPrison);

                Thread.Sleep(500);
                Console.Clear();
            }
        }
        private static void PrintOutPersons(List<Person> town)
        {
            foreach (var person in town)
            {
                if (!person.inPrison)
                {
                    Console.SetCursorPosition(person.position.x, person.position.y);
                    Console.WriteLine(person.name);
                }

            }
        }
        private static void Collision(List<Person>town , int numberInPrison, Random random)
        {
            
            foreach (var personOne in town)
            {
                foreach (var personTwo in town)
                {
                    if (!personOne.Equals(personTwo))
                    {
                        if (personOne.position.x == personTwo.position.x && personOne.position.y == personTwo.position.y)
                        {
                            if (personOne is Thief && personTwo is Citizen)
                            {
                                var copyOfInventory = personTwo.CopyInventory();

                                if (copyOfInventory.Count > 0)
                                {
                                    
                                    var stolenItem = copyOfInventory[random.Next(0, copyOfInventory.Count)];
                                    personOne.AddItem(stolenItem);
                                    personTwo.RemoveItem(stolenItem);
                                    Console.SetCursorPosition(0, 27);
                                    Console.WriteLine("Thief stole item from citizen!");
                                    Thread.Sleep(1000);

                                }
                            }
                            else if (personOne is Police && personTwo is Thief)
                            {
                                var copyOfInventory = personTwo.CopyInventory();

                                if (copyOfInventory.Count > 0)
                                {
                                    for (int i = 0; i < copyOfInventory.Count; i++)
                                    {
                                        personOne.AddItem(copyOfInventory[i]);
                                    }
                                    for (int i = 0; i < copyOfInventory.Count; i++)
                                    {
                                        personTwo.RemoveItem(copyOfInventory[i]);
                                    }

                                    personTwo.inPrison = true;
                                    Console.SetCursorPosition(0, 27);
                                    Console.WriteLine("Police put a thief in to prison!");
                                    Thread.Sleep(1000);

                                }
                            }
                        }
                    }
                }
            }
        }
        private static void CountPrisonTime(List<Person>town, int numberInPrison)
        {
            foreach (var personInPrison in town)
            {
                var personPrisonTime = personInPrison.GetTimeInPrison();

                if (personInPrison.inPrison)
                {
                    if (personPrisonTime < 30)
                    {
                        Console.SetCursorPosition(0, 27);
                        Console.WriteLine($"Time in prison {personPrisonTime} seconds.");
                        personInPrison.AddTimeInPrison();
                    }
                    else
                    {
                        Console.SetCursorPosition(0, 27);
                        Console.WriteLine($"Thief are now free after {personPrisonTime} seconds in prison.");
                        personInPrison.inPrison = false;
                        personInPrison.ResetTimeInPrison();
                        Thread.Sleep(700);
                    }
                }
            }
        }
        private static int CountPrisonerAndMove(List<Person> town, int numberInPrison)
        {
            numberInPrison = 0;
            foreach (var person in town)
            {
                if (!person.inPrison)
                {
                    MovePerson(person);
                }
                else if (person.inPrison)
                {
                    numberInPrison++;
                }
            }
            return numberInPrison;
        }
        private static void WriteOutInfo(List<Person> town, int numberInPrison)
        {
            int citItems = 0;
            int thiefItems = 0;
            int policeItems = 0;

            foreach (var person in town)
            {
                if (person is Citizen)
                {
                    citItems += person.GetListSize();
                }
                else if (person is Thief)
                {
                    thiefItems += person.GetListSize();
                }
                else if (person is Police)
                {
                    policeItems += person.GetListSize();
                }
            }

            Console.SetCursorPosition(0, 28);
            Console.WriteLine($"Prisoners:{numberInPrison}");
            Console.WriteLine($"Citizen number of items:{citItems}");
            Console.WriteLine($"Thief number of items:{thiefItems}");
            Console.WriteLine($"Police number of items:{policeItems}");
        }
        private static List<Person> CreateTown(Random random)
        {
            Console.SetCursorPosition(17,15);
            Console.WriteLine("______________HELLO MAYOR, AND WELCOME HOME TO DOTNETVILLE______________");
            Thread.Sleep(4700);
            Console.SetCursorPosition(17, 15);
            Console.WriteLine("_______BEFORE WE START THE SIMULATION WE NEED TO ADD SOM PERSONS!_______");
            Thread.Sleep(4700);
            Console.SetCursorPosition(17, 15);
            Console.WriteLine("____LETS START WITH SOME CITIZEN, TYPE IN HOW MANY CITIZEN YOU WANT!____");
            int numberOfCitizen = int.Parse(Console.ReadLine());
            Console.Clear();
            Console.SetCursorPosition(17, 15);
            Console.WriteLine("________AND FOR LAW AND ORDER, TYPE IN HOW MANY POLICES YOU WANT________");
            int numberOfPolice = int.Parse(Console.ReadLine());
            Console.Clear();
            Console.SetCursorPosition(17, 15);
            Console.WriteLine("_______THE POLICES NEED TO WORK, TYPE IN HOW MANY THIEFS YOU WANT_______");
            int numberOfThief = int.Parse(Console.ReadLine());
            Console.Clear();
            Console.SetCursorPosition(17, 15);
            Console.WriteLine("________NOW WE ARE READY TO START THE SIMULATION. PRESS ANY KEY!________");
            Console.ReadKey(true);
            Console.Clear();

            List<Person> town = new List<Person>();

            for (int i = 0; i < numberOfCitizen; i++)
            {
                town.Add(new Citizen("C", StarterPosition(random), GiveDirection(random)));
            }

            for (int i = 0; i < numberOfPolice; i++)
            {
                town.Add(new Police("P", StarterPosition(random), GiveDirection(random)));
            }

            for (int i = 0; i < numberOfThief; i++)
            {
                town.Add(new Thief("T", StarterPosition(random), GiveDirection(random)));
            }

            return town;
        }
        private static Position StarterPosition(Random random)
        {
            return new Position(random.Next(0, 101), random.Next(0, 26));
        }
        private static Direction GiveDirection(Random random)
        {
            int rndX;
            int rndY;
            do
            {
                rndX = random.Next(-1, 2);
                rndY = random.Next(-1, 2);
            }
            while (rndX == 0 && rndY == 0);
            return new Direction(rndX, rndY);
        }
        private static void MovePerson(Person person)
        {
            person.position.x += person.direction.x;
            person.position.y += person.direction.y;

            if(person.position.x < 0)
            {
                person.position.x = 100;
            }
            else if(person.position.x >100)
            {
                person.position.x = 0;
            }

            if (person.position.y < 0)
            {
                person.position.y = 25;
            }
            else if (person.position.y > 25)
            {
                person.position.y = 0;
            }
        }
    }
    class Person
    {
        public string name { get; set; }
        public Position position { get; set; }
        public Direction direction { get; set; }
        public bool inPrison { get; set; }

        public Person(string name, Position Pxy, Direction Dxy)
        {
            this.name = name;
            position = Pxy;
            direction = Dxy;
            inPrison = false;
        }
        public virtual List<Item> CopyInventory()
        {
            return null;
        }
        public virtual void AddItem(Item item)
        {

        }
        public virtual void RemoveItem(Item item)
        {

        }
        public virtual int GetListSize()
        {
            return 0;
        }
        public virtual int GetTimeInPrison()
        {
            return 0;
        }
        public virtual void AddTimeInPrison()
        {

        }
        public virtual void ResetTimeInPrison()
        {

        }
    }
    class Citizen : Person
    {
        public List<Item> inventory { get; set; }

        public Citizen(string name, Position Pxy, Direction Dxy) : base(name, Pxy, Dxy)
        {
            List<Item> StarterPack = new List<Item>();
            StarterPack.Add(new Item("Keys"));
            StarterPack.Add(new Item("Phone"));
            StarterPack.Add(new Item("Watch"));
            StarterPack.Add(new Item("Cash"));
            StarterPack.Add(new Item("Wallet"));

            inventory = StarterPack;
        }
        public override List<Item> CopyInventory()
        {
            return new List<Item>(inventory);
        }
        public override void AddItem(Item item)
        {
            inventory.Add(item);
        }
        public override void RemoveItem(Item item)
        {
            inventory.Remove(item);
        }
        public override int GetListSize()
        {
            return inventory.Count;
        }
    }
    class Police : Person
    {
        public List<Item> confiscated { get; set; }

        public Police(string name, Position Pxy, Direction Dxy) : base(name, Pxy, Dxy)
        {
            confiscated = new List<Item>();
        }
        public override void AddItem(Item item)
        {
            confiscated.Add(item);
        }
        public override void RemoveItem(Item item)
        {
            confiscated.Remove(item);
        }
        public override int GetListSize()
        {
            return confiscated.Count;
        }
    }
    class Thief : Person
    {
        public List<Item> loot { get; set; }
        public int timeInPrison { get; set; }

        public Thief(string name, Position Pxy, Direction Dxy) : base(name, Pxy, Dxy)
        {
            loot = new List<Item>();
            timeInPrison = 0;
        }
        public override List<Item> CopyInventory()
        {
            return new List<Item>(loot);
        }
        public override void AddItem(Item item)
        {
            loot.Add(item);
        }
        public override void RemoveItem(Item item)
        {
            loot.Remove(item);
        }
        public override int GetListSize()
        {
            return loot.Count;
        }
        public override int GetTimeInPrison()
        {
            return timeInPrison;
        }
        public override void AddTimeInPrison()
        {
            timeInPrison++;
        }
        public override void ResetTimeInPrison()
        {
            timeInPrison = 0;
        }
    }
    class Item
    {
        public string name { get; set; }
        public Item(string name)
        {
            this.name = name;
        }
    }
    class Position
    {
        public int x { get; set; }
        public int y { get; set; }

        public Position (int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    class Direction
    {
        public int x { get; set; }
        public int y { get; set; }

        public Direction(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
