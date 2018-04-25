namespace aspa.reversi
{
    public class Converters
    {
        public static string ConvertPosToLetter(int coord)
        {
            switch (coord)
            {
                case 0:
                    return "A";
                case 1:
                    return "B";
                case 2:
                    return "C";
                case 3:
                    return "D";
                case 4:
                    return "E";
                case 5:
                    return "F";
                case 6:
                    return "G";
                case 7:
                    return "H";
                case 8:
                    return "I";
                case 9:
                    return "J";

                default:
                    return " ";
            }
        }

        public static int ConvertLetterToPos(char character)
        {
            var str = character.ToString().ToUpper();

            switch (str)
            {
                case "A":
                    return 0;
                case "B":
                    return 1;
                case "C":
                    return 2;
                case "D":
                    return 3;
                case "E":
                    return 4;
                case "F":
                    return 5;
                case "G":
                    return 6;
                case "H":
                    return 7;
                case "I":
                    return 8;
                case "J":
                    return 9;

                default:
                    return -1;
            }
        }
    }
}