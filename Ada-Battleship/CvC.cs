using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    public class CvC
    {
        private readonly int _boardWidth = Setup.Instance.BoardWidth;
        private readonly int _boardHeight = Setup.Instance.BoardHeight;
        private readonly MenuServices _menuServices = new MenuServices();
        private readonly PvC _pvc = new PvC();
        public void Menu()
        {
            

            IPlayer player1 = new Player("Eva");
            List <Ship> playerOneFleet = Setup.Instance.ShipDetailsP1;
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

            IPlayer player2 = new Player("Astra");
            List<Ship> playerTwoFleet = Setup.Instance.ShipDetailsP2;
            player2.PlayerFleet = playerTwoFleet;


            Console.WriteLine($"Greetings player two {player2.Name}! You have the following settings:");
            Console.WriteLine();

            Console.WriteLine($"Board Dimensions:{_boardWidth}x{_boardHeight}");
            Console.WriteLine();

            player2.GameBoard.DisplayBoard();
            Console.WriteLine();


            _pvc.CompBoardSetup(player1);

            Console.WriteLine($"List of player one placed ships:{_menuServices.GetPlacedShips(player1).Count}");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{player2.Name}'s turn to place ships.");
            Console.ResetColor();
            Console.WriteLine();

            _pvc.CompBoardSetup(player2);

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

                _menuServices.GamePlayAI(attacker, defender);

            } while (listOfPlayerOnePlacedShips.Count != 0 || listOfPlayerTwoPlacedShips.Count != 0);
        }
    }
}