#r "paket:
nuget FSharp.Core 4.7.0
nuget Fake.Core.Target //
nuget Fake.DotNet.Cli //
nuget Fake.DotNet.Paket //"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet

[<Literal>]
let ProjectFile = "snowpi.fsproj"

// *** Define Targets ***
Target.create "Clean" (fun _ ->
  Trace.log " --- Cleaning stuff --- "
)

Target.create "Restore" (fun _ ->
  
  Trace.log " --- Paket Restore --- "

  Paket.restore id
)

Target.create "Build" (fun _ ->
  Trace.log " --- Building the app --- "

  DotNet.build id ProjectFile
)

Target.create "Deploy" (fun _ ->
  Trace.log " --- Deploying app --- "

  DotNet.publish (fun args -> 
    { args with
       OutputPath = Some "publish"
       Runtime = Some "linux-arm"
       SelfContained = Some true }) ProjectFile
)

open Fake.Core.TargetOperators

// *** Define Dependencies ***
"Clean"
  ==> "Restore"
  ==> "Build"
  ==> "Deploy"

// *** Start Build ***
Target.runOrDefault "Build"