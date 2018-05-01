using System.IO;
using System.Linq;

namespace aspa.reversi.Models
{
    public class Board
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public char[] Data { get; set; }

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            Data = new char[width * height];
        }

        public Board(Board referenceBoard)
            : this(referenceBoard.Width, referenceBoard.Height)
        {

        }

        public int GetNumberOfPieces(char player)
        {
            return Data.Count(playerPiece => playerPiece == player);
        }

        public void ClearBoard(char clearCharacter)
        {
            for (var i = 0; i < Data.Length; i++)
            {
                Data[i] = clearCharacter;
            }
        }

        public void InitBoard()
        {
            for (var i = 0; i < Data.Length; i++)
            {
                Data[i] = ' ';
            }

            Data[(Width / 2 - 1) * Width + (Height / 2 - 1)] = Constants.White;
            Data[(Width / 2) * Width + (Height / 2)] = Constants.White;
            Data[(Width / 2 - 1) * Width + (Height / 2)] = Constants.Black;
            Data[(Width / 2) * Width + (Height / 2 - 1)] = Constants.Black;
        }

        public bool IsInsideBoard(Pos pos)
        {
            return IsInsideBoard(pos.X, pos.Y);
        }

        public bool IsInsideBoard(int x, int y)
        {
            if (x < 0)
                return false;

            if (x >= Width)
                return false;

            if (y < 0)
                return false;

            if (y >= Height)
                return false;

            return true;
        }


        public char GetPiece(Pos pos)
        {
            return GetPiece(pos.X, pos.Y);
        }

        public char GetPiece(int x, int y)
        {
            return Data[y * Width + x];
        }

        public void SetPiece(Pos pos, char piece)
        {
            SetPiece(pos.X, pos.Y, piece);
        }

        public void SetPiece(int x, int y, char piece)
        {
            Data[y * Width + x] = piece;
        }

        public Board TransformFromScoreBoard(int[] scoreBoard)
        {
            var outScoreBoard = new Board(this);

            if (scoreBoard.Length != Height * Width)
            {
                throw new InvalidDataException("provided scoreboard is not of the proper size");
            }

            for (var y = 0; y < outScoreBoard.Height; y++)
            {
                for (var x = 0; x < outScoreBoard.Width; x++)
                {
                    var currentPos = y * outScoreBoard.Width + x;
                    if (scoreBoard[currentPos] > 0)
                    {
                        var scoreToWrite = scoreBoard[currentPos];
                        outScoreBoard.SetPiece(x, y, Pos.ConvertToAsciiDigit(scoreToWrite));
                    }
                    else
                    {
                        outScoreBoard.SetPiece(x, y, ' ');
                    }

                }
            }

            return outScoreBoard;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Board board))
            {
                return false;
            }

            return Equals(board);

        }

        protected bool Equals(Board other)
        {
            if (Width != other.Width)
            {
                return false;
            }

            if (Height != other.Height)
            {
                return false;
            }

            if (Data.Length != other.Data.Length)
            {
                return false;
            }

            return !Data.Where((t, i) => t != other.Data[i]).Any();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Width;
                hashCode = (hashCode * 397) ^ Height;
                hashCode = (hashCode * 397) ^ (Data != null ? Data.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}