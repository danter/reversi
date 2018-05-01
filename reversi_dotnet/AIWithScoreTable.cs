using System;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class AiWithScoreTable
    {
        public static Board GetNumericHints(Board gameBoard, char currentPlayer)
        {
            var scoreBoard = EvaluateScores(gameBoard, currentPlayer);
            return gameBoard.TransformFromScoreBoard(scoreBoard);
        }

        public static int[] EvaluateScores(Board gameBoard, char currentPlayer)
        {
            var testBoard = ReversiRules.HintPlayer(gameBoard, currentPlayer);

            var scoreBoard = ApplyCaptureScores(gameBoard, testBoard, currentPlayer);

            // Add score for hotspots like corners and edges
            var scoreTable = AiScoreTable(gameBoard);

            for (var y = 0; y < gameBoard.Height; y++)
            {
                for (var x = 0; x < gameBoard.Width; x++)
                {
                    var currentPos = y * gameBoard.Width + x;
                    if (scoreBoard[currentPos] > 0)
                    {
                        scoreBoard[currentPos] += scoreTable[currentPos];
                    }
                }
            }

            return scoreBoard;
        }

        public static Pos GetAiMove(Board gameBoard, char currentPlayer)
        {
            var scoreBoard = EvaluateScores(gameBoard, currentPlayer);
            return SelectRandomTopSpot(scoreBoard, gameBoard.Width, gameBoard.Height);
        }

        private static int[] ApplyCaptureScores(Board gameBoard, Board testBoard, char player)
        {
            var scoreBoard = new int[testBoard.Data.Length];

            for (var y = 0; y < gameBoard.Height; y++)
            {
                for (var x = 0; x < gameBoard.Width; x++)
                {
                    var currentPos = y * gameBoard.Width + x;

                    if (testBoard.GetPiece(x, y) == Constants.Hint)
                    {
                        scoreBoard[currentPos] = AiScoreCalc(gameBoard, new Pos(x, y), player);
                    }
                    else
                    {
                        scoreBoard[currentPos] = 0;
                    }
                }
            }

            return scoreBoard;
        }

        private static Pos SelectRandomTopSpot(int[] scoreBoard, int height, int width)
        {
            var move = new Pos();

            var rand = new Random();
            for (int y = 0, tmpScore = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var currentPos = y * width + x;
                    if (scoreBoard[currentPos] > tmpScore)
                    {
                        tmpScore = scoreBoard[currentPos];
                        move = new Pos(x, y);
                    }
                    else if (scoreBoard[currentPos] == tmpScore)
                    {
                        var random = rand.Next(100);
                        if (random >= 50)
                        {
                            continue;
                        }

                        tmpScore = scoreBoard[currentPos];
                        move = new Pos(x, y);
                    }
                }
            }

            return move;
        }

        public static int AiScoreCalc(Board gameBoard, Pos pos, char player)
        {
            var score = 0;

            if (gameBoard.GetPiece(pos) != ' ')
            {
                return score;
            }

            var otherPlayer = ReversiRules.GetOtherPlayer(player);

            for (var y = pos.Y-1; y <= pos.Y+1; y++)
            {
                for (var x = pos.X-1; x <= pos.X+1; x++)
                {
                    if (!gameBoard.IsInsideBoard(x, y))
                    {
                        continue;
                    }

                    if (gameBoard.GetPiece(x, y) != otherPlayer)
                    {
                        continue;
                    }

                    if (ReversiRules.TraceMove(gameBoard, pos, x-pos.X, y-pos.Y, player))
                    {
                        score += AiTraceMove(gameBoard, pos, x - pos.X, y - pos.Y, player);
                    }
                }
            }

            return score;
        }

        private static int AiTraceMove(Board gameBoard, Pos pos, int dx, int dy, char player)
        {
            var tPos = new Pos(pos);
            var score = 0;

            for (tPos.X += dx, tPos.Y += dy; gameBoard.IsInsideBoard(tPos.X, tPos.Y); tPos.X += dx, tPos.Y += dy)
            {
                if (gameBoard.GetPiece(tPos) == player)
                {
                    return score;
                }

                score++;
            }

            return score;
        }

        public static int[] AiScoreTable(Board gameBoard)
        {
            var scoreTable = new int[gameBoard.Width * gameBoard.Height];

            // 1 bonus for sides and 2 bonus for corners
            for (var i = 0; i < gameBoard.Height; i++)
                scoreTable[0 * gameBoard.Width + i]++;

            for (var i = 0; i < gameBoard.Width; i++)
                scoreTable[i * gameBoard.Width + (gameBoard.Height - 1)]++;

            for (var i = 0; i < gameBoard.Width; i++)
                scoreTable[i * gameBoard.Width + 0]++;

            for (var i = 0; i < gameBoard.Height; i++)
                scoreTable[(gameBoard.Width - 1) * gameBoard.Width + i]++;

            // still a bit extra for corners
            scoreTable[0 * gameBoard.Width + 0]++;
            scoreTable[0 * gameBoard.Width + (gameBoard.Height - 1)]++;
            scoreTable[(gameBoard.Width - 1) * gameBoard.Width + (gameBoard.Height - 1)]++;
            scoreTable[(gameBoard.Width - 1) * gameBoard.Width + 0]++;

            // Preparation for bad zones
            for (var y = 0; y < gameBoard.Width; y++)
            {
                for (var x = 0; x < gameBoard.Height; x++)
                {
                    scoreTable[y * gameBoard.Width + x] += 2;
                }
            }

            // 1 minus for spots 1 square from sides,
            // 2 minus for spots 1 square from corners and
            // 2 minus for spots close to edges but 1 square from corners
            for (var i = 1; i < gameBoard.Height - 1; i++)
                scoreTable[1 * gameBoard.Width + i]--;

            for (var i = 1; i < gameBoard.Width - 1; i++)
                scoreTable[i * gameBoard.Width + (gameBoard.Height - 2)]--;

            for (var i = 1; i < gameBoard.Width - 1; i++)
                scoreTable[i * gameBoard.Width + 1]--;

            for (var i = 1; i < gameBoard.Height - 1; i++)
                scoreTable[(gameBoard.Width - 2) * gameBoard.Width + i]--;

            scoreTable[1 * gameBoard.Width + 0] -= 2;
            scoreTable[0 * gameBoard.Width + 1] -= 2;
            scoreTable[0 * gameBoard.Width + (gameBoard.Height - 2)] -= 2;
            scoreTable[1 * gameBoard.Width + (gameBoard.Height - 1)] -= 2;
            scoreTable[(gameBoard.Width - 2) * gameBoard.Width + (gameBoard.Height - 1)] -= 2;
            scoreTable[(gameBoard.Width - 1) * gameBoard.Width + (gameBoard.Height - 2)] -= 2;
            scoreTable[(gameBoard.Width - 1) * gameBoard.Width + 1] -= 2;
            scoreTable[(gameBoard.Width - 2) * gameBoard.Width + 0] -= 2;

            return scoreTable;
        }
    }
}
