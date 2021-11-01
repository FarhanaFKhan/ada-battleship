using System;
using System.Collections.Generic;

namespace Ada_Battleship
{
    public class MenuServices
    {
        private readonly BoardServices _boardServices = new BoardServices();
        private readonly PlayerServices _playerServices = new PlayerServices();
        public char ToggleOrientation()
        {
            var rand = new Random();
            var number = rand.Next(1, 3);
            var orientation = number == 1 ? 'V' : 'H';

            return orientation;
        }

        public List<Ship> GetAvailableShips(IPlayer player1)
        {
            var listOfAvailableShips = new List<Ship>();

            for (int i = 0; i < player1.PlayerFleet.Count; i++)
            {
                if (player1.PlayerFleet[i].Status == ShipStatus.Pending)
                {
                    listOfAvailableShips.Add(player1.PlayerFleet[i]);
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
        public void GamePlay(IPlayer attacker, IPlayer defender)
        {
            Console.WriteLine($"{attacker.Name} - Please enter coordinates(e.g A2):");
            if (attacker.Name != "Eva")
            {
                var userInput = Console.ReadLine();
                Console.WriteLine(userInput);
                var splitMove = _boardServices.SplitMove(userInput);
                var columnLabel = splitMove.Item1;
                var rowNumber = splitMove.Item2;
                var columnNumber = _boardServices.AlphabetToInt(columnLabel);
                var isValid = attacker.ShotBoard.ValidateMove(columnNumber, rowNumber);
                if (isValid)
                {
                    _playerServices.ShootTorpedo(rowNumber, columnNumber, attacker, defender);
                    attacker.ShotBoard.DisplayBoard();
                    DisplayAvailableShips(defender);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Please enter a valid move");
                    Console.ResetColor();
                }
            }

            else
            {
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

    }
}