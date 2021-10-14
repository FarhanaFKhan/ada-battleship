using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    public class Board
    {
        private readonly int _boardWidth = Setup.Instance.BoardWidth;
        private readonly int _boardHeight = Setup.Instance.BoardHeight;
        private readonly List<char> _columnLabels = new List<char>(){'A','B','C','D','E','F','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        private readonly List<Tile> _tiles = new List<Tile>();

        private void AddTile()
        {
            for (int i = 1; i <= _boardWidth; i++)
            {
                for (int j = 1; j <= _boardHeight; j++)
                {
                    _tiles.Add(new Tile(i,j));
                }
                
            }
        }
        public void DisplayBoard()
        {
            AddTile();
            _tiles[3].TileStatus = TileStatus.Hit;
            _tiles[10].TileStatus = TileStatus.Ship;
            for (int i = 0; i <= _boardWidth; i++)
            {
                Console.Write("\t" +_columnLabels[i]);

            }

            Console.WriteLine();
            
            for (int i =1 ; i <= _boardHeight; i++)
            {
                Console.WriteLine(i + "\n");
                
            }
            
            foreach (var tile in _tiles)
            {
                if (tile.TileStatus == TileStatus.Hit)
                {
                    Console.Write("x"+"\t");
                }

                if (tile.TileStatus == TileStatus.Ship)
                {
                    Console.Write("s"+"\t");
                }

                if (tile.TileStatus == TileStatus.Available)
                {
                    Console.Write("."+"\t");
                }
                //Console.Write("." + "\t");
                //Console.Write(tile.Coordinate.X +","+ tile.Coordinate.Y +"\t");
                if (tile.Coordinate.Y % _boardHeight == 0)
                {
                    Console.Write("\n");
                }

                
            }


        }


    }
}