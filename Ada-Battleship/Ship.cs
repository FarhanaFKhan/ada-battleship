using System.Collections.Generic;

namespace Ada_Battleship
{
    public class Ship
    {
        public string ShipName { get; set; }
        public int ShipLength { get; set; }
        public int Health { get; set; }
        public ShipStatus Status { get; set; }
        //public int ShipCoordinateX { get; set; }
        //public int ShipCoordinateY { get; set; }
        public List<Coordinate> ShipCoordinate = new List<Coordinate>();

        public Ship(string shipName, int shipLength)
        {
            ShipName = shipName;
            ShipLength = shipLength;
            Health = shipLength;
            Status = ShipStatus.Pending;
            SetShipCoordinates();
        }

        public void SetShipCoordinates()
        {
            foreach (var coordinate in ShipCoordinate)
            {
                coordinate.X = 0;
                coordinate.Y = 0;
            }
        }
        
    }
}