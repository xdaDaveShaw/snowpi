module Mock

open Colorful
open System
open System.Drawing

open LEDs

[<Literal>]
let SnowmanFormatString = """
    
    ###############
     #############
      ###########
       #########
   #################
     /           \
    /  [{7}]   [{8}]  \
   |               |
    \     [{6}]     /
     \           /
     /           \
    /     [{11}]     \
   / [{2}]       [{5}] \
  /       [{10}]       \
 |  [{1}]         [{4}]  |
  \       [{9}]       /
   \[{0}]         [{3}]/
    \_____________/
"""

let extraLines = 
    if Runtime.InteropServices.RuntimeInformation.IsOSPlatform(Runtime.InteropServices.OSPlatform.Windows) then
        2 //One for the first and one for the last line
    else
        1

let snowManHeight = 
    let lineCount = 
        SnowmanFormatString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
        |> Seq.length
    lineCount + extraLines

// Lives in this module as it is only needed for mocks, there is no concept of
// redrawing on the Real SnowPi
let mutable private redraw = false

let display (pixels : Pixel list) = 

    let on col = Formatter("X", col)
    let off = Formatter(" ", Color.Black)

    let tryCreateFormatterForPixel { State = state } = 
        match state with
        | On col -> on col |> Some
        | Off -> None

    let getFormatterForPosition position =
        pixels
        |> List.tryFind (fun pixel -> pixel.Position = position)
        |> Option.bind tryCreateFormatterForPixel
        |> Option.defaultValue off

    let format =
        Position.All
        |> Seq.map getFormatterForPosition
        |> Seq.toArray

    if redraw then
        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - snowManHeight)
    else 
        redraw <- true

    Console.WriteLineFormatted(SnowmanFormatString, Color.White, format)