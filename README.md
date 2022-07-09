_forked from [m0ngr31/VirtualDesktopManager](https://github.com/m0ngr31/VirtualDesktopManager "")_


## Updates ##


* version 2.2:	&emsp; <u>_released on 2022-07-09_</u>
  * new menu-items in desktops-list: 
    - "Close All" to close all desktops at once
    - "Add Multiple" to add as many desktops as user enters [range: 1 to 10]			
  * some minor UI improvements in desktops-list: new icons, tooltips ...
  * minor code clean-up / order  



* version 2.1:	[↓↓](#version-21)

* version 1.9-modified:	 [↓↓↓↓](#a-modified-version-19)

* original repo. Readme:	[↓↓↓↓↓↓](#-original-readme-)



------------------------------------------------------------


## version 2.1 ##
<u>_released on 2022-03-03_</u>


[__↓↓ README of previous release (on 2022-02-19) ↓↓__](#a-modified-version-19)



___changes / additions made in version 2.1 :-___

1. added CYCLE/REVERSE-CYCLE functions to cycle desktops automatically; including sub-menus for transition-time and number of cycles, with a special option to cycle-forever (stopped on CTRL+ALT+S).
2. code-shortenings, clean-up and more concise code
3. changed desktops-list: including ADD/CLOSE buttons to add and remove desktops, desktop-names feature.
4. desktop-names: can change on right-clicking each desktop-item. Program reads registry keys for loading names (compatible with early win10 releases), but changing names needs win10 version 2004 or newer.
	+ credits to:  [MScholtes/VirtualDesktop](https://github.com/MScholtes/VirtualDesktop "") , as most of the code for reading/setting desktop names is taken from there.
5. commented out (removed) BalloonTip msg about "Error Setting Hotkeys" , since hotkeys are set, even though an error is thrown, so no need for the msg.
6. added user preferences option, with load/save to an xml file of: chosen color (and brush), transition time and cycles amount. Preferences are saved automatically on exit , but also there is a menu-item to save at anytime.
7. added consts class, for grouping all constants together, including value-ranges for VALIDITY-CHECK on startup after loading user preferences, so that tampering with xml file won't crash the program; in case of any illegal value, then DEFAULTS are loaded from CONST class.
8. all currently chosen user preferences are highlighted now , when opening each sub-menu
9. more user interaction: Message boxes before closing a desktop, before cycling-forever, after saving preferences, and an input-box for changing desktop-name (imported from visual basic).
	+ credits for InputBox (.dll to import from VB) to:  [codeproject](https://www.codeproject.com/articles/32573/exposing-vb-inputbox-dialog-to-c-code "")
10. when hovering over tray icon now, can see desktop name, if exists.
11. a menu item "desktops GUIDs" to show a list of all desktops, their names and GUIDs, with an option to copy data to clipboard.
 
 
<img src="https://user-images.githubusercontent.com/48130426/156536171-2fa37465-09cf-4cd7-9ffe-a33b99ee5bc7.png" width=45% height=45%> <img src="https://user-images.githubusercontent.com/48130426/156536178-7c366275-22b7-44e3-ac9e-da2db925e810.png" width=45% height=45%> <img src="https://user-images.githubusercontent.com/48130426/156536180-ec773f0f-13e3-4afa-84f6-8625a72066e6.png" width=45% height=45%> <img src="https://user-images.githubusercontent.com/48130426/156536183-cd512d37-0361-4c66-8809-e7ac373e32c0.png" width=45% height=45%> <img src="https://user-images.githubusercontent.com/48130426/156536185-1a0f0ec8-189c-4af8-b273-2b7f71b4dec0.png" width=45% height=45%>



------------------------------------------------------------

### a modified version 1.9 ###
<u>_released on 2022-02-19_</u>


[__↓ original repo. README (as of 2022-02-19) ↓__](#-original-readme-)


___changes / additions made in this modification:-___

1. changes to tray icon visibility / appearance: background colors, font size (bigger), and more visible numbers; Because of these changes, tray-icon was restricted back to 1-9 desktops only, and after the 9th desktop a "+" sign will appear instead (no crashing)
2. clean-up of Resources folder of unused icons
3. added a function on LEFT-mouse click on tray icon >> move to next desktop (by numerical order), and if holding SHIFT along with click >> move to previous desktop
4. added two context-menu items: NEXT , PREVIOUS that do the above functionalities in (3.)
5. added desktops'-list sub-menu: to go to each desktop by a single click on its number
6. added (Desktop Number) shown when mouse-hovering over tray icon (useful when more than 9 desktops)
7. added colors' palette (~ 14 options) to choose from to change tray-icon's background color, or make it transparent.


<img src="https://user-images.githubusercontent.com/48130426/154814650-32d65f4c-b4b0-45a1-8d98-b31df779d4fb.png" width=45% height=45%> <img src="https://user-images.githubusercontent.com/48130426/154814667-1013a978-b1e7-47da-97c4-b349f1145f48.png" width=45% height=45%> <img src="https://user-images.githubusercontent.com/48130426/154814673-8701f934-b859-4e3f-ac6d-445acac9a47b.png" width=45% height=45%>

------------------------------------------------------------

### ↓ original README ↓ ###

VirtualDesktopManager
======
About
------------------------
This program was made for people who are using Windows 10's built-in Virtual Desktops, but who don't like the default key-binding, don't like how you can't cycle through your desktops (If you are on your last one you don't want to hotkey over to the first one 8 times), and don't like not knowing what desktop # they are on.

Install
------------------------
There is no installation. Just download the .zip from the Releases, extract it and then run VirtualDesktopManager.exe.

You can use Task Scheduler to make it launch when you login so you don't have to launch it manually every reboot.

Usage
------------------------

You can continue to use the default hotkey to change desktops (Ctrl+Win+Left/Right), but you won't get any of the benefit of the program except knowing which desktop you are on. 

I have added a listener to the hotkey of Ctrl+Alt+Left/Right. With this hotkey, you can cycle through your virtual desktops. If this hotkey doesn't work on your system (Intel utility already uses it), you can open up the settings and select the alternate hotkey (Shift+Alt+Left/Right).

As of v1.5.0, you are no longer limited to 9 desktops. The icon will automatically update up to 999 desktops (can you handle that many?).


Limitations
------------------------
 * <s>Due to not wanting to make lots of tray icons, this program only supports up to 9 virtual desktops (it will crash if you go above that).</s>
 * If you try switch between desktops too quickly, windows on different desktops will try to gain focus (you'll see what I mean when you try it out).
 * It needs more testing to see how well it will handle suspend/hibernation events.
 * You will need to relaunch the program if explorer.exe crashes.
 * <s>Hotkeys are statically coded in, so if you want to configure them, you'll have to modify the source.</s>
 * <s>It doesn't handle it very well when you add or create virtual desktops while it's running. You'll need to relaunch it.</s>

I'm trying to work on these issues, but if you have a solution, just throw in a PR and I'll take a look.
