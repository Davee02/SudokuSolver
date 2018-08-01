using System;
using System.Linq;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            bool shortOutput = false;
            var arguments = args.ToList();
            if (arguments.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please provide a valid argument:");
                Console.WriteLine("\n-solve [Grid]");
                Console.WriteLine("-solve [Path to file with multiple grids]");
                Console.WriteLine("\n-validate [Grid]");
                Console.WriteLine("-validate [Path to file with multiple grids]");
                Console.ReadLine();
                return;
            }
            if(arguments.Contains("-short"))
            {
                shortOutput = true;
            }
            if (arguments.Contains("-solve"))
            {
                int index = arguments.IndexOf("-solve");
                try
                {
                    var grids = Helper.GetGridsFromFile(arguments[index + 1]);
                    if (grids.Length == 1 & grids[0] == "")
                        grids[0] = arguments[index + 1];
                    foreach (string grid in grids)
                    {
                        Solver.Solve(grid, shortOutput);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An Exception occured: " + e.Message);
                    Console.ReadLine();
                }
            }

            else if(arguments.Contains("-validate"))
            {
                int index = arguments.IndexOf("-validate");
                try
                {
                    var grids = Helper.GetGridsFromFile(arguments[index + 1]);
                    if (grids.Length == 1 & grids[0] == "")
                        grids[0] = arguments[index + 1];
                    foreach (string grid in grids)
                    {
                        Solver.Validate(grid, shortOutput);
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
