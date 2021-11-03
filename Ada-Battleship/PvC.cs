using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    public class PvC
    {
        //this class is responsible for a match between a human and AI called Eva

        private readonly int _boardWidth = Setup.Instance.BoardWidth;
        private readonly int _boardHeight = Setup.Instance.BoardHeight;
        private readonly MenuServices _menuServices = new MenuServices();
        private readonly BoardServices _boardServices = new BoardServices();
        public void PvCMenu()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("You are now in PvC mode");
            Console.WriteLine();
            Console.ResetColor();

            Console.WriteLine("Please enter your name:");
            var playerName = Console.ReadLine();

            //player one created/registered
            IPlayer player1 = new Player(playerName);
            player1.Name = playerName;
            List<Ship> playerOneFleet = Setup.Instance.ShipDetailsP1;
            player1.PlayerFleet = playerOneFleet;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Greetings {player1.Name}! You have the following settings:");
            Console.ResetColor();

            Console.WriteLine();

            Console.WriteLine($"{player1.Name} -- Board Dimensions:{_boardWidth}x{_boardHeight}");
            Console.WriteLine();

            player1.GameBoard.DisplayBoard();
            Console.WriteLine();

            _menuServices.DisplayAvailableShips(player1);
            Console.WriteLine();

            //player two created/registered
            IPlayer player2 = new Player("Eva");
            player2.PlayerFleet = Setup.Instance.ShipDetailsP2;

            int pvCMenuOption;

            do
            {
                //this block will keep showing till Quit game option is selected

                var availableShips = _menuServices.GetAvailableShips(player1);

                if (availableShips.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You have placed all of your ships. Hit enter to continue");
                    Console.ResetColor();
                    var input = "";
                    input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        break;
                    }
                }

                Console.WriteLine();

                _menuServices.DisplayAvailableShips(player1);

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
                        PvCMenuOptionTwo(player1);
                        break;
                    case 3:
                        PvCMenuOptionThree(player1);
                        break;
                    case 4:
                        PvCOptionFour(player1);
                        break;
                    case 5:
                        _menuServices.QuitGame();
                        break;
                    default:
                        PvCMenuOptionOne(player1);
                        break;
                }

            } while (pvCMenuOption != 5);
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Comp's turn to place ships.");
            Console.ResetColor();
            Console.WriteLine();

            CompBoardSetup(player2);
            _menuServices.GamePlay(player1, player2);

        }


        public void PvCMenuOptionOne(IPlayer player)
        {
            //place ship manually

            player.GameBoard.DisplayBoard();

            var availableShips = _menuServices.GetAvailableShips(player);

            _menuServices.ShipMenu(player);

            Console.WriteLine($"Available ships : {availableShips.Count}");
            Console.WriteLine();

            var shipOption = Console.ReadLine();
            var shipName = _menuServices.GetShipName(shipOption, player);

            Console.WriteLine($"You selected {shipName}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Please enter a point to position a ship:");
            Console.ResetColor();

            var userInput = Console.ReadLine();
           

            //separate input string
            //convert it into char,int using tuple

            var splitMove = _boardServices.SplitMove(userInput);

            var columnLabel = splitMove.Item1;
            var rowNumber = splitMove.Item2;
            var orientation = splitMove.Item3;
            var columnNumber = _boardServices.AlphabetToInt(columnLabel);

            //validator for input
            var isValid = player.GameBoard.ValidateMove(columnNumber, rowNumber);
            var isOverlap = _menuServices.CheckForShipOverlap(rowNumber, columnNumber, player);
            if (isValid && !isOverlap)
            {
                player.GameBoard.PlaceShip(shipName, rowNumber, columnNumber, orientation, player);
                player.GameBoard.DisplayBoard();
                _menuServices.DisplayAvailableShips(player);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Please enter a valid move");
                Console.ResetColor();
            }


        }

        public void PvCMenuOptionTwo(IPlayer player)
        {
            //Place all ships randomly.
            _boardServices.ResetBoard(player);


            foreach (var ship in player.PlayerFleet)
            {
                var shipLength = ship.ShipLength;
                var isValid = false;

                //this block will keep generating a random number unless its a valid move
                while (isValid == false)
                {
                    
                    var coordinates = _boardServices.RandomlyGenerateCoordinates(ship.ShipCoordinate);
                    var rowNumber = coordinates.Item1;
                    var columnNumber = coordinates.Item2;
                    var orientation = _menuServices.ToggleOrientation();
                    var isOverlap = _menuServices.CheckForShipOverlap(rowNumber, columnNumber, player);
                    if ((_boardWidth > columnNumber + shipLength) && (_boardHeight > rowNumber + shipLength) && !isOverlap)
                    {
                        player.GameBoard.PlaceShip(ship.ShipName, rowNumber, columnNumber, orientation, player);
                        isValid = true;

                    }

                }

            }

            Console.WriteLine();
            _menuServices.DisplayAvailableShips(player);
            player.GameBoard.DisplayBoard();
            Console.WriteLine();

        }


        //check if the ship overlaps with already placed ship
        //if the coordX = x && coordY == y (return true) meaning there is an overlap
        //if the columnNumber is a point on the ship


        public void PvCMenuOptionThree(IPlayer player1)
        {
            // Place remaining ships randomly.
            var availableShips = _menuServices.GetAvailableShips(player1);
            foreach (var ship in availableShips)
            {
                var shipLength = ship.ShipLength;
                var isValid = false;


                //this block will keep generating a random number unless its a valid move
                while (isValid == false)
                {
                    var columnNumber = _boardServices.RandomlyGenerateColumnNumber(ship.ShipCoordinate);
                    var rowNumber = _boardServices.RandomlyGenerateRowNumber(ship.ShipCoordinate);
                    var orientation = _menuServices.ToggleOrientation();
                    var isOverlap = _menuServices.CheckForShipOverlap(rowNumber, columnNumber, player1);
                    if ((_boardWidth > columnNumber + shipLength) && (_boardHeight > rowNumber + shipLength) && !isOverlap)
                    {
                        player1.GameBoard.PlaceShip(ship.ShipName, rowNumber, columnNumber, orientation, player1);
                        isValid = true;

                    }

                }

                Console.WriteLine();
                _menuServices.DisplayAvailableShips(player1);

            }
            player1.GameBoard.DisplayBoard();

        }

        public void PvCOptionFour(IPlayer player)
        {
            //reset board
            _boardServices.ResetBoard(player);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("The board has been reset.");
            Console.ResetColor();
            Console.WriteLine();
            player.GameBoard.DisplayBoard();
        }

        

        public void CompBoardSetup(IPlayer player2)
        {
            var compGameBoard = player2.GameBoard;
            var compShips = player2.PlayerFleet;


            foreach (var ship in compShips)
            {
                var shipLength = ship.ShipLength;
                var isValid = false;

                //this block will keep generating a random number unless its a valid move
                while (isValid == false)
                {
                    var columnNumber = _boardServices.RandomlyGenerateColumnNumber(ship.ShipCoordinate);
                    var rowNumber = _boardServices.RandomlyGenerateRowNumber(ship.ShipCoordinate);
                    var orientation = _menuServices.ToggleOrientation();
                    var isOverlap = _menuServices.CheckForShipOverlap(rowNumber, columnNumber, player2);
                    if ((_boardWidth > columnNumber + shipLength) && (_boardHeight > rowNumber + shipLength) &&
                        !isOverlap)
                    {
                        compGameBoard.PlaceShip(ship.ShipName, rowNumber, columnNumber, orientation, player2);
                        isValid = true;

                    }

                }
            }
            //compGameBoard.DisplayBoard();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Ships placed on Comp's game board.");
            Console.ResetColor();

        }
    }
}