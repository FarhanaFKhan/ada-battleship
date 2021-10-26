namespace Ada_Battleship
{
    public class Ship
    {
        public string ShipName { get; set; }
        public int ShipLength { get; set; }
        public int Health { get; set; }
        public ShipStatus Status { get; set; }
        public int ShipCoordinateX { get; set; }
        public int ShipCoordinateY { get; set; }

        public Ship(string shipName, int shipLength)
        {
            ShipName = shipName;
            ShipLength = shipLength;
            Health = shipLength;
            Status = ShipStatus.Pending;
        }
        
    }
}