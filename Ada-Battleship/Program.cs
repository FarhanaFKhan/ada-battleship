using System;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            Setup config = Setup.Instance;
            Console.WriteLine(config.BoardHeight);
            Console.WriteLine(config.BoardWidth);
            var shipInfo = config.ShipDetails;

            foreach (var ship in shipInfo)
            {
                Console.WriteLine(ship.ShipName + ", " + ship.ShipLength);
            }
            

        }
    }
}
