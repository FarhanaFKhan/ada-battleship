using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    public class PvP
    {
        //this class is responsible for two human players playing the game

        private readonly int _boardWidth = Setup.Instance.BoardWidth;
        private readonly int _boardHeight = Setup.Instance.BoardHeight;
        private readonly MenuServices _menuServices = new MenuServices();
        private readonly PvC _pvc = new PvC();
        public void Menu()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("You are now in PvP mode");
            Console.WriteLine();
            Console.ResetColor();

            Console.WriteLine("Please enter your name:");
            var playerNameP1 = Console.ReadLine();

            IPlayer player1 = new Player(playerNameP1);
            player1.Name = playerNameP1;
            List<Ship> playerOneFleet = Setup.Instance.ShipDetailsP1;
            player1.PlayerFleet = playerOneFleet;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Greetings player one {player1.Name}! You have the following settings:");
            Console.ResetColor();

            Console.WriteLine();

            Console.WriteLine($"{player1.Name} -- Board Dimensions:{_boardWidth}x{_boardHeight}");
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

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Greetings player two {player2.Name}! You have the following settings:");
            Console.ResetColor();

            Console.WriteLine();

            Console.WriteLine($"{player2.Name} -- Board Dimensions:{_boardWidth}x{_boardHeight}");
            Console.WriteLine();

            player2.GameBoard.DisplayBoard();
            Console.WriteLine();


            SetPlayerBoard(player1);
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{player2.Name}'s turn to place ships.");
            Console.ResetColor();
            Console.WriteLine();

            SetPlayerBoard(player2);

            _menuServices.GamePlay(player1 , player2);
        }

        public void SetPlayerBoard(IPlayer currentPlayer)
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
                        Console.Clear();
                        currentPlayer.State = 0;
                        break;
                    }
                }


                _menuServices.SubMenu();

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
                        _menuServices.QuitGame();
                        break;
                    default:
                        _pvc.PvCMenuOptionOne(currentPlayer);
                        break;
                }

            } while (pvCMenuOption != 5);
        }
    }


}