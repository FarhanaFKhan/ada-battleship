using System;
using System.Collections.Generic;
using System.Linq;

namespace Ada_Battleship
{
    public class PlayerServices
    {
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
                            foreach (var coord in ship.ShipCoordinate)
                            {
                                Console.WriteLine($"{defender.Name}--{ship.ShipName}--X:{coord.X},Y:{coord.Y}");
                            }
                            //var coordExists = ship.ShipCoordinate.Contains(new Coordinate(x,y));
                            //if (coordExists)
                            //{ //somethin weird here
                            //    var shipName = ship.ShipName;
                            //    Console.WriteLine($"{shipName} damaged");
                            //    if (ship.Health != 0)
                            //    {
                            //        ship.Health--;
                            //    }

                            //}
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