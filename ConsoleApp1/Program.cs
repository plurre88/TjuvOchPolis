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
            var town = CreateTown();

            while (true)
            {
                foreach (var person in town)
                {
                    Console.SetCursorPosition(person.position.x, person.position.y);
                    Console.WriteLine(person.name);
                    MovePerson(person);
                }
                Thread.Sleep(150);
                Console.Clear();
            }
        }
        private static List<Person> CreateTown()
        {
            List<Person> town = new List<Person>();

            town.Add(new Citizen("C",new Position(30,15),new Direction(-1,-1)));
            town.Add(new Citizen("C", new Position(10, 5), new Direction(-1, 0)));
            town.Add(new Police("P",new Position(5, 20),new Direction(1,1)));
            town.Add(new Police("P", new Position(69, 1), new Direction(1,0)));
            town.Add(new Thief("T",new Position(85,20),new Direction(1,-1)));
            town.Add(new Thief("T", new Position(4, 3), new Direction(1, -1)));

            return town;
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

        public Person(string name, Position Pxy, Direction Dxy)
        {
            this.name = name;
            position = Pxy;
            direction = Dxy;
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
    }
    class Police : Person
    {
        public List<Item> confiscated { get; set; }

        public Police(string name, Position Pxy, Direction Dxy) : base(name, Pxy, Dxy)
        {
            List<Item> confiscated = new List<Item>();
        }
    }
    class Thief : Person
    {
        public List<Item> loot { get; set; }

        public Thief(string name, Position Pxy, Direction Dxy) : base(name, Pxy, Dxy)
        {
            List<Item> loot = new List<Item>();
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