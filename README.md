
## win11 - branch ##



### version 2.4.1_w11--beta1 ### 



released on 2022-10-02



same features as v2.4.1 from the main branch, but adapted to work on windows 11 

& also has backwards compatibility with win10 (at least 20H1)




* basically, I opened a new project to use .NET 5 (aka: .net core 5) , so that [VirtualDesktop.dll](https://github.com/Grabacr07/VirtualDesktop) version 5.0.5 (latest) could be used
* this latest VirtualDesktop.dll , works on windows 11, but the lower limit is at least windows 10 build 19041 (20H1) or later
* also had to fix/change some areas in the code because of .net-core-5 compatibility issues , so that they work as in the original v2.4.1


* the code was made to be backwards compatible with win10 (at least 20H1, as stated)
* it was tested on WIN-10 20H2 build 19042,  & works the same as in the original v2.4.1


* needles to say the .net-core-5 needs to be installed (or later versions) on your system
* but also .net framework version 4.x needs to be installed so that the relatively old (but essential) package of [GlobalHotKey.dll](https://github.com/kyrylomyr/GlobalHotKey) works


**NOTE that this release was NOT tested on windows 11 , because I simply don't have one (and can't have due to incompatible hardware)**

**please test this release and open an issue to state any errors and when do they appear so I could fix them as much as possible**


for more description of the features, check the [main branch](https://github.com/iskmz/VirtualDesktopManager)