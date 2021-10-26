using System.Collections.Generic;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    public class Player
    {
        public string Name { get; set; }
        public int State;
        public Board GameBoard = new Board();
        public Board ShotBoard = new Board();
        public List<Ship> PlayerFleet = Setup.Instance.ShipDetails;
        //score?

        public Player()
        {
            State = 0;
            GameBoard.AddTile();
            ShotBoard.AddTile();
        }
    }
}