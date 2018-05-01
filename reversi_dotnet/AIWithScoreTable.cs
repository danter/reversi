using System;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class AiWithScoreTable
    {
        public static Pos AiEvalBoard(char[] gameBoard, char[] hintBoard, char player)
        {
            var testBoard = new char[Constants.BoardMax];

            InputHandler.HintPlayer(gameBoard, testBoard, player);

            var scoreBoard = ApplyCaptureScores(gameBoard, testBoard, player);

            // Add score for hotspots like corners and edges
            var scoreTable = AiScoreTable();

            for (var y = 0; y < Constants.Row; y++)
            {
                for (var x = 0; x < Constants.Col; x++)
                {
                    var currentPos = y * Constants.Row + x;
                    if (scoreBoard[currentPos] > 0)
                    {
                        scoreBoard[currentPos] += scoreTable[currentPos];
                    }
                }
            }

            TransformScoreBoard(scoreBoard, hintBoard);

            return SelectRandomTopSpot(hintBoard);
        }

        private static void TransformScoreBoard(int[] scoreBoard, char[] hintBoard)
        {
            for (var i = 0; i < scoreBoard.Length; i++)
            {
                if (scoreBoard[i] <= 0)
                {
                    hintBoard[i] = ' ';
                }
                else
                {
                    hintBoard[i] = Pos.ConvertToAsciiDigit(scoreBoard[i]);
                }
            }
        }

        private static int[] ApplyCaptureScores(char[] gameBoard, char[] testBoard, char player)
        {
            var scoreBoard = new int[testBoard.Length];

            for (var y = 0; y < Constants.Row; y++)
            {
                for (var x = 0; x < Constants.Col; x++)
                {
                    var currentPos = y * Constants.Row + x;

                    if (testBoard[currentPos] == Constants.Hint)
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

        private static Pos SelectRandomTopSpot(char[] scoreBoard)
        {
            var move = new Pos();

            var rand = new Random();
            for (int y = 0, tmpScore = 0; y < Constants.Row; y++)
            {
                for (var x = 0; x < Constants.Col; x++)
                {
                    var currentPos = y * Constants.Row + x;
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

        public static int AiScoreCalc(char[] gameBoard, Pos pos, char player)
        {
            var score = 0;

            if (gameBoard[pos.Y*Constants.Row+pos.X] != ' ')
            {
                return score;
            }

            var otherPlayer = InputHandler.GetOtherPlayer(player);

            for (var y = pos.Y-1; y <= pos.Y+1; y++)
            {
                for (var x = pos.X-1; x <= pos.X+1; x++)
                {
                    if (!InputHandler.IsInsideBoard(x, y))
                    {
                        continue;
                    }

                    if (gameBoard[y * Constants.Row + x] != otherPlayer)
                    {
                        continue;
                    }

                    if (InputHandler.TraceMove(gameBoard, pos, x-pos.X, y-pos.Y, player))
                    {
                        score += AiTraceMove(gameBoard, pos, x - pos.X, y - pos.Y, player);
                    }
                }
            }

            return score;
        }

        private static int AiTraceMove(char[] gameBoard, Pos pos, int dx, int dy, char player)
        {
            var tPos = new Pos(pos);
            var score = 0;

            for (tPos.X += dx, tPos.Y += dy; InputHandler.IsInsideBoard(tPos.X, tPos.Y); tPos.X += dx, tPos.Y += dy)
            {
                if (gameBoard[tPos.Y*Constants.Row+tPos.X] == player)
                {
                    return score;
                }

                score++;
            }

            return score;
        }

        public static int[] AiScoreTable()
        {
            var scoreTable = new int[Constants.BoardMax];

            // 1 bonus for sides and 2 bonus for corners
            for (var i = 0; i < Constants.Col; i++)
                scoreTable[0 * Constants.Row + i]++;

            for (var i = 0; i < Constants.Row; i++)
                scoreTable[i * Constants.Row + (Constants.Col - 1)]++;

            for (var i = 0; i < Constants.Row; i++)
                scoreTable[i * Constants.Row + 0]++;

            for (var i = 0; i < Constants.Col; i++)
                scoreTable[(Constants.Row - 1) * Constants.Row + i]++;

            // still a bit extra for corners
            scoreTable[0 * Constants.Row + 0]++;
            scoreTable[0 * Constants.Row + (Constants.Col - 1)]++;
            scoreTable[(Constants.Row - 1) * Constants.Row + (Constants.Col - 1)]++;
            scoreTable[(Constants.Row - 1) * Constants.Row + 0]++;

            // Preparation for bad zones
            for (var y = 0; y < Constants.Row; y++)
            {
                for (var x = 0; x < Constants.Col; x++)
                {
                    scoreTable[y * Constants.Row + x] += 2;
                }
            }

            // 1 minus for spots 1 square from sides,
            // 2 minus for spots 1 square from corners and
            // 2 minus for spots close to edges but 1 square from corners
            for (var i = 1; i < Constants.Col - 1; i++)
                scoreTable[1 * Constants.Row + i]--;

            for (var i = 1; i < Constants.Row - 1; i++)
                scoreTable[i * Constants.Row + (Constants.Col - 2)]--;

            for (var i = 1; i < Constants.Row - 1; i++)
                scoreTable[i * Constants.Row + 1]--;

            for (var i = 1; i < Constants.Col - 1; i++)
                scoreTable[(Constants.Row - 2) * Constants.Row + i]--;

            scoreTable[1 * Constants.Row + 0] -= 2;
            scoreTable[0 * Constants.Row + 1] -= 2;
            scoreTable[0 * Constants.Row + (Constants.Col - 2)] -= 2;
            scoreTable[1 * Constants.Row + (Constants.Col - 1)] -= 2;
            scoreTable[(Constants.Row - 2) * Constants.Row + (Constants.Col - 1)] -= 2;
            scoreTable[(Constants.Row - 1) * Constants.Row + (Constants.Col - 2)] -= 2;
            scoreTable[(Constants.Row - 1) * Constants.Row + 1] -= 2;
            scoreTable[(Constants.Row - 2) * Constants.Row + 0] -= 2;

            return scoreTable;
        }
    }
}
