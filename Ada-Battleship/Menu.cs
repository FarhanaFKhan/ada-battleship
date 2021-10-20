using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;
namespace Ada_Battleship
{
    public class Menu
    {
        private readonly int _boardWidth = Setup.Instance.BoardWidth;
        private readonly int _boardHeight = Setup.Instance.BoardHeight;
        private readonly List<Ship> _shipInfo = Setup.Instance.ShipDetails;
        private readonly Board gameBoard = new Board();
        
        string playerName;
        private int _mainOption;


        public void MainMenu()
        {
            
            //this will be stored in player class
            Console.WriteLine("Please enter your name:");
            playerName = Console.ReadLine();

            Console.WriteLine($"Greetings {playerName}! You have the following settings:");
            Console.WriteLine();

            Console.WriteLine($"Board Dimensions:{_boardWidth}x{_boardHeight}");
            Console.WriteLine();
            gameBoard.DisplayBoard();
            Console.WriteLine();
            DisplayAvailableShips();
            Console.WriteLine();
            Console.WriteLine("Select Game Mode:");
            Console.WriteLine("1.Player v Comp.");
            Console.WriteLine("2.Quit.");
            try
            {
                _mainOption = int.Parse(Console.ReadLine());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw new InvalidOperationException();
            }
            

            switch (_mainOption)
            {
                case 2:
                    Console.WriteLine("Quit Game");
                    break;
                default:
                    PvCMenu();
                    break;
            }

        }

        public void PvCMenu()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("You are now in PvC mode");
            Console.WriteLine();
            DisplayAvailableShips();
            Console.WriteLine();
            Console.WriteLine("1.Place ship manually.");
            Console.WriteLine("2.Place all ships randomly.");
            Console.WriteLine("3.Place remaining ships randomly.");
            Console.WriteLine("4.Reset board.");

            int pvCMenuOption;
            try
            {
                pvCMenuOption = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new InvalidOperationException();
            }
            switch (pvCMenuOption)
            {
                case 2:
                    PvCMenuOptionTwo();
                    break;
                case 3:
                    Console.WriteLine("Place available ships randomly.");
                    break;
                case 4:
                    Console.WriteLine("Reset board.");
                    break;
                default:
                    PvCMenuOptionOne();
                    break;
            }
        }

        public void PvCMenuOptionOne()
        {
            Console.WriteLine("Please enter a point to position a ship:");
            var userInput = Console.ReadLine();
            Console.WriteLine(userInput);

            //separate string
            //convert into string and int
            var splitMove = gameBoard.SplitMove(userInput);

            //Console.WriteLine($"ColumnLabel: {splitMove.Item1}");
            //Console.WriteLine($"rowNumber: {splitMove.Item2}");
            var columnLabel = splitMove.Item1;
            var rowNumber = splitMove.Item2;

            var columnNumber = gameBoard.AlphabetToInt(columnLabel);

            //validator for input
            var isValid = gameBoard.ValidateMove(columnNumber, rowNumber);

            if (isValid)
            {
                gameBoard.PlaceShip("Carrier", rowNumber, columnNumber);
                gameBoard.DisplayBoard();
                DisplayAvailableShips();
            }
            else
            {
                Console.WriteLine("Please enter a valid move");
            }

        }

        public void PvCMenuOptionTwo()
        {
            var columnNumber = gameBoard.RandomlyGenerateColumnNumber();
            var rowNumber = gameBoard.RandomlyGenerateRowNumber();
            gameBoard.PlaceShip("Carrier", rowNumber, columnNumber);
            gameBoard.DisplayBoard();
            Console.WriteLine();
            DisplayAvailableShips();
        }

        private void DisplayAvailableShips()
        {
            Console.WriteLine();

            Console.WriteLine("Available Ships:");
            Console.WriteLine();
            Console.Write("\tName  \tLength  \tHealth  \tstatus");
            Console.WriteLine();

            foreach (var ship in _shipInfo)
            {
                Console.WriteLine("\t" + ship.ShipName + "\t" + ship.ShipLength + "\t " + ship.Health + "\t " + ship.Status);
            }
        }

    }
}