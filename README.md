## MiControl

C# application/library for controlling MiLight WiFi enabled lightbulbs. It is currently in development (alpha).

|   | .NET 4.5.2 | Mono 3.8.0 |
|---|:-----------|:-----------|
| **master** | [![Build status](https://ci.appveyor.com/api/projects/status/vqi584om4kcj3032/branch/master?svg=true)](https://ci.appveyor.com/project/Milfje/micontrol/branch/master) | [![Build Status](https://travis-ci.org/Milfje/MiControl.svg?branch=master)](https://travis-ci.org/Milfje/MiControl) |

### What is it?

MiControl is an application/library for controlling <a href="http://www.milight.com/">MiLight</a> light bulbs connected to a MiLight WiFi controller, written in C#. It has a user interface (MiControlGUI) for directly controlling, and a library (<a href="http://github.com/Milfje/MiControl/wiki/MiControl">MiControl</a>) for writing your own applications to control MiLight light bulbs. Since MiControl succesfully builds on Mono, it is also usable on Linux hosts, including a Raspberry Pi.

The focus in development is currently on the MiControl library. The GUI is constantly undergoing changes to facilitate testing the library.

See the <a href="https://github.com/Milfje/MiControl/wiki">wiki</a> for more information about using MiControl.

### How to use it?

The aim of MiControl is to be as easy in usage as possible. For this reason, one part of the application will be a GUI to allow easy end-user access to controlling MiLight light bulbs. The MiControl library for controlling these lamps is kept as simple as possible in implementation as well.

Switching a lamp on can be done in just two lines of code. First you connect to the MiLight WiFi controller, and then you simply send the desired commands.

```csharp
var controller = new Controller("192.168.0.123"); // Connect to WiFi controller
controller.RGBW.SwitchOn(3); // Switch 'on' RGBW group 3
controller.RGBW.SetColor(3, Color.Red) // Set group 3 color to Red
```

Not sure on which IP your MiLight WiFi controller is located, or the IP is dynamic? Not a problem! Controllers can be discovered on the local network:

```csharp
var controllers = Controller.Discover(); // Find controllers
controllers[0].White.SwitchOn(1); // Switch 'on' white group 1 on first found controller
```

Alternatively you could broadcast the commands on the local subnet, in the case that only one controller is available on the network (or you want to send the same commands to all controllers)

```csharp
var controller = new Controller("255.255.255.255"); // Broadcast to all
controller.RGBW.SwitchOn(); // Switch 'on' all RGBW lights
```

For more information on using the GUI, or <a href="https://github.com/Milfje/MiControl/wiki/MiControl">examples using the MiControl library</a>, see the <a href="https://github.com/Milfje/MiControl/wiki">wiki</a>.

### What will it become?

The goal of this project is to create an application/library with advanced functions to make use of the full potential of these cheap, wirelessly controllable lights. Functions that will be implemented are for example:

* Simple switching and changing color.
* Setting the color of the light to match your screen (Ambilight functionality).
* Synchronising color changes to music (Audio visualisation).
* Scheduling switching/color changing (Visual alarm clock).
* Switching by rules (i.e. turn lights on when phone enters local WiFi).

By using the code of the MiControl library, you could create anything to control these lights! Join the Gitter chat below if you'd like to contribute, have any ideas or need help in using MiControl.

[![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/Milfje/MiControl?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)
