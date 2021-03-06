﻿using System;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigHandler.ReadCommandLineArgumants(args);
            var reversi = new ReversiRules(config);

            reversi.RunGameLoop();

            Console.ReadKey();
        }

    }
}
