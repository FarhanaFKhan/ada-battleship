using System;
using System.Collections.Generic;

namespace Ada_Battleship
{
    public class PlayerServices
    {
        public void ShootTorpedo(int x, int y, Player attacker, Player defender)
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
                            if (ship.ShipCoordinate.Exists(c => c.X == x && c.Y == y))
                            { //somethin weird here
                                var shipName = ship.ShipName;
                                Console.WriteLine($"{shipName} damaged");
                                if (ship.Health != 0)
                                {
                                    ship.Health--;
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