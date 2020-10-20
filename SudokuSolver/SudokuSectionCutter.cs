namespace SudokuSolver
{
    public class SudokuSectionCutter
    {
        public int[] GetSingleRow(int[] grid, int index)
        {
            var row = new int[9];
            for (int j = 0; j < 9; j++)
            {
                row[j] = grid[index * 9 + j];
            }

            return row;
        }

        public int[,] GetAllRows(int[] grid)
        {
            var rows = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    rows[i, j] = grid[i * 9 + j];
                }
            }

            return rows;
        }

        public int[,] GetAllColumns(int[] grid)
        {
            var columns = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    columns[i, j] = grid[i + 9 * j];
                }
            }

            return columns;
        }

        public int[] GetSingleColumn(int[] grid, int index)
        {
            var column = new int[9];
            for (int j = 0; j < 9; j++)
            {
                column[j] = grid[index + 9 * j];
            }

            return column;
        }

        public int[,] GetAllSquares(int[] grid)
        {
            var squares = new int[9, 9];

            int index = 0;
            for (int i = 0; i < 9; i += 3)
            {
                for (int j = 0; j < 9; j += 3, index++)
                {
                    var square = GetSingleSquare(grid, i, j);

                    for (int k = 0; k < 9; k++)
                    {
                        squares[index, k] = square[k];
                    }
                }
            }

            return squares;
        }


        public int[] GetSingleSquare(int[] grid, int rowIndex, int columnIndex)
        {
            int indexOfSubGridRow = rowIndex % 3;
            var rows = new int[3][];

            if (indexOfSubGridRow == 0)
            {
                rows[0] = GetSingleRow(grid, rowIndex + 0);
                rows[1] = GetSingleRow(grid, rowIndex + 1);
                rows[2] = GetSingleRow(grid, rowIndex + 2);
            }
            else if (indexOfSubGridRow == 1)
            {
                rows[0] = GetSingleRow(grid, rowIndex - 1);
                rows[1] = GetSingleRow(grid, rowIndex + 0);
                rows[2] = GetSingleRow(grid, rowIndex + 1);
            }
            else if (indexOfSubGridRow == 2)
            {
                rows[0] = GetSingleRow(grid, rowIndex - 2);
                rows[1] = GetSingleRow(grid, rowIndex - 1);
                rows[2] = GetSingleRow(grid, rowIndex + 0);
            }

            var square = new int[9];

            int j = 0;
            if (columnIndex < 3)
            {
                for (int i = 0; i < 3; i++, j += 3)
                {
                    square[0 + j] = rows[i][0];
                    square[1 + j] = rows[i][1];
                    square[2 + j] = rows[i][2];
                }
            }
            else if (columnIndex < 6)
            {
                for (int i = 0; i < 3; i++, j += 3)
                {
                    square[0 + j] = rows[i][3];
                    square[1 + j] = rows[i][4];
                    square[2 + j] = rows[i][5];
                }
            }
            else
            {
                for (int i = 0; i < 3; i++, j += 3)
                {
                    square[0 + j] = rows[i][6];
                    square[1 + j] = rows[i][7];
                    square[2 + j] = rows[i][8];
                }
            }

            return square;
        }
    }
}
