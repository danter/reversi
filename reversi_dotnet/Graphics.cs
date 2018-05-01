using System;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class Graphics
    {
        public static string RenderToString(char[] board, char[] hints)
        {
            var drawstring = "";

            drawstring += "   ";
            for (var i = 0; i < Constants.Col; i++)
                drawstring += Pos.ConvertPosToLetter(i) + " ";
            drawstring += "\n";

            drawstring += "  ┌";
            for (var i = 0; i < Constants.Col - 1; i++)
                drawstring += "─┬";
            drawstring += "─┐\n";

            for (var y = 0; y < Constants.Row; y++)
            {
                drawstring += " " + y + "│";
                for (var x = 0; x < Constants.Col; x++)
                {
                    var currentPos = y * Constants.Row + x;

                    if (board[currentPos] == ' ')
                    {
                        switch (hints[currentPos])
                        {
                            case ' ':
                                drawstring += " │";
                                break;
                            case Constants.Hint:
                                drawstring += Constants.Hint + "│";
                                break;
                            default:
                                drawstring += hints[currentPos] + "│";

                                break;
                        }
                    }
                    else
                    {
                        drawstring += board[currentPos] + "│";
                    }

                }

                drawstring += "\n";

                if (y >= Constants.Row - 1)
                {
                    continue;
                }

                drawstring += "  ├";
                for (var i = 0; i < Constants.Col - 1; i++)
                    drawstring += "─┼";
                drawstring += "─┤\n";
            }

            drawstring += "  └";
            for (var i = 0; i < Constants.Col - 1; i++)
                drawstring += "─┴";
            drawstring += "─┘\n";

            return drawstring;
        }

        public static void PrintScore(char[] gameBoard)
        {
            Console.WriteLine("BLACK score: " + ReversiRules.CalculateScore(gameBoard, Constants.Black));
            Console.WriteLine("WHITE score: " + ReversiRules.CalculateScore(gameBoard, Constants.White));
        }

        public static void PrintFinalScore(char[] gameBoard)
        {
            var blackScore = ReversiRules.CalculateScore(gameBoard, Constants.Black);
            var whiteScore = ReversiRules.CalculateScore(gameBoard, Constants.White);

            Console.WriteLine("\nNeither BLACK nor WHITE can make a move, GAME OVER!");
            Console.WriteLine("The score was:");
            Console.WriteLine("BLACK: " + blackScore);
            Console.WriteLine("WHITE: " + whiteScore);

            if (blackScore == whiteScore)
            {
                Console.WriteLine("The game was a draw!");
            }
            else if (whiteScore < blackScore)
            {
                Console.WriteLine("Winner is BLACK!");
            }
            else
            {
                Console.WriteLine("Winner is WHITE!");
            }
        }

        public static void DrawBoard(char[] board, char[] hints)
        {
            Console.WriteLine();
            Console.WriteLine(RenderToString(board, hints));
            Console.WriteLine();
        }

        public static void AnnouncePlayerMove(char currentPlayer)
        {
            switch (currentPlayer)
            {
                case Constants.Black:
                    Console.Write("BLACK, make your move: ");
                    break;
                case Constants.White:
                    Console.Write("WHITE, make your move: ");
                    break;
                default:
                    return;
            }
        }
    }
}