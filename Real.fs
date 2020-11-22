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

let display (pixels : Pixel list) =
    
    let toLedTuple pixel =
        match pixel.State with
        | On color -> Some (pixel.Position |> posToLedNumber, color)
        | Off -> None

    pixels
    |> List.choose toLedTuple
    |> List.iter controller.SetLED

    rpi.Render();
