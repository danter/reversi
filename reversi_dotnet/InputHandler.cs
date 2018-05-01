using System;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class InputHandler
    {
        public static bool IsAiPlayerTurn(Config config, char currentPlayer)
        {
            var ai = config.Ai;

            switch (ai)
            {
                case AiPlayer.BothAi:
                    return true;
                case AiPlayer.BlackAi when currentPlayer == Constants.Black:
                    return true;
                case AiPlayer.WhiteAi when currentPlayer == Constants.White:
                    return true;

                default:
                    return false;
            }
        }

        public static Pos ReadInput(Config config, char[] gameBoard, char currentPlayer)
        {
            if (IsAiPlayerTurn(config, currentPlayer))
            {
                var move = AiWithScoreTable.GetAiMove(gameBoard, currentPlayer);
                Console.WriteLine(move);
                return move;
            }
            else
            {
                var input = ReadInput();
                var move = ParseMove(input);
                while (!ReversiRules.IsValidMove(gameBoard, move, currentPlayer))
                {
                    Console.WriteLine("You can't place a piece there!\n");

                    Console.Write(currentPlayer == Constants.Black
                        ? "BLACK, make your move: "
                        : "WHITE, make your move: ");

                    input = ReadInput();
                    move = ParseMove(input);
                }

                return move;
            }
        }

        public static string ReadInput()
        {
            string input = null;
            while (input == null)
            {
                input = Console.ReadLine();
            }

            return input;
        }

        public static Pos ParseMove(string input)
        {
            if (input.Contains("X") || input.Contains("x"))
            {
                Console.WriteLine("\n Termination character X retrieved, ending game!");
                Environment.Exit(0);
            }

            var cord = new Pos();

            foreach (var character in input)
            {
                if (char.IsLetter(character))
                {
                    cord.X = Pos.ConvertLetter(character);
                }

                if (char.IsDigit(character))
                {
                    var str = character.ToString();
                    cord.Y = int.Parse(str);
                }
            }

            return cord;
        }

    }
}