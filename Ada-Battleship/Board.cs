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
        private readonly List<char> _columnLabels = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private readonly List<Tile> _tiles = new List<Tile>();
        

        private void AddTile()
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
            AddTile();

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
                    Console.Write("\t"+_tiles[j+counter].TilePlaceholder);
                }

                counter += _boardWidth;

                Console.Write("\n");

            }
        }
        public void PlaceShip(string shipName,int x, int y)
        {
            //length and direction
            var fleet = Setup.Instance.ShipDetails;
            int shipLength = 0;
            foreach (var ship in fleet)
            {
                if (ship.ShipName == shipName)
                {
                    shipLength = ship.ShipLength;
                }
                
            }
            
            foreach (var tile in _tiles)
            {
                for (int i = 0; i < shipLength; i++)
                {
                    if (tile.Coordinate.X == x && tile.Coordinate.Y == y+i)
                    {
                        tile.TilePlaceholder = 's';

                    }

                }
               
            }


        }
        //using tuple to be able to split the move in one function
        public (char, int) SplitMove(string userInput)
        {
            string input = userInput.Trim().ToUpper();
            var columnlabel = input[0];
            var rowNumber = input[1] - '0';

            return (columnlabel, rowNumber);
        }

        //public bool ValidateMove(string coordinates)
        //{
        //    var isValid = false;
        //    if(_tiles.Contains())
        //}


    }
}