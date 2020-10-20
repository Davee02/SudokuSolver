using System.Linq;

namespace SudokuSolver
{
    public class SudokuValidator
    {
        private readonly SudokuSectionCutter _sectionCutter;
        private readonly SudokuPositionCalculator _positionCalculator;

        public SudokuValidator(SudokuSectionCutter sectionCutter, SudokuPositionCalculator positionCalculator)
        {
            _sectionCutter = sectionCutter;
            _positionCalculator = positionCalculator;
        }

        public bool IsValid(int[] grid)
        {
            return IsValidWholeGrid(grid);
        }

        public bool IsValidForIndex(int[] grid, int index)
        {
            return IsValidSectionOfGrid(grid, index);
        }

        public bool IsSolved(int[] grid)
        {
            return IsFilledOut(grid) && IsValid(grid);
        }

        private bool IsFilledOut(int[] gridIntArray)
        {
            return gridIntArray
                .AsEnumerable()
                .All(number => number != 0);
        }

        private bool IsValidSectionOfGrid(int[] grid, int index)
        {
            var positions = _positionCalculator.GetPosition(index);

            var row = _sectionCutter.GetSingleRow(grid, positions[0]);
            var column = _sectionCutter.GetSingleColumn(grid, positions[1]);
            var square = _sectionCutter.GetSingleSquare(grid, positions[0], positions[1]);

            return !ContainsNoDuplicates(row) &&
                !ContainsNoDuplicates(column) &&
                !ContainsNoDuplicates(square);
        }

        private bool IsValidWholeGrid(int[] grid)
        {
            var rows = _sectionCutter.GetAllRows(grid);
            var columns = _sectionCutter.GetAllColumns(grid);
            var squares = _sectionCutter.GetAllSquares(grid);

            return ContainsNoDuplicates(squares) &&
                ContainsNoDuplicates(rows) &&
                ContainsNoDuplicates(columns);
        }

        private bool ContainsNoDuplicates(int[,] squares)
        {
            for (int i = 0; i < 9; i++)
            {
                var square = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    square[j] = squares[i, j];
                }

                if (ContainsNoDuplicates(square))
                {
                    return false;
                }
            }

            return true;
        }

        private bool ContainsNoDuplicates(int[] values) //https://stackoverflow.com/questions/723213/sudoku-algorithm-in-c-sharp
        {
            int flag = 0;
            foreach (int number in values)
            {
                if (number != 0)
                {
                    int bit = 1 << number;
                    if ((flag & bit) != 0)
                        return true;
                    flag |= bit;
                }
            }
            return false;
        }
    }
}
