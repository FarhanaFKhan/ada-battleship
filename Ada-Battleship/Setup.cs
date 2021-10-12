using System;
using System.IO;
using System.Reflection;

namespace Ada_Battleship
{
    public sealed class Setup
    {
        public static int Board { get; set; }
        public static string ShipName { get; set; }
        private static readonly string ConfigFile = "Setup-config.ini";
        public static readonly Setup Instance = new Setup();
        Setup() {}

        static Setup()
        {
            string[] configurations = File.ReadAllLines(ConfigFile);
            foreach (var line in configurations)
            {
                try
                {
                    string[] split = line.Split(new char[] { ':' }, 2);
                    if (split.Length != 2)
                    {
                        continue;
                    }

                    var property = split[0].Trim();
                    string value = split[1].Trim();
                    Console.WriteLine(property);
                    Console.WriteLine(value);
                }
                catch
                {
                    throw new Exception("Invalid");
                }
            }
        }


    }
}