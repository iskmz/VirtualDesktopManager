

= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 


for further information check:	https://github.com/MScholtes/VirtualDesktop


this folder contains .exe files fro MScholtes Virtual Desktop [compiled using the batch provided in its repository]


= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 

MScholtes Virtual Desktop is a C# command line tool to manage virtual desktops in Windows 10 and Windows 11

it is used in this program ONLY to OVERRIDE the default behavior of WINKEY+CTRL+RIGHT/LEFT  (and also of touchpad-4-fingers+RIGHT/LEFT)
so that there is desktop wrapping when reaching the edge from either side

	this behavior (desktop wrapping, or cycling) is the DEFAULT behavior in VDM also when clicking on desktop# icon 
	and when using the program's hotkey CTRL+ALT+LEFT/RIGHT
	and when clicking on NEXT/PREVIOUS menu items 
	and when using auto-cycling desktops
	but is implemented internally in all these cases (in VDM program itself using VirtualDesktop.dll library) 


HOWEVER, in case of the default WINKEY+CTRL+RIGHT/LEFT  (or touchpad gestures) , MScholtes Virtual Desktop executable was used , 
using an AutoHotKey Script launched at program start to re-map the default windows keys to "msvd.exe" commands

= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 


provided are five versions of msvd.exe:-

1.	vd.exe 		is for Windows 10 1809 to 21H2
2.	vd11.exe		is for Windows 11
3.	vd11Insider.exe	is for Windows 11 Insider
4.	vd1607.exe	is for Windows 10 1607 to 1709 and Windows Server 2016
5.	vd1803.exe	is for Windows 10 1803


the vd.exe version [FIRST] is used by default	[named msvd.exe]

** in order to CHANGE the version to WORK ON YOUR OS, 
simply REPLACE msvd.exe in the default VDM folder [root folder of extraction; where VirtualDesktopManager.exe is found]
with one of the above (and change its name to "msvd.exe") so that the AutoHotKey script recognizes it (as "msvd.exe")

= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 


