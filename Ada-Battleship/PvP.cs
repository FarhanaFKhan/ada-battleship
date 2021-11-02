using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    public class PvP
    {
        private readonly int _boardWidth = Setup.Instance.BoardWidth;
        private readonly int _boardHeight = Setup.Instance.BoardHeight;
        private readonly MenuServices _menuServices = new MenuServices();
        private readonly PvC _pvc = new PvC();
        public void Menu()
        {
            Console.WriteLine("Please enter your name:");
            var playerNameP1 = Console.ReadLine();

            IPlayer player1 = new Player(playerNameP1);
            player1.Name = playerNameP1;
            List<Ship> playerOneFleet = Setup.Instance.ShipDetailsP1;
            player1.PlayerFleet = playerOneFleet;


            Console.WriteLine($"Greetings player one {player1.Name}! You have the following settings:");
            Console.WriteLine();

            Console.WriteLine($"Board Dimensions:{_boardWidth}x{_boardHeight}");
            Console.WriteLine();

            player1.GameBoard.DisplayBoard();
            Console.WriteLine();

            _menuServices.DisplayAvailableShips(player1);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Please enter your name:");
            var playerNameP2 = Console.ReadLine();

            IPlayer player2 = new Player(playerNameP2);
            player2.Name = playerNameP2;
            List<Ship> playerTwoFleet = Setup.Instance.ShipDetailsP2;
            player2.PlayerFleet = playerTwoFleet;


            Console.WriteLine($"Greetings player two {player2.Name}! You have the following settings:");
            Console.WriteLine();

            Console.WriteLine($"Board Dimensions:{_boardWidth}x{_boardHeight}");
            Console.WriteLine();

            player2.GameBoard.DisplayBoard();
            Console.WriteLine();


            HelperMethod(player1);

            Console.WriteLine($"List of player one placed ships:{_menuServices.GetPlacedShips(player1).Count}");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{player2.Name}'s turn to place ships.");
            Console.ResetColor();
            Console.WriteLine();

            HelperMethod(player2);

            Console.WriteLine($"List of player one placed ships:{_menuServices.GetPlacedShips(player1).Count}");
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

        public void HelperMethod(IPlayer currentPlayer)
        {
            int pvCMenuOption;
            List<Ship> availableShips;
            currentPlayer.State = 1; 
            do
            {

                availableShips = _menuServices.GetAvailableShips(currentPlayer);

                if (availableShips.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You have placed all of your ships. Hit enter to continue");
                    Console.ResetColor();

                    var input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        currentPlayer.State = 0;
                        break;
                    }
                }



                Console.WriteLine();
                Console.WriteLine($"available ships {currentPlayer.Name} = {availableShips.Count}");

                Console.WriteLine();
                Console.WriteLine("You are now in PvP mode");
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
        }
    }


}