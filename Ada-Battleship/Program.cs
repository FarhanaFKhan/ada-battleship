using System;
using System.Threading.Channels;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            //launcher
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

            string userInput;
            (char, int) splitMove;
            char columnLabel;
            int rowNumber;

            //menu
            Console.WriteLine("Please enter a point to position a ship:");
            userInput = Console.ReadLine();
            Console.WriteLine(userInput);
            //separate string
            //convert into string and int
            splitMove =  gameBoard.SplitMove(userInput);
            Console.WriteLine($"ColumnLabel: {splitMove.Item1}");
            Console.WriteLine($"rowNumber: {splitMove.Item2}");
            columnLabel = splitMove.Item1;
            rowNumber = splitMove.Item2;
            //validator for input
            gameBoard.PlaceShip("Carrier",columnLabel,rowNumber);
            //Console.Clear(); //await or state?
            gameBoard.DisplayBoard();
            //gameBoard.PlaceShip("Battleship", 2, 2);
            //gameBoard.DisplayBoard();
           // gameBoard.PlaceShip("Patrol Boat", 1, 9);
           // gameBoard.DisplayBoard();
           // gameBoard.PlaceShip("Destroyer", 6, 3);
           // gameBoard.DisplayBoard();
            //gameBoard.PlaceShip("Submarine", 9, 7);
            //gameBoard.DisplayBoard();

            //another instance of board for 'shot' board
            //it will check if the tile's placeholder is 's' then it will mark it as a miss 'x'
        }
    }
}
