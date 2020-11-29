open System.Drawing

open LEDs

type Config = {
    UseSnowpi : bool
    UseMock   : bool
    ExecSimple : bool
    ExecTraffic : bool
    ExecWipe : bool
    ExecTheater : bool
    ExecRainbow : bool
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
        Colorful.Console.WriteLine((sprintf "Executing: %s" name), Color.White)
        f cmds)

let teardown config =
    if config.UseSnowpi then
        Real.teardown ()
    if config.UseMock then
        Mock.teardown ()

let createConfigFromArgs args = 
    let has switch = Array.contains switch args
    let hasProg switch = has "-a" || has switch

    { UseSnowpi = has "-r"
      UseMock = has "-m"
      ExecSimple = hasProg "-ps"
      ExecTraffic = hasProg "-pl"
      ExecWipe = hasProg "-pw"
      ExecTheater = hasProg "-pt" 
      ExecRainbow = hasProg "-pr" }

let validateConfig config = 
    if not config.UseMock && not config.UseSnowpi then
        printfn ""
        printfn "Invalid mode, you must pass either -m or -r."
        printfn ""
        printfn " -m : Use Console output"
        printfn " -r : Use Snowpi output"
        printfn ""
        false
    elif not config.ExecSimple &&
         not config.ExecTraffic &&
         not config.ExecWipe &&
         not config.ExecTheater &&
         not config.ExecRainbow then
            printfn ""
            printfn "No program specified."
            printfn ""
            printfn " -a  : Run all programs"
            printfn " -ps : Run the simple program"
            printfn " -pl : Run the traffic lights program"
            printfn " -pw : Run the colour wipe program"
            printfn " -pt : Run the theater programs"
            printfn " -pr : Run the rainbow programs"
            false
    else
        true

[<EntryPoint>]
let main argv =

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

    let simpleProgram = [
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
                    SetLed { Position = ledNumberToPos (i + q); Color = Color.Empty }
    ]
    
    let theaterChaseProgram =
        [ Color.White
          Color.Red
          Color.Blue
        ]
        |> List.collect (fun col -> theater 10 (fun _ _ -> col))

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


    // Get the config from the CLI
    let config = createConfigFromArgs argv

    // Validate that a progam and mode have been selected
    let validConfig = validateConfig config
    if not validConfig then
        exit -1
    
    // Setup the Snowpi / Console
    setup config

    try
        // Partially apply execute with the config
        let execute = execute config

        // Run the programs, then clear the LEDs
        try
            if config.ExecSimple then
                execute simpleProgram "simpleProgram"
            if config.ExecTraffic then
                execute trafficLights "trafficLights"
            if config.ExecWipe then
                execute colorWipeProgram "colorWipeProgram"
            if config.ExecTheater then
                execute theaterChaseProgram "theaterChaseProgram"
                execute theaterRainbowProgram "theaterRainbowProgram"
            if config.ExecRainbow then
                execute rainbowProgram "rainbowProgram"
                execute rainbowCycleProgram "rainbowCycleProgram" 
        finally 
            execute [ Clear ] "Clearing all LEDs"

    finally
        teardown config

    0