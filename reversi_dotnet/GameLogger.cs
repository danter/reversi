using System.IO;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class GameLogger
    {
        public const string LogName = "gamelog.txt";

        public GameLogger()
        {
            if (File.Exists(LogName))
            {
                File.Delete(LogName);
            }
        }

        public void WriteToGamelog(Pos move)
        {
            File.AppendAllText(LogName, move + " ");
        }
    }
}