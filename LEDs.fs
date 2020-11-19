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

type LED = {
    Position: Position
    State : State
}

let createLeds state positions = 
    positions
    |> List.map (fun pos -> { Position = pos; State = state; })

let createLedsOn color positions = 
    positions
    |> List.map (fun pos -> { Position = pos; State = On color; })

let createAll state = 
    Position.All
    |> createLeds state

let createAllOn color = 
    Position.All
    |> createLedsOn color

let allOff =
    createAll Off

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