using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Linq.Enumerable;

namespace SudokuSolver
{
    class Solver
    {
        public static void Solve(string grid)
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
            Console.WriteLine("Your Input:\n\n" + Parser.prettyPrintString(grid));

            var watch = Stopwatch.StartNew();
            (var solvedGrid, var iterations) = SolveSudoku.solveGrid(grid);

            watch.Stop();
            Console.WriteLine("Elapsed Time: " + watch.ElapsedMilliseconds + "ms\nTotal Iterations: " + iterations + "\n");
            Console.WriteLine("The Solution:\n\n" + Parser.prettyPrintString(solvedGrid));
            Console.ReadLine();
        }

        public static void Validate(string grid)
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

            if (isValid)
            {
                Console.WriteLine("Your inputed Sudoku is valid:\n\n\n" + Parser.prettyPrintString(grid));
            }
            else
            {
                Console.WriteLine("Your inputed Sudoku is invalid:\n\n\n" + Parser.prettyPrintString(grid));
            }
            Console.ReadLine();
        }
    }

    class ValidateGrid
    {
        public static bool isValid(int[] grid)
        {
            var _grid = grid;

            if (validateSquare(_grid))
            {
                if (validateRow(_grid))
                {
                    if (validateColumn(_grid))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;

        }

        private static bool validateRow(int[] grid)
        {
            var _grid = grid;
            var _rows = GetSectionOfGrid.Rows(_grid);
            foreach (var number in Range(0, 9))
            {
                if (searchForDuplicates(_rows[number]))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool validateColumn(int[] grid)
        {
            var _grid = grid;
            var _columns = GetSectionOfGrid.Columns(_grid);
            foreach (var number in Range(0, 9))
            {
                if (searchForDuplicates(_columns[number]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool validateSquare(int[] grid)
        {
            var _grid = grid;
            var _rows = GetSectionOfGrid.Rows(_grid);
            var _squares = GetSectionOfGrid.Squares(_rows);

            foreach (var number in Range(0, 9))
            {
                if (searchForDuplicates(_squares[number]))
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
        public static string[] Rows(int[] grid)
        {
            var _rows = new string[9];

            var _grid = grid;
            for (int i = 0; i < 9; i++)
            {
                var numbersInRow = new int[9];
                foreach (var number in Range(0, 9))
                {
                    numbersInRow[number] = _grid[i * 9 + number];
                }
                _rows[i] = string.Join("", numbersInRow);
            }

            return _rows;
        }

        public static string[] Columns(int[] grid)
        {
            var _columns = new string[9];

            var _grid = grid;
            for (int i = 0; i < 9; i++)
            {
                var numbersInColumn = new int[9];
                foreach (var number in Range(0, 9))
                {
                    numbersInColumn[number] = _grid[i + 9 * number];
                }
                _columns[i] = string.Join("", numbersInColumn);
            }

            return _columns;
        }

        public static string[] Squares(string[] rows)
        {
            var _squares = new List<string>();
            var _rows = rows;

            foreach (var times in Range(0, 3))
            {
                var littleSquares = new string[3];
                foreach (var row in Range(0, 3))
                {
                    foreach (var column in Range(0, 9))
                    {
                        if (column < 3)
                            littleSquares[0] += rows[row + 3 * times][column];
                        else if (column < 6)
                            littleSquares[1] += rows[row + 3 * times][column];
                        else
                            littleSquares[2] += rows[row + 3 * times][column];
                    }
                }
                _squares.AddRange(littleSquares);
            }

            return _squares.ToArray();
        }
    }

    class GetPositionInGrid
    {
        public static int[] getPosition(string grid, int indexOfNumber)
        {
            var postitions = new int[3];
            int rowIndex = getRowIndex(grid, indexOfNumber);
            int columnIndex = getColumnIndex(grid, indexOfNumber);
            int squareIndex = getSquareIndex(grid, rowIndex, columnIndex);

            postitions[0] = rowIndex;
            postitions[1] = columnIndex;
            postitions[2] = squareIndex;

            return postitions;
        }

        private static int getRowIndex(string grid, int indexOfNumber)
        {
            int _rowIndex = 0;

            var rowIndexTemp = (double)indexOfNumber / 9;
            _rowIndex = (int)rowIndexTemp;

            return _rowIndex;
        }

        private static int getColumnIndex(string grid, int indexOfNumber)
        {
            int _columnIndex = 0;

            var columnIndexTemp = (double)indexOfNumber / 9;
            columnIndexTemp = columnIndexTemp - (int)columnIndexTemp;
            columnIndexTemp *= 9;
            columnIndexTemp += 0.00001; // Tolerance of the rounding above
            _columnIndex = (int)columnIndexTemp;

            return _columnIndex;
        }

        private static int getSquareIndex(string grid, int indexOfRow, int indexOfColumn)
        {
            int _squareIndex = 0;
            var allPossibilities = Range(0, 9).ToList();

            if (indexOfRow / 3 == 0)
                allPossibilities = allPossibilities.Except(Range(3, 9).ToList()).ToList();
            else if (indexOfRow / 3 == 1)
                allPossibilities = allPossibilities.Except(new int[] { 0, 1, 2, 6, 7, 8 }).ToList();
            else if (indexOfRow / 3 == 2)
                allPossibilities = allPossibilities.Except(new int[] { 0, 1, 2, 3, 4, 5 }).ToList();

            if (indexOfColumn / 3 == 0)
                _squareIndex = allPossibilities[0];
            else if (indexOfColumn / 3 == 1)
                _squareIndex = allPossibilities[1];
            else if (indexOfColumn / 3 == 2)
                _squareIndex = allPossibilities[2];



            return _squareIndex;
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
            var possibilitiesIndexes = new List<int> { };
            for (int i = 0; i < 81; i++)
            {
                possibilitiesIndexes.Add(-1);
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
                        isValid = ValidateGrid.isValid(grid);
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
                    var possibleNumbers = Array.ConvertAll(Range(0, 10).ToArray(), s => s).ToList();
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
            var isFilledOut = true;

            foreach (int number in gridIntArray)
            {
                if (number == 0)
                {
                    isFilledOut = false;
                    break;
                }
            }

            return isFilledOut;
        }

        private static (int[], bool) InsertCandidatesWithOnePossibility(int[] grid, List<List<int>> possibilities)
        {
            var onePossibilityIndexes = new List<int> { };
            int onePossibilityIndexesFromLastIteration = 1;
            var gridString = "";
            var index = 0;

            while (onePossibilityIndexes.Count != onePossibilityIndexesFromLastIteration)
            {
                onePossibilityIndexesFromLastIteration = onePossibilityIndexes.Count;
                onePossibilityIndexes.Clear();
                gridString = Helper.intArrayToString(grid);
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
