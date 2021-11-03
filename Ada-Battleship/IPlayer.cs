using System.Collections.Generic;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    public interface IPlayer
    {
        public string Name { get; set; }
        public int State { get; set; }
        public Board GameBoard { get; }
        public Board ShotBoard { get; }
        public List<Ship> PlayerFleet { get; set; }

    }
}