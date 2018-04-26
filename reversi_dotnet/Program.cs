using System;
using aspa.reversi.Models;

namespace aspa.reversi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var board = new char[Constants.BoardMax + 1];
            var hints = new char[Constants.BoardMax + 1];

            HelperFunctions.InitGame(board);
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
            Console.WriteLine("BLACK score: " + HelperFunctions.CalcScore(board, Constants.Black));
            Console.WriteLine("WHITE score: " + HelperFunctions.CalcScore(board, Constants.White));

            RunGameLoop(config, board, hints);

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

                var move = HelperFunctions.ReadInput(config, board, hints);

                gameLogger.WriteToGamelog(move);

                // Todo: Continue implementing the main loop
            }
        }
    }
}
