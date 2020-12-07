# SnowPi

More details to come...

Part of the F# Advent Calendar 2020.

< BLOG POST LINK HERE >

## Building

How do I build / work on this

## Deploying / Installing

Where do I get the magic native lib from?
Where do I put the magic lib?

How do I deploy to an RPi?

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