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

let timeit fn = 
    let start = System.DateTime.Now
    fn ()
    printfn "Elapsed Time: %A" (System.DateTime.Now - start)


let board = sudoku.convert_to_2darray test_board
printfn "Initial board:"
sudoku.pretty_print (Some board)
printfn "Final board:"


let doit () =
    sudoku.pretty_print (sudoku.solve board)

///timeit doit

open System.Windows.Forms
open System.Drawing

let set_sudoku_panel_style (panel:TableLayoutPanel) =
    for row in 1..3 do
        panel.RowStyles.Add(new RowStyle(SizeType = SizeType.Percent, Height = float32 33.3)) |> ignore
    for col in 1..3 do
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, float32 33.3)) |> ignore

let main_window = new Form(Text="Sudoku Solver")

let sudoku_panel = new TableLayoutPanel(Dock = DockStyle.Fill, BackColor = Color.Azure, RowCount = 3, ColumnCount = 3)
set_sudoku_panel_style sudoku_panel
main_window.Controls.Add(sudoku_panel)

let solve_button = new Button(Text="Solve!", Dock = DockStyle.Bottom)
main_window.Controls.Add(solve_button)

let sudoku_supercells = 
    [for i in 1..9 do
        let supercell = new TableLayoutPanel(Dock = DockStyle.Fill, BackColor = Color.AliceBlue, RowCount = 3, ColumnCount = 3)
        sudoku_panel.Controls.Add(supercell)
        set_sudoku_panel_style supercell
        yield supercell]

let sudoku_numbercells = [
    for i in 0..80 do
        let textbox = new TextBox(Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, Text="?")
        textbox.GotFocus.Add (fun _ -> textbox.SelectAll())
        let x = (i % 9) / 3
        let y = (i / 9) / 3
        sudoku_supercells.[x + 3 * y].Controls.Add(textbox)
        yield textbox]

List.iteri (fun idx (textbox:TextBox) ->
    let focus_next () = 
        if  idx < 80 then 
            sudoku_numbercells.[idx+1].Focus() |> ignore
        else
            solve_button.Focus() |> ignore

    textbox.TextChanged.Add(fun _ ->
            let parsed, number = System.Int32.TryParse(textbox.Text)
            match parsed with
                | true ->
                    if 1 <= number && number <= 9 then 
                        focus_next()
                    else 
                        textbox.Undo()
                        textbox.Focus() |> ignore
                | false ->
                    if textbox.Text = "?" then 
                        focus_next()
                    else 
                        textbox.Undo()
                        textbox.Focus() |> ignore
        )
    ) 
    sudoku_numbercells
    
solve_button.Click.Add(fun _ ->
    let ary = Array2D.create 9 9 -1
    List.iteri (fun idx (textbox:TextBox) ->
            let parsed, number = System.Int32.TryParse(textbox.Text)
            match parsed with
                | true -> ary.[idx % 9, idx / 9] <- number
                | _ -> ()
        ) 
        sudoku_numbercells
    let result = sudoku.solve ary
    match result with
        | None -> MessageBox.Show("FAIL") |> ignore
        | Some(a) ->
            for i in 0..80 do
                sudoku_numbercells.[i].Text <- a.[i].toString ()
    printfn "%A" ary
)
    

Application.EnableVisualStyles()
main_window.Visible <- true

