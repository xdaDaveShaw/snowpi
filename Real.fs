module Real

open LEDs
open rpi_ws281x

let settings = Settings.CreateDefaultSettings();
let controller = 
    settings.AddController(
        ControllerType.PWM0, 
        NumberOfLeds, 
        StripType.Unknown, 
        100uy, 
        false)

let rpi = new WS281x(settings)

let setup() = 
    controller.Reset();

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
    | SetLed p -> setLeds [p]
    | SetLeds ps -> setLeds ps
    | Display -> render()
    | SetAndDisplayLeds ps -> 
        executeCmd (SetLeds ps)
        executeCmd Display
    | Sleep ms -> System.Threading.Thread.Sleep(ms)

let execute (cmds : Command list) =
    
    cmds
    |> List.iter executeCmd

   

