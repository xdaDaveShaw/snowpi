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

## Contributing

What can you contrib?

F#
XPlat
Programs

## Credits

Sources / Libs I used.

----

Notes:

Publishing

dotnet publish -o publish --self-contained -r linux-arm

Copying
scp -rp publish/ pi@raspberrypi:/home/pi/snowpi

on Pi

cd ~/snowpi/

./install

cd publish/
./snowpi


First Run, Seg Fault!
95350a3b0b7ac46c8271491fa86f0780f644932f

Then:
WS2811_ERROR_HW_NOT_SUPPORTED

Fixing by install and building the native lib on the pi...

cloned https://github.com/jgarff/rpi_ws281x
scons (from as per above)
copied rpi_ws281x.i and rpi_ws281x_wrap.c from https://github.com/klemmchr/rpi_ws281x.Net/tree/master/src/ws281x.Net/Native
gcc -c -fpic ws2811.c rpi_ws281x_wrap.c (as per .NET)
gcc -shared ws2811.o rpi_ws281x_wrap.o -o librpi_ws281x.so
sudo ldconfig

Running...
./snowpi: symbol lookup error: /usr/local/lib/librpi_ws281x.so: undefined symbol: rpi_hw_detect

Better...
https://github.com/kenssamson/rpi-ws281x-csharp/tree/master/src/rpi_ws281x
Using this lib and the latest build of native libs from https://github.com/jgarff/rpi_ws281x
But these instructions
$ sudo apt-get install build-essential git scons
$ git clone https://github.com/jgarff/rpi_ws281x.git
$ cd rpi_ws281x
$ scons
$ gcc -shared -o ws2811.so *.o
$ sudo cp ws2811.so /usr/lib

Also copied to /usr/local/lib/ NOT sure if this helped though ;)