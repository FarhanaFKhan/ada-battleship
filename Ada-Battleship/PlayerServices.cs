using System;

namespace Ada_Battleship
{
    public class PlayerServices
    {
        public void ShootTorpedo(int x, int y,Player attacker, Player defender)
        {
            var defenderBoardTiles = defender.GameBoard.Tiles;
            var opponentShipInfo = defender.PlayerFleet;
            var playerShotBoardTiles = attacker.ShotBoard.Tiles;
            foreach (var playerShotBoardTile in playerShotBoardTiles)
            {
                foreach (var tile in defenderBoardTiles)
                {//need to update tileStatus to ship as well when changing placeholder 
                    if (tile.Coordinate.X == x && tile.Coordinate.Y == y && tile.TilePlaceholder == 's'&& playerShotBoardTile.Coordinate.X == x && playerShotBoardTile.Coordinate.Y == y)
                    {
                        tile.TileStatus = TileStatus.Hit;
                        tile.TilePlaceholder = 'H';
                        playerShotBoardTile.TileStatus = TileStatus.Hit;
                        playerShotBoardTile.TilePlaceholder = 'H';
                        foreach (var ship in opponentShipInfo)
                        {
                            if (ship.ShipCoordinate.Exists(c=>c.X == x) && ship.ShipCoordinate.Exists(c => c.Y == y))
                            { //somethin weird here
                                var shipName = ship.ShipName;
                                Console.WriteLine($"{shipName} damaged");
                                if (ship.Health != 0)
                                {
                                    ship.Health = ship.Health - 1;
                                }
                                
                            }
                        }

                    }
                    if (tile.Coordinate.X == x && tile.Coordinate.Y == y && tile.TilePlaceholder == '.' && playerShotBoardTile.Coordinate.X == x && playerShotBoardTile.Coordinate.Y == y)
                    {
                        tile.TileStatus = TileStatus.Miss;
                        tile.TilePlaceholder = 'M';
                        playerShotBoardTile.TileStatus = TileStatus.Miss;
                        playerShotBoardTile.TilePlaceholder = 'M';
                    }
                }
            }
            

        }
        //keep track of player turns

    }
}