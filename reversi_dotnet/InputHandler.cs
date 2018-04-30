using System;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class InputHandler
    {
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

        public static Pos ReadInput(Config config, char[] gameBoard, char[] hintBoard)
        {
            var move = new Pos();

            // Todo: Implement AIEvalBoard
            if (IsAiPlayerTurn(config))
            {
                //move = AIEvalBoard(gameBoard, hintBoard, config.Player);
                //fprintf(stdout, "%c%d\n", move.x + 65, move.y);
            }
            else
            {
                var input = ReadInput();
                move = ReadMove(input);
                while (!IsValidMove(gameBoard, move, config.Player))
                {
                    Console.WriteLine("You can't place a piece there!\n");

                    Console.Write(config.Player == Constants.Black
                        ? "BLACK, make your move: "
                        : "WHITE, make your move: ");

                    input = ReadInput();
                    move = ReadMove(input);
                }
            }

            return move;
        }

        public static char GetOtherPlayer(char player)
        {
            var otherPlayer = '\0';
            switch (player)
            {
                case Constants.Black:
                    otherPlayer = Constants.White;
                    break;
                case Constants.White:
                    otherPlayer = Constants.Black;
                    break;
                default:
                    Console.WriteLine("Unknown piece color in IsValidMove(): exiting");
                    Environment.Exit(1);
                    break;
            }

            return otherPlayer;
        }

        public static bool IsValidMove(char[] gameBoard, Pos pos, char player)
        {
            if (!IsInsideBoard(pos))
                return false;

            if (gameBoard[pos.Y * Constants.Row + pos.X] != ' ')
            {
                return false;
            }

            var otherPlayer = GetOtherPlayer(player);

            for (var y = pos.Y-1; y <= pos.Y + 1; y++)
            {
                for (var x = pos.X-1; x <= pos.X + 1; x++)
                {
                    if (!IsInsideBoard(x, y))
                    {
                        continue;
                    }

                    if (gameBoard[y * Constants.Row + x] != otherPlayer)
                    {
                        continue;
                    }

                    if (TraceMove(gameBoard, pos, x - pos.X, y - pos.Y, player))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static void PlacePiece(char[] gameBoard, Pos move, char currentPlayer)
        {
            gameBoard[move.Y * Constants.Row + move.X] = currentPlayer;
        }

        // Validates the move and turns the pieces
        public static void MakeMove(char[] gameBoard, Pos move, char player)
        {
            if (gameBoard[move.Y * Constants.Row + move.X] != ' ')
            {
                return;
            }

            var otherPlayer = GetOtherPlayer(player);

            for (var y = move.Y-1; y <= move.Y+1; y++)
            {
                for (var x = move.X-1; x <= move.X+1; x++)
                {
                    if (!IsInsideBoard(x, y))
                    {
                        continue;
                    }

                    if (gameBoard[y * Constants.Row + x] != otherPlayer)
                    {
                        continue;
                    }

                    if (TraceMove(gameBoard, move, x-move.X, y-move.Y, player))
                    {
                        DoTraceMove(gameBoard, move, x-move.X, y-move.Y, player);
                    }
                }
            }
        }

        // Checks if a row of markers can be captured,
        // ie. if a marker of your own color is at the end of the row.
        // It takes the gameBoard, the current coordinate, the difference in x and y
        // and what value it should check for
        private static bool TraceMove(char[] gameBoard, Pos move, int dx, int dy, char player)
        {
            var tMove = new Pos(move);

            for (tMove.Y += dy, tMove.X += dx; IsInsideBoard(tMove); tMove.X += dx, tMove.Y+=dy)
            {
                var tracePos = tMove.Y * Constants.Row + tMove.X;
                if (gameBoard[tracePos] == player)
                {
                    return true;
                }
                if (gameBoard[tracePos] == ' ')
                {
                    return false;
                }
            }

            return false;
        }

        // Almost the same as TraceMove(), exept this methos actually performs the move
        // WARNING: TraceMove() should be used before this or pieces that
        // should not be turned will get turned. 
        private static void DoTraceMove(char[] board, Pos move, int dx, int dy, char player)
        {
            var tMove = new Pos(move);

            for (tMove.X+=dx, tMove.Y+=dy; IsInsideBoard(tMove); tMove.X+=dx, tMove.Y += dy)
            {
                if (board[tMove.Y*Constants.Row+tMove.X] == player)
                {
                    return;
                }

                board[tMove.Y * Constants.Row + tMove.X] = player;
            }
        }

        private static bool IsInsideBoard(Pos pos)
        {
            return IsInsideBoard(pos.X, pos.Y);
        }

        private static bool IsInsideBoard(int x, int y)
        {
            if (x < 0)
                return false;

            if (x >= Constants.Col)
                return false;

            if (y < 0)
                return false;

            if (y >= Constants.Row)
                return false;

            return true;
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

        public static void HintPlayer(char[] gameBoard, char[] hints, char player)
        {
            for (var y = 0; y < Constants.Row; y++)
            {
                for (var x = 0; x < Constants.Col; x++)
                {
                    var currentPos = y * Constants.Row + x;

                    if (IsValidMove(gameBoard, new Pos(x, y), player))
                    {
                        hints[currentPos] = Constants.Hint;
                    }
                    else
                    {
                        hints[currentPos] = ' ';
                    }
                }
            }
        }
    }
}