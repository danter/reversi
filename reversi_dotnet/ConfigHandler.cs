using System;
using System.IO;
using aspa.reversi.Models;

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

        public static Config ReadCommandLineArgumants(char[] board, string[] arguments)
        {
            var config = new Config
            {
                Ai = AiPlayer.NoAi,
                Hints = BoardHints.NoHints,
                Player = Constants.Black,
            };

            for (var index = 0; index < arguments.Length; index++)
            {
                var argument = arguments[index];
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
                        var saveFile = arguments[++index];
                        config.Player = LoadGame(board, saveFile);
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
        public static char LoadGame(char[] board, string saveFile)
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

            var player = Constants.Black;

            foreach (var character in logLine)
            {
                var coord = new Pos();

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
                    // Todo: implement DoMove and DrawPiece
                    //DoMove(board, coord, player);
                    //DrawPiece(board, coord, player);

                    player = player == Constants.Black ? Constants.White : Constants.Black;
                }

            }

            return player;
        }
    }
}