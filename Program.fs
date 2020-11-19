open System.Drawing

open LEDs

let sleep (ms : int) =
    System.Threading.Thread.Sleep(ms)

let display leds = 
    [ Mock.display ]
    |> List.iter (fun f -> f leds)

[<EntryPoint>]
let main argv =

    let redNose = 
        { Position = Nose 
          State = On Color.Red }
    let greenEyeL = 
        { Position = LeftEye
          State = On Color.LimeGreen }
    let greenEyeR = 
        { Position = RightEye
          State = On Color.LimeGreen }
    let topMiddle = 
        { Position = TopMiddle
          State = On Color.Blue }
    let midMiddle = 
        { Position = MiddleMiddle
          State = On Color.Blue }
    let bottomMiddle = 
        { Position = BottomMiddle
          State = On Color.Blue }

    display [ redNose; greenEyeL; greenEyeR ]
    sleep 1000
    display [ redNose; greenEyeL; greenEyeR; topMiddle ]
    sleep 1000
    display [ redNose; greenEyeL; greenEyeR; topMiddle; midMiddle; ]
    sleep 1000
    display [ redNose; greenEyeL; greenEyeR; topMiddle; midMiddle; bottomMiddle; ]
    sleep 1000

    let allPink = createAllOn Color.HotPink
    
    for _ in 1..5 do
        display allPink
        sleep 500
        display allOff
        sleep 500

    let red = 
        [ LeftEye; RightEye; Nose]
        |> createLedsOn Color.Red

    let amber = 
        [ TopLeft; TopMiddle; TopRight; MiddleMiddle ]
        |> createLedsOn Color.Orange

    let green = 
        [ MiddleLeft; BottomLeft; BottomMiddle; MiddleRight; BottomRight ]
        |> createLedsOn Color.Green

    let redAmber = 
        List.append red amber

    display green
    sleep 3000
    display amber 
    sleep 1000
    display red
    sleep 3000
    display redAmber
    sleep 1000
    display green
    sleep 1000
    display allOff

    //TODO: Implement and test "Real"
    //TODO: Change to "Commands"
    //TODO: Allow HTTP driven control
    //TODO: Cli switches
    //BUG: Console colors are changed after running all the above traffic lights.

    0