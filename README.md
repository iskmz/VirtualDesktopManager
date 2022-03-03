## Virtual Desktop Manager -- a modified version 1.9 ##

_forked from [m0ngr31/VirtualDesktopManager](https://github.com/m0ngr31/VirtualDesktopManager "")_

__↓ below is the original README (as of 2022-02-19) ↓__


___changes / additions made in this modification:-___

1. changes to tray icon visibility / appearance: background colors, font size (bigger), and more visible numbers; Because of these changes, tray-icon was restricted back to 1-9 desktops only, and after the 9th desktop a "+" sign will appear instead (no crashing)
2. clean-up of Resources folder of unused icons
3. added a function on LEFT-mouse click on tray icon >> move to next desktop (by numerical order), and if holding SHIFT along with click >> move to previous desktop
4. added two context-menu items: NEXT , PREVIOUS that do the above functionalities in (3.)
5. added desktops'-list sub-menu: to go to each desktop by a single click on its number
6. added (Desktop Number) shown when mouse-hovering over tray icon (useful when more than 9 desktops)
7. added colors' palette (~ 14 options) to choose from to change tray-icon's background color, or make it transparent.


![Screenshot 2022-02-19 000216](https://user-images.githubusercontent.com/48130426/154814650-32d65f4c-b4b0-45a1-8d98-b31df779d4fb.png)


![Screenshot 2022-02-19 102819](https://user-images.githubusercontent.com/48130426/154814667-1013a978-b1e7-47da-97c4-b349f1145f48.png)


![Screenshot 2022-02-19 174355](https://user-images.githubusercontent.com/48130426/154814673-8701f934-b859-4e3f-ac6d-445acac9a47b.png)

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
