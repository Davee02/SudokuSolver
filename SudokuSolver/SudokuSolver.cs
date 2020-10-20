using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class SudokuSolver
    {
        private readonly SudokuSectionCutter _sectionCutter;
        private readonly SudokuPositionCalculator _positionCalculator;
        private readonly SudokuValidator _validator;

        public SudokuSolver(SudokuSectionCutter sectionCutter, SudokuPositionCalculator positionCalculator, SudokuValidator validator)
        {
            _sectionCutter = sectionCutter;
            _positionCalculator = positionCalculator;
            _validator = validator;
        }

        public (string SolvedGrid, int Iterations, bool CouldSolve) SolveGrid(string unsolvedGrid)
        {
            var grid = Helper.GenerateGridArray(unsolvedGrid);
            var allPossibilities = GetPossibleNumbersInGrid(grid);
            var possibilitiesIndexes = new int[81];
            for (int i = 0; i < 81; i++)
            {
                possibilitiesIndexes[i] = -1;
            }

            if (_validator.IsSolved(grid) && _validator.IsValid(grid))
            {
                return (Helper.IntArrayToString(grid), 0, true);
            }

            bool isSolved;
            (grid, isSolved) = SolveNakedSingles(grid);

            int index = 0;
            bool isValid = false;
            bool cameFromAbove = false;
            int iterations = 0;
            while (!isSolved && index > -1)
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
                        isValid = _validator.IsValidForIndex(grid, index);
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
                    isSolved = _validator.IsSolved(grid);
                }

                ++index;
                ++iterations;
            }

            return (Helper.IntArrayToString(grid), iterations, isSolved);
        }

        private int[][] GetPossibleNumbersInGrid(int[] gridIntArray)
        {
            var possibilities = new int[81][];
            int indexInGrid = 0;
            var rawRows = _sectionCutter.GetAllRows(gridIntArray);
            var allRows = rawRows.ToJaggedArray();
            var allColumns = _sectionCutter.GetAllColumns(gridIntArray).ToJaggedArray();
            var allSquares = _sectionCutter.GetAllSquares(gridIntArray).ToJaggedArray();

            for (int i = 0; i < 81; i++)
            {
                var number = gridIntArray[i];

                if (number == 0)
                {
                    var possibleNumbers = Helper.ZeroToNineArray.AsEnumerable();
                    var position = _positionCalculator.GetPosition(indexInGrid);
                    var currentRow = allRows[position[0]];
                    var currentColumn = allColumns[position[1]];
                    var currentSquare = allSquares[position[2]];

                    var itemsToRemove = currentRow.Union(currentColumn).Union(currentSquare);
                    possibleNumbers = possibleNumbers.Except(itemsToRemove);

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

        private (int[], bool) SolveNakedSingles(int[] grid)
        {
            var onePossibilityIndexes = new List<int>(81);
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
                    index++;
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
