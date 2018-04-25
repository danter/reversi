namespace aspa.reversi.Models
{
    public class Pos
    {
        public int X;
        public int Y;

        public override string ToString()
        {
            return Converters.ConvertPosToLetter(X) + Y;
        }
    }
}