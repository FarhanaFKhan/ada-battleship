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
            gameBoard.AddTile();
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
            catch (Exception ex)
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
            var availableShips = GetAvailableShips();
            for (int i = 1; i <= availableShips.Count; i++) //should it availableShips.count?
            {
               // Console.Clear();
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


        }

        public void PvCMenuOptionOne()
        {
            gameBoard.DisplayBoard();
            var availableShips = GetAvailableShips();


            Console.WriteLine(availableShips.Count);

            ShipMenu();
            Console.WriteLine($"Available ships : {availableShips.Count}");
            Console.WriteLine();
            var shipOption = Console.ReadLine();
            var shipName = GetShipName(shipOption);

            Console.WriteLine($"You selected {shipName}");
            Console.WriteLine();
            Console.WriteLine("Please enter a point to position a ship:");
            var userInput = Console.ReadLine();
            Console.WriteLine(userInput);

            //separate string
            //convert into string and int
            var splitMove = gameBoard.SplitMove(userInput);

            var columnLabel = splitMove.Item1;
            var rowNumber = splitMove.Item2;

            var columnNumber = gameBoard.AlphabetToInt(columnLabel);

            //validator for input
            var isValid = gameBoard.ValidateMove(columnNumber, rowNumber);
            var isOverlap = CheckForShipOverlap(rowNumber, columnNumber);
            if (isValid && !isOverlap)
            {
                gameBoard.PlaceShip(shipName, rowNumber, columnNumber);
                gameBoard.DisplayBoard();
                DisplayAvailableShips();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Please enter a valid move");
                Console.ResetColor();
            }


        }

        public void PvCMenuOptionTwo()
        {
            var availableShips = GetAvailableShips();
            foreach (var ship in availableShips)
            {
                var shipLength = ship.ShipLength;
                int columnNumber = 1;
                int rowNumber = 1;
                var isValid = false;
                //this block will keep generating a random number unless its a valid move
                while (isValid == false)
                {
                    columnNumber = gameBoard.RandomlyGenerateColumnNumber();
                    rowNumber = gameBoard.RandomlyGenerateRowNumber();
                    var isOverlap = CheckForShipOverlap(rowNumber, columnNumber);
                    if (_boardWidth > columnNumber + shipLength && !isOverlap)
                    {
                        isValid = true;
                    }

                }

                gameBoard.PlaceShip(ship.ShipName, rowNumber, columnNumber);
                Console.WriteLine();
                DisplayAvailableShips();



            }
            gameBoard.DisplayBoard();

        }
        //needs to be extracted to another class
        private List<Ship> GetAvailableShips()
        {
            var listOfAvailableShips = new List<Ship>();

            for (int i = 0; i < _shipInfo.Count; i++)
            {
                if (_shipInfo[i].Status == ShipStatus.Pending)
                {
                    listOfAvailableShips.Add(_shipInfo[i]);
                }
            }

            return listOfAvailableShips;
        }
        private void ShipMenu()
        {
            Console.WriteLine();
            var availableShips = GetAvailableShips();
            Console.WriteLine("Please select an available ship:");
            for (int i = 0; i < availableShips.Count; i++)
            {
                Console.WriteLine("\t" + i + "." + availableShips[i].ShipName);

            }
        }

        private string GetShipName(string shipOption)
        {
            int shipNumber;
            var availableShips = GetAvailableShips();
            var shipName = availableShips[0].ShipName; //could be better

            try
            {
                shipNumber = int.Parse(shipOption);

                //return shipName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception();
            }

            for (int i = 0; i < availableShips.Count; i++)
            {
                if (i == shipNumber)
                {
                    shipName = availableShips[i].ShipName;
                }
            }

            return shipName;

        }

        private void DisplayAvailableShips()
        {
            Console.WriteLine();

            Console.WriteLine("Ships Details:");
            Console.WriteLine();
            Console.Write("\tName  \tLength  \tHealth  \tstatus");
            Console.WriteLine();

            foreach (var ship in _shipInfo)
            {
                Console.WriteLine("\t" + ship.ShipName + "\t" + ship.ShipLength + "\t " + ship.Health + "\t " + ship.Status);
            }
        }

        //check if the ship overlaps with already placed ship
        //if the coordX = x && coordY == y (return true) meaning there is an overlap
        //if the columnNumber is a point on the ship

        public bool CheckForShipOverlap(int x, int y)
        {
            var isOverlap = false;
            foreach (var ship in _shipInfo)
            {
                if (ship.ShipCoordinateX == x && ship.ShipCoordinateY == y)
                {
                    isOverlap = true; //there is overlap
                }

                if (ship.ShipCoordinateX == x)
                {
                    if (y > 1 && y <= (ship.ShipCoordinateY + ship.ShipLength))
                    {
                        isOverlap = true;
                    }
                    
                }


            }

            return isOverlap;
        }

    }
}