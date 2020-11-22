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

type State = 
    | Off
    | On of Color

type Pixel = {
    Position: Position
    State : State
}

type Command = 
    | SetLed of Pixel
    | SetLeds of Pixel list
    | Sleep of int
    | Display

let createPixels state positions = 
    positions
    |> List.map (fun pos -> { Position = pos; State = state; })

let createPixelOn color pos =
    { Position = pos; State = On color; }

let createPixelsOn color positions = 
    positions
    |> List.map (fun pos -> createPixelOn color pos)

let createAll state = 
    Position.All
    |> createPixels state

let createAllOn color = 
    Position.All
    |> createPixelsOn color

let allOff =
    createAll Off

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