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

let display (pixels : Pixel list) =
    
    let toLedTuple pixel =
        match pixel.State with
        | On color -> Some (pixel.Position |> posToLedNumber, color)
        | Off -> None

    pixels
    |> List.choose toLedTuple
    |> List.iter controller.SetLED

    rpi.Render();
