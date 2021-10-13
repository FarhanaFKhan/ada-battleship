﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Ada_Battleship
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

       public List<ShipConfig> ShipDetails = new List<ShipConfig>();





       public static Setup Instance
       {
           get
           {
               Console.WriteLine("Instance called");
               return _lazy.Value;
           }
       }

       private Setup()
       {
           Console.WriteLine("Constructor invoked.");
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

                   if (property.Contains("Board"))
                   {
                       var value = splitLine[1].Trim();
                       string [] boardDimensions = value.Split(new char[] { 'x' }, 2);
                       BoardWidth = int.Parse(boardDimensions[0]);
                       BoardHeight = int.Parse(boardDimensions[1]);
                   }

                    if (property.Contains("Boat"))
                    {
                        string value = splitLine[1].Trim();
                        string[] shipInformation = value.Split(new char[] { ',' }, 2);
                        _shipName = shipInformation[0];
                        _shipLength = int.Parse(shipInformation[1]);
                        ShipDetails.Add(new ShipConfig() { ShipName = _shipName, ShipLength = _shipLength });
                    }

               }
               catch
               {
                   throw new Exception("Invalid");
               }
           }

        }



    }
}