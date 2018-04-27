using System;

namespace aspa.reversi
{
    public class Graphics
    {
        public static void DrawBoard(char[] board, char[] hints)
        {
            Console.WriteLine();
            Console.Write("   ");
            for (var i = 0; i < Constants.Col; i++)
                Console.Write((char)(i + 65) + " ");
            Console.Write(" \n");

            Console.Write("  ┌");
            for (var i = 0; i < Constants.Col - 1; i++)
                Console.Write("─┬");
            Console.Write("─┐\n");

            for (var y = 0; y < Constants.Row; y++)
            {
                Console.Write(" " + y + "│");
                for (var x = 0; x < Constants.Col; x++)
                {
                    var currentPos = y * Constants.Row + x;

                    if (board[currentPos] == ' ')
                    {
                        switch (hints[currentPos])
                        {
                            case ' ':
                                Console.Write(" │");
                                break;
                            case Constants.Hint:
                                Console.Write(Constants.Hint + "│");
                                break;
                            default:
                                if (hints[currentPos] < 0)
                                {
                                    Console.Write(hints[currentPos] + "│");
                                }

                                break;
                        }
                    }
                    else
                    {
                        Console.Write(board[currentPos] + "│");
                    }

                }

                Console.WriteLine();

                if (y >= Constants.Row - 1)
                {
                    continue;
                }

                Console.Write("  ├");
                for (var i = 0; i < Constants.Col - 1; i++)
                    Console.Write("─┼");
                Console.Write("─│\n");
            }

            Console.Write("  └");
            for (var i = 0; i < Constants.Col - 1; i++)
                Console.Write("─┴");
            Console.WriteLine("─┘\n");
        }
    }
}