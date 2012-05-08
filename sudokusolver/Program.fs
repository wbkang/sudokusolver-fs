// Learn more about F# at http://fsharp.net
namespace sudoku_solver
    module entry_point =
        [<EntryPoint>]
        let main args =
           
            let test_board = 
                            [|  
                                [| -1; -1; -1; 1; -1; 5; -1; -1; -1 |];
                                [| 1; 4; -1; -1; -1; -1; 6; 7; -1 |];
                                [| -1; 8; -1; -1; -1; 2; 4; -1; -1 |];
                                [| -1; 6; 3; -1; 7; -1; -1; 1; -1 |];
                                [| 9; -1; -1; -1; -1; -1; -1; -1; 3 |];
                                [| -1; 1; -1; -1; 9; -1; 5; 2; -1 |];
                                [| -1; -1; 7; 2; -1; -1; -1; 8; -1 |];
                                [| -1; 2; 6; -1; -1; -1; -1; 3; 5 |];
                                [| -1; -1; -1; 4; -1; 9; -1; -1; -1 |]
                            |]

            sudoku.convert_to_2darray test_board
            0