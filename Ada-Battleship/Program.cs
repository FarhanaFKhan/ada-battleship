using System;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            Setup config = Setup.Instance;
            Console.WriteLine(config.BoardHeight);
            Console.WriteLine(config.BoardWidth);
            var shipInfo = config.ShipDetails;

            foreach (var ship in shipInfo)
            {
                Console.WriteLine("Name: "+ship.ShipName + " ,Length:" + ship.ShipLength +" ,Health: "+ship.Health + " ,Status:" + ship.Status);
            }

            Board gameBoard = new Board();
            gameBoard.DisplayBoard();
            gameBoard.PlaceShip("Carrier",5,5);
            //Console.Clear(); //await or state?
            gameBoard.DisplayBoard();
            gameBoard.PlaceShip("Battleship", 2, 2);
            gameBoard.DisplayBoard();
            gameBoard.PlaceShip("Patrol Boat", 1, 9);
            gameBoard.DisplayBoard();
            gameBoard.PlaceShip("Destroyer", 6, 3);
            gameBoard.DisplayBoard();
            gameBoard.PlaceShip("Submarine", 9, 7);
            gameBoard.DisplayBoard();
        }
    }
}
