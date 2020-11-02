// Learn more about F# at http://fsharp.org

open System

[<EntryPoint>]
let main argv =
    Colorful.Console.WriteLine("Hello World from F#!", Drawing.Color.Pink)
    0 // return an integer exit code
