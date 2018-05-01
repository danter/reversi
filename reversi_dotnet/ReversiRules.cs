using System;
using System.Linq;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class ReversiRules
    {
        public Board GameBoard { get; set; }

        public char CurrentPlayer { get; set; }
        public Config GameConfig { get; set; }

        public ReversiRules(Config gameConfig)
        {
            GameConfig = gameConfig;
            GameBoard = new Board(gameConfig.BoardWidth, gameConfig.BoardHeight);
            GameBoard.InitBoard();

            CurrentPlayer = !string.IsNullOrEmpty(gameConfig.SaveGame) ? LoadSaveGame(gameConfig.SaveGame) : gameConfig.StartPlayer;
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

                    player = GetOtherPlayer(player);
                }
            }

            return player;
        }

        public void RunGameLoop()
        {
            var gameLogger = new GameLogger();

            var canBlackMove = true;
            var canWhiteMove = true;

            var hintBoard = new Board(GameBoard);
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
                    CurrentPlayer = GetOtherPlayer(CurrentPlayer);
                }

                if (CurrentPlayer == Constants.White && !canWhiteMove)
                {
                    Console.WriteLine("\nWHITE, can't make a move");
                    CurrentPlayer = GetOtherPlayer(CurrentPlayer);
                }
            }

            Graphics.DrawBoard(GameBoard, hintBoard);
            Graphics.PrintFinalScore(GameBoard);
        }

        public Board CalculateHints(BoardHints hints, char player)
        {
            switch (hints)
            {
                case BoardHints.Hints:
                    return HintPlayer(GameBoard, player);
                case BoardHints.NumericHints:
                    return AiWithScoreTable.GetNumericHints(GameBoard, player);
                default:
                    var hintBoard = new Board(GameBoard);
                    hintBoard.ClearBoard(' ');
                    return hintBoard;
            }
        }

        public bool CanPlayerMove(char player)
        {
            var playerCanMove = false;

            for (var y = 0; y < GameBoard.Height; y++)
            {
                for (var x = 0; x < GameBoard.Width && playerCanMove == false; x++)
                {
                    playerCanMove = IsValidMove(GameBoard, new Pos(x, y), player);
                }
            }

            return playerCanMove;
        }

        public static bool IsValidMove(Board gameBoard, Pos pos, char player)
        {
            if (!gameBoard.IsInsideBoard(pos))
                return false;

            if (gameBoard.GetPiece(pos) != ' ')
            {
                return false;
            }

            var otherPlayer = GetOtherPlayer(player);

            for (var y = pos.Y - 1; y <= pos.Y + 1; y++)
            {
                for (var x = pos.X - 1; x <= pos.X + 1; x++)
                {
                    if (!gameBoard.IsInsideBoard(x, y))
                    {
                        continue;
                    }

                    if (gameBoard.GetPiece(x, y) != otherPlayer)
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

        public static Board HintPlayer(Board gameBoard, char player)
        {
            var hints = new Board(gameBoard);

            for (var y = 0; y < hints.Height; y++)
            {
                for (var x = 0; x < hints.Width; x++)
                {
                    hints.SetPiece(x, y, IsValidMove(gameBoard, new Pos(x, y), player) ? Constants.Hint : ' ');
                }
            }

            return hints;
        }


        public void PlacePiece(Pos move, char currentPlayer)
        {
            GameBoard.SetPiece(move, currentPlayer);
        }

        // Validates the move and turns the pieces
        public void MakeMove(Pos move, char player)
        {
            if (GameBoard.GetPiece(move) != ' ')
            {
                return;
            }

            var otherPlayer = GetOtherPlayer(player);

            for (var y = move.Y - 1; y <= move.Y + 1; y++)
            {
                for (var x = move.X - 1; x <= move.X + 1; x++)
                {
                    if (!GameBoard.IsInsideBoard(x, y))
                    {
                        continue;
                    }

                    if (GameBoard.GetPiece(x, y ) != otherPlayer)
                    {
                        continue;
                    }

                    if (TraceMove(GameBoard, move, x - move.X, y - move.Y, player))
                    {
                        DoTraceMove(GameBoard, move, x - move.X, y - move.Y, player);
                    }
                }
            }
        }

        // Checks if a row of markers can be captured,
        // ie. if a marker of your own color is at the end of the row.
        // It takes the gameBoard, the current coordinate, the difference in x and y
        // and what value it should check for
        public static bool TraceMove(Board gameBoard, Pos move, int dx, int dy, char player)
        {
            var tMove = new Pos(move);

            for (tMove.Y += dy, tMove.X += dx; gameBoard.IsInsideBoard(tMove); tMove.X += dx, tMove.Y += dy)
            {
                if (gameBoard.GetPiece(tMove) == player)
                {
                    return true;
                }
                if (gameBoard.GetPiece(tMove) == ' ')
                {
                    return false;
                }
            }

            return false;
        }

        // Almost the same as TraceMove(), exept this methos actually performs the move
        // WARNING: TraceMove() should be used before this or pieces that
        // should not be turned will get turned. 
        private void DoTraceMove(Board gameBoard, Pos move, int dx, int dy, char player)
        {
            var tMove = new Pos(move);

            for (tMove.X += dx, tMove.Y += dy; gameBoard.IsInsideBoard(tMove); tMove.X += dx, tMove.Y += dy)
            {
                if (GameBoard.GetPiece(tMove) == player)
                {
                    return;
                }

                GameBoard.SetPiece(tMove, player);
            }
        }
    }
}