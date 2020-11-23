module LEDs

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

let NumberOfLeds = 
    Position.All |> Seq.length

let posToLedNumber = function
    | BottomLeft -> 0
    | MiddleLeft -> 1
    | TopLeft -> 2
    | BottomMiddle -> 3
    | MiddleMiddle -> 4
    | TopMiddle -> 5
    | BottomRight -> 6
    | MiddleRight -> 7
    | TopRight -> 8
    | Nose -> 9
    | RightEye -> 10
    | LeftEye -> 11

let ledNumberToPos = function
    |  0 -> BottomLeft  
    |  1 -> MiddleLeft  
    |  2 -> TopLeft  
    |  3 -> BottomMiddle  
    |  4 -> MiddleMiddle  
    |  5 -> TopMiddle  
    |  6 -> BottomRight  
    |  7 -> MiddleRight  
    |  8 -> TopRight  
    |  9 -> Nose  
    |  10 -> RightEye  
    |  11 -> LeftEye  
    | n -> failwithf "Unknown Led Number: %d" n

type Pixel = {
    Position: Position
    Color : Color
}

let createPixels color positions = 
    positions
    |> List.map (fun pos -> { Position = pos; Color = color })

type Command = 
    | SetLed of Pixel
    | SetLeds of Pixel list
    | Display
    | SetAndDisplayLeds of Pixel list
    | Sleep of int
    | Clear

