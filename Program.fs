open System.Drawing

open LEDs

let useReal = false
let useMock = true

let setup () = 
    if useReal then
        Real.setup ()
    if useMock then
        Mock.setup ()

let execute cmds = 
    [ 
        if useReal then
            Real.execute
        if useMock then
            Mock.execute
    ]
    |> List.iter (fun f -> f cmds)

let color r g b = 
    Color.FromArgb(r, g, b)

[<EntryPoint>]
let main argv =

    //Warmup Code
    setup ()

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
        |> createPixels Color.Orange

    let green = 
        [ MiddleLeft; BottomLeft; BottomMiddle; MiddleRight; BottomRight ]
        |> createPixels Color.Green

    let redAmber = 
        List.append red amber

    let trafficLights = [
        SetAndDisplayLeds green
        Sleep 3000
        SetAndDisplayLeds amber 
        Sleep 1000
        SetAndDisplayLeds red
        Sleep 3000
        SetAndDisplayLeds redAmber
        Sleep 1000
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


    //TODO: Test "Real" with examples
    //TODO: Clear LED's option
    //TODO: List comprehension examples
    //TODO: Allow HTTP driven control
    //TODO: Cli switches
    //TODO: .NET 5 / Single Exe?

    0