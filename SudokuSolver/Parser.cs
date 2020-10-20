using System.Text;

namespace SudokuSolver
{
    class Parser
    {
        public static string PrettyPrintString(string gridString)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < gridString.Length; i++)
            {
                sb.Append(gridString[i] + " ");
                if ((i + 1) % 27 == 0)
                {
                    sb.Append("\n\n");
                }
                else
                {
                    if ((i + 1) % 9 == 0)
                    {
                        sb.Append("\n");
                    }
                    else
                    {
                        if ((i + 1) % 3 == 0)
                        {
                            sb.Append("\t");
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public static string TransformGridFromPointToZero(string grid)
        {
            var sb = new StringBuilder();

            foreach (var item in grid)
            {
                sb.Append(item == '.'
                    ? "0"
                    : item.ToString());
            }

            return sb.ToString();
        }
    }
}
