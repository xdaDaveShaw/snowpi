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

    0