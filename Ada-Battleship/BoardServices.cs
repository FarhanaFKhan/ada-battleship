using System;
using System.Text.RegularExpressions;

namespace Ada_Battleship
{
    public class BoardServices
    {
        //to place the ship
        //using tuple to be able to split the move in one function
        public (char, int,char) SplitMove(string userInput)
        {
            //needs to cater for A10
            string input = userInput.Trim().ToUpper();
            var columnLabel = input[0];
            var rowNumber = 0;
            var orientation = 'H';

            string[] digits = Regex.Split(userInput, @"\D+");
            foreach (var s in digits)
            {
                if (int.TryParse(s, out int number))
                {
                    rowNumber = number;
                }
            }

            if (Regex.Match(input, @"V$").Success)
            {
                orientation = 'V';
            }
            

            return (columnLabel, rowNumber, orientation);
        }


        public int AlphabetToInt(char columnLabel)
        {
            int alphabetToNum = columnLabel - 64;
            return alphabetToNum;
        }

        public void UpdateShipStatusToHit()
        {
            throw new NotImplementedException();
        }
    }
}