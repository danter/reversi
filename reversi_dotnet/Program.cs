using System;
using System.Collections.Generic;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class Program
    {
        static void Main(string[] args)
        {
            var board = new char[Constants.BoardMax];
            var hints = new char[Constants.BoardMax];

            InitBoard(board);
            var config = ConfigHandler.ReadCommandLineArgumants(board, args);

            // AIScoreTable(temp);
            //DrawBoard(board, temp);

            // Calculate Hints
            switch (config.Hints)
            {
                // Todo: Implement HintPlayer and AIEvalBoard
                case BoardHints.Hints:
                    //HintPlayer(board, hints, config.Player);
                    break;
                case BoardHints.NumericHints:
                    //AIEvalBoard(board, hints, config.Player)
                    break;
                default:
                    for (var i = 0; i < Constants.BoardMax; i++)
                    {
                        hints[i] = ' ';
                    }

                    break;
            }

            Graphics.DrawBoard(board, hints);
            Console.WriteLine("BLACK score: " + CalculateScore(board, Constants.Black));
            Console.WriteLine("WHITE score: " + CalculateScore(board, Constants.White));

            RunGameLoop(config, board, hints);

            PrintScore(board);

            Console.ReadKey();
        }

        private static void RunGameLoop(Config config, char[] board, char[] hints)
        {
            var gameLogger = new GameLogger();

            var canBlackMove = true;
            var canWhiteMove = true;

            while (canBlackMove || canWhiteMove)
            {
                switch (config.Player)
                {
                    case Constants.Black:
                        Console.Write("BLACK, make your move: ");
                        break;
                    case Constants.White:
                        Console.Write("WHITE, make your move: ");
                        break;
                    default:
                        return;
                }

                var move = InputHandler.ReadInput(config, board, hints);

                gameLogger.WriteToGamelog(move);
                InputHandler.MakeMove(board, move, config.Player);
                InputHandler.PlacePiece(board, move, config.Player);

                // Change the current player
                config.Player = InputHandler.GetOtherPlayer(config.Player);

                // Test if players can move
                canBlackMove = CanPlayerMove(board, Constants.Black);
                canWhiteMove = CanPlayerMove(board, Constants.White);

                // And change player if current player can't
                if (config.Player == Constants.Black && !canBlackMove)
                {
                    Console.WriteLine("\nBLACK, can't make a move");
                    config.Player = Constants.White;
                }

                if (config.Player == Constants.White && !canWhiteMove)
                {
                    Console.WriteLine("\nWHITE, can't make a move");
                    config.Player = Constants.Black;
                }

                CalculateHints(config, board, hints, config.Player);

                Graphics.DrawBoard(board, hints);
            }
        }

        private static void PrintScore(char[] board)
        {
            var blackScore = CalculateScore(board, Constants.Black);
            var whiteScore = CalculateScore(board, Constants.White);

            Console.WriteLine("\nNeither BLACK nor WHITE can make a move, GAME OVER!");
            Console.WriteLine("The score was:");
            Console.WriteLine("BLACK: " + blackScore);
            Console.WriteLine("WHITE: " + whiteScore);

            if (blackScore == whiteScore)
            {
                Console.WriteLine("The game was a draw!");
            } else if (whiteScore < blackScore)
            {
                Console.WriteLine("Winner is BLACK!");
            }
            else
            {
                Console.WriteLine("Winner is WHITE!");
            }
        }

        private static void CalculateHints(Config config, char[] board, char[] hints, char player)
        {
            switch (config.Hints)
            {
                case BoardHints.Hints:
                    //HintPlayer(board, hints, player);
                    break;
                case BoardHints.NumericHints:
                    //AIEvalBoard(board, hints, player);
                    break;
                default:
                    for (var i = 0; i < hints.Length; i++)
                    {
                        hints[i] = ' ';
                    }

                    break;
            }
        }

        private static bool CanPlayerMove(char[] board, char player)
        {
            var playerCanMove = false;

            for (var y = 0; y < Constants.Row; y++)
            {
                for (var x = 0; x < Constants.Col && playerCanMove == false; x++)
                {
                    playerCanMove = InputHandler.IsValidMove(board, new Pos(x, y), player);
                }
            }

            return playerCanMove;
        }

        public static void InitBoard(IList<char> board)
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

        public static int CalculateScore(char[] board, char player)
        {
            var score = 0;

            for (var i = 0; i < Constants.BoardMax; i++)
            {
                if (board[i] == player)
                {
                    score++;
                }
            }

            return score;
        }

    }
}
