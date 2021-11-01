using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    public class PvP 
    {
        private readonly MenuServices _menuServices = new MenuServices();
        private readonly PvC _pvc = new PvC();
        public void Menu(IPlayer player1)
        {
            Console.WriteLine("Please enter your name:");
            var playerName = Console.ReadLine();
            IPlayer player2 = new Player(playerName);
            player2.PlayerFleet = Setup.Instance.ShipDetailsP2;


            int pvCMenuOption;

            do
            {
                IPlayer currentPlayer;
                if (player1.State == 0)
                {
                    currentPlayer = player1;
                    player1.State = 1;
                }
                else
                {
                    currentPlayer = player2;
                    player1.State = 0;
                }

                var availableShips = _menuServices.GetAvailableShips(currentPlayer);

                if (availableShips.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You have placed all of your ships. Hit enter to continue");
                    Console.ResetColor();
                    var input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        break;
                    }
                }

                Console.WriteLine();
                Console.WriteLine("You are now in PvC mode");
                Console.WriteLine();
                _menuServices.DisplayAvailableShips(currentPlayer);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("1.Place ship manually.");
                Console.WriteLine("2.Place all ships randomly.");
                Console.WriteLine("3.Place remaining ships randomly.");
                Console.WriteLine("4.Reset board.");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("5.Quit.");
                Console.ResetColor();

                try
                {

                    pvCMenuOption = int.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw new InvalidOperationException();
                }

                switch (pvCMenuOption)
                {
                    case 2:
                       _pvc.PvCMenuOptionTwo(currentPlayer);
                        break;
                    case 3:
                        _pvc.PvCMenuOptionThree(currentPlayer);
                        break;
                    case 4:
                        _pvc.PvCOptionFour(currentPlayer);
                        break;
                    case 5:
                        _pvc.QuitGame();
                        break;
                    default:
                        _pvc.PvCMenuOptionOne(currentPlayer);
                        break;
                }

            } while (pvCMenuOption != 5);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Comp's turn to place ships.");
            Console.ResetColor();


            List<Ship> listOfPlayerOnePlacedShips;
            List<Ship> listOfPlayerTwoPlacedShips;



            do
            {
                listOfPlayerOnePlacedShips = _menuServices.GetPlacedShips(player1);
                listOfPlayerTwoPlacedShips = _menuServices.GetPlacedShips(player2);
                Console.WriteLine($"p1 placed ship {listOfPlayerOnePlacedShips.Count}");
                Console.WriteLine($"p2 placed ship {listOfPlayerTwoPlacedShips.Count}");
                IPlayer attacker;
                IPlayer defender;
                if (listOfPlayerOnePlacedShips.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"Congratulations! Player 2 ({player2.Name}) won! ");
                    Console.ResetColor();
                    break;
                }

                if (listOfPlayerTwoPlacedShips.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"Congratulations! Player 1 ({player1.Name}) won! ");
                    Console.ResetColor();
                    break;
                }

                //this needs to be a method
                if (player1.State == 0)
                {
                    attacker = player1;
                    defender = player2;
                    player1.State = 1;
                }
                else
                {
                    attacker = player2;
                    defender = player1;
                    player1.State = 0;
                }

                _menuServices.GamePlay(attacker, defender);

            } while (listOfPlayerOnePlacedShips.Count != 0 || listOfPlayerTwoPlacedShips.Count != 0);
        }
    }


}