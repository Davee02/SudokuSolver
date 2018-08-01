namespace SudokuSolver
{
    class Parser
    {
        public static string PrettyPrintString(string gridString)
        {
            var prettyGrid = "";

            for (int i = 0; i < gridString.Length; i++)
            {
                prettyGrid += gridString[i] + " ";
                if ((i + 1) % 27 == 0)
                {
                    prettyGrid += "\n\n";
                }
                else
                {
                    if ((i + 1) % 9 == 0)
                    {
                        prettyGrid += "\n";
                    }
                    else
                    {
                        if ((i + 1) % 3 == 0)
                        {
                            prettyGrid += "\t";
                        }
                    }
                }
            }

            return prettyGrid;
        }

        public static string TransformGridFromPointToZero(string grid)
        {
            var newGrid = "";

            foreach (var item in grid)
            {
                newGrid += item == '.' ? "0" : item.ToString();
            }

            return newGrid;
        }
    }
}
