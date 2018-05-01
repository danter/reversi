namespace aspa.reversi.Models
{
    public class Config
    {
        public Player Ai;
        public BoardHints Hints;
        public char StartPlayer;
        public int BoardWidth;
        public int BoardHeight;
        public string SaveGame;

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

            if (StartPlayer != other.StartPlayer)
            {
                return false;
            }

            if (BoardWidth != other.BoardWidth)
            {
                return false;
            }

            if (BoardHeight != other.BoardHeight)
            {
                return false;
            }

            return SaveGame == other.SaveGame;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Ai;
                hashCode = (hashCode * 397) ^ (int)Hints;
                hashCode = (hashCode * 397) ^ StartPlayer.GetHashCode();
                hashCode = (hashCode * 397) ^ BoardWidth;
                hashCode = (hashCode * 397) ^ BoardHeight;
                hashCode = (hashCode * 397) ^ (SaveGame != null ? SaveGame.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}