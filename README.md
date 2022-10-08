_forked from [m0ngr31/VirtualDesktopManager](https://github.com/m0ngr31/VirtualDesktopManager "")_



_for a win11-compatible version check [win11-branch](https://github.com/iskmz/VirtualDesktopManager/tree/win11)_


------------------------



***Tip regarding windows-focus issue***  

_2022-10-08_

```
After many failed attempts recently to fix this issue, as described 
in commented-out sections of recent commits (unreleased: v2.4.2.11); 
what I found is that if you click on an empty area on the taskbar, 
before doing desktop transitions (no matter in which way: keyboard, 
mouse or otherwise), then focus issue is resolved and no window tries 
to gain focus. 
Tried to simulate this mouse-click on taskbar in one of the 
recent 'fixes' but it did not work out. 
```


------------------------



## Updates ##




### version 2.4.2 ### 
<details>
<summary> released on 2022-10-03 ... </summary>

* a partial fix for longterm-problem of windows gaining focus while switching desktops 
  - first mentioned in 2nd point of Limitations in [original-readme](https://github.com/iskmz/VirtualDesktopManager#original-readme)
  - a simple fix really: by commenting-out the use of saveApplicationFocus() and restoreApplicationFocus() functions
  - don't know if this "fix" removes any needed functionality; it does not appear to be so, as everything works fine
  - it is only a "partial" fix because, this problem happens much less often; but still happens sometimes!
* Added "mouse-hook" using [globalmousekeyhook](https://github.com/gmamaladze/globalmousekeyhook) package to detect mouse-wheel movement over main-taskbar area (found on main screen by default)
  - code to get location coordinates of main-taskbar is from [stackoverflow](https://stackoverflow.com/questions/29330440/get-precise-location-and-size-of-taskbar)
  - mouse-wheel-down moves to next desktop
  - mouse-wheel-up moves to previous desktop
  - works only over main-taskbar area !
  - if taskbar location is changed while program is running, then VDM needs a restart to reload the new coordinates
  - Hotkeys List was updated accordingly

</details>


### version 2.4.1 ### 
<details>
<summary> released on 2022-09-28 ... </summary>

* Added hotkey for (P)anic ! : Ctrl+Alt+Shift+P , for fast saving of desktop data & screenshots 
* Added hotkey to show/hide desktops (L)ist : Ctrl+Alt+Shift+L , for fast select of desktop #, Add & Close items from menu using up/down arrows & Enter
  - Note that sometimes it loses focus and arrows don't work suddenly; a simple fix is "Alt+Tab" to it so it works again  
* Added hotkey Ctrl+Alt+Shift+H :  to show (H)otkeys list

</details>

### version 2.4 ###  
<details>
<summary> released on 2022-09-27 ... </summary>
	
* a workaround to override default windows combination: Ctrl+Winkey+Right/Left, and also touchpad 4-finger-swipe Right/Left using [AutoHotkey.dll](https://github.com/HotKeyIt/ahkdll-v1-release/tree/master/Win32w) script which runs on program load, and uses compiled binaries from [MScholtes/VirtualDesktop](https://github.com/MScholtes/VirtualDesktop)
  <details>
    <summary> (more details ...) </summary>
	
    - use Ctrl+Alt+Shift+S at anytime to toggle this override on/off
    - this override enables desktops wraping/cycling when reaching edges (this is the purpose of it)
    - the reason for using external binaries and not the C# code itself to move to desktops, is that I'm new to AHK scripts and couldn't find a way to make script interact with C# code (and vice versa); would be happy for suggestions on how to do it.
  </details>
  
* major (and pretty useful) updates to Data sub-menu:-
  - "list all browsers' URLs" option, with an option to copy to clipboard a list of all tabs and their URLs
    <details>
    <summary> (more details ...) </summary>
	
	- supports Firefox, Chrome, MSEdge & I.E.
	- uses SHDocVw (for I.E.)  &  UIAutomationCore.dll (for the rest)
	- UIAutomationCore method, basically traverses the UI tree of each browser looking for the tabs list; therefore, it may not work on future versions if changes happen to UI.
	- UIAutomationCore method, also depends that names of some UI elements are in English, so localized versions of the browsers might break it.
	- was tested & working on the ENGLISH-language versions of: chrome (v105.0.5195.127_64-bit), firefox (v105.0_64-bit), msedge (v105.0.1343.42_64-bit), I.E. (v11.00.19041.1566, on win10_20H2)
    </details>
    
  - "Export URLs", same as list URLs , but exports them to an HTML file which has clickable links
  - "list all open folders", lists all open folders' full paths, with an option to copy to clipboard
  - "Export Folders", same as the above, but saves the full paths to a BATCH file which opens all folders when run
  - "Export All Data" was updated to include also URLs list & folders list from the above items
  - "Screenshot Current" which takes a screenshot(s) of current desktop and saves to image file(s)
  - "Screenshot All" which takes screenshots of all desktops and saves all of them
  - updated default filename for all "Export"-items above to include current date-time
  - icons to all items ( from [icons8](https://icons8.com "") )
* Panic! item in data menu, which quickly does all the exports and screenshots mentioned above, to a default directory (on User's Desktop) with minimal prompts
* in "About" dialog, added Hotkeys button which opens a message-box with Hotkeys list information.
* more organized and concise code (Functions.cs file which separates extra classes from Form1.cs)

<div align="center">
<img src="https://user-images.githubusercontent.com/48130426/192449839-9d781691-9af8-47c3-b1f8-8685d4705d8d.png" width=50% height=50% align="center">
</div>

</details>

### version 2.3.1 ###	
<details>
<summary> released on 2022-07-21 ... </summary>
	
* minor 'cosmetic' changes: added two submenus: 'cycling' & 'data', to group together similar items and make main-menu more compact
  
<div align="center">
<img src="https://user-images.githubusercontent.com/48130426/180141305-0b4c79b7-508b-43c3-b179-7b8192e902a0.png" width=50% height=50% align="center">
</div>

</details>

### version 2.3 ###
<details>
<summary> released on 2022-07-20 ... </summary>
	
* new icons in main-menu ; mostly from: [icons8](https://icons8.com "")
* splash screen option, to show desktop# & title for a couple of seconds, when desktop is changed
  - note that it is automatically de-activated before cycle / rev-cycle (because of conflict in 'timers')
* "list windows" feature , to list all open windows (their handles & titles) in the CURRENT desktop ; can copy data to clipboard as in 'desktops GUIDs' from before
* "export data" feature, to save a text file with all desktop data: titles, GUIDs, and a windows-list per each desktop (could be useful to help restore open windows, when sudden restart for example;  manually!)
* descriptive tooltips for main-menu
* "About" dialog  

<div align="center">
<img src="https://user-images.githubusercontent.com/48130426/180053663-4c6b4762-0a4b-4366-aa89-629850e00f74.png" width=50% height=50% align="center">
</div>

</details>

### version 2.2 ###
<details>
<summary> released on 2022-07-09 ... </summary>
	
* new menu-items in desktops-list: 
  - "Close All" to close all desktops at once
  - "Add Multiple" to add as many desktops as user enters [range: 1 to 10]			
* some minor UI improvements in desktops-list: new icons, tooltips ...
* minor code clean-up / order  

</details>

### version 2.1 ###
<details>
<summary> released on 2022-03-03 ... </summary>

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


</details>


### version 1.9-modified ###
<details>
<summary> released on 2022-02-19 ... </summary>



___changes / additions made in this modification:-___

1. changes to tray icon visibility / appearance: background colors, font size (bigger), and more visible numbers; Because of these changes, tray-icon was restricted back to 1-9 desktops only, and after the 9th desktop a "+" sign will appear instead (no crashing)
2. clean-up of Resources folder of unused icons
3. added a function on LEFT-mouse click on tray icon >> move to next desktop (by numerical order), and if holding SHIFT along with click >> move to previous desktop
4. added two context-menu items: NEXT , PREVIOUS that do the above functionalities in (3.)
5. added desktops'-list sub-menu: to go to each desktop by a single click on its number
6. added (Desktop Number) shown when mouse-hovering over tray icon (useful when more than 9 desktops)
7. added colors' palette (~ 14 options) to choose from to change tray-icon's background color, or make it transparent.


<img src="https://user-images.githubusercontent.com/48130426/154814650-32d65f4c-b4b0-45a1-8d98-b31df779d4fb.png" width=45% height=45%> <img src="https://user-images.githubusercontent.com/48130426/154814667-1013a978-b1e7-47da-97c4-b349f1145f48.png" width=45% height=45%> <img src="https://user-images.githubusercontent.com/48130426/154814673-8701f934-b859-4e3f-ac6d-445acac9a47b.png" width=45% height=45%>


</details>



------------------------------------------------------------

## original README ##
@ [m0ngr31/VirtualDesktopManager](https://github.com/m0ngr31/VirtualDesktopManager "")

<details>
<summary> ↓ ↓ ↓ ↓ ↓ </summary>

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


</details>
