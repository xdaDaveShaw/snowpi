module Mock

open Colorful
open System.Drawing

open LEDs

[<Literal>]
let Snowman = """
    
    ###############
     #############
      ###########
       #########
   #################
     /           \
    /  [X]   [X]  \
   |               |
    \     [X]     /
     \           /
     /           \
    /     [X]     \
   / [X]       [X] \
  /       [X]       \
 |  [X]         [X]  |
  \       [X]       /
   \[X]         [X]/
    \_____________/
"""

let mutable private originalPos = (0, 0)

let setup =
    Console.Clear()
    Console.WriteLine(Snowman, Color.White)
    originalPos <- Console.CursorLeft, Console.CursorTop

let private mapPosToConsole = function
    | TopLeft -> 6, 14
    | MiddleLeft -> 5, 16
    | BottomLeft -> 5, 18
    | TopMiddle -> 11, 13
    | MiddleMiddle -> 11, 15
    | BottomMiddle -> 11, 17
    | TopRight -> 16, 14
    | MiddleRight -> 17, 16
    | BottomRight -> 17, 18
    | Nose -> 11, 10
    | RightEye -> 14, 8
    | LeftEye ->  8, 8

let private setLed led =

    let character, color = 
        match led.State with
        | On col -> 'X', col
        | Off -> ' ', Color.Black
    
    try
        Console.SetCursorPosition (mapPosToConsole led.Position)
        Console.Write(character, color)
    finally
        Console.SetCursorPosition originalPos

let private executeCmd cmd = 
    match cmd with
    | SetLed p -> setLed p
    | SetLeds ps -> ps |> List.iter setLed
    | Display -> ()
    | Sleep ms -> System.Threading.Thread.Sleep(ms)

let execute (cmds : Command list) = 
    cmds
    |> List.iter executeCmd