using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    public class CvC
    {
        //this class is responsible for two AI player Eva and Astra

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

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{player2.Name}'s turn to place ships.");
            Console.ResetColor();
            Console.WriteLine();

            _pvc.CompBoardSetup(player2);
            _menuServices.GamePlay(player1, player2);

        }
    }
}