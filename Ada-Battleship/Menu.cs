﻿using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;
namespace Ada_Battleship
{
    public class Menu
    {
        private readonly int _boardWidth = Setup.Instance.BoardWidth;
        private readonly int _boardHeight = Setup.Instance.BoardHeight;
        private readonly List<Ship> _shipInfo = Setup.Instance.ShipDetails;
        private readonly Board _gameBoard = new Board();
        private readonly BoardServices _boardServices = new BoardServices();
        private readonly MenuServices _menuServices = new MenuServices();
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
            _gameBoard.AddTile();
            _gameBoard.DisplayBoard();
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
            
            int pvCMenuOption;
            do
            {
                Console.WriteLine();
                Console.WriteLine("You are now in PvC mode");
                Console.WriteLine();
                DisplayAvailableShips();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("1.Place ship manually.");
                Console.WriteLine("2.Place all ships randomly.");
                Console.WriteLine("3.Place remaining ships randomly.");
                Console.WriteLine("4.Reset board.");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("5.Quit.");
                Console.ResetColor();

                
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
                        PvCMenuOptionThree();
                        break;
                    case 4:
                        PvCOptionFour();
                        break;
                    case 5:
                        QuitGame();
                        break;
                    default:
                        PvCMenuOptionOne();
                        break;
                }
            } while (pvCMenuOption != 5);


        }

        public void PvCMenuOptionOne()
        {
            //place ship manually
            _gameBoard.DisplayBoard();
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
            //var splitMove = gameBoard.SplitMove(userInput);
            var splitMove = _boardServices.SplitMove(userInput);

            var columnLabel = splitMove.Item1;
            var rowNumber = splitMove.Item2;
            var orientation = splitMove.Item3;
            var columnNumber = _gameBoard.AlphabetToInt(columnLabel);

            //validator for input
            var isValid = _gameBoard.ValidateMove(columnNumber, rowNumber);
            var isOverlap = CheckForShipOverlap(rowNumber, columnNumber);
            if (isValid && !isOverlap)
            {
                _gameBoard.PlaceShip(shipName, rowNumber, columnNumber,orientation);
                _gameBoard.DisplayBoard();
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
            //Place all ships randomly.
            _gameBoard.ResetBoard();

            foreach (var ship in _shipInfo)
            {
                var shipLength = ship.ShipLength;
                int columnNumber = 1;
                int rowNumber = 1;
                var isValid = false;
                var orientation = 'H';
                //this block will keep generating a random number unless its a valid move
                while (isValid == false)
                {
                    columnNumber = _gameBoard.RandomlyGenerateColumnNumber();
                    rowNumber = _gameBoard.RandomlyGenerateRowNumber();
                    var isOverlap = CheckForShipOverlap(rowNumber, columnNumber);
                    if (_boardWidth > columnNumber + shipLength && !isOverlap)
                    {
                        isValid = true;
                    }

                }

                _gameBoard.PlaceShip(ship.ShipName, rowNumber, columnNumber,orientation);
                Console.WriteLine();
                DisplayAvailableShips();
            }
            _gameBoard.DisplayBoard();

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
                if (ship.ShipCoordinateY == y)
                {
                    if (x > 1 && x <= (ship.ShipCoordinateX + ship.ShipLength))
                    {
                        isOverlap = true;
                    }

                }

            }
            return isOverlap;
        }

        public void PvCMenuOptionThree()
        {
           // Place remaining ships randomly.
            var availableShips = GetAvailableShips();
            foreach (var ship in availableShips)
            {
                var shipLength = ship.ShipLength;
                var isValid = false;
                

                //this block will keep generating a random number unless its a valid move
                while (isValid == false)
                {
                   var columnNumber = _gameBoard.RandomlyGenerateColumnNumber();
                   var rowNumber = _gameBoard.RandomlyGenerateRowNumber();
                   var orientation = _menuServices.ToggleOrientation();
                    var isOverlap = CheckForShipOverlap(rowNumber, columnNumber);
                    if ((_boardWidth > columnNumber + shipLength) && (_boardHeight > rowNumber + shipLength) && !isOverlap)
                    {
                        isValid = true;
                        _gameBoard.PlaceShip(ship.ShipName, rowNumber, columnNumber, orientation);
                    }

                }

                Console.WriteLine();
                DisplayAvailableShips();

            }
            _gameBoard.DisplayBoard();

        }

        private void PvCOptionFour()
        {
            //reset board
           _gameBoard.ResetBoard();
           Console.ForegroundColor = ConsoleColor.Green;
           Console.WriteLine("The board has been reset.");
           Console.ResetColor();
           Console.WriteLine();
           _gameBoard.DisplayBoard();
        }

        private void QuitGame()
        { 
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("You quit game!");
            Console.ResetColor();

        }

    }
}