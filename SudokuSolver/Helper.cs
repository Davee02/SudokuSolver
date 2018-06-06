using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SudokuSolver
{
    class Helper
    {
        public static int[] generateGridArray(string gridString)
        {
            var _gridArray = new int[81];
            int _counter = -1;
            foreach (char number in gridString)
            {
                ++_counter;
                var numberString = number.ToString();
                try
                {
                    _gridArray[_counter] = Int32.Parse(numberString);
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

            return _gridArray;
        }

        public static List<int> stringToIntList(string inputString)
        {
            var outputList = new List<int>();
            foreach (char singleChar in inputString)
            {
                var stringToAdd = Int32.Parse(singleChar.ToString());
                outputList.Add(stringToAdd);
            }
            return outputList;
        }

        public static string intArrayToString(int[] intArray)
        {
            return new string(Array.ConvertAll(intArray, x => (char)('0' + x)));
        }
        public static string removeWhitespaces(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public static string[] getGridsFromFile(string path)
        {
            try
            {
                File.GetAttributes(path);
            }
            catch
            {
                return new string[] { "" };
            }
            var grids = File.ReadAllLines(path);
            return grids;
        }
    }
}
