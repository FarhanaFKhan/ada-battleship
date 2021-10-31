using System;
using System.Collections.Generic;

namespace Ada_Battleship
{
    public class Ship
    {
        public string ShipName { get; set; }
        public int ShipLength { get; set; }
        public int Health { get; set; }
        public ShipStatus Status { get; set; }
        public List<Coordinate> ShipCoordinate => _listOfCoordinates;
        private readonly List<Coordinate> _listOfCoordinates = new List<Coordinate>();
        
        public Ship(string shipName, int shipLength)
        {
            ShipName = shipName;
            ShipLength = shipLength;
            Health = shipLength;
            Status = ShipStatus.Pending;
            
        }

        public List<Coordinate> SetCoordinates(int x, int y)
        {
            _listOfCoordinates.Add(new Coordinate(x, y));


            return _listOfCoordinates;
        }

    }
}