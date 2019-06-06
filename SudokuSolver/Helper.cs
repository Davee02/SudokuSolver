using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SudokuSolver
{
    class Helper
    {
        public static readonly int[] OneToNineArray = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

        public static int[] GenerateGridArray(string gridString)
        {
            var gridArray = new int[81];
            int counter = -1;
            foreach (char number in gridString)
            {
                ++counter;
                var numberString = number.ToString();
                try
                {
                    gridArray[counter] = int.Parse(numberString);
                }
                catch (FormatException)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please Input a valid grid");
                    Console.ReadLine();
                    Environment.Exit(1);
                }
            }

            return gridArray;
        }

        public static List<int> StringToIntList(string inputString)
        {
            var outputList = new List<int>();
            foreach (char singleChar in inputString)
            {
                var stringToAdd = int.Parse(singleChar.ToString());
                outputList.Add(stringToAdd);
            }
            return outputList;
        }

        public static string IntArrayToString(int[] intArray)
        {
            return new string(Array.ConvertAll(intArray, x => (char)('0' + x)));
        }
        public static string RemoveWhitespaces(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }

        public static string[] GetGridsFromFile(string path)
        {
            try
            {
                File.GetAttributes(path);
            }
            catch
            {
                return new[] { "" };
            }
            var grids = File.ReadAllLines(path);
            return grids;
        }
    }
}
