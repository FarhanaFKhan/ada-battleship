using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    public class Menu
    {
        private readonly int _boardWidth = Setup.Instance.BoardWidth;
        private readonly int _boardHeight = Setup.Instance.BoardHeight;
        private readonly List<Ship> _shipInfo = Setup.Instance.ShipDetails;

        private int _mainOption;

        public void MainMenu()
        {
            Console.WriteLine($"Board Dimensions:{_boardWidth}x{_boardHeight}");
            Console.WriteLine("Available Ships:");
            Console.WriteLine();
            Console.Write("\tName  \tLength  \tHealth  \tstatus");
            Console.WriteLine();
            foreach (var ship in _shipInfo)
            {
                Console.WriteLine("\t"+ ship.ShipName + "\t" + ship.ShipLength + "\t " + ship.Health + "\t " + ship.Status);
            }
            Console.WriteLine();
            Console.WriteLine("Select Game Mode:");
            Console.WriteLine("1.Player v Player.");
            Console.WriteLine("2.Player v Comp.");
            try
            {
                _mainOption = int.Parse(Console.ReadLine());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw new InvalidOperationException();
            }
            

            switch (_mainOption)
            {
                case 3:
                    Console.WriteLine("Quit Game");
                    break;
                case 2:
                    Console.WriteLine("PvC");
                    break;
                default:
                    Console.WriteLine("PvP");
                    break;
            }

        }

        public void PvPMenu()
        {

        }

    }
}