# SnowPi RGB

Programming the [SnowPi RGB](https://snowpi.xyz/) in F# and .NET 5.

![Snowpi](https://cdn.shopify.com/s/files/1/0446/2143/0938/products/snowmanOnPi_1000x1500.jpg?v=1596114774)

This is my contribution to the F# Advent Calendar 2020.

< BLOG POST LINK HERE >

## About

Using this application you can write and test programs to run on the SnowPi RGB.

Testing is done by launching the application in Console mode where it will draw `X` to the console.

## OS Note

I developed this on Windows and run it on Raspberry Pi - I did not develop on the RPi.

I've used `fake` as the main build infrastructure, and tried to keep it generally OS agnostic
but I may have failed somewhere (probably the deployment stuff).

The `cmd` scripts I recommend below are just helpers to save me remembering all the args
and typing them, you should be able to run the commands from inside them on *Nix.

If you find a problem, please submit a PR.

## Building

To compile the application just run `build.cmd`.

This should restore the .NET Tools:

- Fake
- Paket

Then run Fake to actually compile the app.

### Console Testing

To test in console mode you can use `run.cmd`.

This launches a new `cmd` prompt on Windows because that has the default colours,
whereas my Windows Terminal is customised.

## Deploying / Installing

To get this working on a RPi you need a native library.

This was the hardest bit of this whole project.

I recommend the "Building the Native Library" approach from
[here](https://github.com/kenssamson/rpi-ws281x-csharp/tree/master/lib#building-the-native-library) (copied below); it is the only one
that worked on my RPi and isn't too much work.

```sh
sudo apt install build-essential git scons
git clone https://github.com/jgarff/rpi_ws281x.git
cd rpi_ws281x
scons
gcc -shared -o ws2811.so *.o
sudo cp ws2811.so /usr/lib
```

### Deploying to RPi

To deploy the build application to your Raspberry Pi you can either try and use the
automatic deployment in the Fake script, or you can just SCP it over yourself.

I've tested this on my Raspberry Pi 4.

#### Manual SCP

If you don't want to use the Fake script to deploy you can get it to publish the file...

```cmd
dotnet fake run build.fsx --target Publish
```

then copy it from the `publish` folder using SCP.

Once you SCP the file over to the RPi you need to `chmod +x` the file to make it
executable.

#### Automatic Fake

If you want to try the Fake script, just run `deploy.cmd`

The Fake script is set to my personal setup:

- For my RPi I have a separate private key called `pi_rsa`.
- My login is `pi`
- My RPi's hostname is `raspberrypi`

You can tweak all these in the build.fsx.

### Running on RPi

If you used the Fake deployment the file will be in `~/snowpi/publish/`.

You need to use `sudo` to run the application because of the native libs:

```sh
# Simple program
sudo ./snowpi -r -ps

# All programs
sudo ./snowpi -r -a
```

### List of programs

There are a few light programs that exist.

You can run either all programs or one or many individual ones:

```sh
 -a  : Run all programs
 -ps : Run the simple program
 -pl : Run the traffic lights program
 -pw : Run the colour wipe program
 -pt : Run the theater programs
 -pr : Run the rainbow programs
```

## Contributing

If you would like to contribute to this project I'm more than
happy to accept PR's to improve anything, e.g.:

- F# Coding improvements
- Cross Platform support for the build / dev.
- New "Programs" to show off
- Other RPi Models.

## Credits

- [Ryan Walmsley](https://github.com/ryanteck)
  - For creating the [SnowPi RGB](https://snowpi.xyz/)
  - For the [Python Demo App](https://github.com/ryanteck/snowpirgb-python)
- [Ken Samson](https://github.com/kenssamson)
  - For the [RPi Library](https://github.com/kenssamson/rpi-ws281x-csharp)
