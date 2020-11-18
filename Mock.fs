module Mock

open Colorful
open System
open System.Drawing

type Position =
    | BottomLeft
    | MiddleLeft
    | TopLeft
    | BottomRight
    | MiddleRight
    | TopRight
    | Nose
    | LeftEye
    | RightEye
    | BottomMiddle
    | MiddleMiddle
    | TopMiddle
    static member All = 
        Reflection.FSharpType.GetUnionCases(typeof<Position>)
        |> Seq.map (fun u -> Reflection.FSharpValue.MakeUnion(u, Array.empty) :?> Position)
        |> Seq.toList

type State = 
| Off
| On of Color

type LED = {
    Position: Position
    State : State
}

let posToLedNumber = function
    | BottomLeft -> 0
    | MiddleLeft -> 1
    | TopLeft -> 2
    | BottomRight -> 3
    | MiddleRight -> 4
    | TopRight -> 5
    | Nose -> 6
    | LeftEye -> 7
    | RightEye -> 8
    | BottomMiddle -> 9
    | MiddleMiddle -> 10
    | TopMiddle -> 11

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

let snowManHeight = 
    let lineCount = 
        SnowmanFormatString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
        |> Seq.length
    lineCount + 2 //One for the first and one for the last line

[<Literal>]
let NumberOfPixels = 12

let run (leds : LED list) redraw = 

    let on col = Formatter("X", col)
    let off = Formatter(" ", Color.Black)

    let tryCreateFormatterForLed { State = state} = 
        match state with
        | On col -> on col |> Some
        | Off -> None

    let tryGetLedByLedNumber ledNumber = 
        leds
        |> List.tryFind (fun led -> posToLedNumber led.Position = ledNumber)

    let getFormatterForLedNumber ledNumber =
        tryGetLedByLedNumber ledNumber
        |> Option.bind tryCreateFormatterForLed
        |> Option.defaultValue off

    let format =
        [| 0..NumberOfPixels - 1 |]
        |> Array.map getFormatterForLedNumber

    if redraw then
        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - snowManHeight)

    Console.WriteLineFormatted(SnowmanFormatString, Color.White, format)