using aspa.reversi;
using aspa.reversi.Models;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class TestReversi
    {
        [TestCase("A5", 0, 5)]
        [TestCase("E2", 4, 2)]
        [TestCase("G3", 6, 3)]
        public void ConvertPosToString_ConvertPosToAsciiCharacters_CorrectAsciiStringIsRetrieved(string expectedAsciiString, int xPos, int yPos)
        {
            var posToConvert = new Pos { X = xPos, Y = yPos };

            var actualAsciiString = posToConvert.ToString();

            Assert.AreEqual(expectedAsciiString, actualAsciiString);
        }

        [TestCase("A9", 0, 9)]
        [TestCase("B8", 1, 8)]
        [TestCase("C7", 2, 7)]
        [TestCase("D6", 3, 6)]
        [TestCase("E5", 4, 5)]
        [TestCase("F4", 5, 4)]
        [TestCase("G3", 6, 3)]
        [TestCase("H2", 7, 2)]
        [TestCase("I1", 8, 1)]
        [TestCase("J0", 9, 0)]
        [TestCase("a0", 0, 0)]
        [TestCase("b1", 1, 1)]
        [TestCase("c2", 2, 2)]
        [TestCase("d3", 3, 3)]
        [TestCase("e4", 4, 4)]
        [TestCase("f5", 5, 5)]
        [TestCase("g6", 6, 6)]
        [TestCase("h7", 7, 7)]
        [TestCase("i8", 8, 8)]
        [TestCase("j9", 9, 9)]
        public void ReadMove_ConvertAsciiCharactersToPos_CorrectPosIsRetrieved(string stringToConvert, int expectedXPos, int expectedYPos)
        {
            var expectedPos = new Pos { X = expectedXPos, Y = expectedYPos};

            var actualPos = InputHandler.ReadMove(stringToConvert);

            Assert.AreEqual(expectedPos, actualPos);
        }

        [TestCase("D2", Constants.Black, true)]
        [TestCase("C3", Constants.Black, true)]
        [TestCase("E5", Constants.Black, true)]
        [TestCase("F4", Constants.Black, true)]
        [TestCase("E2", Constants.Black, false)]
        [TestCase("F3", Constants.Black, false)]
        [TestCase("C4", Constants.Black, false)]
        [TestCase("D5", Constants.Black, false)]
        [TestCase("A0", Constants.Black, false)]
        [TestCase("H0", Constants.Black, false)]
        [TestCase("A7", Constants.Black, false)]
        [TestCase("H7", Constants.Black, false)]
        [TestCase("D3", Constants.Black, false)]
        [TestCase("E3", Constants.Black, false)]
        [TestCase("D4", Constants.Black, false)]
        [TestCase("E4", Constants.Black, false)]
        [TestCase("E2", Constants.White, true)]
        [TestCase("F3", Constants.White, true)]
        [TestCase("C4", Constants.White, true)]
        [TestCase("D5", Constants.White, true)]
        [TestCase("I0", Constants.White, false)]
        [TestCase("A8", Constants.White, false)]
        [TestCase("A0", Constants.White, false)]
        [TestCase("H0", Constants.White, false)]
        [TestCase("A7", Constants.White, false)]
        [TestCase("H7", Constants.White, false)]
        [TestCase("D3", Constants.White, false)]
        [TestCase("E3", Constants.White, false)]
        [TestCase("D4", Constants.White, false)]
        [TestCase("E4", Constants.White, false)]
        public void IsValidMove_CheckIfSimpleMoveIsValid_ExpectCorrectBehaviourFromProvidedMoveResult(string moveInput, char currentPlayer, bool expectedMoveResult )
        {
            const char b = Constants.Black;
            const char w = Constants.White;
            var move = InputHandler.ReadMove(moveInput);

            var board = new[]
            {
              // A   B   C   D   E   F   G   H
                ' ',' ',' ',' ',' ',' ',' ',' ', // 0
                ' ',' ',' ',' ',' ',' ',' ',' ', // 1
                ' ',' ',' ',' ',' ',' ',' ',' ', // 2
                ' ',' ',' ', w , b ,' ',' ',' ', // 3
                ' ',' ',' ', b , w ,' ',' ',' ', // 4
                ' ',' ',' ',' ',' ',' ',' ',' ', // 5
                ' ',' ',' ',' ',' ',' ',' ',' ', // 6
                ' ',' ',' ',' ',' ',' ',' ',' ', // 7
            };

            var actualResult = InputHandler.IsValidMove(board, move, currentPlayer);

            Assert.AreEqual(expectedMoveResult, actualResult);

        }

        [TestCase("E0", Constants.White, true)]
        [TestCase("B1", Constants.White, true)]
        [TestCase("C1", Constants.White, true)]
        [TestCase("D1", Constants.White, true)]
        [TestCase("F2", Constants.White, true)]
        [TestCase("F3", Constants.White, true)]
        [TestCase("C4", Constants.White, true)]
        [TestCase("D5", Constants.White, true)]
        [TestCase("E5", Constants.White, true)]
        [TestCase("D2", Constants.Black, true)]
        [TestCase("A4", Constants.Black, true)]
        [TestCase("C4", Constants.Black, true)]
        [TestCase("F4", Constants.Black, true)]
        [TestCase("E5", Constants.Black, true)]
        [TestCase("B6", Constants.Black, true)]
        [TestCase("G6", Constants.Black, true)]
        [TestCase("F0", Constants.White, false)]
        [TestCase("B2", Constants.White, false)]
        [TestCase("D2", Constants.White, false)]
        [TestCase("E0", Constants.Black, false)]
        [TestCase("C1", Constants.Black, false)]
        [TestCase("F2", Constants.Black, false)]
        [TestCase("B4", Constants.Black, false)]
        [TestCase("D5", Constants.Black, false)]
        public void IsValidMove_CheckIfComplexMoveIsValid_ExpectCorrectBehaviourFromProvidedMoveResult(string moveInput, char currentPlayer, bool expectedMoveResult )
        {
            const char b = Constants.Black;
            const char w = Constants.White;
            var move = InputHandler.ReadMove(moveInput);

            var board = new[]
            {
              // A   B   C   D   E   F   G   H
                ' ',' ',' ',' ',' ',' ',' ',' ', // 0
                ' ',' ',' ',' ', b ,' ',' ',' ', // 1
                ' ',' ', b ,' ', b ,' ',' ',' ', // 2
                ' ', w , w , w , b ,' ',' ',' ', // 3
                ' ',' ',' ', b , w ,' ',' ',' ', // 4
                ' ',' ', w ,' ',' ', w ,' ',' ', // 5
                ' ',' ',' ',' ',' ',' ',' ',' ', // 6
                ' ',' ',' ',' ',' ',' ',' ',' ', // 7
            };

            var actualResult = InputHandler.IsValidMove(board, move, currentPlayer);

            Assert.AreEqual(expectedMoveResult, actualResult);

        }

        private const char B = Constants.Black;
        private const char W = Constants.White;

        [TestCase("G6", Constants.Black, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ', B ,' ',' ',' ', // 1
            ' ',' ', B ,' ', B ,' ',' ',' ', // 2
            ' ', W , W , W , B ,' ',' ',' ', // 3
            ' ',' ',' ', B , W ,' ',' ',' ', // 4
            ' ',' ', W ,' ',' ', W ,' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        }, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ', B ,' ',' ',' ', // 1
            ' ',' ', B ,' ', B ,' ',' ',' ', // 2
            ' ', W , W , B , B ,' ',' ',' ', // 3
            ' ',' ',' ', B , B ,' ',' ',' ', // 4
            ' ',' ', W ,' ',' ', B ,' ',' ', // 5
            ' ',' ',' ',' ',' ',' ', B ,' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        [TestCase("E5", Constants.Black, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ', B ,' ',' ',' ', // 1
            ' ',' ', B ,' ', B ,' ',' ',' ', // 2
            ' ', W , W , W , B ,' ',' ',' ', // 3
            ' ',' ',' ', B , W ,' ',' ',' ', // 4
            ' ',' ', W ,' ',' ', W ,' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        }, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ', B ,' ',' ',' ', // 1
            ' ',' ', B ,' ', B ,' ',' ',' ', // 2
            ' ', W , W , W , B ,' ',' ',' ', // 3
            ' ',' ',' ', B , B ,' ',' ',' ', // 4
            ' ',' ', W ,' ', B , W ,' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        [TestCase("E0", Constants.White, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ', B ,' ',' ',' ', // 1
            ' ',' ', B ,' ', B ,' ',' ',' ', // 2
            ' ', W , W , W , B ,' ',' ',' ', // 3
            ' ',' ',' ', B , W ,' ',' ',' ', // 4
            ' ',' ', W ,' ',' ', W ,' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        }, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ', W ,' ',' ',' ', // 0
            ' ',' ',' ',' ', W ,' ',' ',' ', // 1
            ' ',' ', B ,' ', W ,' ',' ',' ', // 2
            ' ', W , W , W , W ,' ',' ',' ', // 3
            ' ',' ',' ', B , W ,' ',' ',' ', // 4
            ' ',' ', W ,' ',' ', W ,' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        [TestCase("D5", Constants.White, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ', B ,' ',' ',' ', // 1
            ' ',' ', B ,' ', B ,' ',' ',' ', // 2
            ' ', W , W , W , B ,' ',' ',' ', // 3
            ' ',' ',' ', B , W ,' ',' ',' ', // 4
            ' ',' ', W ,' ',' ', W ,' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        }, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ', B ,' ',' ',' ', // 1
            ' ',' ', B ,' ', B ,' ',' ',' ', // 2
            ' ', W , W , W , B ,' ',' ',' ', // 3
            ' ',' ',' ', W , W ,' ',' ',' ', // 4
            ' ',' ', W , W ,' ', W ,' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        [TestCase("C4", Constants.Black, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ', B ,' ',' ',' ', // 1
            ' ',' ', B ,' ', B ,' ',' ',' ', // 2
            ' ', W , W , W , B ,' ',' ',' ', // 3
            ' ',' ',' ', B , W ,' ',' ',' ', // 4
            ' ',' ', W ,' ',' ', W ,' ',' ', // 5
            ' ',' ', B ,' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        }, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ', B ,' ',' ',' ', // 1
            ' ',' ', B ,' ', B ,' ',' ',' ', // 2
            ' ', W , B , B , B ,' ',' ',' ', // 3
            ' ',' ', B , B , W ,' ',' ',' ', // 4
            ' ',' ', B ,' ',' ', W ,' ',' ', // 5
            ' ',' ', B ,' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        [TestCase("D4", Constants.Black, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ',' ',' ',' ',' ', // 1
            ' ', B , B , B , B , B ,' ',' ', // 2
            ' ', B , W , W , W , B ,' ',' ', // 3
            ' ', B , W ,' ', W , B ,' ',' ', // 4
            ' ', B , W , W , W , B ,' ',' ', // 5
            ' ', B , B , B , B , B ,' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        }, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ',' ',' ',' ',' ', // 1
            ' ', B , B , B , B , B ,' ',' ', // 2
            ' ', B , B , B , B , B ,' ',' ', // 3
            ' ', B , B , B , B , B ,' ',' ', // 4
            ' ', B , B , B , B , B ,' ',' ', // 5
            ' ', B , B , B , B , B ,' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        public void MakeMove_CheckIfMoveTurnsPiecesProperly_CorrectNewBoardIsObtained(string stringMove, char currentPlayer, char[] startBoard, char[] expectedBoardAfterMove)
        {
            var expectedStringBoardAfterMove = new string(expectedBoardAfterMove);
            var move = InputHandler.ReadMove(stringMove);

            InputHandler.MakeMove(startBoard, move, currentPlayer);
            InputHandler.PlacePiece(startBoard, move, currentPlayer);

            var actualBoardAfterMove = new string(startBoard);

            Assert.AreEqual(expectedStringBoardAfterMove, actualBoardAfterMove);
        }
    }
}
