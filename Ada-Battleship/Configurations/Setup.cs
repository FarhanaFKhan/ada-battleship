using System;
using System.Collections.Generic;
using System.IO;

namespace Ada_Battleship.Configurations
{
    public sealed class Setup
    {
        //by using this approach we don't have to worry about checking the instance for null
        //The Lazy<T>type handles this for us in a thread safe manner.
        //also has performance and thread safety characteristics
        public static readonly Lazy<Setup> _lazy = new Lazy<Setup>(() => new Setup());


        //public static string ShipName { get; set; }
        private static readonly string ConfigFile = "Setup-config.ini";

        public int BoardWidth { get; private set; }

        public int BoardHeight { get; private set; }

        private string _shipName;
        private int _shipLength;

        public List<Ship> ShipDetailsP1 = new List<Ship>();
        public List<Ship> ShipDetailsP2 = new List<Ship>();

        public static Setup Instance => _lazy.Value;

        private Setup()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\t\t\t\t\t*****ADA_BATTLESHIP*****");
            Console.ResetColor();
            Console.WriteLine();
            FileParser();
        }

        private void FileParser()
        {
            string[] configurations = File.ReadAllLines(ConfigFile);
            foreach (var line in configurations)
            {
                try
                {
                    string[] splitLine = line.Split(new char[] { ':' }, 2);
                    if (splitLine.Length != 2)
                    {
                        continue;
                    }

                    var property = splitLine[0].Trim();
                    string value = splitLine[1].Trim();

                    if (property.Contains("Board"))
                    {
                        string[] boardDimensions = value.Split(new char[] { 'x' }, 2);
                        BoardWidth = int.Parse(boardDimensions[0]);
                        BoardHeight = int.Parse(boardDimensions[1]);
                    }

                    if (property.Contains("Boat"))
                    {


                        string[] shipInformation = value.Split(new char[] { ',' }, 2);
                        if (!ShipDetailsP1.Exists(x => x.ShipName == shipInformation[0]) && !ShipDetailsP2.Exists(x => x.ShipName == shipInformation[0]))
                        {
                            _shipName = shipInformation[0];
                            _shipLength = int.Parse(shipInformation[1]);
                            ShipDetailsP1.Add(new Ship(_shipName,_shipLength));
                            ShipDetailsP2.Add(new Ship(_shipName, _shipLength));

                        }

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw new InvalidOperationException();

                }

            }

        }



    }
}