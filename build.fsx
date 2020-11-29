#r "paket:
nuget FSharp.Core 4.7.0
nuget Fake.Core.Target //
nuget Fake.DotNet.Cli //
nuget Fake.DotNet.Paket //"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet

let SshKeyFile = 
  System.IO.Path.Combine(
    System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), 
    ".ssh", 
    "pi_rsa")

[<Literal>]
let PiLogin = "pi@raspberrypi"
[<Literal>]
let SnowPiOnPi = "/home/pi/snowpi"

[<Literal>]
let ProjectFile = "snowpi.fsproj"

// *** Define Targets ***
Target.create "Info" (fun _ ->
  Trace.log " --- Info --- "

  let version = DotNet.getVersion id

  Trace.log (sprintf "dotnet version in use: %s" version)
)

Target.create "Restore" (fun _ ->
  
  Trace.log " --- Paket Restore --- "

  Paket.restore id
)

Target.create "Build" (fun _ ->
  Trace.log " --- Building the app --- "

  DotNet.build id ProjectFile
)

Target.create "CleanPublish" (fun _ ->
  Trace.log " --- Cleaning Publish --- "

  Fake.IO.Directory.delete "publish"

)

Target.create "Publish" (fun _ ->
  Trace.log " --- Publishing app --- "

  let opt = 
    DotNet.Options.Create()
    |> DotNet.Options.withCustomParams (Some "-p:PublishSingleFile=true -p:PublishTrimmed=true")

  DotNet.publish (fun args -> 
    { args with
       OutputPath = Some "publish"
       Runtime = Some "linux-arm"
       SelfContained = Some true
       Common = opt }) ProjectFile
)

Target.create "Deploy" (fun _ ->
  Trace.log " --- Deploying app --- "

  CreateProcess.fromRawCommand 
    "scp.exe" 
    [ "-rp"
      "-i"
      SshKeyFile
      "publish"
      PiLogin + ":" + SnowPiOnPi ]
  |> Proc.run
  |> ignore
  
  Trace.log " --- Making app executable --- "

  CreateProcess.fromRawCommand
    "ssh.exe"
    [ "-i"
      SshKeyFile
      PiLogin
      "chmod +x " + SnowPiOnPi + "/publish/snowpi" ]
  |> Proc.run
  |> ignore
)

open Fake.Core.TargetOperators

// *** Define Dependencies ***
"Info"
  ==> "Restore"
  ==> "Build"
  ==> "CleanPublish"
  ==> "Publish"
  ==> "Deploy"

// *** Start Build ***
Target.runOrDefault "Build"