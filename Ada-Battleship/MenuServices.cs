using System;
using System.Collections.Generic;

namespace Ada_Battleship
{
    public class MenuServices
    {
        //helper functions associated with menu 

        private readonly BoardServices _boardServices = new BoardServices();
        private readonly PlayerServices _playerServices = new PlayerServices();
        public char ToggleOrientation()
        {
            var rand = new Random();
            var number = rand.Next(1, 3);
            var orientation = number == 1 ? 'V' : 'H';

            return orientation;
        }

        public List<Ship> GetAvailableShips(IPlayer player)
        {
            var listOfAvailableShips = new List<Ship>();

            for (int i = 0; i < player.PlayerFleet.Count; i++)
            {
                if (player.PlayerFleet[i].Status == ShipStatus.Pending)
                {
                    listOfAvailableShips.Add(player.PlayerFleet[i]);
                }
            }

            return listOfAvailableShips;
        }

        public void ShipMenu(IPlayer player)
        {
            Console.WriteLine();
            var availableShips = GetAvailableShips(player);
            Console.WriteLine("Please select an available ship:");
            for (int i = 0; i < availableShips.Count; i++)
            {
                Console.WriteLine("\t" + i + "." + availableShips[i].ShipName);

            }
        }

        public string GetShipName(string shipOption, IPlayer player)
        {
            int shipNumber;
            var availableShips = GetAvailableShips(player);
            var shipName = availableShips[0].ShipName; 

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

        public bool CheckForShipOverlap(int x, int y, IPlayer player)
        {
            var isOverlap = false;
            foreach (var ship in player.PlayerFleet)
            {
                foreach (var coordinate in ship.ShipCoordinate)
                {
                    if (coordinate.X == x && coordinate.Y == y)
                    {
                        isOverlap = true; //there is overlap
                    }
                    
                }

            }
            return isOverlap;
        }

        public List<Ship> GetPlacedShips(IPlayer player)
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
        public void HumanPlayerTurn(IPlayer attacker, IPlayer defender)
        {
           
            bool isValid;
            do
            {
                Console.WriteLine($"{attacker.Name} - Please enter coordinates(e.g A2):");
                var userInput = Console.ReadLine();
                
                var splitMove = _boardServices.SplitMove(userInput);
                var columnLabel = splitMove.Item1;
                var rowNumber = splitMove.Item2;
                var columnNumber = _boardServices.AlphabetToInt(columnLabel);

                isValid = attacker.ShotBoard.ValidateMoveTorpedo(columnNumber, rowNumber);

                if (isValid == true)
                {
                    _playerServices.ShootTorpedo(rowNumber, columnNumber, attacker, defender);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"{attacker.Name} -- Shot board");
                    Console.ResetColor();
                    attacker.ShotBoard.DisplayBoard();
                    Console.WriteLine();
                    DisplayAvailableShips(defender);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Please enter a valid move");
                    Console.ResetColor();
                }
            } while (!isValid);


        }

        public void AITurn(IPlayer attacker, IPlayer defender)
        {

            Console.WriteLine($"{attacker.Name} - Please enter coordinates(e.g A2):");


            var coordinateGenerator = _boardServices.RandomlyGenerateCoordinates(attacker.ShotBoard.Tiles);
            var rowNumber = coordinateGenerator.Item1;
            var columnNumber = coordinateGenerator.Item2;

            bool isValid;
            do
            {
                isValid = attacker.ShotBoard.ValidateMove(columnNumber, rowNumber);

                _playerServices.ShootTorpedo(rowNumber, columnNumber, attacker, defender);
                attacker.ShotBoard.DisplayBoard();
                DisplayAvailableShips(defender);
            } while (!isValid);


        }

        public void DisplayAvailableShips(IPlayer currentPlayer)
        {
            Console.WriteLine();

            Console.WriteLine($"{currentPlayer.Name} -- Ships Details:");
            Console.WriteLine();
            Console.Write("\tName  \tLength  \tHealth  \tstatus");
            Console.WriteLine();

            foreach (var ship in currentPlayer.PlayerFleet)
            {
                Console.WriteLine("\t" + ship.ShipName + "\t" + ship.ShipLength + "\t " + ship.Health + "\t " + ship.Status);
            }
        }

        public void SubMenu()
        {
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
            Console.WriteLine();
        }

        public void TorpedoMenu()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Please select from the following:");
            Console.WriteLine("1.Enter coordinates to shoot a torpedo");
            Console.WriteLine("2.Auto fire");
            Console.ResetColor();
        }

        public void GamePlay(IPlayer player1, IPlayer player2)
        {
            List<Ship> listOfPlayerOnePlacedShips;
            List<Ship> listOfPlayerTwoPlacedShips;



            do
            {
                listOfPlayerOnePlacedShips = GetPlacedShips(player1);
                listOfPlayerTwoPlacedShips = GetPlacedShips(player2);
                
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

                
                if (attacker.Name == "Eva" || attacker.Name == "Astra")
                {
                    AITurn(attacker, defender);
                }
                if (attacker.Name != "Eva" && attacker.Name != "Astra")
                {
                    TorpedoMenu();

                    int torpedoShot;
                    try
                    {

                        torpedoShot = int.Parse(Console.ReadLine());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        throw new InvalidOperationException();
                    }

                    switch (torpedoShot)
                    {
                        case 2:
                            AITurn(attacker, defender);
                            break;
                        default:
                            HumanPlayerTurn(attacker, defender);
                            break;

                    }
                }

            } while (listOfPlayerOnePlacedShips.Count != 0 || listOfPlayerTwoPlacedShips.Count != 0);
        }
        public void QuitGame()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("You quit game!");
            Console.ResetColor();

        }

    }
}