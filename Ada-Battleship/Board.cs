using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Security.Cryptography.X509Certificates;
using Ada_Battleship.Configurations;
using Microsoft.VisualBasic.CompilerServices;

namespace Ada_Battleship
{
    public class Board
    {
        private readonly int _boardWidth = Setup.Instance.BoardWidth;
        private readonly int _boardHeight = Setup.Instance.BoardHeight;
        private readonly List<char> _columnLabels = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G','H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private readonly List<Tile> _tiles = new List<Tile>();
        private readonly List<Ship> _fleet = Setup.Instance.ShipDetails;


        public void AddTile()
        {
            for (int i = 1; i <= _boardWidth; i++)
            {
                for (int j = 1; j <= _boardHeight; j++)
                {
                    _tiles.Add(new Tile(i, j));
                }

            }
        }
        public void DisplayBoard()
        {
           // Console.Clear();
           // AddTile();

            for (var i = 0; i < _boardWidth; i++)
            {
                Console.Write("\t" + _columnLabels[i]);

            }

            Console.WriteLine();

            var counter = 0;

            for (var i = 1; i <= _boardHeight; i++)
            {
                Console.Write(i);
                

                for (var j = 0; j < _boardWidth; j++)
                {
                    if (_tiles[j + counter].TilePlaceholder == 's')
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write("\t" + _tiles[j + counter].TilePlaceholder);
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write("\t" + _tiles[j + counter].TilePlaceholder);
                    }
                    
                }

                counter += _boardWidth;

                Console.Write("\n");

            }
        }

        public int GetShipLength(string name)
        {
            int shipLength = 0;
            foreach (var ship in _fleet)
            {
                if (ship.ShipName == name)
                {
                    shipLength = ship.ShipLength;
                    
                }

            }

            return shipLength;
        }

        public void UpdateShipStatus(string name,int x,int y)
        {
            foreach (var ship in _fleet)
            {
                if (ship.ShipName == name)
                {
                    ship.ShipCoordinateX = x;
                    ship.ShipCoordinateY = y;
                    ship.Status = ShipStatus.Placed;

                }

            }
        }
        public void PlaceShip(string shipName,int x, int y)
        {
            //length and direction
            //how to avoid overlap of ships?

            int shipLength = GetShipLength(shipName);

            if (_boardWidth < shipLength + y)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Ship length is greater than board width. Try placing vertically");
                Console.ResetColor();
            }
            else
            {
               
                foreach (var tile in _tiles)
                {
                    for (int i = 0; i < shipLength; i++)
                    {
                        if (tile.Coordinate.X == x && tile.Coordinate.Y == y + i)
                        {
                            tile.TilePlaceholder = 's';

                        }

                    }

                }
                UpdateShipStatus(shipName,x,y);
            }
            

        }

       

        
        public int RandomlyGenerateColumnNumber()
        {
            var rand = new Random();
            var columnNumber = rand.Next(1, _boardWidth);
            return columnNumber;

        }

        public int RandomlyGenerateRowNumber()
        {
            var rand = new Random();
            var rowNumber = rand.Next(1, _boardHeight);
            return rowNumber;

        }


        //using tuple to be able to split the move in one function
        public (char, int) SplitMove(string userInput)
        {
            //needs to cater for A10
            string input = userInput.Trim().ToUpper();
            var columnlabel = input[0];
            var rowNumber = input[1] - '0';

            return (columnlabel, rowNumber);
        }

        public int AlphabetToInt(char columnLabel)
        {
            int alphabetToNum = columnLabel - 64;
            return alphabetToNum;
        }

        public bool ValidateMove(int columnNumber, int rowNumber)
        {
            var isValid = false;

            foreach (var tile in _tiles)
            {
                if (tile.Coordinate.X == columnNumber && tile.Coordinate.Y == rowNumber)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private void ResetShip()
        {
            foreach (var ship in _fleet)
            {
                ship.ShipCoordinateX = 0;
                ship.ShipCoordinateY = 0;
                ship.Status = ShipStatus.Pending;

            }
        }

        public void ResetBoard()
        {
            ResetShip();
            foreach (var tile in _tiles)
            {
                tile.TilePlaceholder = '.';
            }
            
        }


    }
}