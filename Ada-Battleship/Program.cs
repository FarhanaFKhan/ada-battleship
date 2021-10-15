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
            gameBoard.PlaceShip(3,9);
            gameBoard.DisplayBoard();
        }
    }
}
