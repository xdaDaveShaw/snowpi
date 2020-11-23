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

    // for _ in [1..5] do
    //     for col in [Color.Red; Color.Green; Color.Blue] do
    //         Position.All
    //         |> List.sortBy posToLedNumber
    //         |> List.map (fun pos -> createPixelOn col pos)
    //         |> List.iter (fun pix -> 
    //             display [pix]
    //             sleep 50)

    

    // display [ redNose; greenEyeL; greenEyeR ]
    // sleep 1000
    // display [ redNose; greenEyeL; greenEyeR; topMiddle ]
    // sleep 1000
    // display [ redNose; greenEyeL; greenEyeR; topMiddle; midMiddle; ]
    // sleep 1000
    // display [ redNose; greenEyeL; greenEyeR; topMiddle; midMiddle; bottomMiddle; ]
    // sleep 1000

    // let allPink = createAllOn Color.HotPink
    
    // for _ in 1..5 do
    //     display allPink
    //     sleep 500
    //     display allOff
    //     sleep 500

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

    //TODO: Test "Real" with examples
    //TODO: Allow HTTP driven control
    //TODO: Cli switches
    //TODO: .NET 5 / Single Exe?

    0