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

        [TestCase("A5", 0, 5)]
        [TestCase("E2", 4, 2)]
        [TestCase("G3", 6, 3)]
        public void ReadMove_ConvertAsciiCharactersToPos_CorrectPosIsRetrieved(string stringToConvert, int xPos, int yPos)
        {
            var expectedPos = new Pos { X = xPos, Y = yPos};

            var actualPos = InputHandler.ReadMove(stringToConvert);

            Assert.AreEqual(expectedPos, actualPos);
        }
    }
}
