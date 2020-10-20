using System.Text;

namespace SudokuSolver
{
    public class IOTransformer
    {
        public string TransformInput(string inputString)
        {
            var sb = new StringBuilder();

            foreach (var inputChar in inputString.Replace('.', '0'))
            {
                if(!char.IsWhiteSpace(inputChar))
                {
                    sb.Append(inputChar);
                }
            }

            return sb.ToString();
        }

        public string TransformOutput(string outputString)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < outputString.Length; i++)
            {
                sb.Append(outputString[i]).Append(' ');
                if ((i + 1) % 27 == 0)
                {
                    sb.Append("\n\n");
                }
                else
                {
                    if ((i + 1) % 9 == 0)
                    {
                        sb.Append('\n');
                    }
                    else
                    {
                        if ((i + 1) % 3 == 0)
                        {
                            sb.Append('\t');
                        }
                    }
                }
            }

            return sb.ToString();
        }
    }
}
