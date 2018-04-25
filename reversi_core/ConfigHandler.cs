using System;
using System.IO;
using aspa.reversi.Models;
using reversi_core;

namespace aspa.reversi
{
    public class ConfigHandler
    {
        public static void DisplayHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Usage> reversi -par1 -par2 -l <filename>");
            Console.WriteLine("-h\t\tShow this help.\n");
            Console.WriteLine("-ht\t\tShow hints.");
            Console.WriteLine("-hn\t\tShow numeric hints that the ai plays after.\n");
            Console.WriteLine("-ai\t\tPlay against an ai.\n");
            Console.WriteLine("-ai1\t\tWHITE player is an ai.\n");
            Console.WriteLine("-ai2\t\tBLACK player is an ai.\n");
            Console.WriteLine("-ai3\t\tLet 2 ai's play against eachother.\n");
            Console.WriteLine("-l <file>\tLoads the saved game <file>.\n");
        }

        public static Config ReadCommandLineArgumants(int[] board, string[] arguments, string logFileName)
        {
            var config = new Config
            {
                Ai = AiPlayer.NoAi,
                Hints = BoardHints.NoHints,
                Player = Constants.Black,
            };

            foreach (var argument in arguments)
            {
                if (argument.Contains("-h"))
                {
                    DisplayHelp();
                    Environment.Exit(0);
                }

                switch (argument)
                {
                    case "-ai":
                    case "-ai1":
                        config.Ai = AiPlayer.WhiteAi;
                        continue;
                    case "-ai2":
                        config.Ai = AiPlayer.BlackAi;
                        continue;
                    case "-ai3":
                        config.Ai = AiPlayer.BothAi;
                        continue;
                    case "-ht":
                        config.Hints = BoardHints.Hints;
                        continue;
                    case "-hn":
                        config.Hints = BoardHints.NumericHints;
                        continue;
                    case "-l":
                        config.Player = LoadGame(board, logFileName);
                        if (config.Player == 0)
                        {
                            Console.WriteLine("File load failed, exiting.\n");
                            DisplayHelp();
                            Environment.Exit(1);
                        }
                        continue;

                    default:
                        Console.WriteLine("Unknown parameter: " + argument);
                        DisplayHelp();
                        Environment.Exit(1);
                        break;
                }
            }

            return config;
        }

        // Todo: Verify the magic numbers 65 and 48 when it's UTF16
        public static int LoadGame(int[] board, string logFile)
        {
            var logLine = "";
            if (File.Exists(logFile))
            {
                logLine = File.ReadAllText(Constants.LogName);
            }

            var player = Constants.Black;

            foreach (var character in logLine)
            {
                var coord = new Cord();

                if (char.IsLetter(character))
                {
                    coord.X = character - 65;
                }
                else if (char.IsDigit(character))
                {
                    coord.Y = character - 48;
                }
                else
                {
                    //DoMove(board, coord, player);
                    //DrawPiece(board, coord, player);

                    player = player == Constants.Black ? Constants.White : Constants.Black;
                }

            }

            return player;
        }
    }
}