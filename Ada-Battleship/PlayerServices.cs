using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Ada_Battleship
{
    public class PlayerServices
    {
        private readonly BoardServices _boardServices = new BoardServices();
        //logic for shooting a torpedo
        public void ShootTorpedo(int x, int y, IPlayer attacker, IPlayer defender)
        {
            var defenderBoardTiles = defender.GameBoard.Tiles;
            var opponentShipInfo = defender.PlayerFleet;
            var attackerShotBoardTiles = attacker.ShotBoard.Tiles;
            foreach (var attackerShotBoardTile in attackerShotBoardTiles)
            {
                foreach (var tile in defenderBoardTiles)
                {//need to update tileStatus to ship as well when changing placeholder 
                    if (tile.Coordinate.X == x && tile.Coordinate.Y == y && tile.TilePlaceholder == 's' && attackerShotBoardTile.Coordinate.X == x && attackerShotBoardTile.Coordinate.Y == y)
                    {
                        tile.TileStatus = TileStatus.Hit;
                        tile.TilePlaceholder = 'H';
                        attackerShotBoardTile.TileStatus = TileStatus.Hit;
                        attackerShotBoardTile.TilePlaceholder = 'H';
                        
                        foreach (var ship in opponentShipInfo)
                        {
                            
                            var coords = ship.ShipCoordinate.Where(c => c.X == x && c.Y == y).ToList();
                            var coordExists = coords.Any(c=>c.X==x) && ship.ShipCoordinate.Any(c=>c.Y == y);

                            if (coordExists)
                            {
                                var shipName = ship.ShipName;
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine($"{shipName} damaged from {defender.Name}'s fleet");
                                Console.ResetColor();
                                Console.WriteLine();
                                if (ship.Health != 0)
                                {
                                    ship.Health--;
                                }

                                if (ship.Health == 0)
                                {
                                    _boardServices.UpdateShipStatus(shipName,"hit",defender);
                                }

                            }

                        }

                    }
                    if (tile.Coordinate.X == x && tile.Coordinate.Y == y && tile.TilePlaceholder == '.' && attackerShotBoardTile.Coordinate.X == x && attackerShotBoardTile.Coordinate.Y == y)
                    {
                        tile.TileStatus = TileStatus.Miss;
                        tile.TilePlaceholder = 'M';
                        attackerShotBoardTile.TileStatus = TileStatus.Miss;
                        attackerShotBoardTile.TilePlaceholder = 'M';
                    }
                }
            }


        }
        //keep track of player turns

    }
}