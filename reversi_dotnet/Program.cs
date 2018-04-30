using System;
using System.Collections.Generic;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class Program
    {
        static void Main(string[] args)
        {
            var gameBoard = new char[Constants.BoardMax];
            var hintBoard = new char[Constants.BoardMax];

            InitBoard(gameBoard);
            var config = ConfigHandler.ReadCommandLineArgumants(gameBoard, args);

            config.Hints = BoardHints.Hints;

            // AIScoreTable(temp);
            //DrawBoard(gameBoard, temp);

            RunGameLoop(config, gameBoard, hintBoard);

            Graphics.DrawBoard(gameBoard, hintBoard);
            PrintScore(gameBoard);

            Console.ReadKey();
        }

        private static void RunGameLoop(Config config, char[] gameBoard, char[] hintBoard)
        {
            var gameLogger = new GameLogger();

            var canBlackMove = true;
            var canWhiteMove = true;

            while (canBlackMove || canWhiteMove)
            {
                CalculateHints(config.Hints, gameBoard, hintBoard, config.Player);
                Graphics.DrawBoard(gameBoard, hintBoard);

                Console.WriteLine("BLACK score: " + CalculateScore(gameBoard, Constants.Black));
                Console.WriteLine("WHITE score: " + CalculateScore(gameBoard, Constants.White));

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

                var move = InputHandler.ReadInput(config, gameBoard, hintBoard);

                gameLogger.WriteToGamelog(move);
                InputHandler.MakeMove(gameBoard, move, config.Player);
                InputHandler.PlacePiece(gameBoard, move, config.Player);

                // Change the current player
                config.Player = InputHandler.GetOtherPlayer(config.Player);

                // Test if players can move
                canBlackMove = CanPlayerMove(gameBoard, Constants.Black);
                canWhiteMove = CanPlayerMove(gameBoard, Constants.White);

                // And change player if current player can't move
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
            }
        }

        private static void PrintScore(char[] gameBoard)
        {
            var blackScore = CalculateScore(gameBoard, Constants.Black);
            var whiteScore = CalculateScore(gameBoard, Constants.White);

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

        private static void CalculateHints(BoardHints hints, char[] gameBoard, char[] hintBoard, char player)
        {
            // Todo: Implement AIEvalBoard
            switch (hints)
            {
                case BoardHints.Hints:
                    InputHandler.HintPlayer(gameBoard, hintBoard, player);
                    break;
                case BoardHints.NumericHints:
                    //AIEvalBoard(gameBoard, hintBoard, player);
                    break;
                default:
                    for (var i = 0; i < hintBoard.Length; i++)
                    {
                        hintBoard[i] = ' ';
                    }

                    break;
            }
        }

        private static bool CanPlayerMove(char[] gameBoard, char player)
        {
            var playerCanMove = false;

            for (var y = 0; y < Constants.Row; y++)
            {
                for (var x = 0; x < Constants.Col && playerCanMove == false; x++)
                {
                    playerCanMove = InputHandler.IsValidMove(gameBoard, new Pos(x, y), player);
                }
            }

            return playerCanMove;
        }

        public static void InitBoard(IList<char> gameBoard)
        {
            for (var i = 0; i < gameBoard.Count; i++)
            {
                gameBoard[i] = ' ';
            }

            gameBoard[(Constants.Row / 2 - 1) * Constants.Row + (Constants.Col / 2 - 1)] = Constants.White;
            gameBoard[(Constants.Row / 2) * Constants.Row + (Constants.Col / 2)] = Constants.White;
            gameBoard[(Constants.Row / 2 - 1) * Constants.Row + (Constants.Col / 2)] = Constants.Black;
            gameBoard[(Constants.Row / 2) * Constants.Row + (Constants.Col / 2 - 1)] = Constants.Black;
        }

        public static int CalculateScore(char[] gameBoard, char player)
        {
            var score = 0;

            for (var i = 0; i < Constants.BoardMax; i++)
            {
                if (gameBoard[i] == player)
                {
                    score++;
                }
            }

            return score;
        }

    }
}
