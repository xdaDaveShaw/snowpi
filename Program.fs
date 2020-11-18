open System.Drawing

open Mock

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

    run [ redNose; greenEyeL; greenEyeR ] false
    System.Threading.Thread.Sleep(1000)
    run [ redNose; greenEyeL; greenEyeR; topMiddle ] true
    System.Threading.Thread.Sleep(1000)
    run [ redNose; greenEyeL; greenEyeR; topMiddle; midMiddle; ] true
    System.Threading.Thread.Sleep(1000)
    run [ redNose; greenEyeL; greenEyeR; topMiddle; midMiddle; bottomMiddle; ] true
    System.Threading.Thread.Sleep(1000)

    0