using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "-solve")
            {
                try
                {
                    var grids = Helper.getGridsFromFile(args[1]);
                    if (grids.Length == 1 & grids[0] == "")
                        grids[0] = args[1];
                    foreach (string grid in grids)
                    {
                        Solver.Solve(grid);
                    }
                }
                catch
                {
                    Console.WriteLine("Please input your grid!");
                    Console.ReadLine();
                }
            }

            else if (args[0] == "-validate")
            {
                try
                {
                    var grids = Helper.getGridsFromFile(args[1]);
                    if (grids.Length == 1 & grids[0] == "")
                        grids[0] = args[1];
                    foreach (string grid in grids)
                    {
                        Solver.Validate(grid);
                    }
                }
                catch
                {
                    Console.WriteLine("Please input your grid!");
                    Console.ReadLine();
                }
            }
        }
    }
}
