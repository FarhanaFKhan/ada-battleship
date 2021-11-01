using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;
namespace Ada_Battleship
{
    public class Menu
    {
        private readonly int _boardWidth = Setup.Instance.BoardWidth;
        private readonly int _boardHeight = Setup.Instance.BoardHeight;
        
        private readonly MenuServices _menuServices = new MenuServices();
        private readonly PvC _pvc = new PvC();
        private readonly PvP _pvp = new PvP();



        private int _mainOption;


        public void MainMenu()
        {

            Console.WriteLine("Please enter your name:");
            var playerName = Console.ReadLine();

            IPlayer player1 = new Player(playerName);
            player1.Name = playerName;
            List<Ship> playerOneFleet = Setup.Instance.ShipDetailsP1;
            player1.PlayerFleet = playerOneFleet;


            Console.WriteLine($"Greetings {player1.Name}! You have the following settings:");
            Console.WriteLine();

            Console.WriteLine($"Board Dimensions:{_boardWidth}x{_boardHeight}");
            Console.WriteLine();

            player1.GameBoard.DisplayBoard();
            Console.WriteLine();

            _menuServices.DisplayAvailableShips(player1);
            Console.WriteLine();

            Console.WriteLine("Select Game Mode:");
            Console.WriteLine("1.Player v Comp.");
            Console.WriteLine("2.Player v Player.");
            Console.WriteLine("3.Quit.");

            try
            {
                _mainOption = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new InvalidOperationException();
            }


            switch (_mainOption)
            {
                case 2:
                    _pvp.Menu(player1);
                    break;
                case 3:
                    Console.WriteLine("Quit Game");
                    break;
                default:
                    _pvc.PvCMenu(player1);
                    break;
            }

        }

        

        

    }
}