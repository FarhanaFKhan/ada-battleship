using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    public class PvC
    {
        private readonly int _boardWidth = Setup.Instance.BoardWidth;
        private readonly int _boardHeight = Setup.Instance.BoardHeight;
        private readonly MenuServices _menuServices = new MenuServices();
        private readonly BoardServices _boardServices = new BoardServices();
        public void PvCMenu(IPlayer player1)
        {
            IPlayer player2 = new Player("Eva");
            player2.PlayerFleet = Setup.Instance.ShipDetailsP2;

            int pvCMenuOption;

            do
            {
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
                Console.WriteLine("You are now in PvC mode");
                Console.WriteLine();
                _menuServices.DisplayAvailableShips(player1);
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
                        PvCMenuOptionTwo(player1);
                        break;
                    case 3:
                        PvCMenuOptionThree(player1);
                        break;
                    case 4:
                        PvCOptionFour(player1);
                        break;
                    case 5:
                        QuitGame();
                        break;
                    default:
                        PvCMenuOptionOne(player1);
                        break;
                }

            } while (pvCMenuOption != 5);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Comp's turn to place ships.");
            Console.ResetColor();

            CompBoardSetup(player2);

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


        public void PvCMenuOptionOne(IPlayer player1)
        {
            //place ship manually

            player1.GameBoard.DisplayBoard();
            var availableShips = _menuServices.GetAvailableShips(player1);


            Console.WriteLine(availableShips.Count);

            _menuServices.ShipMenu(player1);
            Console.WriteLine($"Available ships : {availableShips.Count}");
            Console.WriteLine();
            var shipOption = Console.ReadLine();
            var shipName = _menuServices.GetShipName(shipOption, player1);

            Console.WriteLine($"You selected {shipName}");
            Console.WriteLine();
            Console.WriteLine("Please enter a point to position a ship:");
            var userInput = Console.ReadLine();
            Console.WriteLine(userInput);

            //separate string
            //convert into string and int

            var splitMove = _boardServices.SplitMove(userInput);

            var columnLabel = splitMove.Item1;
            var rowNumber = splitMove.Item2;
            var orientation = splitMove.Item3;
            var columnNumber = _boardServices.AlphabetToInt(columnLabel);

            //validator for input
            var isValid = player1.GameBoard.ValidateMove(columnNumber, rowNumber);
            var isOverlap = _menuServices.CheckForShipOverlap(rowNumber, columnNumber, player1);
            if (isValid && !isOverlap)
            {
                player1.GameBoard.PlaceShip(shipName, rowNumber, columnNumber, orientation, player1);
                player1.GameBoard.DisplayBoard();
                _menuServices.DisplayAvailableShips(player1);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Please enter a valid move");
                Console.ResetColor();
            }


        }

        public void PvCMenuOptionTwo(IPlayer player1)
        {
            //Place all ships randomly.
            _boardServices.ResetBoard();


            foreach (var ship in player1.PlayerFleet)
            {
                var shipLength = ship.ShipLength;
                var isValid = false;

                //this block will keep generating a random number unless its a valid move
                while (isValid == false)
                {
                    //var columnNumber = _boardServices.RandomlyGenerateColumnNumber(ship.ShipCoordinate);
                    //var rowNumber = _boardServices.RandomlyGenerateRowNumber(ship.ShipCoordinate);
                    var coordinates = _boardServices.RandomlyGenerateCoordinates(ship.ShipCoordinate);
                    var rowNumber = coordinates.Item1;
                    var columnNumber = coordinates.Item2;
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
                    //var columnNumber = _boardServices.RandomlyGenerateColumnNumber(ship.ShipCoordinate);
                    //var rowNumber = _boardServices.RandomlyGenerateRowNumber(ship.ShipCoordinate);
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
            _boardServices.ResetBoard();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("The board has been reset.");
            Console.ResetColor();
            Console.WriteLine();
            player.GameBoard.DisplayBoard();
        }

        public void QuitGame()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("You quit game!");
            Console.ResetColor();

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
                    //this code is creating issues for some reason
                    //var coordinates = _boardServices.RandomlyGenerateCoordinates(ship.ShipCoordinate);
                    //var rowNumber = coordinates.Item1;
                    //var columnNumber = coordinates.Item2;
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
            compGameBoard.DisplayBoard();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Ships placed on Comp's game board.");
            Console.ResetColor();

        }
    }
}