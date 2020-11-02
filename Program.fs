// Learn more about F# at http://fsharp.org

open System
open System.Drawing

[<EntryPoint>]
let main argv =
    Colorful.Console.WriteLine("Starting SnowPi!", Color.Blue)

    let ledCount = 12
    let pin = 18
    let freq = 800000u
    let dma = 10
    let brightness = 100uy
    let invert = false
    let channel = 0

    use strip = new ws281x.Net.Neopixel(ledCount, pin, frequency = freq, dma = dma, brightness = brightness, invert = invert, channel = channel)
    (*
    for i in range(strip.numPixels()):
        strip.setPixelColor(i, color)
        strip.show()
        time.sleep(wait_ms / 1000.0) *)

    let wipe col = 
        for i in 0..strip.GetNumberOfPixels() do
            strip.SetPixelColor(i, col)
            strip.Show()
            Threading.Thread.Sleep(50)

    let rec mainLoop() = 

        wipe Color.Red
        wipe Color.Blue
        wipe Color.Green

        if Console.KeyAvailable then
            match Console.ReadKey().Key with
            | ConsoleKey.Q -> ()
            | _ -> mainLoop()
        else
            mainLoop()

    Colorful.Console.WriteLine("Running, press Q to quit!", Color.Blue)

    mainLoop()

    Colorful.Console.WriteLine("Ending SnowPi!", Color.Blue)
    0 // return an integer exit code
