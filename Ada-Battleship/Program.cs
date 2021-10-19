using System;
using System.Threading.Channels;
using Ada_Battleship.Configurations;

namespace Ada_Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            //launcher

            var menu = new Menu();
            menu.MainMenu();
            

            Board gameBoard = new Board();
            gameBoard.DisplayBoard();

            string userInput;
            (char, int) splitMove;
            char columnLabel;
            int columnNumber;
            int rowNumber;
            int option;
            //menu
            Console.WriteLine("Please select Option:");
            Console.WriteLine("1.Place ship manually");
            Console.WriteLine("2.Randomly place ship");
            Console.WriteLine("3. to quit.");
            try
            {
                option = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception();
            }


            if (option == 1)
            {
                Console.WriteLine("Please enter a point to position a ship:");
                userInput = Console.ReadLine();
                Console.WriteLine(userInput);
                //separate string
                //convert into string and int
                splitMove = gameBoard.SplitMove(userInput);
                Console.WriteLine($"ColumnLabel: {splitMove.Item1}");
                Console.WriteLine($"rowNumber: {splitMove.Item2}");
                columnLabel = splitMove.Item1;
                rowNumber = splitMove.Item2;

                columnNumber = gameBoard.AlphabetToInt(columnLabel);

                //validator for input
                var isValid = gameBoard.ValidateMove(columnNumber, rowNumber);

                if (isValid)
                {
                    gameBoard.PlaceShip("Carrier", rowNumber, columnNumber);
                    gameBoard.DisplayBoard();
                }
                else
                {
                    Console.WriteLine("Please enter a valid move");
                }
            }

            if (option == 2)
            {
                columnNumber = gameBoard.RandomlyGenerateColumnNumber();
                rowNumber = gameBoard.RandomlyGenerateRowNumber();
                gameBoard.PlaceShip("Carrier", rowNumber, columnNumber);
                gameBoard.DisplayBoard();

            }





            

            //Console.Clear(); //await or state?
            //gameBoard.PlaceShip("Battleship", 2, 2);
            //gameBoard.DisplayBoard();
            // gameBoard.PlaceShip("Patrol Boat", 1, 9);
            // gameBoard.DisplayBoard();
            // gameBoard.PlaceShip("Destroyer", 6, 3);
            // gameBoard.DisplayBoard();
            //gameBoard.PlaceShip("Submarine", 9, 7);
            //gameBoard.DisplayBoard();

            //another instance of board for 'shot' board
            //it will check if the tile's placeholder is 's' then it will mark it as a miss 'x'
        }
    }
}
