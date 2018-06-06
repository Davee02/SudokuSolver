using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace RunSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            SudokuSolver.Test.Main(new string[] { "-solve"}); 
        }
    }
}
