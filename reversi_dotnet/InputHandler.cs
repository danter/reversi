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

        public static Pos ReadInput(Config config, char[] board, char[] hints)
        {
            var move = new Pos();

            // Todo: Implement AIEvalBoard
            if (IsAiPlayerTurn(config))
            {
                //move = AIEvalBoard(board, hints, config.Player);
                //fprintf(stdout, "%c%d\n", move.x + 65, move.y);
            }
            else
            {
                var input = ReadInput();
                move = ReadMove(input);
                while (!IsValidMove(board, move, config.Player))
                {
                    Console.WriteLine("You can't place a piece there!\n");

                    Console.WriteLine(config.Player == Constants.Black
                        ? "BLACK, make your move: "
                        : "WHITE, make your move: ");

                    input = ReadInput();
                    move = ReadMove(input);
                }
            }

            return move;
        }

        private static char GetOtherPlayer(char player)
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

        public static bool IsValidMove(char[] board, Pos pos, char player)
        {
            if (pos.X < 0)
                return false;
            if (pos.X > Constants.Col - 1)
                return false;
            if (pos.Y < 0)
                return false;
            if (pos.Y > Constants.Row - 1)
                return false;

            if (board[pos.Y * Constants.Row + pos.X] != ' ')
            {
                return false;
            }

            var otherPlayer = GetOtherPlayer(player);

            for (var y = 0; y <= pos.Y + 1; y++)
            {
                for (var x = 0; x <= pos.X + 1; x++)
                {
                    if (!IsInsideBoard(x, y))
                    {
                        continue;
                    }

                    if (board[y * Constants.Row + x] != otherPlayer)
                    {
                        continue;
                    }

                    if (TraceMove(board, pos, x - pos.X, y - pos.Y, player))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static void PlacePiece(char[] board, Pos move, char currentPlayer)
        {
            board[move.Y * Constants.Row + move.X] = currentPlayer;
        }

        // Validates the move and turns the pieces
        public static void MakeMove(char[] board, Pos move, char player)
        {
            if (board[move.Y * Constants.Row + move.X] != ' ')
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

                    if (board[y * Constants.Row + x] != otherPlayer)
                    {
                        continue;
                    }

                    if (TraceMove(board, move, x-move.X, y-move.Y, player))
                    {
                        DoTraceMove(board, move, x-move.X, y-move.Y, player);
                    }
                }
            }
        }

        // Checks if a row of markers can be captured,
        // ie. if a marker of your own color is at the end of the row.
        // It takes the board, the current coordinate, the difference in x and y
        // and what value it should check for
        private static bool TraceMove(char[] board, Pos move, int dx, int dy, char player)
        {
            var tMove = new Pos(move);

            for (tMove.Y += dy, tMove.X += dx; IsInsideBoard(tMove.X, tMove.Y); tMove.X += dx, tMove.Y+=dy)
            {
                var tracePos = tMove.Y * Constants.Row + tMove.X;
                if (board[tracePos] == player)
                {
                    return true;
                }
                if (board[tracePos] == ' ')
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

            for (tMove.X+=dx, tMove.Y+=dy; IsInsideBoard(tMove.X, tMove.Y); tMove.X+=dx, tMove.Y += dy)
            {
                if (board[tMove.Y*Constants.Row+tMove.X] == player)
                {
                    return;
                }

                board[tMove.Y * Constants.Row + tMove.X] = player;
            }
        }

        private static bool IsInsideBoard(int x, int y)
        {
            if (y >= Constants.Row)
                return false;

            if (y < 0)
                return false;

            if (x >= Constants.Col)
                return false;

            return x >= 0;
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

    }
}