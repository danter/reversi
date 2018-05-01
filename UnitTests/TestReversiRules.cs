using aspa.reversi.Models;
using NUnit.Framework;

namespace aspa.reversi.UnitTests
{
    [TestFixture]
    public class TestReversiRules
    {
        private const char B = Constants.Black;
        private const char W = Constants.White;
        private const char H = Constants.Hint;

        private Config CreateDefaultConfig()
        {
            return new Config { Player = Constants.Black, BoardWidth = 8, BoardHeight = 8 };
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
            var expectedCurrentPlayer = Constants.Black;
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
        public void IsValidMove_CheckIfSimpleMoveIsValid_ExpectCorrectBehaviourFromProvidedMoveResult(string moveInput, char currentPlayer, bool expectedMoveResult)
        {
            var move = InputHandler.ParseMove(moveInput);
            var gameBoard = CreateBoard();

            var actualResult = ReversiRules.IsValidMove(gameBoard, move, currentPlayer);

            Assert.AreEqual(expectedMoveResult, actualResult);
        }

        [TestCase(Constants.Black, new[]
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
        [TestCase(Constants.White, new[]
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
        [TestCase(Constants.Black, new[]
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
        [TestCase(Constants.White, new[]
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
        [TestCase(Constants.White, new[]
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

        [TestCase("E5 F3 E2 ", Constants.White, new[]
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
        [TestCase("E5 F3 E2 D5 ", Constants.Black, new[]
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
        [TestCase("E5 F3 E2 D5 G3 H3 C5 E1 E0 ", Constants.White, new[]
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
                  "G2 A0 A6 E6 B7 A1 B1 A7 G4 B6 G6 G1 ", Constants.Black, new[]
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