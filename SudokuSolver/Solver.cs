using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace SudokuSolver
{
    class Solver
    {
        public static void Solve(string grid, bool shortOutput)
        {
            grid = Helper.RemoveWhitespaces(grid);
            grid = Parser.TransformGridFromPointToZero(grid);
            Console.Clear();
            if (grid.Length != 81)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please Input a valid grid");
                Console.ReadLine();
                return;
            }
            if (!shortOutput)
                Console.WriteLine("Your Input:\n\n" + Parser.PrettyPrintString(grid));

            var watch = Stopwatch.StartNew();
            var (solvedGrid, iterations) = SolveSudoku.SolveGrid(grid);

            watch.Stop();
            if (shortOutput)
            {
                Console.WriteLine(solvedGrid);
            }
            else
            {
                Console.WriteLine("Elapsed Time: " + watch.ElapsedMilliseconds + "ms\nTotal Iterations: " + iterations + "\n");
                Console.WriteLine("The Solution:\n\n" + Parser.PrettyPrintString(solvedGrid));
            }
            Console.ReadLine();
        }

        public static void Validate(string grid, bool shortOutput)
        {
            grid = Helper.RemoveWhitespaces(grid);
            grid = Parser.TransformGridFromPointToZero(grid);
            Console.Clear();

            if (grid.Length != 81)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please Input a valid grid");
                Console.ReadLine();
                return;
            }

            var transformedGrid = Helper.GenerateGridArray(grid);
            bool isValid = ValidateGrid.IsValid(transformedGrid);

            if (shortOutput)
            {
                Console.WriteLine(isValid);
            }
            else
            {
                if (isValid)
                {
                    Console.WriteLine("Your inputed Sudoku is valid:\n\n\n" + Parser.PrettyPrintString(grid));
                }
                else
                {
                    Console.WriteLine("Your inputed Sudoku is invalid:\n\n\n" + Parser.PrettyPrintString(grid));
                }
            }
            Console.ReadLine();
        }
    }

    class ValidateGrid
    {
        public static bool IsValid(int[] grid, int index = -1)
        {
            if (index == -1)
            {
                return IsValidWholeGrid(grid);
            }
            else
            {
                return IsValidSectionOfGrid(grid, index);
            }
        }

        private static bool IsValidSectionOfGrid(int[] grid, int index)
        {
            var positions = GetPositionInGrid.GetPosition(index);

            var row = GetSectionOfGrid.SingleRow(grid, positions[0]);
            var column = GetSectionOfGrid.SingleColumn(grid, positions[1]);
            var square = GetSectionOfGrid.SingleSquare(grid, positions[0], positions[1]);

            return !(SearchForDuplicates(row)) && !(SearchForDuplicates(column)) && !(SearchForDuplicates(square));
        }

        private static bool IsValidWholeGrid(int[] grid)
        {
            var rows = GetSectionOfGrid.Rows(grid);
            var columns = GetSectionOfGrid.Columns(grid);
            var squares = GetSectionOfGrid.Squares(grid);

            return ValidateSquare(squares) && ValidateRow(rows) && ValidateColumn(columns);
        }

        private static bool ValidateRow(int[,] rows)
        {
            for (int i = 0; i < 9; i++)
            {
                var row = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    row[j] = rows[i, j];
                }

                if (SearchForDuplicates(row))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateColumn(int[,] columns)
        {
            for (int i = 0; i < 9; i++)
            {
                var column = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    column[j] = columns[i, j];
                }

                if (SearchForDuplicates(column))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateSquare(int[,] squares)
        {
            for (int i = 0; i < 9; i++)
            {
                var square = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    square[j] = squares[i, j];
                }

                if (SearchForDuplicates(square))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool SearchForDuplicates(int[] values) //https://stackoverflow.com/questions/723213/sudoku-algorithm-in-c-sharp
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

    class GetSectionOfGrid
    {
        public static int[] SingleRow(int[] grid, int index)
        {
            var row = new int[9];
            for (int j = 0; j < 9; j++)
            {
                row[j] = grid[index * 9 + j];
            }

            return row;
        }

        public static int[,] Rows(int[] grid)
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

        public static int[,] Columns(int[] grid)
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

        public static int[] SingleColumn(int[] grid, int index)
        {
            var column = new int[9];
            for (int j = 0; j < 9; j++)
            {
                column[j] = grid[index + 9 * j];
            }

            return column;
        }

        public static int[,] Squares(int[] grid)
        {
            var squares = new int[9, 9];

            int index = 0;
            for (int i = 0; i < 9; i += 3)
            {
                for (int j = 0; j < 9; j += 3, index++)
                {
                    var square = SingleSquare(grid, i, j);

                    for (int k = 0; k < 9; k++)
                    {
                        squares[index, k] = square[k];
                    }
                }
            }

            return squares;
        }


        public static int[] SingleSquare(int[] grid, int rowIndex, int columnIndex)
        {
            int indexOfSubGridRow = rowIndex % 3;
            var rows = new int[3][];

            if (indexOfSubGridRow == 0)
            {
                rows[0] = SingleRow(grid, rowIndex + 0);
                rows[1] = SingleRow(grid, rowIndex + 1);
                rows[2] = SingleRow(grid, rowIndex + 2);
            }
            else if (indexOfSubGridRow == 1)
            {
                rows[0] = SingleRow(grid, rowIndex - 1);
                rows[1] = SingleRow(grid, rowIndex + 0);
                rows[2] = SingleRow(grid, rowIndex + 1);
            }
            else if (indexOfSubGridRow == 2)
            {
                rows[0] = SingleRow(grid, rowIndex - 2);
                rows[1] = SingleRow(grid, rowIndex - 1);
                rows[2] = SingleRow(grid, rowIndex + 0);
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

    class GetPositionInGrid
    {
        public static int[] GetPosition(int indexOfNumber)
        {
            var postitions = new int[3];
            postitions[0] = GetRowIndex(indexOfNumber);
            postitions[1] = GetColumnIndex(indexOfNumber);
            postitions[2] = GetSquareIndex(postitions[0], postitions[1]);

            return postitions;
        }

        private static int GetRowIndex(int indexOfNumber)
        {
            var rowIndexTemp = (double)indexOfNumber / 9;

            return (int)rowIndexTemp;
        }

        private static int GetColumnIndex(int indexOfNumber)
        {
            var columnIndexTemp = (double)indexOfNumber / 9;
            columnIndexTemp -= (int)columnIndexTemp;
            columnIndexTemp *= 9;
            columnIndexTemp += 0.00001; // Tolerance of the rounding above

            return (int)columnIndexTemp;
        }

        private static int GetSquareIndex(int indexOfRow, int indexOfColumn)
        {
            var allPossibilities = Helper.OneToNineArray;

            switch (indexOfRow / 3)
            {
                case 0:
                    allPossibilities = allPossibilities.Except(new[] { 3, 4, 5, 6, 7, 8, 9 }).ToArray();
                    break;
                case 1:
                    allPossibilities = allPossibilities.Except(new[] { 0, 1, 2, 6, 7, 8 }).ToArray();
                    break;
                case 2:
                    allPossibilities = allPossibilities.Except(new[] { 0, 1, 2, 3, 4, 5 }).ToArray();
                    break;
            }

            return (indexOfColumn / 3) switch
            {
                0 => allPossibilities[0],
                1 => allPossibilities[1],
                _ => allPossibilities[2],
            };
        }
    }
    class SolveSudoku
    {
        public static (string, int) SolveGrid(string unsolvedGrid)
        {
            var grid = Helper.GenerateGridArray(unsolvedGrid);
            var allPossibilities = GetPossibleNumbersInGrid(grid);
            var possibilitiesIndexes = new int[81];
            for (int i = 0; i < 81; i++)
            {
                possibilitiesIndexes[i] = -1;
            }

            if (IsSolved(grid) && ValidateGrid.IsValid(grid))
            {
                return (Helper.IntArrayToString(grid), 0);
            }

            bool isSolved;
            (grid, isSolved) = InsertCandidatesWithOnePossibility(grid);

            int index = 0;
            bool isValid = false;
            bool cameFromAbove = false;
            int iterations = 0;
            while (!isSolved)
            {
                var currentPossibility = allPossibilities[index];
                if (currentPossibility.Length == 1)
                {
                    if (cameFromAbove)
                    {
                        index -= 2;
                    }
                }
                else
                {
                    int indexOfPossibility = possibilitiesIndexes[index];
                    if (indexOfPossibility == -1)
                    {
                        indexOfPossibility = 0;
                        possibilitiesIndexes[index] = 0;
                    }
                    if (indexOfPossibility < currentPossibility.Length)
                    {
                        ++possibilitiesIndexes[index];
                        grid[index] = currentPossibility[indexOfPossibility];
                        isValid = ValidateGrid.IsValid(grid, index);
                        if (!isValid)
                        {
                            grid[index] = 0;
                            --index;
                        }
                        else
                        {
                            cameFromAbove = false;
                        }
                    }
                    else
                    {
                        if (cameFromAbove)
                        {
                            grid[index] = 0;
                            possibilitiesIndexes[index] = -1;
                            index -= 2;
                        }
                        else
                        {
                            grid[index] = 0;
                            possibilitiesIndexes[index] = -1;
                            index -= 2;
                            cameFromAbove = true;
                        }
                    }
                }
                if (isValid)
                {
                    isSolved = IsSolved(grid);
                }
                ++index;
                ++iterations;
            }

            return (Helper.IntArrayToString(grid), iterations);
        }

        private static int[][] GetPossibleNumbersInGrid(int[] gridIntArray)
        {
            var possibilities = new int[81][];
            int indexInGrid = 0;
            var rawRows = GetSectionOfGrid.Rows(gridIntArray);
            var allRows = rawRows.ToJaggedArray();
            var allColumns = GetSectionOfGrid.Columns(gridIntArray).ToJaggedArray();
            var allSquares = GetSectionOfGrid.Squares(gridIntArray).ToJaggedArray();

            for (int i = 0; i < 81; i++)
            {
                var number = gridIntArray[i];

                if (number == 0)
                {
                    var possibleNumbers = Helper.OneToNineArray.ToList();
                    var position = GetPositionInGrid.GetPosition(indexInGrid);
                    var currentRow = allRows[position[0]];
                    var currentColumn = allColumns[position[1]];
                    var currentSquare = allSquares[position[2]];

                    var itemsToRemove = currentRow.Union(currentColumn).Union(currentSquare).ToList();
                    possibleNumbers = possibleNumbers.Except(itemsToRemove).ToList();

                    possibilities[i] = possibleNumbers.ToArray();
                }
                else
                {
                    possibilities[i] = new int[1] { gridIntArray[indexInGrid] };
                }

                ++indexInGrid;
            }

            return possibilities;
        }

        private static bool IsSolved(int[] grid)
        {
            bool isGridFilledOut = IsFilledOut(grid);
            if (!isGridFilledOut)
                return false;

            return ValidateGrid.IsValid(grid);
        }

        private static bool IsFilledOut(IEnumerable<int> gridIntArray)
        {
            return gridIntArray.All(number => number != 0);
        }

        private static (int[], bool) InsertCandidatesWithOnePossibility(int[] grid)
        {
            var onePossibilityIndexes = new List<int>();
            int onePossibilityIndexesFromLastIteration = 1;

            while (onePossibilityIndexes.Count != onePossibilityIndexesFromLastIteration)
            {
                onePossibilityIndexesFromLastIteration = onePossibilityIndexes.Count;
                onePossibilityIndexes.Clear();
                var possibilities = GetPossibleNumbersInGrid(grid);
                var index = 0;
                foreach (var possibility in possibilities)
                {
                    if (possibility.Length == 1)
                    {
                        onePossibilityIndexes.Add(index);
                    }
                    ++index;
                }
                foreach (var indexOfOne in onePossibilityIndexes)
                {
                    grid[indexOfOne] = possibilities[indexOfOne][0];
                }
                if (onePossibilityIndexes.Count == 81)
                {
                    return (grid, true);
                }
            }

            return (grid, false);
        }
    }
}
