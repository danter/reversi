namespace aspa.reversi.Models
{
    public class Config
    {
        public AiPlayer Ai;
        public BoardHints Hints;
        public char Player;

        public override bool Equals(object obj)
        {
            if (!(obj is Config config))
            {
                return false;
            }

            return Equals(config);
        }

        protected bool Equals(Config other)
        {
            if (Ai != other.Ai)
            {
                return false;
            }

            if (Hints != other.Hints)
            {
                return false;
            }

            return Player == other.Player;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Ai;
                hashCode = (hashCode * 397) ^ (int) Hints;
                hashCode = (hashCode * 397) ^ Player.GetHashCode();
                return hashCode;
            }
        }
    }
}