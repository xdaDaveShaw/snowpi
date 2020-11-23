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

type Pixel = {
    Position: Position
    Color : Color
}

let createPixels color positions = 
    positions
    |> List.map (fun pos -> { Position = pos; Color = color })

type Command = 
    | SetLeds of Pixel list
    | Display
    | SetAndDisplayLeds of Pixel list
    | Sleep of int

