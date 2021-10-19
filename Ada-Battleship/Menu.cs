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

        string playerName;
        private int _mainOption;

        public void MainMenu()
        {
            
            //this will be stored in player class
            Console.WriteLine("Please enter your name:");
            playerName = Console.ReadLine();

            Console.WriteLine($"Greetings {playerName}! You have the following settings:");
            Console.WriteLine();

            Console.WriteLine($"Board Dimensions:{_boardWidth}x{_boardHeight}");
            DisplayAvailableShips();
            Console.WriteLine();
            Console.WriteLine("Select Game Mode:");
            Console.WriteLine("1.Player v Comp.");
            Console.WriteLine("2.Quit.");
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
                case 2:
                    Console.WriteLine("Quit Game");
                    break;
                default:
                    PvCMenu();
                    break;
            }

        }

        public void PvCMenu()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("You are now in PvC mode");
            Console.WriteLine();
            DisplayAvailableShips();
            Console.WriteLine();
            Console.WriteLine("1.Place all ships randomly.");
            Console.WriteLine("2.Place remaining ships randomly.");
            Console.WriteLine("3.Place ship manually.");
            Console.WriteLine("4.Reset board.");
        }

        private void DisplayAvailableShips()
        {
            Console.WriteLine("Available Ships:");
            Console.WriteLine();
            Console.Write("\tName  \tLength  \tHealth  \tstatus");
            Console.WriteLine();
            foreach (var ship in _shipInfo)
            {
                Console.WriteLine("\t" + ship.ShipName + "\t" + ship.ShipLength + "\t " + ship.Health + "\t " + ship.Status);
            }
        }

    }
}