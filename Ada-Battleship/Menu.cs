using System;
using System.Collections.Generic;
using Ada_Battleship.Configurations;
namespace Ada_Battleship
{
    public class Menu
    {
       
        private readonly MenuServices _menuServices = new MenuServices();
        private readonly PvC _pvc = new PvC();
        private readonly PvP _pvp = new PvP();
        private readonly CvC _cvc = new CvC();



        private int _mainOption;


        public void MainMenu()
        {

           

            Console.WriteLine("Select Game Mode:");
            Console.WriteLine("1.Player v Comp.");
            Console.WriteLine("2.Player v Player.");
            Console.WriteLine("3.Computer v Computer.");
            Console.WriteLine("4.Quit.");

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
                    _pvp.Menu();
                    break;
                case 3:
                    _cvc.Menu();
                    break;
                case 4:
                    Console.WriteLine("Quit Game");
                    break;
                default:
                    _pvc.PvCMenu();
                    break;
            }

        }

        

        

    }
}