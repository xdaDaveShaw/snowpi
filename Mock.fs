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

let run (leds : LED list) redraw = 

    let toFormat led = 
        match led.State with
        | On c -> Formatter("X", c)
        | Off -> Formatter(" ", Color.Black)

    let getOnLed i off =
        let setLed = 
            leds
            |> List.choose (fun led -> 
                if (posToLedNumber led.Position) = i then
                    Some (toFormat led)
                else
                    None)
            |> List.tryExactlyOne
        match setLed with
        | Some led -> led
        | None -> off

    let format =
        Formatter(" ", Color.Black)
        |> List.replicate 12
        |> List.mapi getOnLed
        |> List.toArray

    if redraw then
        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - snowManHeight)

    Console.WriteLineFormatted(SnowmanFormatString, Color.White, format)