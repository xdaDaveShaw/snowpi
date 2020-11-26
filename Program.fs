open System.Drawing

open LEDs

type Config = {
    UseSnowpi : bool
    UseMock   : bool
}

let setup config = 
    if config.UseSnowpi then
        Real.setup ()
    if config.UseMock then
        Mock.setup ()

let execute config cmds name = 
    [ 
        if config.UseSnowpi then
            Real.execute
        if config.UseMock then
            Mock.execute
    ]
    |> List.iter (fun f -> 
        printfn "Executing: %s" name
        f cmds)

let createConfigFromArgs args = 
    { UseSnowpi = args |> Array.contains("-r")
      UseMock = args |> Array.contains("-m") }

[<EntryPoint>]
let main argv =

    let config = createConfigFromArgs argv
    
    //Warmup Code
    setup config

    //partially apply execute with the config
    let execute = execute config


    //Programs

    let redNose = 
        { Position = Nose 
          Color = Color.Red }
    let greenEyeL = 
        { Position = LeftEye
          Color = Color.LimeGreen }
    let greenEyeR = 
        { Position = RightEye
          Color = Color.LimeGreen }
    let topMiddle = 
        { Position = TopMiddle
          Color = Color.Blue }
    let midMiddle = 
        { Position = MiddleMiddle
          Color = Color.Blue }
    let bottomMiddle = 
        { Position = BottomMiddle
          Color = Color.Blue }

    let program1 = [
        SetLeds [ redNose; greenEyeL; greenEyeR ]
        Display
        Sleep 1000
        SetLeds [ redNose; greenEyeL; greenEyeR; topMiddle ]
        Display
        Sleep 1000
        SetLeds [ redNose; greenEyeL; greenEyeR; topMiddle; midMiddle; ]
        Display
        Sleep 1000
        SetLeds [ redNose; greenEyeL; greenEyeR; topMiddle; midMiddle; bottomMiddle; ]
        Display
        Sleep 1000
    ]

    let red = 
        [ LeftEye; RightEye; Nose]
        |> createPixels Color.Red

    let amber = 
        [ TopLeft; TopMiddle; TopRight; MiddleMiddle ]
        |> createPixels Color.Yellow

    let green = 
        [ MiddleLeft; BottomLeft; BottomMiddle; MiddleRight; BottomRight ]
        |> createPixels Color.LimeGreen

    let redAmber = 
        List.append red amber

    let trafficLights = [
        Clear
        SetAndDisplayLeds green
        Sleep 3000
        Clear
        SetAndDisplayLeds amber 
        Sleep 1000
        Clear
        SetAndDisplayLeds red
        Sleep 3000
        Clear
        SetAndDisplayLeds redAmber
        Sleep 1000
        Clear
        SetAndDisplayLeds green
        Sleep 1000
    ]
    
    let colorWipe col = 
        Position.All
        |> List.sortBy posToLedNumber
        |> List.collect (fun pos -> 
            [ SetAndDisplayLeds (createPixels col [pos])
              Sleep 50 ])
              
    let colorWipeProgram = [
        for _ in [1..5] do
            for col in [ Color.Red; Color.Green; Color.Blue; ] do
                yield! colorWipe col
    ]

    let theater times col = [
        for j in [1..times] do
            for q in [0..2] do
                for i in [0..3..NumberOfLeds-1] do
                    SetLed { Position = ledNumberToPos (i + q); Color = col j i }
                Display
                Sleep 50
                for i in [0..3..NumberOfLeds-1] do
                    SetLed { Position = ledNumberToPos (i + q); Color = Color.Black }
    ]
    
    let theaterChaseProgram =
        [ Color.White
          Color.Red
          Color.Blue
        ]
        |> List.collect (fun col -> theater 256 (fun _ _ -> col))

    let wheel pos = 
        if pos < 85 then
            Color.FromArgb(pos * 3, 255 - pos * 3, 0)
        elif pos < 170 then
            let pos = pos - 85
            Color.FromArgb(255 - pos * 3, 0, pos * 3)
        else
            let pos = pos - 170
            Color.FromArgb(0, pos * 3, 255 - pos * 3)
        
    let theaterRainbowProgram =
        theater 255 (fun j i -> wheel ((i + j) % 255))

    let rainbowProgram = [
        for j in [0..255 * 5] do
            for i in [0..NumberOfLeds-1] do
                let colorNumber = (i + j) &&& 255
                SetLed { Position = ledNumberToPos i; Color = (wheel colorNumber) }
            Display
            Sleep 20
    ]

    let rainbowCycleProgram = [
        for j in [0..255 * 5] do
            for i in [0..NumberOfLeds-1] do
                let colorNumber = ((i * 256 / NumberOfLeds) + j) &&& 255
                SetLed { Position = ledNumberToPos i; Color = (wheel colorNumber) }
            Display
            Sleep 10
    ]

    // Execute programs:
    execute program1 "program1"
    execute trafficLights "trafficLights"
    execute colorWipeProgram "colorWipeProgram"
    execute theaterChaseProgram "theaterChaseProgram"
    execute theaterRainbowProgram "theaterRainbowProgram"
    execute rainbowProgram "rainbowProgram"
    execute rainbowCycleProgram "rainbowCycleProgram"

    execute [ Clear ] "Clearing all LEDs"

    //TODO: Cli switches for programs?
    //TODO: Other Conosle lib from Scott H?
    //TODO: .NET 5 / Single Exe?
    //TODO: Investigate poot color range
    //TODO: Allow HTTP driven control?

    0