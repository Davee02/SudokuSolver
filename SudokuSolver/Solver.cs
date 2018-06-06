using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Linq.Enumerable;
using System.Text;

namespace SudokuSolver
{
    class Solver
    {
        public static void Solve(string grid, bool shortOutput)
        {
            grid = Helper.removeWhitespaces(grid);
            grid = Parser.transformGridFromPointToZero(grid);
            Console.Clear();
            if (grid.Length != 81)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please Input a valid grid");
                Console.ReadLine();
                return;
            }
            if (shortOutput == false)
                Console.WriteLine("Your Input:\n\n" + Parser.prettyPrintString(grid));

            var watch = Stopwatch.StartNew();
            (var solvedGrid, var iterations) = SolveSudoku.solveGrid(grid);

            watch.Stop();
            if (shortOutput)
            {
                Console.WriteLine(solvedGrid);
            }
            else
            {
                Console.WriteLine("Elapsed Time: " + watch.ElapsedMilliseconds + "ms\nTotal Iterations: " + iterations + "\n");
                Console.WriteLine("The Solution:\n\n" + Parser.prettyPrintString(solvedGrid));
            }
            Console.ReadLine();
        }

        public static void Validate(string grid, bool shortOutput)
        {
            grid = Helper.removeWhitespaces(grid);
            grid = Parser.transformGridFromPointToZero(grid);
            Console.Clear();
            if (grid.Length != 81)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please Input a valid grid");
                Console.ReadLine();
                return;
            }

            var transformedGrid = Helper.generateGridArray(grid);
            bool isValid = ValidateGrid.isValid(transformedGrid);

            if (shortOutput)
            {
                Console.WriteLine(isValid);
            }
            else
            {
                if (isValid)
                {
                    Console.WriteLine("Your inputed Sudoku is valid:\n\n\n" + Parser.prettyPrintString(grid));
                }
                else
                {
                    Console.WriteLine("Your inputed Sudoku is invalid:\n\n\n" + Parser.prettyPrintString(grid));
                }
            }
            Console.ReadLine();
        }
    }

    class ValidateGrid
    {
        public static bool isValid(int[] grid, int index = -1)
        {
            if (index == -1)
            {
                return isValidWholeGrid(grid);
            }
            else
            {
                return isValidSectionOfGrid(grid, index);
            }

        }

        private static bool isValidSectionOfGrid(int[] grid, int index)
        {
            var gridString = Helper.intArrayToString(grid);
            var positions = GetPositionInGrid.getPosition(gridString, index);

            var row = GetSectionOfGrid.singleRow(grid, positions[0]);
            var column = GetSectionOfGrid.singleColumn(grid, positions[1]);
            var square = GetSectionOfGrid.singleSquare(row, grid, positions[0], positions[1]);

            if (!(searchForDuplicates(row)))
            {
                if (!(searchForDuplicates(column)))
                {
                    if (!(searchForDuplicates(square)))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        private static bool isValidWholeGrid(int[] grid)
        {
            var rows = GetSectionOfGrid.Rows(grid);
            var columns = GetSectionOfGrid.Columns(grid);
            var squares = GetSectionOfGrid.Squares(rows);

            if (validateSquare(squares) && validateRow(rows) && validateColumn(columns))
                return true;
            else
                return false;
        }

        private static bool validateRow(string[] rows)
        {
            for (int i = 0; i < 9; i++)
            {
                if (searchForDuplicates(rows[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool validateColumn(string[] columns)
        {
            for (int i = 0; i < 9; i++)
            {
                if (searchForDuplicates(columns[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool validateSquare(string[] squares)
        {
            for (int i = 0; i < 9; i++)
            {
                if (searchForDuplicates(squares[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool searchForDuplicates(string values) //https://stackoverflow.com/questions/723213/sudoku-algorithm-in-c-sharp
        {
            int flag = 0;
            foreach (char number in values)
            {
                int value = Convert.ToInt32(number.ToString());
                if (value != 0)
                {
                    int bit = 1 << value;
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
        private static StringBuilder sb = new StringBuilder();
        public static string singleRow(int[] grid, int index)
        {
            sb.Clear();
            for (int j = 0; j < 9; j++)
            {
                sb.Append(grid[index * 9 + j]);
            }

            return sb.ToString(); ;
        }

        public static string[] Rows(int[] grid)
        {
            var _rows = new string[9];
            for (int i = 0; i < 9; i++)
            {
                sb.Clear();
                for (int j = 0; j < 9; j++)
                {
                    sb.Append(grid[i * 9 + j]);
                }
                _rows[i] = sb.ToString();
            }

            return _rows;
        }

        public static string[] Columns(int[] grid)
        {
            var _columns = new string[9];
            for (int i = 0; i < 9; i++)
            {
                sb.Clear();
                for (int j = 0; j < 9; j++)
                {
                    sb.Append(grid[i + 9 * j]);
                }
                _columns[i] = sb.ToString();
            }

            return _columns;
        }
        public static string singleColumn(int[] grid, int index)
        {
            sb.Clear();
            for (int j = 0; j < 9; j++)
            {
                sb.Append(grid[index + 9 * j]);
            }
            return sb.ToString();
        }
        public static string[] Squares(string[] rows)
        {
            var _squares = new List<string>();
            foreach (var times in Range(0, 3))
            {
                var littleSquares = new string[3]; 
                foreach (var row in Range(0, 3))
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (i < 3)
                            littleSquares[0] += rows[row + 3 * times][i];
                        else if (i < 6)
                            littleSquares[1] += rows[row + 3 * times][i];
                        else
                            littleSquares[2] += rows[row + 3 * times][i];
                    }
                }
                _squares.AddRange(littleSquares);
            }

            return _squares.ToArray();
        }

        public static string singleSquare(string row, int[] grid, int rowIndex, int columnIndex)
        {
            int indexOfSubGridRow = rowIndex % 3;
            var rows = new string[3];

            if (indexOfSubGridRow == 0)
            {
                rows[0] = row;
                rows[1] = singleRow(grid, rowIndex + 1);
                rows[2] = singleRow(grid, rowIndex + 2);
            }
            else if (indexOfSubGridRow == 1)
            {
                rows[0] = singleRow(grid, rowIndex - 1);
                rows[1] = row;
                rows[2] = singleRow(grid, rowIndex + 1);
            }
            else if (indexOfSubGridRow == 2)
            {
                rows[0] = singleRow(grid, rowIndex - 2);
                rows[1] = singleRow(grid, rowIndex - 1);
                rows[2] = row;
            }
            sb.Clear();
            for (int i = 0; i < 3; i++)
            {
                if (columnIndex < 3)
                {
                    sb.Append(String.Concat(rows[i][0], rows[i][1], rows[i][2]));
                }
                else if (columnIndex < 6)
                {
                    sb.Append(String.Concat(rows[i][3], rows[i][4], rows[i][5]));
                }
                else
                {
                    sb.Append(String.Concat(rows[i][6], rows[i][7], rows[i][8]));
                }
            }
            
            return sb.ToString();
        }
    }

    class GetPositionInGrid
    {
        public static int[] getPosition(string grid, int indexOfNumber)
        {
            var postitions = new int[3];
            postitions[0] = getRowIndex(grid, indexOfNumber);
            postitions[1] = getColumnIndex(grid, indexOfNumber);
            postitions[2] = getSquareIndex(postitions[0], postitions[1]);

            return postitions;
        }

        private static int getRowIndex(string grid, int indexOfNumber)
        {
            var rowIndexTemp = (double)indexOfNumber / 9;
            int rowIndex = (int)rowIndexTemp;

            return rowIndex;
        }

        private static int getColumnIndex(string grid, int indexOfNumber)
        {
            var columnIndexTemp = (double)indexOfNumber / 9;
            columnIndexTemp = columnIndexTemp - (int)columnIndexTemp;
            columnIndexTemp *= 9;
            columnIndexTemp += 0.00001; // Tolerance of the rounding above

            return (int)columnIndexTemp;
        }

        private static int getSquareIndex(int indexOfRow, int indexOfColumn)
        {
            var allPossibilities = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            if (indexOfRow / 3 == 0)
                allPossibilities = allPossibilities.Except(new int[] { 3, 4, 5, 6, 7, 8, 9 }).ToArray();
            else if (indexOfRow / 3 == 1)
                allPossibilities = allPossibilities.Except(new int[] { 0, 1, 2, 6, 7, 8 }).ToArray();
            else if (indexOfRow / 3 == 2)
                allPossibilities = allPossibilities.Except(new int[] { 0, 1, 2, 3, 4, 5 }).ToArray();

            if (indexOfColumn / 3 == 0)
                return allPossibilities[0];
            else if (indexOfColumn / 3 == 1)
                return allPossibilities[1];
            else
                return allPossibilities[2];
        }
    }
    class SolveSudoku
    {
        public static (string, int) solveGrid(string unsolvedGrid)
        {
            var index = 0;
            var grid = Helper.generateGridArray(unsolvedGrid);
            bool isSolved = false;
            var allPossibilities = GetPossibleNumbersInGrid(grid);
            var possibilitiesIndexes = new int[81];
            for (int i = 0; i < 81; i++)
            {
                possibilitiesIndexes[i] = -1;
            }

            unsolvedGrid = Helper.intArrayToString(grid);
            if (IsSolved(unsolvedGrid))
            {
                if (ValidateGrid.isValid(grid))
                    return (unsolvedGrid, 0);
            }

            (grid, isSolved) = InsertCandidatesWithOnePossibility(grid, allPossibilities);

            index = 0;
            bool isValid = false;
            bool cameFromAbove = false;
            int iterations = 0;
            while (isSolved == false)
            {
                var currentPossibility = allPossibilities[index];
                if (currentPossibility.Count == 1)
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
                    if (indexOfPossibility < currentPossibility.Count)
                    {
                        ++possibilitiesIndexes[index];
                        grid[index] = currentPossibility[indexOfPossibility];
                        isValid = ValidateGrid.isValid(grid, index);
                        if (isValid == false)
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
                    isSolved = IsSolved(Helper.intArrayToString(grid));
                }
                ++index;
                ++iterations;
            }

            return (Helper.intArrayToString(grid), iterations);
        }

        private static List<List<int>> GetPossibleNumbersInGrid(int[] gridIntArray)
        {
            var possibilities = new List<List<int>>();
            int indexInGrid = 0;
            var allRows = GetSectionOfGrid.Rows(gridIntArray);
            var allColumns = GetSectionOfGrid.Columns(gridIntArray);
            var allSquares = GetSectionOfGrid.Squares(allRows);
            string gridString = Helper.intArrayToString(gridIntArray);

            foreach (char number in gridString)
            {
                if (number == '0')
                {
                    var possibleNumbers = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.ToList();
                    var position = GetPositionInGrid.getPosition(gridString, indexInGrid);
                    var currentRow = Helper.stringToIntList(allRows[position[0]]);
                    var currentColumn = Helper.stringToIntList(allColumns[position[1]]);
                    var currentSquare = Helper.stringToIntList(allSquares[position[2]]);

                    var itemsToRemove = currentRow.Union(currentColumn).Union(currentSquare).ToList();
                    possibleNumbers = possibleNumbers.Except(itemsToRemove).ToList();

                    possibilities.Add(possibleNumbers);
                }
                else
                    possibilities.Add(new List<int> { Convert.ToInt32(gridString[indexInGrid].ToString()) });
                ++indexInGrid;
            }

            return possibilities;
        }

        private static bool IsSolved(string grid)
        {
            var gridArray = Helper.generateGridArray(grid);
            bool isGridFilledOut = isFilledOut(gridArray);
            if (isGridFilledOut == false)
                return false;
            bool isGridValid = ValidateGrid.isValid(gridArray);
            return isGridValid;
        }

        private static bool isFilledOut(int[] gridIntArray)
        {
            return gridIntArray.All(number => number != 0);
        }

        private static (int[], bool) InsertCandidatesWithOnePossibility(int[] grid, List<List<int>> possibilities)
        {
            var onePossibilityIndexes = new List<int> { };
            int onePossibilityIndexesFromLastIteration = 1;
            var index = 0;

            while (onePossibilityIndexes.Count != onePossibilityIndexesFromLastIteration)
            {
                onePossibilityIndexesFromLastIteration = onePossibilityIndexes.Count;
                onePossibilityIndexes.Clear();
                possibilities = GetPossibleNumbersInGrid(grid);
                index = 0;
                foreach (var possibility in possibilities)
                {
                    if (possibility.Count == 1)
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
                    return(grid, true);
                }
            }

            return (grid, false);
        }
    }
}
