using System;
using System.Collections.Generic;
using System.Dynamic;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    public class Player: IPlayer
    {
        public string Name { get; set; }
        public int State { get; set; }
        public Board GameBoard { get; }
        public Board ShotBoard { get; }
        public List<Ship> PlayerFleet { get; set; }


        //score?

        public Player(string name)
        {
            State = 0;
            GameBoard = new Board();
            ShotBoard = new Board();
            GameBoard.AddTile();
            ShotBoard.AddTile();
            Name = name;
            Console.WriteLine($"{Name} has joined");
        }

        

    }
}