Ringtone2iPhone (alpha)
=======================

This app is created for my eleven-year-old daughter who loves his iPhone and often nags me to create and/or upload ringtones to her phone.

![Main screen](screenshots/main.png)

The audio files are stored in the `Audio` folder.

![Audio cut screen](screenshots/cut.png)

The program modifies the `Ringtones.plist` file on the phone.
The added ringtones are available instantly (no reboot needed) but the modifications are not shown in **iTunes**.
*Ignore this if you don't use iTunes.*

Download
--------

Check out the [releases](https://github.com/ZalaPanda/Ringtone2iPhone/releases) section.

HOWTO
-----

> The phone is not in showing up.

Please check these points:
* The phone is connected with tha cable to the computer.
* The computer is trusted by the phone.
  * Unlock the device.
  * If asked, trust the computer.
  * Done.

> "Windows protected your PC" blue window.

This is because I don't own a [code signing certificate](https://comodosslstore.com/code-signing) (~100 USD/year) you have 3 options:
* Ignore the warning:  
    Click on **More info** link and pushing the **Run anyway** button.
* Build the application on your computer:  
    Download or clone the repo and **build** the solution with Visual Studio on your computer.
* Make a suggestion:  
    If you have a good idea to *fix* this problem, contact me.

TODO
----
Things I plan to fix:
* Select added items in lists.
* Get rid of [libimobiledevice](https://github.com/libimobiledevice/libimobiledevice) wrapper to reduce size.
* Some kind of logging?
* File explorer?
