namespace sudoku_solver
module sudoku =
    type sudoku_board = int[,]
    type next_step = 
        | Fail
        | Done
        | NextStep of int * int * Set<int>

    let all_numbers = Set.ofList [ 1..9 ]

    let convert_to_2darray (ary : int[][]) =
        let result = Array2D.create 9 9 -1
        ary |> Array.iteri (fun row_idx row ->
                row |> Array.iteri (fun col_idx num -> 
                    if row_idx > 8 || col_idx > 8 then failwithf "row: %d, col: %d" row_idx col_idx
                    result.[row_idx, col_idx] <- num
                    ))
        result

    let generate_possible_numbers (board : sudoku_board) row col =
        let all_row_col_vals = seq {
            // row values
            for c in [0..8] -> board.[row, c]
            for r in [0..8] -> board.[r, col]
        }

        Seq.fold (fun avail_nums value -> Set.remove value avail_nums) all_numbers all_row_col_vals



    let find_best_loc (board : sudoku_board) =
        let all_unsolved_row_cols = 
            seq { for c in [0..8] do
                    for r in [0..8] do
                        if board.[r, c] = -1 then yield (r, c) }
    
        all_unsolved_row_cols |> 
            Seq.fold (fun candidate (row, col) ->
                match candidate with
                    | Fail -> Fail
                    | _ ->
                        let possible_set = generate_possible_numbers board row col
                        let set_size = Set.count possible_set
                        if set_size = 0 then
                            Fail
                        else
                            let answer = NextStep(row, col, possible_set)
                            match candidate with
                                | NextStep(r,c,s) as oldans ->
                                    if set_size <= (Set.count s) then
                                        answer
                                    else
                                        oldans
                                | Done -> answer
                                | Fail -> Fail // the stupid compiler complains
            ) Done

    let board_set_value (board:sudoku_board) r c value =
        let copy = Array2D.copy board
        if copy.[r,c] <> -1 then failwithf "row:%d, col:%d, value already set to %d" r c copy.[r,c]
        copy.[r,c] <- value
        copy

    let solve board =
        let rec solve_rec (cur_board:sudoku_board) next =
            match next with
                | Fail -> None
                | Done -> Some(cur_board)
                | NextStep(row, col, nums) ->
                    nums |> Seq.fold (fun prev_result possibility ->
                        match prev_result with
                            | None -> 
                                let next_board = board_set_value cur_board row col possibility
                                solve_rec next_board (find_best_loc next_board)
                            | ans -> ans
                        ) None
        solve_rec board (find_best_loc board)

    let pretty_print (b:sudoku_board option) =
        match b with
            | None -> printfn "Couldn't solve :("
            | Some(board) ->
                let print_bar () = 
                    for x in [1..31] do printf "-"
                    printfn ""
                let print_char row col =
                    match board.[row,col] with
                        | -1 -> printf " ? "
                        | x -> printf " %d " x
                for row in [0..8] do
                    if row % 3 = 0 then
                        print_bar ()
                    for col in [0..8] do
                        if col % 3 = 0 then
                            printf "|"    
                        print_char row col
                    printfn "|"
                print_bar ()
                    
