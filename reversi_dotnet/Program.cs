using System;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class Program
    {
        static void Main(string[] args)
        {
            var gameBoard = new char[Constants.BoardMax];
            var hintBoard = new char[Constants.BoardMax];

            InputHandler.InitBoard(gameBoard);
            var config = ConfigHandler.ReadCommandLineArgumants(gameBoard, args);

            RunGameLoop(config, gameBoard, hintBoard);

            Graphics.DrawBoard(gameBoard, hintBoard);
            Graphics.PrintFinalScore(gameBoard);

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
                Graphics.PrintScore(gameBoard);

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
                canBlackMove = InputHandler.CanPlayerMove(gameBoard, Constants.Black);
                canWhiteMove = InputHandler.CanPlayerMove(gameBoard, Constants.White);

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

        private static void CalculateHints(BoardHints hints, char[] gameBoard, char[] hintBoard, char player)
        {
            switch (hints)
            {
                case BoardHints.Hints:
                    InputHandler.HintPlayer(gameBoard, hintBoard, player);
                    break;
                case BoardHints.NumericHints:
                    AiWithScoreTable.AiEvalBoard(gameBoard, hintBoard, player);
                    break;
                default:
                    for (var i = 0; i < hintBoard.Length; i++)
                    {
                        hintBoard[i] = ' ';
                    }

                    break;
            }
        }
    }
}
