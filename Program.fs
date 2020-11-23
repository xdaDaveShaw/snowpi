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

let execute config cmds = 
    [ 
        if config.UseSnowpi then
            Real.execute
        if config.UseMock then
            Mock.execute
    ]
    |> List.iter (fun f -> f cmds)

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

    execute program1

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
    
    execute trafficLights

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

    execute colorWipeProgram

    let theater times col = [
        for _ in [1..times] do
            for q in [0..2] do
                for i in [0..3..NumberOfLeds-1] do
                    SetLed { Position = ledNumberToPos (i + q); Color = col }
                Display
                Sleep 50
                for i in [0..3..NumberOfLeds-1] do
                    SetLed { Position = ledNumberToPos (i + q); Color = Color.Black }
    ]
    
    let theaterProgram =
        [ Color.White
          Color.Red
          Color.Blue
        ]
        |> List.collect (fun col -> theater 10 col)

    execute theaterProgram

    execute [ Clear ]

    //TODO: More "Real" with examples
    //TODO: Allow HTTP driven control?
    //TODO: Cli switches for programs?
    //TODO: .NET 5 / Single Exe?

    0