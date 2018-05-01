using aspa.reversi.Models;
using NUnit.Framework;

namespace aspa.reversi.UnitTests
{
    [TestFixture]
    public class TestReversi
    {
        private const char B = Constants.Black;
        private const char W = Constants.White;
        private const char H = Constants.Hint;

        [TestCase(new[] { "" }, AiPlayer.NoAi, BoardHints.NoHints, Constants.Black)]
        [TestCase(new[] { "-ai" }, AiPlayer.WhiteAi, BoardHints.NoHints, Constants.Black)]
        [TestCase(new[] { "-ai1" }, AiPlayer.WhiteAi, BoardHints.NoHints, Constants.Black)]
        [TestCase(new[] { "-ai2" }, AiPlayer.BlackAi, BoardHints.NoHints, Constants.Black)]
        [TestCase(new[] { "-ai3" }, AiPlayer.BothAi, BoardHints.NoHints, Constants.Black)]
        [TestCase(new[] { "-ht" }, AiPlayer.NoAi, BoardHints.Hints, Constants.Black)]
        [TestCase(new[] { "-hn" }, AiPlayer.NoAi, BoardHints.NumericHints, Constants.Black)]
        [TestCase(new[] { "-ai3", "-hn" }, AiPlayer.BothAi, BoardHints.NumericHints, Constants.Black)]
        public void ConfigHandler_ReadCommandLine_CorrectParametersAreReturned(string[] commandLine, AiPlayer expectedAiConfig, BoardHints expectedHintConfig, char expectedPlayerConfig)
        {
            var expectedConfig = new Config
            {
                Ai = expectedAiConfig,
                Hints = expectedHintConfig,
                Player = expectedPlayerConfig,
                BoardRows = 8,
                BoardColumns = 8,
                SaveGame = null,
            };

            var actualConfig = ConfigHandler.ReadCommandLineArgumants(commandLine);

            Assert.AreEqual(expectedConfig, actualConfig);
        }

        [TestCase("A5", 0, 5)]
        [TestCase("E2", 4, 2)]
        [TestCase("G3", 6, 3)]
        public void ConvertPosToString_ConvertPosToAsciiCharacters_CorrectAsciiStringIsRetrieved(string expectedAsciiString, int xPos, int yPos)
        {
            var posToConvert = new Pos(xPos, yPos);

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
        public void ParseMove_ConvertAsciiCharactersToPos_CorrectPosIsRetrieved(string stringToConvert, int expectedXPos, int expectedYPos)
        {
            var expectedPos = new Pos(expectedXPos, expectedYPos);

            var actualPos = InputHandler.ParseMove(stringToConvert);

            Assert.AreEqual(expectedPos, actualPos);
        }


        [Test]
        public void RenderToString_DrawInitialBoardWithHints_ExpectedStringForDrawingIsReturned()
        {
            var expectedRenderString =
                "   A B C D E F G H \n" +
                "  ┌─┬─┬─┬─┬─┬─┬─┬─┐\n" +
                " 0│ │ │ │ │ │ │ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 1│ │ │ │ │ │ │ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 2│ │ │ │+│ │ │ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 3│ │ │+│█│░│ │ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 4│ │ │ │░│█│+│ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 5│ │ │ │ │+│ │ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 6│ │ │ │ │ │ │ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 7│ │ │ │ │ │ │ │ │\n" +
                "  └─┴─┴─┴─┴─┴─┴─┴─┘\n";

            var player = Constants.Black;
            var gameBoard = new[]
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
            };

            var hintBoard = ReversiRules.HintPlayer(gameBoard, player);

            var actualDrawString = Graphics.RenderToString(gameBoard, hintBoard);

            Assert.AreEqual(expectedRenderString, actualDrawString);
        }

        [Test]
        public void RenderToString_DrawInitialBoardWithNumericHints_ExpectedStringForDrawingIsReturned()
        {
            var expectedRenderString =
                "   A B C D E F G H \n" +
                "  ┌─┬─┬─┬─┬─┬─┬─┬─┐\n" +
                " 0│ │ │ │ │ │ │ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 1│ │ │ │ │ │ │ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 2│ │ │ │3│ │ │ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 3│ │ │3│█│░│ │ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 4│ │ │ │░│█│3│ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 5│ │ │ │ │3│ │ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 6│ │ │ │ │ │ │ │ │\n" +
                "  ├─┼─┼─┼─┼─┼─┼─┼─┤\n" +
                " 7│ │ │ │ │ │ │ │ │\n" +
                "  └─┴─┴─┴─┴─┴─┴─┴─┘\n";

            var player = Constants.Black;
            var gameBoard = new[]
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
            };

            var hintBoard = AiWithScoreTable.GetNumericHints(gameBoard, player);
            var actualDrawString = Graphics.RenderToString(gameBoard, hintBoard);

            Assert.AreEqual(expectedRenderString, actualDrawString);
        }
    }
}
