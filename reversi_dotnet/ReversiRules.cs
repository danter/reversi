using System;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class ReversiRules
    {
        public readonly int MaxRows;
        public readonly int MaxCols;
        public char[] GameBoard { get; set; }
        public char CurrentPlayer { get; set; }
        public Config GameConfig { get; set; }

        public ReversiRules(Config gameConfig)
        {
            GameConfig = gameConfig;
            MaxRows = gameConfig.BoardRows;
            MaxCols = gameConfig.BoardColumns;
            GameBoard = InitBoard();

            if (!string.IsNullOrEmpty(gameConfig.SaveGame))
            {
                CurrentPlayer = LoadSaveGame(gameConfig.SaveGame);
            }
            else
            {
                CurrentPlayer = gameConfig.Player;
            }
        }

        public char[] InitBoard()
        {
            var gameBoard = new char[MaxRows * MaxCols];

            for (var i = 0; i < gameBoard.Length; i++)
            {
                gameBoard[i] = ' ';
            }

            gameBoard[(MaxRows / 2 - 1) * MaxRows + (MaxCols / 2 - 1)] = Constants.White;
            gameBoard[(MaxRows / 2) * MaxRows + (MaxCols / 2)] = Constants.White;
            gameBoard[(MaxRows / 2 - 1) * MaxRows + (MaxCols / 2)] = Constants.Black;
            gameBoard[(MaxRows / 2) * MaxRows + (MaxCols / 2 - 1)] = Constants.Black;

            return gameBoard;
        }

        public char LoadSaveGame(string saveGame)
        {
            var player = Constants.Black;
            var move = new Pos();

            foreach (var character in saveGame)
            {
                if (char.IsLetter(character))
                {
                    move.X = Pos.ConvertLetter(character);
                }
                else if (char.IsDigit(character))
                {
                    move.Y = Pos.ConvertFromAsciiDigit(character);
                }
                else
                {
                    MakeMove(move, player);
                    PlacePiece(move, player);

                    player = player == Constants.Black ? Constants.White : Constants.Black;
                }
            }

            return player;
        }

        public void RunGameLoop()
        {
            var gameLogger = new GameLogger();

            var canBlackMove = true;
            var canWhiteMove = true;

            var hintBoard = new char[GameBoard.Length];
            while (canBlackMove || canWhiteMove)
            {
                hintBoard = CalculateHints(GameConfig.Hints, CurrentPlayer);
                Graphics.DrawBoard(GameBoard, hintBoard);
                Graphics.PrintScore(GameBoard);
                Graphics.AnnouncePlayerMove(CurrentPlayer);

                var move = InputHandler.ReadInput(GameConfig, GameBoard, CurrentPlayer);

                gameLogger.WriteToGamelog(move);
                MakeMove(move, CurrentPlayer);
                PlacePiece(move, CurrentPlayer);

                // Change the current player
                CurrentPlayer = GetOtherPlayer(CurrentPlayer);

                // Test if players can move
                canBlackMove = CanPlayerMove(Constants.Black);
                canWhiteMove = CanPlayerMove(Constants.White);

                // And change player if current player can't move
                if (CurrentPlayer == Constants.Black && !canBlackMove)
                {
                    Console.WriteLine("\nBLACK, can't make a move");
                    CurrentPlayer = Constants.White;
                }

                if (CurrentPlayer == Constants.White && !canWhiteMove)
                {
                    Console.WriteLine("\nWHITE, can't make a move");
                    CurrentPlayer = Constants.Black;
                }
            }

            Graphics.DrawBoard(GameBoard, hintBoard);
            Graphics.PrintFinalScore(GameBoard);
        }

        public char[] CalculateHints(BoardHints hints, char player)
        {
            switch (hints)
            {
                case BoardHints.Hints:
                    return HintPlayer(player);
                case BoardHints.NumericHints:
                    return AiWithScoreTable.GetNumericHints(GameBoard, player);
                default:
                    var hintBoard = new char[GameBoard.Length];
                    for (var i = 0; i < hintBoard.Length; i++)
                    {
                        hintBoard[i] = ' ';
                    }

                    return hintBoard;
            }
        }

        public static int CalculateScore(char[] gameBoard, char player)
        {
            var score = 0;

            for (var i = 0; i < gameBoard.Length; i++)
            {
                if (gameBoard[i] == player)
                {
                    score++;
                }
            }

            return score;
        }

        public bool CanPlayerMove(char player)
        {
            var playerCanMove = false;

            for (var y = 0; y < MaxRows; y++)
            {
                for (var x = 0; x < MaxCols && playerCanMove == false; x++)
                {
                    playerCanMove = IsValidMove(GameBoard, new Pos(x, y), player);
                }
            }

            return playerCanMove;
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

            for (var y = pos.Y - 1; y <= pos.Y + 1; y++)
            {
                for (var x = pos.X - 1; x <= pos.X + 1; x++)
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

        public char[] HintPlayer(char player)
        {
            return HintPlayer(GameBoard, player);
        }

        public static char[] HintPlayer(char[] gameBoard, char player)
        {
            var hints = new char[gameBoard.Length];

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

            return hints;
        }


        public void PlacePiece(Pos move, char currentPlayer)
        {
            GameBoard[move.Y * Constants.Row + move.X] = currentPlayer;
        }

        // Validates the move and turns the pieces
        public void MakeMove(Pos move, char player)
        {
            if (GameBoard[move.Y * Constants.Row + move.X] != ' ')
            {
                return;
            }

            var otherPlayer = GetOtherPlayer(player);

            for (var y = move.Y - 1; y <= move.Y + 1; y++)
            {
                for (var x = move.X - 1; x <= move.X + 1; x++)
                {
                    if (!IsInsideBoard(x, y))
                    {
                        continue;
                    }

                    if (GameBoard[y * Constants.Row + x] != otherPlayer)
                    {
                        continue;
                    }

                    if (TraceMove(GameBoard, move, x - move.X, y - move.Y, player))
                    {
                        DoTraceMove(move, x - move.X, y - move.Y, player);
                    }
                }
            }
        }

        // Checks if a row of markers can be captured,
        // ie. if a marker of your own color is at the end of the row.
        // It takes the gameBoard, the current coordinate, the difference in x and y
        // and what value it should check for
        public static bool TraceMove(char[] gameBoard, Pos move, int dx, int dy, char player)
        {
            var tMove = new Pos(move);

            for (tMove.Y += dy, tMove.X += dx; IsInsideBoard(tMove); tMove.X += dx, tMove.Y += dy)
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
        private void DoTraceMove(Pos move, int dx, int dy, char player)
        {
            var tMove = new Pos(move);

            for (tMove.X += dx, tMove.Y += dy; IsInsideBoard(tMove); tMove.X += dx, tMove.Y += dy)
            {
                if (GameBoard[tMove.Y * Constants.Row + tMove.X] == player)
                {
                    return;
                }

                GameBoard[tMove.Y * Constants.Row + tMove.X] = player;
            }
        }

        public static bool IsInsideBoard(Pos pos)
        {
            return IsInsideBoard(pos.X, pos.Y);
        }

        public static bool IsInsideBoard(int x, int y)
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
    }
}