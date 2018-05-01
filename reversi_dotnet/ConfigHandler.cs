using System;
using System.IO;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class ConfigHandler
    {
        public static int DefaultHeight = 8;
        public static int DefaultWidth = 8;

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

        public static Config ReadCommandLineArgumants(string[] arguments)
        {
            var config = new Config
            {
                Ai = Player.None,
                Hints = BoardHints.NoHints,
                StartPlayer = Constants.Black,
                BoardWidth = DefaultWidth,
                BoardHeight = DefaultHeight,
            };

            for (var index = 0; index < arguments.Length; index++)
            {
                var argument = arguments[index];
                if (argument.Equals("-h"))
                {
                    DisplayHelp();
                    Environment.Exit(0);
                }

                switch (argument)
                {
                    case "":
                        continue;
                    case "-ai":
                    case "-ai1":
                        config.Ai = Player.White;
                        continue;
                    case "-ai2":
                        config.Ai = Player.Black;
                        continue;
                    case "-ai3":
                        config.Ai = Player.Both;
                        continue;
                    case "-ht":
                        config.Hints = BoardHints.Hints;
                        continue;
                    case "-hn":
                        config.Hints = BoardHints.NumericHints;
                        continue;
                    case "-l":
                        var saveFile = arguments[++index];
                        config.SaveGame = LoadSaveFile(saveFile);
                        if (config.StartPlayer == 0)
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

        public static string LoadSaveFile(string saveFile)
        {
            var logLine = "";
            if (File.Exists(saveFile))
            {
                logLine = File.ReadAllText(saveFile);
            }
            else
            {
                Console.WriteLine("Invalid savefile: " + saveFile);
                DisplayHelp();
                Environment.Exit(1);
            }

            return logLine;
        }
    }
}