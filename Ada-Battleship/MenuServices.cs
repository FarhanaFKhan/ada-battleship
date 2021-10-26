using System;

namespace Ada_Battleship
{
    public class MenuServices
    {
        public char ToggleOrientation()
        {
            var rand = new Random();
            var number = rand.Next(1, 3);
            var orientation = number == 1 ? 'V' : 'H';

            return orientation;
        }

        public void PlayerShootTorpedo()
        {
            Console.WriteLine();

        }
    }
}