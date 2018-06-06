using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    class Helper
    {
        public static int[] generateGridArray(string gridString)
        {
            var _gridString = gridString;
            var _gridArray = new int[81];
            int _counter = -1;
            foreach (char number in _gridString)
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
            int arrayLength = intArray.Length;
            var outputString = "";

            for (int i = 0; i < arrayLength; i++)
            {
                outputString += intArray[i].ToString();
            }

            return outputString;
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
