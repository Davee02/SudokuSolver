using System.Linq;

namespace SudokuSolver
{
    public class SudokuPositionCalculator
    {
        public int[] GetPosition(int indexOfNumber)
        {
            var postitions = new int[3];
            postitions[0] = GetRowIndex(indexOfNumber);
            postitions[1] = GetColumnIndex(indexOfNumber);
            postitions[2] = GetSquareIndex(postitions[0], postitions[1]);

            return postitions;
        }

        private int GetRowIndex(int indexOfNumber)
        {
            var rowIndexTemp = (double)indexOfNumber / 9;

            return (int)rowIndexTemp;
        }

        private int GetColumnIndex(int indexOfNumber)
        {
            var columnIndexTemp = (double)indexOfNumber / 9;
            columnIndexTemp -= (int)columnIndexTemp;
            columnIndexTemp *= 9;
            columnIndexTemp += 0.00001; // Tolerance of the rounding above

            return (int)columnIndexTemp;
        }

        private int GetSquareIndex(int indexOfRow, int indexOfColumn)
        {
            var allPossibilities = Helper.ZeroToNineArray;

            allPossibilities = (indexOfRow / 3) switch
            {
                0 => allPossibilities.Except(new[] { 3, 4, 5, 6, 7, 8, 9 }).ToArray(),
                1 => allPossibilities.Except(new[] { 0, 1, 2, 6, 7, 8 }).ToArray(),
                2 => allPossibilities.Except(new[] { 0, 1, 2, 3, 4, 5 }).ToArray(),
                _ => throw new System.NotImplementedException()
            };

            return (indexOfColumn / 3) switch
            {
                0 => allPossibilities[0],
                1 => allPossibilities[1],
                2 => allPossibilities[2],
                _ => throw new System.NotImplementedException()
            };
        }
    }
}
