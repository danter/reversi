using aspa.reversi.Models;
using NUnit.Framework;

namespace aspa.reversi.UnitTests
{
    [TestFixture]
    public class TestAiWithScoreTable
    {
        private const char Black = Constants.Black;
        private const char White = Constants.White;
        private const char B = Constants.Black;
        private const char W = Constants.White;

        private Board CreateBoard(char[] boardData = null)
        {
            var board = new Board(8, 8);
            if (boardData != null)
            {
                board.Data = boardData;
            }
            else
            {
                board.InitBoard();
            }

            return board;
        }

        [TestCase(Black, 0, 0, 0)]
        [TestCase(Black, 3, 3, 0)]
        [TestCase(Black, 3, 2, 1)]
        [TestCase(Black, 2, 3, 1)]
        [TestCase(Black, 5, 4, 1)]
        [TestCase(Black, 4, 5, 1)]
        [TestCase(White, 3, 2, 0)]
        [TestCase(White, 4, 4, 0)]
        [TestCase(White, 4, 2, 1)]
        [TestCase(White, 5, 3, 1)]
        [TestCase(White, 2, 4, 1)]
        [TestCase(White, 3, 5, 1)]
        public void AiScoreCalc_TestSimpleBoardForCaptureScores_CaptureScoreIsAsExpected(char currentPlayer, int xPos, int yPos, int expectedScore)
        {
            var testPos = new Pos(xPos, yPos);

            var gameBoard = new Board(8, 8);
            gameBoard.InitBoard();

            var actualScore = AiWithScoreTable.AiScoreCalc(gameBoard, testPos, currentPlayer);

            Assert.AreEqual(expectedScore, actualScore);
        }

        [TestCase(Black, 4, 0, 0)]
        [TestCase(Black, 1, 2, 1)]
        [TestCase(Black, 3, 2, 1)]
        [TestCase(Black, 0, 3, 3)]
        [TestCase(Black, 2, 4, 2)]
        [TestCase(Black, 5, 4, 1)]
        [TestCase(Black, 6, 6, 3)]
        [TestCase(White, 4, 0, 3)]
        [TestCase(White, 0, 3, 0)]
        [TestCase(White, 1, 1, 1)]
        [TestCase(White, 2, 1, 1)]
        [TestCase(White, 3, 1, 1)]
        [TestCase(White, 5, 1, 1)]
        [TestCase(White, 5, 3, 1)]
        [TestCase(White, 2, 4, 1)]
        [TestCase(White, 3, 5, 3)]
        [TestCase(White, 4, 6, 1)]
        [TestCase(White, 5, 6, 2)]
        public void AiScoreCalc_TestBoardForCaptureScores_CaptureScoreIsAsExpected(char currentPlayer, int xPos, int yPos, int expectedScore)
        {
            var testPos = new Pos(xPos, yPos);
            var boardData  = new[]
            {
              // A   B   C   D   E   F   G   H
              // 0   1   2   3   4   5   6   7
                ' ',' ',' ',' ',' ',' ',' ',' ', // 0
                ' ',' ',' ',' ', B ,' ',' ',' ', // 1
                ' ',' ', B ,' ', B ,' ',' ',' ', // 2
                ' ', W , W , W , B ,' ',' ',' ', // 3
                ' ',' ',' ', B , W ,' ',' ',' ', // 4
                ' ', W , B ,' ', B , W ,' ',' ', // 5
                ' ',' ',' ',' ',' ',' ',' ',' ', // 6
                ' ',' ',' ',' ',' ',' ',' ',' ', // 7
            };
            var gameBoard = CreateBoard(boardData);

            var actualScore = AiWithScoreTable.AiScoreCalc(gameBoard, testPos, currentPlayer);

            Assert.AreEqual(expectedScore, actualScore);
        }

        [TestCase(Black, 4, 3, 8)]
        public void AiScoreCalc_TestBoardForAllSidesCaptureScores_CaptureScoreIsAsExpected(char currentPlayer, int xPos, int yPos, int expectedScore)
        {
            var testPos = new Pos(xPos, yPos);
            var boardData = new[]
            {
              // A   B   C   D   E   F   G   H
              // 0   1   2   3   4   5   6   7
                ' ',' ',' ',' ',' ',' ',' ',' ', // 0
                ' ',' ', B , B , B , B , B ,' ', // 1
                ' ',' ', B , W , W , W , B ,' ', // 2
                ' ',' ', B , W ,' ', W , B ,' ', // 3
                ' ',' ', B , W , W , W , B ,' ', // 4
                ' ',' ', B , B , B , B , B ,' ', // 5
                ' ',' ',' ',' ',' ',' ',' ',' ', // 6
                ' ',' ',' ',' ',' ',' ',' ',' ', // 7
            };
            var gameBoard = CreateBoard(boardData);

            var actualScore = AiWithScoreTable.AiScoreCalc(gameBoard, testPos, currentPlayer);

            Assert.AreEqual(expectedScore, actualScore);
        }

        [Test]
        public void AiScoreTable_TryGetAiScoreTable_AiScoreTableReturnsExpectedTable()
        {
            var expectedScoreTable = new[]
            {
                5, 1, 3, 3, 3, 3, 1, 5,
                1, 0, 1, 1, 1, 1, 0, 1,
                3, 1, 2, 2, 2, 2, 1, 3,
                3, 1, 2, 2, 2, 2, 1, 3,
                3, 1, 2, 2, 2, 2, 1, 3,
                3, 1, 2, 2, 2, 2, 1, 3,
                1, 0, 1, 1, 1, 1, 0, 1,
                5, 1, 3, 3, 3, 3, 1, 5,
            };
            var gameBoard = CreateBoard();

            var actualScoreTable = AiWithScoreTable.AiScoreTable(gameBoard);

            Assert.AreEqual(expectedScoreTable, actualScoreTable);
        }

        [Test]
        public void AiEvalBoard_GetNumericHintBoard_ProperNumericHintBoardIsReturned()
        {
            var currenPlayer = Constants.Black;
            var gameBoard = CreateBoard();

            var expectedHintData = new[]
            {
                // A   B   C   D   E   F   G   H
                // 0   1   2   3   4   5   6   7
                ' ',' ',' ',' ',' ',' ',' ',' ', // 0
                ' ',' ',' ',' ',' ',' ',' ',' ', // 1
                ' ',' ',' ','3',' ',' ',' ',' ', // 2
                ' ',' ','3',' ',' ',' ',' ',' ', // 3
                ' ',' ',' ',' ',' ','3',' ',' ', // 4
                ' ',' ',' ',' ','3',' ',' ',' ', // 5
                ' ',' ',' ',' ',' ',' ',' ',' ', // 6
                ' ',' ',' ',' ',' ',' ',' ',' ', // 7
            };
            var expectedHintBoard = CreateBoard(expectedHintData);

            var actualHintBoard = AiWithScoreTable.GetNumericHints(gameBoard, currenPlayer);

            Assert.AreEqual(expectedHintBoard, actualHintBoard);
        }
    }
}
