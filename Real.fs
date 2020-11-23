module Real

open LEDs
open rpi_ws281x

let settings = Settings.CreateDefaultSettings();
let controller = 
    settings.AddController(
        ControllerType.PWM0, 
        12, 
        StripType.Unknown, 
        100uy, 
        false)

let rpi = new WS281x(settings)

let setup() = 
    controller.Reset();

let private posToLedNumber = function
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

let private setLeds pixels = 
    let toLedTuple pixel =
        (pixel.Position |> posToLedNumber, pixel.Color)

    pixels
    |> List.map toLedTuple
    |> List.iter controller.SetLED

let private render() = 
    rpi.Render();

let rec private executeCmd cmd = 
    match cmd with
    | SetLeds ps -> setLeds ps
    | Display -> render()
    | SetAndDisplayLeds ps -> 
        executeCmd (SetLeds ps)
        executeCmd Display
    | Sleep ms -> System.Threading.Thread.Sleep(ms)

let execute (cmds : Command list) =
    
    cmds
    |> List.iter executeCmd

   

