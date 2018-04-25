namespace aspa.reversi.Models
{
    public class Pos
    {
        private const int AsciiCharacterA = 65;

        public int X;
        public int Y;

        public static int ConvertLetter(char character)
        {
            return character - AsciiCharacterA;
        }

        public static string ConvertPosToLetter(int pos)
        {
            return ((char)(pos + AsciiCharacterA)).ToString();
        }

        public override string ToString()
        {
            return ConvertPosToLetter(X) + Y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Pos pos))
            {
                return false;
            }

            return Equals(pos);
        }

        protected bool Equals(Pos other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }
}