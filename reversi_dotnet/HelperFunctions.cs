using System;
using System.Collections.Generic;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class HelperFunctions
    {

        public static void InitGame(IList<int> board)
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

        public static Pos ReadInput(Config config, int[] board, int[] hints)
        {
            var cord = new Pos();

            // Todo: Implement AIEvalBoard
            if (IsAiPlayerTurn(config))
            {
                //cord = AIEvalBoard(board, hints, config.Player);
                //fprintf(stdout, "%c%d\n", move.x + 65, move.y);
            }
            else
            {
                var input = ReadInput();
                cord = ReadMove(input);
                while (!IsValidMove(board, cord, config.Player))
                {
                    Console.WriteLine("You can't place a piece there!\n");

                    Console.WriteLine(config.Player == Constants.Black
                        ? "BLACK, make your move: "
                        : "WHITE, make your move: ");

                    input = ReadInput();
                    cord = ReadMove(input);
                }
            }

            return cord;
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

        public static Pos ReadMove(string input)
        {
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

        public static bool IsValidMove(int[] board, Pos readLine, int player)
        {
            throw new NotImplementedException();
        }

        public static bool IsAiPlayerTurn(Config config)
        {
            var ai = config.Ai;
            var player = config.Player;

            switch (ai)
            {
                case AiPlayer.BothAi:
                    return true;
                case AiPlayer.BlackAi when player == Constants.Black:
                    return true;
                case AiPlayer.WhiteAi when player == Constants.White:
                    return true;

                default:
                    return false;
            }
        }

        public static int CalcScore(int[] board, char player)
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