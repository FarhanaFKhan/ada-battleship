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
        private readonly List<char> _columnLabels = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        public readonly List<Tile> Tiles = new List<Tile>();
        private readonly List<Ship> _fleet = Setup.Instance.ShipDetailsP1;

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
            //Console.Clear();

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
                        Console.ResetColor();
                    }
                    else if (Tiles[j + counter].TilePlaceholder == 'H')
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write("\t" + Tiles[j + counter].TilePlaceholder);
                        Console.ResetColor();
                    }
                    else if (Tiles[j + counter].TilePlaceholder == 'M')
                    {
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        Console.Write("\t" + Tiles[j + counter].TilePlaceholder);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write("\t" + Tiles[j + counter].TilePlaceholder);
                        //Console.Write("\t" + Tiles[j + counter].Coordinate.X +","+ Tiles[j + counter].Coordinate.Y);
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
        public void AddShipCoordinates(string shipName, int x, int y, IPlayer currentPlayer)
        {
            foreach (var ship in currentPlayer.PlayerFleet)
            {
                if (ship.ShipName == shipName)
                {
                    //ship.ShipCoordinate.Add(new Coordinate(x, y));
                    ship.SetCoordinates(x, y);

                }

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

        public void PlaceShip(string shipName, int x, int y, char orientation, IPlayer currentPlayer)
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
                                AddShipCoordinates(shipName, x, y + i,currentPlayer);

                            }
                        }
                        if (orientation == 'V')
                        {
                            if (tile.Coordinate.X == x + i && tile.Coordinate.Y == y)
                            {
                                tile.TilePlaceholder = 's';
                                AddShipCoordinates(shipName, x + i, y,currentPlayer);

                            }
                        }


                    }

                }
                UpdateShipStatus(shipName, "placed", currentPlayer);
            }


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






    }
}