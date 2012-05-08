#load "sudoku.fs"

open sudoku_solver

let test_board = 
                [|  
                    [| -1; -1; -1; 1; -1; 5; -1; -1; -1 |];
                    [| 1; 4; -1; -1; -1; -1; 6; 7; -1 |];
                    [| -1; 8; -1; -1; -1; 2; 4; -1; -1; |];
                    [| -1; 6; 3; -1; 7; -1; -1; 1; -1 |];
                    [| 9; -1; -1; -1; -1; -1; -1; -1; 3 |];
                    [| -1; 1; -1; -1; 9; -1; 5; 2; -1 |];
                    [| -1; -1; 7; 2; -1; -1; -1; 8; -1 |];
                    [| -1; 2; 6; -1; -1; -1; -1; 3; 5 |];
                    [| -1; -1; -1; 4; -1; 9; -1; -1; -1 |]
                |]

let board = sudoku.convert_to_2darray test_board
printfn "Initial board:"
sudoku.pretty_print (Some board)
printfn "Final board:"
sudoku.pretty_print (sudoku.solve board)

