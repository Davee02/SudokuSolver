using System;
using System.Diagnostics;

namespace SudokuSolver
{
    class Solver
    {
        static IOTransformer _ioTransformer = new IOTransformer();
        static SudokuSectionCutter _sectionCutter = new SudokuSectionCutter();
        static SudokuPositionCalculator _positionCalculator = new SudokuPositionCalculator();
        static SudokuValidator _validator = new SudokuValidator(_sectionCutter, _positionCalculator);
        static SudokuSolver _solver = new SudokuSolver(_sectionCutter, _positionCalculator, _validator);

        public static void Solve(string grid, bool shortOutput)
        {
            grid = _ioTransformer.TransformInput(grid);
            Console.Clear();
            if (grid.Length != 81)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please Input a valid grid");

                return;
            }
            if (!shortOutput)
            {
                Console.WriteLine("Your Input:\n\n" + _ioTransformer.TransformOutput(grid));
            }

            var watch = Stopwatch.StartNew();
            var (solvedGrid, iterations, isSolvable) = _solver.SolveGrid(grid);

            watch.Stop();

            if(!isSolvable)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The sudoku you entered is not solvable!");

                return;
            }

            if (shortOutput)
            {
                Console.WriteLine(solvedGrid);
            }
            else
            {
                Console.WriteLine("Elapsed Time: " + watch.ElapsedMilliseconds + "ms\nTotal Iterations: " + iterations + "\n");
                Console.WriteLine("The Solution:\n\n" + _ioTransformer.TransformOutput(solvedGrid));
            }
        }

        public static void Validate(string grid, bool shortOutput)
        {
            grid = _ioTransformer.TransformInput(grid);
            Console.Clear();

            if (grid.Length != 81)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please Input a valid grid");

                return;
            }

            var transformedGrid = Helper.GenerateGridArray(grid);
            bool isValid = _validator.IsValid(transformedGrid);

            if (shortOutput)
            {
                Console.WriteLine(isValid);
            }
            else
            {
                if (isValid)
                {
                    Console.WriteLine("Your inputed Sudoku is valid:\n\n\n" + _ioTransformer.TransformOutput(grid));
                }
                else
                {
                    Console.WriteLine("Your inputed Sudoku is invalid:\n\n\n" + _ioTransformer.TransformOutput(grid));
                }
            }
        }
    }
}
