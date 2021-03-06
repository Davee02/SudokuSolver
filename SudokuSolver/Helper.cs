﻿using System;
using System.IO;

namespace SudokuSolver
{
    static class Helper
    {
        public static readonly int[] ZeroToNineArray = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public static int[] GenerateGridArray(string gridString)
        {
            var gridArray = new int[81];
            int counter = 0;
            foreach (char number in gridString)
            {
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

                counter++;
            }

            return gridArray;
        }

        public static string IntArrayToString(int[] intArray)
        {
            return new string(Array.ConvertAll(intArray, x => (char)('0' + x)));
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

            return File.ReadAllLines(path);
        }

        public static T[][] ToJaggedArray<T>(this T[,] twoDimensionalArray)
        {
            int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
            int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
            int numberOfRows = rowsLastIndex + 1;

            int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
            int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex + 1;

            T[][] jaggedArray = new T[numberOfRows][];
            for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    jaggedArray[i][j] = twoDimensionalArray[i, j];
                }
            }

            return jaggedArray;
        }
    }
}
