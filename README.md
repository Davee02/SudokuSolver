# SudokuSolver
## Input forms
The solver supports two input-forms for the grid:

85...24..72......9..4.........1.7..23.5...9...4...........8..7..17..........36.4.

and

850002400720000009004000000000107002305000900040000000000080070017000000000036040   

where the '0's and '.'s are the blank fields in the grid

## Start the solver
The solver is kicked on via commandlineinterface. It Is capable of processing the following **parameters**:
* -solve <grid/s>: Solves the grid and outputs the solved one with pretty-print applied
* -validate <grid/s>: Doesn't solve anything. It just checks if the inputet grid is valid (it can be solved or not solved)

<grid/s> can be a grid or a path to a file with multiple grids separated by \n.
