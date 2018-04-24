using System;
using System.Collections.Generic;
using System.IO;

namespace reversi_core
{
    public static class Constants
    {
        public const string LogName = "gamelog.txt";

        public const int Col = 8;
        public const int Row = 8;
        public const int BoardMax = Col * Row;

        public const char Black = '░';
        public const char White = '█';
        public const char Hint = '+';

    }

    public enum BoardHints
    {
        NoHints,
        Hints,
        NumericHints
    };

    public enum AiPlayer
    {
        NoAi,
        WhiteAi,
        BlackAi,
        BothAi
    };

    public class Cord
    {
        public int X;
        public int Y;
    }

    public class Settings
    {
        public AiPlayer Ai;
        public BoardHints Hints;
        public int Player;
    }

    public class Graphics
    {
        public static void DrawBoard(int[] board, int[] hints)
        {
            Console.WriteLine();
            Console.Write("   ");
            for (var i = 0; i < Constants.Col; i++)
                Console.Write((char)(i + 65) + " ");
            Console.Write(" \n");

            Console.Write("  ┌");
            for (var i = 0; i < Constants.Col - 1; i++)
                Console.Write("─┬");
            Console.Write("─┐\n");

            for (var y = 0; y < Constants.Row; y++)
            {
                Console.Write(" " + y + "│");
                for (var x = 0; x < Constants.Col; x++)
                {
                    var currentPos = y * Constants.Row + x;

                    if (board[currentPos] == ' ')
                    {
                        switch (hints[currentPos])
                        {
                            case 0:
                            case ' ':
                                Console.Write(" │");
                                break;
                            case Constants.Hint:
                                Console.Write(Constants.Hint + "│");
                                break;
                            default:
                                if (hints[currentPos] < 0)
                                {
                                    Console.Write((char)hints[currentPos] + "│");
                                }

                                break;
                        }
                    }
                    else
                    {
                        Console.Write((char)board[currentPos] + "│");
                    }

                }

                Console.WriteLine();

                if (y >= Constants.Row - 1)
                {
                    continue;
                }

                Console.Write("  ├");
                for (var i = 0; i < Constants.Col - 1; i++)
                    Console.Write("─┼");
                Console.Write("─│\n");
            }

            Console.Write("  └");
            for (var i = 0; i < Constants.Col - 1; i++)
                Console.Write("─┴");
            Console.WriteLine("─┘\n");
        }
    }


    internal class Program
    {

        static void Main(string[] args)
        {
            var canBlackMove = true;
            var canWhiteMove = true;

            var board = new int[Constants.BoardMax + 1];
            var hints = new int[Constants.BoardMax + 1];

            InitGame(board);
            var settings = ReadParameters(board, args, Constants.LogName);

            // AIScoreTable(temp);
            //DrawBoard(board, temp);

            // Calculate Hints
            switch (settings.Hints)
            {
                // Todo: Implement HintPlayer and AIEvalBoard
                case BoardHints.Hints:
                    //HintPlayer(board, hints, settings.Player);
                    break;
                case BoardHints.NumericHints:
                    //AIEvalBoard(board, hints, settings.Player)
                    break;
                default:
                    for (var i = 0; i < Constants.BoardMax; i++)
                    {
                        hints[i] = ' ';
                    }

                    break;
            }

            Graphics.DrawBoard(board, hints);
            Console.WriteLine("BLACK score: " + CalcScore(board, Constants.Black));
            Console.WriteLine("WHITE score: " + CalcScore(board, Constants.White));

            while (canBlackMove || canWhiteMove)
            {
                if (settings.Player == Constants.Black)
                {
                    
                }
            }
            Console.ReadKey();
        }


        private static int CalcScore(int[] board, char player)
        {
            var score = 0;

            for (var i = 0; i < Constants.BoardMax; i++)
            {
                if (board[i] == player)
                    score++;

            }

            return score;
        }

        private static Settings ReadParameters(int[] board, string[] arguments, string logFileName)
        {
            var settings = new Settings
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
                        settings.Ai = AiPlayer.WhiteAi;
                        continue;
                    case "-ai2":
                        settings.Ai = AiPlayer.BlackAi;
                        continue;
                    case "-ai3":
                        settings.Ai = AiPlayer.BothAi;
                        continue;
                    case "-ht":
                        settings.Hints = BoardHints.Hints;
                        continue;
                    case "-hn":
                        settings.Hints = BoardHints.NumericHints;
                        continue;
                    case "-l":
                        settings.Player = LoadGame(board, logFileName);
                        if (settings.Player == 0)
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

            return settings;
        }

        // Todo: Verify the magic numbers 65 and 48 when it's UTF16
        private static int LoadGame(int[] board, string logFile)
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

        private static void DisplayHelp()
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

        private static void InitGame(IList<int> board)
        {
            for (var i = 0; i < board.Count; i++)
            {
                board[i] = ' ';
            }

            board[(Constants.Row / 2 - 1) * Constants.Row + (Constants.Col / 2 - 1)] = Constants.White;
            board[(Constants.Row / 2) * Constants.Row + (Constants.Col / 2)] = Constants.White;
            board[(Constants.Row / 2 - 1) * Constants.Row + (Constants.Col / 2)] = Constants.Black;
            board[(Constants.Row / 2) * Constants.Row + (Constants.Col / 2 - 1)] = Constants.Black;
        }
    }
}
