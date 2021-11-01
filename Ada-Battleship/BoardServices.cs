﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    public class BoardServices
    {

        private readonly int _boardWidth = Setup.Instance.BoardWidth;
        private readonly int _boardHeight = Setup.Instance.BoardHeight;
        private readonly List<Ship> _fleet = Setup.Instance.ShipDetailsP1;
        public readonly List<Tile> Tiles = new List<Tile>();
        //to place the ship
        //using tuple to be able to split the move in one function
        public (char, int, char) SplitMove(string userInput)
        {
            //needs to cater for A10
            string input = userInput.Trim().ToUpper();
            var columnLabel = input[0];
            var rowNumber = 0;
            var orientation = 'H';

            string[] digits = Regex.Split(userInput, @"\D+");
            foreach (var s in digits)
            {
                if (int.TryParse(s, out int number))
                {
                    rowNumber = number;
                }
            }

            if (Regex.Match(input, @"V$").Success)
            {
                orientation = 'V';
            }


            return (columnLabel, rowNumber, orientation);
        }


        public int AlphabetToInt(char columnLabel)
        {
            int alphabetToNum = columnLabel - 64;
            return alphabetToNum;
        }

        

        //combine the generators into one and use a tuple
        public int RandomlyGenerateColumnNumber(List<Coordinate> shipCoordinates)
        {
            var rand = new Random();
            var isValid = false;
            var columnNumber = rand.Next(1, _boardWidth);
            var alreadyTaken = new List<int>();

            while (isValid == false)
            {
                if (!alreadyTaken.Contains(columnNumber))
                {
                    
                    
                    isValid = true;
                }
                else
                {
                    alreadyTaken.Add(columnNumber);
                }
                
            }

            return columnNumber;

        }

        public int RandomlyGenerateRowNumber(List<Coordinate> shipCoordinates)
        {
            var rand = new Random();
            var rowNumber = rand.Next(1, _boardHeight);
            
            var isValid = false;
            var alreadyTaken = new List<int>();

            while (isValid == false)
            {
                if (!alreadyTaken.Contains(rowNumber))
                {
                    
                    isValid = true;
                }
                else
                {
                    alreadyTaken.Add(rowNumber);
                }
               
            }

            return rowNumber;
        }

        public (int, int) RandomlyGenerateCoordinates(List<Coordinate> shipCoordinates)
        {
            var rand = new Random();
            var rowNumber = rand.Next(1, _boardHeight);
            var columnNumber = rand.Next(1, _boardHeight);
            var isValid = false;
            var listOfCoordinates = shipCoordinates;
            

            while (isValid == false && listOfCoordinates.Count != 0)
            {
                if (!listOfCoordinates.Exists(c => c.X == rowNumber && c.Y == columnNumber))
                {
                    isValid = true;
                    
                }
            }

            return (rowNumber, columnNumber);
        }
        private void ResetShip()
        {
            foreach (var ship in _fleet)
            {
                foreach (var coordinate in ship.ShipCoordinate)
                {
                    coordinate.X = 0;
                    coordinate.Y = 0;
                }


                ship.Status = ShipStatus.Pending;

            }
        }

        public void UpdateShipStatus(string name, string status, IPlayer currentPlayer)
        {
            foreach (var ship in currentPlayer.PlayerFleet)
            {
                if (ship.ShipName == name)
                {
                    switch (status)
                    {
                        case "hit":
                            ship.Status = ShipStatus.Hit;
                            break;
                        default:
                            ship.Status = ShipStatus.Placed;
                            break;
                    }

                }

            }
        }

        public void ResetBoard()
        {
            ResetShip();
            foreach (var tile in Tiles)
            {
                tile.TilePlaceholder = '.';
            }

        }
    }
}