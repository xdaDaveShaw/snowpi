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
    /  [ ]   [ ]  \
   |               |
    \     [ ]     /
     \           /
     /           \
    /     [ ]     \
   / [ ]       [ ] \
  /       [ ]       \
 |  [ ]         [ ]  |
  \       [ ]       /
   \[ ]         [ ]/
    \_____________/
"""

let mutable private originalPos = (0, 0)

let private drawSnowman () =
    Console.Clear()
    Console.WriteLine(Snowman, Color.White)

let setup () =
    drawSnowman ()
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

let private toRender = ResizeArray<Pixel>()

let setLeds pixels = 
    toRender.AddRange(pixels)

let private drawLed led =
    try
        Console.SetCursorPosition (mapPosToConsole led.Position)
        Console.Write('X', led.Color)
    finally
        Console.SetCursorPosition originalPos

let private render () = 
    
    toRender
    |> Seq.iter drawLed

    toRender.Clear()

let rec private executeCmd cmd = 
    match cmd with
    | SetLed p -> setLeds [p]
    | SetLeds ps -> setLeds ps
    | Display -> render ()
    | SetAndDisplayLeds ps -> 
        executeCmd (SetLeds ps)
        executeCmd Display
    | Sleep ms -> System.Threading.Thread.Sleep(ms)
    | Clear -> drawSnowman ()

let execute (cmds : Command list) = 
    cmds
    |> List.iter executeCmd