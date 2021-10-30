using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;
namespace Ada_Battleship
{
    public class Menu
    {
        private readonly int _boardWidth = Setup.Instance.BoardWidth;
        private readonly int _boardHeight = Setup.Instance.BoardHeight;
        //private readonly List<Ship> _shipInfo = Setup.Instance.ShipDetails; //make sure this is directly not used. can be handled in player class
        //private readonly Board _gameBoard = new Board();
        private readonly Player _player1 = new Player();
        private readonly Player _player2 = new Player();
        private readonly BoardServices _boardServices = new BoardServices();
        private readonly MenuServices _menuServices = new MenuServices();
        private readonly PlayerServices _playerServices = new PlayerServices();

        private string _playerName;
        private int _mainOption;


        public void MainMenu()
        {

            //this will be stored in player class
            Console.WriteLine("Please enter your name:");
            _playerName = Console.ReadLine();
            _player1.Name = _playerName;
            Console.WriteLine($"Greetings {_player1.Name}! You have the following settings:");
            Console.WriteLine();

            Console.WriteLine($"Board Dimensions:{_boardWidth}x{_boardHeight}");
            Console.WriteLine();
            //_gameBoard.AddTile();
            _player1.GameBoard.DisplayBoard();
            Console.WriteLine();
            DisplayAvailableShips();
            Console.WriteLine();
            Console.WriteLine("Select Game Mode:");
            Console.WriteLine("1.Player v Comp.");
            Console.WriteLine("2.Quit.");
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
                    Console.WriteLine("Quit Game");
                    break;
                default:
                    PvCMenu();
                    break;
            }

        }

        public void PvCMenu()
        {

            int pvCMenuOption;

            do
            {
                var availableShips = GetAvailableShips();
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
                DisplayAvailableShips();
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
                        PvCMenuOptionTwo();
                        break;
                    case 3:
                        PvCMenuOptionThree();
                        break;
                    case 4:
                        PvCOptionFour();
                        break;
                    case 5:
                        QuitGame();
                        break;
                    default:
                        PvCMenuOptionOne();
                        break;
                }

            } while (pvCMenuOption != 5);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Comp's turn to place ships.");
            Console.ResetColor();

            CompBoardSetup();

            var listOfPlayerOnePlacedShips = GetPlacedShips(_player1);
            var listOfPlayerTwoPlacedShips = GetPlacedShips(_player2);

            

            while (listOfPlayerOnePlacedShips.Count != 0 || listOfPlayerTwoPlacedShips.Count != 0)
            {
                Player attacker;
                Player defender;
                if (listOfPlayerOnePlacedShips.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"Congratulations! Player 2 ({_player2.Name}) won! ");
                    Console.ResetColor();
                    break;
                }
                if (listOfPlayerTwoPlacedShips.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"Congratulations! Player 1 ({_player1.Name}) won! ");
                    Console.ResetColor();
                    break;
                }
                //this needs to be a method
                if (_player1.State == 0)
                {
                    attacker = _player1;
                    defender = _player2;
                    _player1.State = 1;
                }
                else
                {
                    attacker = _player2;
                    defender = _player1;
                    _player1.State = 0;
                }
                _menuServices.GamePlay(attacker,defender);
                DisplayAvailableShips();

            }

        }

        public void PvCMenuOptionOne()
        {
            //place ship manually

            _player1.GameBoard.DisplayBoard();
            var availableShips = GetAvailableShips();


            Console.WriteLine(availableShips.Count);

            ShipMenu();
            Console.WriteLine($"Available ships : {availableShips.Count}");
            Console.WriteLine();
            var shipOption = Console.ReadLine();
            var shipName = GetShipName(shipOption);

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
            var isValid = _player1.GameBoard.ValidateMove(columnNumber, rowNumber);
            var isOverlap = CheckForShipOverlap(rowNumber, columnNumber);
            if (isValid && !isOverlap)
            {
                _player1.GameBoard.PlaceShip(shipName, rowNumber, columnNumber, orientation);
                _player1.GameBoard.DisplayBoard();
                DisplayAvailableShips();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Please enter a valid move");
                Console.ResetColor();
            }


        }

        public void PvCMenuOptionTwo() //maybe send in player instance?
        {
            //Place all ships randomly.
            _boardServices.ResetBoard();

            foreach (var ship in _player1.PlayerFleet)
            {
                var shipLength = ship.ShipLength;
                var isValid = false;

                //this block will keep generating a random number unless its a valid move
                while (isValid == false)
                {
                    var columnNumber = _boardServices.RandomlyGenerateColumnNumber();
                    var rowNumber = _boardServices.RandomlyGenerateRowNumber();
                    var orientation = _menuServices.ToggleOrientation();
                    var isOverlap = CheckForShipOverlap(rowNumber, columnNumber);
                    if ((_boardWidth > columnNumber + shipLength) && (_boardHeight > rowNumber + shipLength) && !isOverlap)
                    {
                        _player1.GameBoard.PlaceShip(ship.ShipName, rowNumber, columnNumber, orientation);
                        isValid = true;

                    }

                }

                Console.WriteLine();
                DisplayAvailableShips();
            }
            _player1.GameBoard.DisplayBoard();

        }
        //needs to be extracted to another class
        private List<Ship> GetAvailableShips()
        {
            var listOfAvailableShips = new List<Ship>();

            for (int i = 0; i < _player1.PlayerFleet.Count; i++)
            {
                if (_player1.PlayerFleet[i].Status == ShipStatus.Pending)
                {
                    listOfAvailableShips.Add(_player1.PlayerFleet[i]);
                }
            }

            return listOfAvailableShips;
        }
        private void ShipMenu()
        {
            Console.WriteLine();
            var availableShips = GetAvailableShips();
            Console.WriteLine("Please select an available ship:");
            for (int i = 0; i < availableShips.Count; i++)
            {
                Console.WriteLine("\t" + i + "." + availableShips[i].ShipName);

            }
        }

        private string GetShipName(string shipOption)
        {
            int shipNumber;
            var availableShips = GetAvailableShips();
            var shipName = availableShips[0].ShipName; //could be better

            try
            {
                shipNumber = int.Parse(shipOption);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception();
            }

            for (int i = 0; i < availableShips.Count; i++)
            {
                if (i == shipNumber)
                {
                    shipName = availableShips[i].ShipName;
                }
            }

            return shipName;

        }

        private void DisplayAvailableShips()
        {
            Console.WriteLine();

            Console.WriteLine("Ships Details:");
            Console.WriteLine();
            Console.Write("\tName  \tLength  \tHealth  \tstatus");
            Console.WriteLine();

            foreach (var ship in _player1.PlayerFleet)
            {
                Console.WriteLine("\t" + ship.ShipName + "\t" + ship.ShipLength + "\t " + ship.Health + "\t " + ship.Status);
            }
        }

        //check if the ship overlaps with already placed ship
        //if the coordX = x && coordY == y (return true) meaning there is an overlap
        //if the columnNumber is a point on the ship

        public bool CheckForShipOverlap(int x, int y)
        {
            var isOverlap = false;
            foreach (var ship in _player1.PlayerFleet)
            {
                foreach (var coordinate in ship.ShipCoordinate)
                {
                    if (coordinate.X == x && coordinate.Y == y)
                    {
                        isOverlap = true; //there is overlap
                    }
                    //if (coordinate.X == x)
                    //{
                    //    if (y > 1 && y < (coordinate.Y + ship.ShipLength))
                    //    {
                    //        isOverlap = true;
                    //    }

                    //}
                    //if (coordinate.Y == y)
                    //{
                    //    if (x > 1 && x < (coordinate.X + ship.ShipLength))
                    //    {
                    //        isOverlap = true;
                    //    }

                    //}
                }

            }
            return isOverlap;
        }
        public void PvCMenuOptionThree()
        {
            // Place remaining ships randomly.
            var availableShips = GetAvailableShips();
            foreach (var ship in availableShips)
            {
                var shipLength = ship.ShipLength;
                var isValid = false;


                //this block will keep generating a random number unless its a valid move
                while (isValid == false)
                {
                    var columnNumber = _boardServices.RandomlyGenerateColumnNumber();
                    var rowNumber = _boardServices.RandomlyGenerateRowNumber();
                    var orientation = _menuServices.ToggleOrientation();
                    var isOverlap = CheckForShipOverlap(rowNumber, columnNumber);
                    if ((_boardWidth > columnNumber + shipLength) && (_boardHeight > rowNumber + shipLength) && !isOverlap)
                    {
                        _player1.GameBoard.PlaceShip(ship.ShipName, rowNumber, columnNumber, orientation);
                        isValid = true;

                    }

                }

                Console.WriteLine();
                DisplayAvailableShips();

            }
            _player1.GameBoard.DisplayBoard();

        }

        private void PvCOptionFour()
        {
            //reset board
            _boardServices.ResetBoard();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("The board has been reset.");
            Console.ResetColor();
            Console.WriteLine();
            _player1.GameBoard.DisplayBoard();
        }

        private void QuitGame()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("You quit game!");
            Console.ResetColor();

        }

        public void CompBoardSetup()
        {
            var compGameBoard = _player2.GameBoard;
            var compShips = _player2.PlayerFleet;
            _player2.Name = "Eva";


            foreach (var ship in compShips)
            {
                var shipLength = ship.ShipLength;
                var isValid = false;

                //this block will keep generating a random number unless its a valid move
                while (isValid == false)
                {
                    var columnNumber = _boardServices.RandomlyGenerateColumnNumber();
                    var rowNumber = _boardServices.RandomlyGenerateRowNumber();
                    var orientation = _menuServices.ToggleOrientation();
                    var isOverlap = CheckForShipOverlap(rowNumber, columnNumber);
                    if ((_boardWidth > columnNumber + shipLength) && (_boardHeight > rowNumber + shipLength) &&
                        !isOverlap)
                    {
                        compGameBoard.PlaceShip(ship.ShipName, rowNumber, columnNumber, orientation);
                        isValid = true;

                    }

                }
            }
            compGameBoard.DisplayBoard();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Ships placed on Comp's game board.");
            Console.ResetColor();
            //Console.WriteLine("Comp's shot board.");
            //compShotBoard.DisplayBoard();
        }

        private List<Ship> GetPlacedShips(Player player)
        {
            var listOfPlacedShips = new List<Ship>();

            for (int i = 0; i < player.PlayerFleet.Count; i++)
            {
                if (player.PlayerFleet[i].Status == ShipStatus.Placed)
                {
                    listOfPlacedShips.Add(player.PlayerFleet[i]);
                }
            }

            return listOfPlacedShips;
        }

        //public void ShootTorpedo(int x, int y)
        //{

        //    Console.WriteLine("get opponent's fleet");
        //    Console.WriteLine("check if a tile has x,y coordinates AND status 'placed' ");
        //    Console.WriteLine("change status to hit if placeholder is 's' otherwise miss");


        //}
    }
}