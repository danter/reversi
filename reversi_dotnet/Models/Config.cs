namespace aspa.reversi.Models
{
    public class Config
    {
        public AiPlayer Ai;
        public BoardHints Hints;
        public char Player;
        public int BoardRows;
        public int BoardColumns;
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

            if (Player != other.Player)
            {
                return false;
            }

            if (BoardRows != other.BoardRows)
            {
                return false;
            }

            if (BoardColumns != other.BoardColumns)
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
                hashCode = (hashCode * 397) ^ Player.GetHashCode();
                hashCode = (hashCode * 397) ^ BoardRows;
                hashCode = (hashCode * 397) ^ BoardColumns;
                hashCode = (hashCode * 397) ^ (SaveGame != null ? SaveGame.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}