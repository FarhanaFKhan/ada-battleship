using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Security.Cryptography.X509Certificates;
using Ada_Battleship.Configurations;

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
                    /*Console.Write("\t" + _tiles[j + counter].Coordinate.X + "," + _tiles[j + counter].Coordinate.Y)*/;
                    Console.Write("\t"+_tiles[j+counter].TilePlaceholder);
                }

                counter += _boardWidth;

                Console.Write("\n");

            }
        }
        public void PlaceShip(int x, int y)
        {
            foreach (var tile in _tiles)
            {
                if (tile.Coordinate.X == x && tile.Coordinate.Y == y)
                {
                    tile.TilePlaceholder = 's';
                }
            }


        }


    }
}