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
        public readonly List<Tile> Tiles = new List<Tile>();
        private readonly List<Ship> _fleet = Setup.Instance.ShipDetails;


        public void AddTile()
        {
            for (int i = 1; i <= _boardWidth; i++)
            {
                for (int j = 1; j <= _boardHeight; j++)
                {
                    Tiles.Add(new Tile(i, j));
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
                    if (Tiles[j + counter].TilePlaceholder == 's')
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write("\t" + Tiles[j + counter].TilePlaceholder);
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write("\t" + Tiles[j + counter].TilePlaceholder);
                        //Console.Write("\t" + _tiles[j + counter].Coordinate.X );
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

                    ship.Status = ShipStatus.Placed;

                }

            }
        }

        public void AddShipCoordinates(string shipName, int x, int y)
        {
            foreach (var ship in _fleet)
            {
                if (ship.ShipName == shipName)
                {
                    ship.ShipCoordinate.Add(new Coordinate(x, y));

                }

            }
            
        }
        public void PlaceShip(string shipName,int x, int y,char orientation)
        {
            //length and direction

            int shipLength = GetShipLength(shipName);

            if (_boardWidth < shipLength + y || _boardHeight < shipLength + x)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Ship length is greater than board width.");
                Console.ResetColor();
            }
            else
            {

                foreach (var tile in Tiles)
                {
                    for (int i = 0; i < shipLength; i++)
                    {
                        if (orientation == 'H')
                        {
                            if (tile.Coordinate.X == x && tile.Coordinate.Y == y + i)
                            {
                                tile.TilePlaceholder = 's';
                                AddShipCoordinates(shipName, x, y+i);

                            }
                        }
                        if(orientation == 'V')
                        {
                            if ( tile.Coordinate.X == x + i && tile.Coordinate.Y == y )
                            {
                                tile.TilePlaceholder = 's';
                                AddShipCoordinates(shipName, x+i, y);

                            }
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



        public bool ValidateMove(int columnNumber, int rowNumber)
        {
            var isValid = false;

            foreach (var tile in Tiles)
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
                foreach (var coordinate in ship.ShipCoordinate)
                {
                    coordinate.X = 0;
                    coordinate.Y = 0;
                }
                
                
                ship.Status = ShipStatus.Pending;

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