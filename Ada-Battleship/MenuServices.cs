﻿using System;

namespace Ada_Battleship
{
    public class MenuServices
    {
        private readonly BoardServices _boardServices = new BoardServices();
        private readonly PlayerServices _playerServices = new PlayerServices();
        public char ToggleOrientation()
        {
            var rand = new Random();
            var number = rand.Next(1, 3);
            var orientation = number == 1 ? 'V' : 'H';

            return orientation;
        }

        public void GamePlay(IPlayer attacker, IPlayer defender)
        {
            Console.WriteLine($"{attacker.Name} - Please enter coordinates(e.g A2):");
            if (attacker.Name != "Eva")
            {
                var userInput = Console.ReadLine();
                Console.WriteLine(userInput);
                var splitMove = _boardServices.SplitMove(userInput);
                var columnLabel = splitMove.Item1;
                var rowNumber = splitMove.Item2;
                var columnNumber = _boardServices.AlphabetToInt(columnLabel);
                var isValid = attacker.ShotBoard.ValidateMove(columnNumber, rowNumber);
                if (isValid)
                {
                    _playerServices.ShootTorpedo(rowNumber, columnNumber, attacker, defender);
                    attacker.ShotBoard.DisplayBoard();
                    DisplayAvailableShips(defender);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Please enter a valid move");
                    Console.ResetColor();
                }
            }

            else
            {
                var coordinateGenerator = _boardServices.RandomlyGenerateCoordinates(attacker.ShotBoard.Tiles);
                var rowNumber = coordinateGenerator.Item1;
                var columnNumber = coordinateGenerator.Item2;
                //var columnNumber = _boardServices.RandomlyGenerateColumnNumber();
                //var rowNumber = _boardServices.RandomlyGenerateRowNumber();
                
                bool isValid;
                do
                {
                    isValid = attacker.ShotBoard.ValidateMove(columnNumber, rowNumber);
                    _playerServices.ShootTorpedo(rowNumber, columnNumber, attacker, defender);
                    attacker.ShotBoard.DisplayBoard();
                    DisplayAvailableShips(defender);
                } while (!isValid);
            }

        }

        public void DisplayAvailableShips(IPlayer currentPlayer)
        {
            Console.WriteLine();

            Console.WriteLine($"{currentPlayer.Name} -- Ships Details:");
            Console.WriteLine();
            Console.Write("\tName  \tLength  \tHealth  \tstatus");
            Console.WriteLine();

            foreach (var ship in currentPlayer.PlayerFleet)
            {
                Console.WriteLine("\t" + ship.ShipName + "\t" + ship.ShipLength + "\t " + ship.Health + "\t " + ship.Status);
            }
        }

    }
}