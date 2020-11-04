open System
open System.Drawing

open rpi_ws281x

[<EntryPoint>]
let main argv =

    let settings = Settings.CreateDefaultSettings();

    let controller = settings.AddController(ControllerType.PWM0, 12, StripType.Unknown, 100uy, false)

    use rpi = new WS281x(settings)
    
    Console.WriteLine("Running...");

    controller.Reset();
    controller.SetAll(Color.Red);
    rpi.Render();

    Console.WriteLine("Render 1...");
    Console.ReadLine() |> ignore;
    
    rpi.Reset();

    Console.WriteLine("Reset 1...");
    Console.ReadLine() |> ignore;

    rpi.Render();
    Console.WriteLine("Render 2...");
    Console.ReadLine() |> ignore;

    0