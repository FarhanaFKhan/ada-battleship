using System;

namespace Ada_Battleship
{
    public class PlayerServices
    {
        public void ShootTorpedo(int x, int y,Player player, Player opponentPlayer)
        {
            var opponentBoardTiles = opponentPlayer.GameBoard.Tiles;
            var opponentShipInfo = opponentPlayer.PlayerFleet;
            var playerShotBoardTiles = player.ShotBoard.Tiles;
            foreach (var playerShotBoardTile in playerShotBoardTiles)
            {
                foreach (var tile in opponentBoardTiles)
                {//need to update tilestatus to ship as well when changing placeholder 
                    if (tile.Coordinate.X == x && tile.Coordinate.Y == y && tile.TilePlaceholder == 's'&& playerShotBoardTile.Coordinate.X == x && playerShotBoardTile.Coordinate.Y == y)
                    {
                        tile.TileStatus = TileStatus.Hit;
                        tile.TilePlaceholder = 'H';
                        playerShotBoardTile.TileStatus = TileStatus.Hit;
                        playerShotBoardTile.TilePlaceholder = 'H';

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
            


            Console.WriteLine("get opponent's fleet");
            Console.WriteLine("check if a tile has x,y coordinates AND status 'placed' ");
            Console.WriteLine("change status to hit if placeholder is 's' otherwise miss");


        }
    }
}