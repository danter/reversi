using aspa.reversi.Models;
using NUnit.Framework;

namespace aspa.reversi.UnitTests
{
    [TestFixture]
    public class TestReversiRules
    {
        private const char Black = Constants.Black;
        private const char White = Constants.White;
        private const char H = Constants.Hint;
        private const char B = Black;
        private const char W = White;

        private Config CreateDefaultConfig()
        {
            return new Config { StartPlayer = Black, BoardWidth = 8, BoardHeight = 8 };
        }

        private Board CreateBoard(char[] boardStartData = null)
        {
            var board = new Board(8, 8);
            if (boardStartData != null)
            {
                board.Data = boardStartData;
            }
            else
            {
                board.InitBoard();
            }

            return board;
        }

        [Test]
        public void CreationReversiRules_CreateRversiRulesObject_CorrectStartPlayerIsStarting()
        {
            var expectedCurrentPlayer = Black;
            var config = CreateDefaultConfig();

            var reversi = new ReversiRules(config);
            var actualCurrentPlayer = reversi.CurrentPlayer;

            Assert.AreEqual(expectedCurrentPlayer, actualCurrentPlayer);
        }

        [Test]
        public void CreationReversiRules_CreateRversiRulesObject_CorrectStartBoardIsCreated()
        {
            var config = CreateDefaultConfig();
            var expectedGameBoard = CreateBoard();

            var reversi = new ReversiRules(config);
            var actualGameBoard = reversi.GameBoard;

            Assert.AreEqual(expectedGameBoard, actualGameBoard);
        }


        [TestCase("D2", Black, true)]
        [TestCase("C3", Black, true)]
        [TestCase("E5", Black, true)]
        [TestCase("F4", Black, true)]
        [TestCase("E2", Black, false)]
        [TestCase("F3", Black, false)]
        [TestCase("C4", Black, false)]
        [TestCase("D5", Black, false)]
        [TestCase("A0", Black, false)]
        [TestCase("H0", Black, false)]
        [TestCase("A7", Black, false)]
        [TestCase("H7", Black, false)]
        [TestCase("D3", Black, false)]
        [TestCase("E3", Black, false)]
        [TestCase("D4", Black, false)]
        [TestCase("E4", Black, false)]
        [TestCase("E2", White, true)]
        [TestCase("F3", White, true)]
        [TestCase("C4", White, true)]
        [TestCase("D5", White, true)]
        [TestCase("I0", White, false)]
        [TestCase("A8", White, false)]
        [TestCase("A0", White, false)]
        [TestCase("H0", White, false)]
        [TestCase("A7", White, false)]
        [TestCase("H7", White, false)]
        [TestCase("D3", White, false)]
        [TestCase("E3", White, false)]
        [TestCase("D4", White, false)]
        [TestCase("E4", White, false)]
        public void IsValidMove_CheckIfSimpleMoveIsValid_ExpectCorrectBehaviourFromProvidedMoveResult(string moveInput, char currentPlayer, bool expectedMoveResult)
        {
            var move = InputHandler.ParseMove(moveInput);
            var gameBoard = CreateBoard();

            var actualResult = ReversiRules.IsValidMove(gameBoard, move, currentPlayer);

            Assert.AreEqual(expectedMoveResult, actualResult);
        }

        [TestCase(Black, new[]
        {
            // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ',' ',' ',' ',' ', // 1
            ' ',' ',' ',' ',' ',' ',' ',' ', // 2
            ' ',' ',' ', W , B ,' ',' ',' ', // 3
            ' ',' ',' ', B , W ,' ',' ',' ', // 4
            ' ',' ',' ',' ',' ',' ',' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        }, new[]
        {
            // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ',' ',' ',' ',' ', // 1
            ' ',' ',' ', H ,' ',' ',' ',' ', // 2
            ' ',' ', H ,' ',' ',' ',' ',' ', // 3
            ' ',' ',' ',' ',' ', H ,' ',' ', // 4
            ' ',' ',' ',' ', H ,' ',' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        [TestCase(White, new[]
        {
            // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ',' ',' ',' ',' ', // 1
            ' ',' ',' ', B ,' ',' ',' ',' ', // 2
            ' ',' ',' ', B , B ,' ',' ',' ', // 3
            ' ',' ',' ', B , W ,' ',' ',' ', // 4
            ' ',' ',' ',' ',' ',' ',' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        }, new[]
        {
            // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ',' ',' ',' ',' ', // 1
            ' ',' ', H ,' ', H ,' ',' ',' ', // 2
            ' ',' ',' ',' ',' ',' ',' ',' ', // 3
            ' ',' ', H ,' ',' ',' ',' ',' ', // 4
            ' ',' ',' ',' ',' ',' ',' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        [TestCase(Black, new[]
        {
            // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ',' ',' ',' ',' ', // 1
            ' ',' ',' ', B ,' ',' ',' ',' ', // 2
            ' ',' ',' ', B , B ,' ',' ',' ', // 3
            ' ',' ', W , W , W ,' ',' ',' ', // 4
            ' ',' ',' ',' ',' ',' ',' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        }, new[]
        {
            // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ',' ',' ',' ',' ', // 1
            ' ',' ',' ',' ',' ',' ',' ',' ', // 2
            ' ',' ',' ',' ',' ',' ',' ',' ', // 3
            ' ',' ',' ',' ',' ',' ',' ',' ', // 4
            ' ', H , H , H , H , H ,' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        [TestCase(White, new[]
        {
            // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ',' ',' ',' ',' ', // 1
            ' ',' ',' ', B ,' ',' ',' ',' ', // 2
            ' ',' ',' ', B , B ,' ',' ',' ', // 3
            ' ',' ', W , W , B ,' ',' ',' ', // 4
            ' ',' ',' ',' ', B ,' ',' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        }, new[]
        {
            // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ', H ,' ',' ',' ',' ', // 1
            ' ',' ',' ',' ', H , H ,' ',' ', // 2
            ' ',' ',' ',' ',' ',' ',' ',' ', // 3
            ' ',' ',' ',' ',' ', H ,' ',' ', // 4
            ' ',' ',' ',' ',' ',' ',' ',' ', // 5
            ' ',' ',' ',' ',' ', H ,' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        [TestCase(White, new[]
        {
            // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ',' ',' ',' ',' ', // 1
            ' ',' ', B , B , B , B ,' ',' ', // 2
            ' ',' ', B , W , W , B ,' ',' ', // 3
            ' ',' ', B , W , W , B ,' ',' ', // 4
            ' ',' ', B , B , B , B ,' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        }, new[]
        {
            // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ', H , H , H , H , H , H ,' ', // 1
            ' ', H ,' ',' ',' ',' ', H ,' ', // 2
            ' ', H ,' ',' ',' ',' ', H ,' ', // 3
            ' ', H ,' ',' ',' ',' ', H ,' ', // 4
            ' ', H ,' ',' ',' ',' ', H ,' ', // 5
            ' ', H , H , H , H , H , H ,' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        public void HintPlayer_DrawHintsInHintBoard_ExpectCorrectHintsAreDrawn(char currentPlayer, char[] startData, char[] expectedHintData)
        {
            var gameBoard = CreateBoard(startData);
            var expectedHintBoard = CreateBoard(expectedHintData);

            var actualHintBoard = ReversiRules.HintPlayer(gameBoard, currentPlayer);

            Assert.AreEqual(expectedHintBoard, actualHintBoard);
        }

        [TestCase("E0", White, true)]
        [TestCase("B1", White, true)]
        [TestCase("C1", White, true)]
        [TestCase("D1", White, true)]
        [TestCase("F2", White, true)]
        [TestCase("F3", White, true)]
        [TestCase("C4", White, true)]
        [TestCase("D5", White, true)]
        [TestCase("E5", White, true)]
        [TestCase("D2", Black, true)]
        [TestCase("A4", Black, true)]
        [TestCase("C4", Black, true)]
        [TestCase("F4", Black, true)]
        [TestCase("E5", Black, true)]
        [TestCase("B6", Black, true)]
        [TestCase("G6", Black, true)]
        [TestCase("F0", White, false)]
        [TestCase("B2", White, false)]
        [TestCase("D2", White, false)]
        [TestCase("E0", Black, false)]
        [TestCase("C1", Black, false)]
        [TestCase("F2", Black, false)]
        [TestCase("B4", Black, false)]
        [TestCase("D5", Black, false)]
        public void IsValidMove_CheckIfComplexMoveIsValid_ExpectCorrectBehaviourFromProvidedMoveResult(string moveInput, char currentPlayer, bool expectedMoveResult)
        {
            var move = InputHandler.ParseMove(moveInput);
            var boardData = new[]
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
            };
            var gameBoard = new Board(8,8){ Data = boardData};

            var actualResult = ReversiRules.IsValidMove(gameBoard, move, currentPlayer);

            Assert.AreEqual(expectedMoveResult, actualResult);
        }

        [TestCase("G6", Black, new[]
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
        [TestCase("E5", Black, new[]
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
        [TestCase("E0", White, new[]
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
        [TestCase("D5", White, new[]
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
        [TestCase("C4", Black, new[]
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
        [TestCase("D4", Black, new[]
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
        public void MakeMove_CheckIfMoveTurnsPiecesProperly_CorrectNewBoardIsObtained(string stringMove, char currentPlayer, char[] startBoard, char[] expectedDataAfterMove)
        {
            var expectedBoardAfterMove = CreateBoard(expectedDataAfterMove);
            var config = CreateDefaultConfig();
            var move = InputHandler.ParseMove(stringMove);
            var gameBoard = CreateBoard(startBoard);
            var reversi = new ReversiRules(config) { GameBoard = gameBoard };

            reversi.MakeMove(move, currentPlayer);
            reversi.PlacePiece(move, currentPlayer);
            var actualBoardAfterMove = reversi.GameBoard;

            Assert.AreEqual(expectedBoardAfterMove, actualBoardAfterMove);
        }

        [TestCase("E5 F3 E2 ", White, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ',' ',' ',' ',' ', // 1
            ' ',' ',' ',' ', B ,' ',' ',' ', // 2
            ' ',' ',' ', W , B , W ,' ',' ', // 3
            ' ',' ',' ', B , B ,' ',' ',' ', // 4
            ' ',' ',' ',' ', B ,' ',' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        [TestCase("E5 F3 E2 D5 ", Black, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ',' ',' ',' ',' ', // 0
            ' ',' ',' ',' ',' ',' ',' ',' ', // 1
            ' ',' ',' ',' ', B ,' ',' ',' ', // 2
            ' ',' ',' ', W , B , W ,' ',' ', // 3
            ' ',' ',' ', W , W ,' ',' ',' ', // 4
            ' ',' ',' ', W , B ,' ',' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        [TestCase("E5 F3 E2 D5 G3 H3 C5 E1 E0 ", White, new[]
        {
          // A   B   C   D   E   F   G   H
            ' ',' ',' ',' ', B ,' ',' ',' ', // 0
            ' ',' ',' ',' ', B ,' ',' ',' ', // 1
            ' ',' ',' ',' ', B ,' ',' ',' ', // 2
            ' ',' ',' ', W , B , W , W , W , // 3
            ' ',' ',' ', W , B ,' ',' ',' ', // 4
            ' ',' ', B , B , B ,' ',' ',' ', // 5
            ' ',' ',' ',' ',' ',' ',' ',' ', // 6
            ' ',' ',' ',' ',' ',' ',' ',' ', // 7
        })]
        [TestCase("E5 F3 F2 D5 C5 D6 E7 D7 C7 F5 D2 C3 F4 F1 G5 F6 " +
                  "G3 H3 F0 H4 F7 B5 H2 E1 D1 D0 H5 G7 H7 C1 A5 H6 " +
                  "B2 H1 H0 A3 B0 B3 C2 C0 E0 C6 E2 A4 A2 C4 B4 G0 " +
                  "G2 A0 A6 E6 B7 A1 B1 A7 G4 B6 G6 G1 ", Black, new[]
        {
         // A   B   C   D   E   F   G   H
            W , W , W , W , W , W , W , B , // 0
            W , W , B , B , W , W , W , B , // 1
            W , W , B , B , W , W , B , B , // 2
            W , W , W , B , W , B , B , B , // 3
            W , W , B , W , B , B , B , B , // 4
            W , W , W , B , W , B , B , B , // 5
            W , W , W , W , W , B , B , B , // 6
            W , B , B , B , B , B , B , B , // 7
        })]
        public void LoadSaveGame_LoadGameLogFile_CorrectBoardAndCurrentPlayerIsRetrieved(string saveGame, char expectedCurrentPlayer, char[] expectedGameData)
        {
            var config = CreateDefaultConfig();
            config.SaveGame = saveGame;
            var expectedGameBoard = CreateBoard(expectedGameData);

            var reversi = new ReversiRules(config);
            var actualGameBoard = reversi.GameBoard;
            var actualCurrentPlayer = reversi.CurrentPlayer;

            Assert.AreEqual(expectedCurrentPlayer, actualCurrentPlayer);
            Assert.AreEqual(expectedGameBoard, actualGameBoard);
        }

    }
}