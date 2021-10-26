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
            
            //another instance of board for 'shot' board
            //it will check if the tile's placeholder is 's' then it will mark it as a miss 'x'
        }
    }
}
