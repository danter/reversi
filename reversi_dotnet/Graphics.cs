using System;
using aspa.reversi.Models;

namespace aspa.reversi
{
    public class Graphics
    {
        public static string RenderToString(Board board, Board hints)
        {
            var drawstring = "";

            drawstring += "   ";
            for (var i = 0; i < board.Width; i++)
                drawstring += Pos.ConvertPosToLetter(i) + " ";
            drawstring += "\n";

            drawstring += "  ┌";
            for (var i = 0; i < board.Width - 1; i++)
                drawstring += "─┬";
            drawstring += "─┐\n";

            for (var y = 0; y < board.Height; y++)
            {
                drawstring += " " + y + "│";
                for (var x = 0; x < board.Width; x++)
                {
                    if (board.GetPiece(x, y) == ' ')
                    {
                        switch (hints.GetPiece(x, y))
                        {
                            case ' ':
                                drawstring += " │";
                                break;
                            case Constants.Hint:
                                drawstring += Constants.Hint + "│";
                                break;
                            default:
                                drawstring += hints.GetPiece(x, y) + "│";

                                break;
                        }
                    }
                    else
                    {
                        drawstring += board.GetPiece(x, y) + "│";
                    }

                }

                drawstring += "\n";

                if (y >= board.Height - 1)
                {
                    continue;
                }

                drawstring += "  ├";
                for (var i = 0; i < board.Width - 1; i++)
                    drawstring += "─┼";
                drawstring += "─┤\n";
            }

            drawstring += "  └";
            for (var i = 0; i < board.Width - 1; i++)
                drawstring += "─┴";
            drawstring += "─┘\n";

            return drawstring;
        }

        public static void PrintScore(Board gameBoard)
        {
            Console.WriteLine("BLACK score: " + gameBoard.GetNumberOfPieces(Constants.Black));
            Console.WriteLine("WHITE score: " + gameBoard.GetNumberOfPieces(Constants.White));
        }

        public static void PrintFinalScore(Board gameBoard)
        {
            var blackScore = gameBoard.GetNumberOfPieces(Constants.Black);
            var whiteScore = gameBoard.GetNumberOfPieces(Constants.White);

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

        public static void DrawBoard(Board board, Board hints)
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