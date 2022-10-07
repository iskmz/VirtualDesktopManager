using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Runtime.InteropServices;
using HWND = System.IntPtr;
using System.Text;
using System.Diagnostics;
using UIAutomationClient;
using SHDocVw;
using System.Windows.Forms;
using System.IO;
using WindowsDesktop;

namespace VirtualDesktopManager
{

    public static class Consts
    {
        // to make life easier //
        public const ModifierKeys CTRL = ModifierKeys.Control;
        public const ModifierKeys SHIFT = ModifierKeys.Shift;
        public const ModifierKeys ALT = ModifierKeys.Alt;
        public const ModifierKeys CTRL_ALT_SHIFT = CTRL | ALT | SHIFT;
        public const ModifierKeys CTRL_ALT = CTRL | ALT;
        public const ModifierKeys ALT_SHIFT = ALT | SHIFT;
        public const ModifierKeys NONE = ModifierKeys.None;

        // ranges of valid values; for checks on loading preferences from xml // 
        private static int[] cycles_amount_values = new int[] { 1, 2, 3, 4, 5, -1 };
        private static int[] transition_time_values = new int[] { 2, 4, 6, 8, 10, 12, 14 };
        private static string[] light_colors_values = new string[] { "back_Pink", "back_Red", "back_Green",
            "back_Blue", "back_Yellow", "back_White"};
        private static string[] dark_colors_values = new string[] {"back_Black", "back_Brown", "back_Dark_Blue",
            "back_Dark_Green", "back_Dark_Red", "back_Purple"};
        private static string[] brush_values = new string[] { "white", "black" };


        public static bool isValidColorAndBrush(string color_to_check, string brush_to_check)
        {
            return (dark_colors_values.Contains(color_to_check) && brush_to_check.Equals("white")) ||
                 (light_colors_values.Contains(color_to_check) && brush_to_check.Equals("black")) ||
                 (color_to_check.Equals("Transparent") && brush_values.Contains(brush_to_check));
        }

        public static bool isValidCyclesAmount(string cycles_to_check)
        {
            int res;
            return ((int.TryParse(cycles_to_check, out res)) && (cycles_amount_values.Contains(res)));
        }

        public static bool isValidTransTime(string trans_time_to_check)
        {
            int res;
            return ((int.TryParse(trans_time_to_check, out res)) && (transition_time_values.Contains(res)));
        }

        // defaults to fall-back on , when loading from xml fails somehow // 
        public const string DEFAULT_COLOR = "back_Dark_Blue";
        public const string DEFAULT_BRUSH = "white";
        public const int DEFAULT_CYCLES_AMOUNT = 1;
        public const int DEFAULT_TRANS_TIME = 4;

        // added 2022-09-24
        public enum BROWSER { CHROME, FIREFOX, MSEDGE, IEXPLORER };
        public const string CHROME_process_name = "chrome";
        public const string FIREFOX_process_name = "firefox";
        public const string MSEDGE_process_name = "msedge";
        public const string IEXPLORER_process_name = "iexplore";

        // added 2022-09-28
        public const string HotkeysList_TITLE = "Hotkeys List";
        private const string dot = "\n\u25CF "; //  ●
        private const string tt1 = "\n\u25B7 ";   //  ▷
        private const string tt2 = "\n\n\u25B6 ";   //  ▶
        private const string sqr = "\n\u25FD ";   //  ◽
        public const string HotkeysList_MSG = ""
                + tt2 + "Ctrl+Alt+Right/Left: move to right/left desktop , with wraping/cycling when reaching edges; "
                + "alternate combination is Alt+Shift+Right/Left , to be chosen in Settings."
                + tt2 + "Ctrl+Alt+Digit[1-9]: move to desktop with digit selected; "
                + "alternate combination is Alt+Shift+Digit[1-9] , to be chosen in Settings."
                + tt2 + "special combination only when cycling (or reverse cycling) "
                + "FOREVER, in order to stop cycling is: Ctrl+Alt+S"
                + tt2 + "as of version 2.4, "
                + "default windows combination: Ctrl+Winkey+Right/Left "
                + "is overriden to make desktops wrap/cycle when reaching edges "
                + "{also touchpad-4-fingers swipe Right/Left is overriden}; "
                + "in order to Deactivate/Activate this override feature use "
                + "the following combination: Ctrl+Alt+Shift+S"
                + tt2 + "as of version 2.4.1"
                + dot + "Ctrl+Alt+Shift+P : activates Panic! item."
                + dot + "Ctrl+Alt+Shift+L : shows/hides desktops List, at the center of the main screen; "
                + "for fast desktop #, Add, Close selections using arrows & Enter."
                + dot + "Ctrl+Alt+Shift+H : shows Hotkeys List (this one)."
                + tt2 + "using Mouse:-"
                + dot + "Left-click on tray-icon: go-to next desktop"
                + dot + "Shift+Left-click on tray-icon: go-to previous"
                + tt2 + "as of version 2.4.2"
                + dot + "Mouse-wheel-down over main-taskbar area: next desktop"
                + dot + "Mouse-wheel-up over main-taskbar area: previous desktop"
                + "\n\n";
    }

    [Serializable]
    public class UserPreferences
    {
        internal string _BackColor;
        public string BackColor
        {
            get { return _BackColor; }
            set { _BackColor = value; }
        }

        internal string _BrushName;
        public string BrushName
        {
            get { return _BrushName; }
            set { _BrushName = value; }
        }

        internal string _cycleTransTime;
        public string cycleTransTime
        {
            get { return _cycleTransTime; }
            set { _cycleTransTime = value; }
        }

        internal string _cyclesAmount;
        public string cyclesAmount
        {
            get { return _cyclesAmount; }
            set { _cyclesAmount = value; }
        }
    }



    // added 2022-07-20 // sources: https://stackoverflow.com/a/43640787   &    http://www.tcx.be/blog/2006/list-open-windows/   // 
    //  modified and added more methods on 2022-09-24 to extend functionality for extracting open folder paths, and internet browsers' windows //
    /// <summary>Contains functionality to get all the open windows.</summary>
    public static class OpenWindowGetter
    {
        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        public static IDictionary<HWND, string> GetOpenWindows()
        {
            HWND shellWindow = GetShellWindow();
            Dictionary<HWND, string> windows = new Dictionary<HWND, string>();
            EnumWindows(delegate (HWND hWnd, int lParam)
            {
                if ((hWnd == shellWindow) || !IsWindowVisible(hWnd)) return true;
                int length = GetWindowTextLength(hWnd);
                if (length == 0) return true;
                StringBuilder builder = new StringBuilder(length);
                GetWindowText(hWnd, builder, length + 1);
                windows[hWnd] = builder.ToString();
                return true;
            }, 0);
            return windows;
        }

        private delegate bool EnumWindowsProc(HWND hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(HWND hWnd, StringBuilder lpString, int nMaxCount);

        private static string getWinTxt(HWND hWnd)
        {
            int length = GetWindowTextLength(hWnd);
            StringBuilder builder = new StringBuilder(length);
            GetWindowText(hWnd, builder, length + 1);
            return builder.ToString();
        }

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern IntPtr GetShellWindow();

        [DllImport("USER32.DLL")]
        private static extern int GetWindowThreadProcessId(HWND hWnd, out int processId);

        private static int getPID(HWND hWnd)
        {
            GetWindowThreadProcessId(hWnd, out int p);
            return p;
        }

        [DllImport("USER32.DLL")]
        private static extern bool EnumChildWindows(HWND hWnd, EnumWindowsProc enumFunc, int lParam);

        public class explorerFolderObj
        {
            HWND win; // main window handle of each folder
            string title; // main window title (text)
            string address; // full address  
            public explorerFolderObj(HWND win, string title, string address)
            {
                this.win = win; this.title = title; this.address = address;
            }
            public override string ToString() => win + " ,\t" + title + "\n [ " + address + " ]";
            public HWND getWindowHandle() => this.win;
            public string getTitle() => this.title;
            public string getAddress() => this.address;
        }

        public static List<explorerFolderObj> getExplorerAddressList()
        {
            HWND shellWindow = GetShellWindow();
            List<explorerFolderObj> addressList = new List<explorerFolderObj>();
            EnumWindows(delegate (HWND hWnd, int lParam)
            {
                if ((hWnd == shellWindow) || (!IsWindowVisible(hWnd)) || (GetWindowTextLength(hWnd) == 0))
                    return true;
                if (Process.GetProcessById(getPID(hWnd)).ProcessName.Equals("explorer"))
                {
                    EnumChildWindows(hWnd, delegate (HWND hWndChild, int lp)
                    {
                        if (getWinTxt(hWndChild).IndexOf("Address:") == 0)
                            addressList.Add(new explorerFolderObj(hWnd, getWinTxt(hWnd),
                                getWinTxt(hWndChild).Replace("Address: ", "")));
                        return true;
                    }
                    , 0);
                }
                return true;
            }, 0);
            return addressList;
        }

        // get Browsers' Open [& NOT MINIMIZED] Windows (handles of windows)
        //  if any window is minimized, handles would be added, but might not be able to extract tabs further down the "code"
        public static IDictionary<Consts.BROWSER, List<HWND>> GetBrowsersWindowsLists()
        {
            Dictionary<Consts.BROWSER, List<HWND>> res = new Dictionary<Consts.BROWSER, List<HWND>>();
            // lists of windows' handles
            List<HWND> lstChrome = new List<HWND>();
            List<HWND> lstFirefox = new List<HWND>();
            List<HWND> lstMSEdge = new List<HWND>();
            List<HWND> lstIExplorer = new List<HWND>();
            foreach (KeyValuePair<IntPtr, string> window in GetOpenWindows())
            {
                IntPtr handle = window.Key;
                string processName = Process.GetProcessById(getPID(handle)).ProcessName;
                if (processName.Equals(Consts.CHROME_process_name)) lstChrome.Add(handle);
                else if (processName.Equals(Consts.FIREFOX_process_name)) lstFirefox.Add(handle);
                else if (processName.Equals(Consts.MSEDGE_process_name)) lstMSEdge.Add(handle);
                else if (processName.Equals(Consts.IEXPLORER_process_name)) lstIExplorer.Add(handle);
            }
            res[Consts.BROWSER.CHROME] = lstChrome;
            res[Consts.BROWSER.FIREFOX] = lstFirefox;
            res[Consts.BROWSER.MSEDGE] = lstMSEdge;
            res[Consts.BROWSER.IEXPLORER] = lstIExplorer;
            return res;
        }


        #region Failed attempts to fix window-focus issue - 2022-10-04 - v2.4.2.11

        // added 2022-10-04
        // returns handle to the first open (& not minimized) window that is found on currentDesktop
        // if desktop has no open windows (or all are minimized), then IntPtr.Zero is returned 
        private static HWND GetAnyOpenWindow(VirtualDesktop currentDesktop)
        {
            HWND retValue = IntPtr.Zero; // default assumption:  this desktop has NO windows open , or All are minimized //
            HWND shellWindow = GetShellWindow();
            EnumWindows(delegate (HWND hWnd, int lParam)
            {
                if ((hWnd == shellWindow) || !IsWindowVisible(hWnd) || GetWindowTextLength(hWnd) == 0) return true; // continue
                if (IsIconic(hWnd)) return true; // if window is minimized , continue
                var winDesk = VirtualDesktop.FromHwnd(hWnd);
                if (winDesk != null && winDesk.Id.Equals(currentDesktop.Id))
                {
                    retValue = hWnd;
                    return false; // stop looking!
                }
                return true;
            }, 0);
            return retValue;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);


        // added 2022-10-04 
        // credits: https://stackoverflow.com/questions/825595/how-to-get-the-z-order-in-windows  //
        // returns a list of handles for all non-iconic (not minimized) open windows found on the current desktop
        // list is ordered by z-order, from the lowest to top-most window (at the end)
        private static List<HWND> getNonIconicWindowsByZOrder(VirtualDesktop currentDesktop)
        {
            List<HWND> windows = new List<HWND>();
            HWND startPoint = GetAnyOpenWindow(currentDesktop);
            startPoint = GetWindow(startPoint, (uint)GW_Cmd.GW_HWNDLAST); // bottom window
            for (var h = startPoint; h != IntPtr.Zero; h = GetWindow(h, (uint)GW_Cmd.GW_HWNDPREV))
            {
                var hVD = VirtualDesktop.FromHwnd(h);
                if (hVD != null && hVD.Id.Equals(currentDesktop.Id) && !IsIconic(h)) windows.Add(h);
            }
            return windows;
        }


        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        enum GW_Cmd : uint   // reference:  https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindow //
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,    // lowest in Z-Order
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,    // window above in Z-Order
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        public static void showWindows_ByZOrder(VirtualDesktop currentDesktop) // added 2022-10-04
        {
            List<HWND> windows = getNonIconicWindowsByZOrder(currentDesktop);
            foreach (HWND win in windows) ShowWindow(win,5); // also tried SetForegroundWindow(hWnd) ... 
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow); // nCmdShow: SW_HIDE=0, SW_SHOW=5  ... // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-showwindow

        #endregion
    }
    // ************************************************************************************************************ //


    // class was added 2022-09-24:  to add functionality to extract Browsers' tabs URLs //
    public class BrowsersURLs
    {
        public class tabUrlObj
        {
            HWND win; // main window handle of tabs
            string title; // tab title
            string url; // full url address
            public tabUrlObj(HWND win, string title, string url)
            {
                this.win = win; this.title = title; this.url = url;
            }
            public override string ToString() => win + " ,\t" + title + "\n [ " + url + " ]";
            public HWND getWindowHandle() => this.win;
            public string getTitle() => this.title;
            public string getURL() => this.url;
        }

        private static IUIAutomationElement Nth_Parent(IUIAutomationTreeWalker tw, int depth, IUIAutomationElement e)
        {
            while (0 < depth--) e = tw.GetParentElement(e);
            return e;
        }

        private static IUIAutomationElement Nth_FirstChild(IUIAutomationTreeWalker tw, int depth, IUIAutomationElement e)
        {
            while (0 < depth--) e = tw.GetFirstChildElement(e);
            return e;
        }

        private static IUIAutomationElement NthChild(IUIAutomationTreeWalker tw, int order, IUIAutomationElement e)
        {
            e = tw.GetFirstChildElement(e); // first child
            while (1 < order--) e = tw.GetNextSiblingElement(e);
            return e;
        }

        private static List<tabUrlObj> getChromeTabURLs()
        {
            List<tabUrlObj> tabs = new List<tabUrlObj>();
            // get Chrome's Open [& NOT MINIMIZED] Windows // if any window is minimized, no tabs are added !! // 
            List<HWND> chromeWindows = browsersWindowsLists[Consts.BROWSER.CHROME];
            if (chromeWindows.Count == 0) return tabs;
            // go-over each window, and extract tabs list in each //
            foreach (HWND win in chromeWindows)
            {
                // FASTEST way,  almost instant , {apparently FIND-ALL methods in previous ways make things very slow}
                // but LESS reliable for previous or future versions of chrome, as UI-tree order might change !  
                // & this method basically walks-along chrome's UI-tree as described in M.S. INSPECT.exe tool in WinSDK
                // also it uses some PropertyNames in English (30005) , so localizations other than English might break it 
                try
                {
                    var uiaClassObject = new CUIAutomation();
                    IUIAutomationElement root = uiaClassObject.ElementFromHandle(win);
                    IUIAutomationElement elmGoogleChrome = root.FindFirst(TreeScope.TreeScope_Children,
                        uiaClassObject.CreatePropertyCondition(30005, "Google Chrome")); //UIA_NamePropertyId: 30005
                    // UIA_IsControlElementPropertyId: 30016
                    IUIAutomationTreeWalker tw = uiaClassObject.CreateTreeWalker(uiaClassObject.CreatePropertyCondition(30016, true));
                    IUIAutomationElement firstTab = Nth_FirstChild(tw, 5, tw.GetLastChildElement(tw.GetFirstChildElement(elmGoogleChrome)));
                    IUIAutomationElement toolBar = tw.GetNextSiblingElement(Nth_Parent(tw, 3, firstTab));
                    IUIAutomationElement addrBar = toolBar.FindFirst(TreeScope.TreeScope_Descendants,
                        uiaClassObject.CreatePropertyCondition(30005, "Address and search bar"));
                    IUIAutomationElement tab = firstTab;
                    do
                    {
                        ((IUIAutomationLegacyIAccessiblePattern)tab.GetCurrentPattern(10018)).DoDefaultAction(); //UIA_LegacyIAccessiblePatternId = 10018
                        var pBar = addrBar.GetCurrentPattern(10018) as IUIAutomationLegacyIAccessiblePattern;
                        pBar.DoDefaultAction(); pBar.DoDefaultAction(); // DefaultAction x TWICE:  to expose https/http .. etc // 
                        tabs.Add(new tabUrlObj(win, tab.CurrentName, addrBar.GetCurrentPropertyValue(30093)));  // UIA_LegacyIAccessibleValuePropertyId: 30093
                    }
                    while ((tab = tw.GetNextSiblingElement(tab)) != null);
                }
                catch (Exception e) { continue; }  // in case any window was minimized, or NULL_pointer_exception , or else ... 
            }
            return tabs;
        }

        private static List<tabUrlObj> getFirefoxTabURLs()
        {
            List<tabUrlObj> tabs = new List<tabUrlObj>();
            // get Firefox Open [& NOT MINIMIZED] Windows // if any window is minimized, no tabs are added !! // 
            List<HWND> firefoxWindows = browsersWindowsLists[Consts.BROWSER.FIREFOX];
            if (firefoxWindows.Count == 0) return tabs;
            // go-over each window, and extract tabs list in each //
            foreach (HWND win in firefoxWindows)
            {
                // FASTEST way,  almost instant , but LESS reliable for previous or future versions of firefox, 
                // as UI-tree order might change ! , this method basically traverses firefox UI-tree as described in M.S.INSPECT
                // also it uses some PropertyNames in English (30005) , so localizations other than English might break it 
                try
                {
                    var uiaClassObject = new CUIAutomation();
                    IUIAutomationElement root = uiaClassObject.ElementFromHandle(win);
                    IUIAutomationElement elmBrowserTabs = root.FindFirst(TreeScope.TreeScope_Children,
                        uiaClassObject.CreatePropertyCondition(30005, "Browser tabs")); //UIA_NamePropertyId: 30005
                    // UIA_IsControlElementPropertyId: 30016
                    IUIAutomationTreeWalker tw = uiaClassObject.CreateTreeWalker(uiaClassObject.CreatePropertyCondition(30016, true));
                    IUIAutomationElement firstTab = tw.GetFirstChildElement(tw.GetFirstChildElement(elmBrowserTabs));
                    IUIAutomationElement toolBar = root.FindFirst(TreeScope.TreeScope_Children,
                        uiaClassObject.CreatePropertyCondition(30005, "Navigation")); //UIA_NamePropertyId: 30005
                    IUIAutomationElement addrBar = tw.GetFirstChildElement(toolBar.FindFirst(TreeScope.TreeScope_Children,
                        uiaClassObject.CreatePropertyCondition(30003, 50003))); // UIA_ControlTypePropertyId: 30003 , UIA_ComboBoxControlTypeId: 50003
                    IUIAutomationElement tab = firstTab;
                    do
                    {
                        if (tab.CurrentControlType != 50019) continue;  //UIA_TabItemControlTypeId: 50019
                        ((IUIAutomationLegacyIAccessiblePattern)tab.GetCurrentPattern(10018)).DoDefaultAction(); //UIA_LegacyIAccessiblePatternId = 10018
                        tabs.Add(new tabUrlObj(win, tab.CurrentName, addrBar.GetCurrentPropertyValue(30093)));  // UIA_LegacyIAccessibleValuePropertyId: 30093
                    }
                    while ((tab = tw.GetNextSiblingElement(tab)) != null);
                }
                catch (Exception e) { continue; }  // in case any window was minimized, or NULL_pointer_exception , or else ... 
            }
            return tabs;
        }

        private static List<tabUrlObj> getMSEdgeTabURLs()
        {
            List<tabUrlObj> tabs = new List<tabUrlObj>();
            // get MSEdge Open [& NOT MINIMIZED] Windows // if any window is minimized, no tabs are added !! // 
            List<HWND> msedgeWindows = browsersWindowsLists[Consts.BROWSER.MSEDGE];
            if (msedgeWindows.Count == 0) return tabs;
            // go-over each window, and extract tabs list in each //
            foreach (HWND win in msedgeWindows)
            {
                // FASTEST way,  almost instant , but LESS reliable for previous or future versions of msedge, 
                // as UI-tree order might change ! , this method basically traverses msedge UI-tree as described in M.S.INSPECT
                try
                {
                    var uiaClassObject = new CUIAutomation();
                    IUIAutomationElement root = uiaClassObject.ElementFromHandle(win);
                    // UIA_IsEnabledPropertyId: 30010 // unlike chrome/firefox, here some elements are NOT of control-type, so needed to find more "general" property
                    IUIAutomationTreeWalker tw = uiaClassObject.CreateTreeWalker(uiaClassObject.CreatePropertyCondition(30010, true));
                    IUIAutomationElement elmMsEdge = Nth_FirstChild(tw, 2, root);
                    IUIAutomationElement tabBar = Nth_FirstChild(tw, 2, NthChild(tw, 4, (tw.GetFirstChildElement(elmMsEdge))));
                    IUIAutomationElement appBar = tw.GetNextSiblingElement(tabBar);
                    IUIAutomationElement firstTab = Nth_FirstChild(tw, 2, (NthChild(tw, 2, tabBar)));
                    IUIAutomationElement addrBar = appBar.FindFirst(TreeScope.TreeScope_Descendants,
                        uiaClassObject.CreatePropertyCondition(30003, 50004)); // UIA_ControlTypePropertyId: 30003 , UIA_EditControlTypeId: 50004
                    IUIAutomationElement tab = firstTab;
                    do
                    {
                        ((IUIAutomationLegacyIAccessiblePattern)tab.GetCurrentPattern(10018)).DoDefaultAction(); //UIA_LegacyIAccessiblePatternId = 10018
                        tabs.Add(new tabUrlObj(win, tab.CurrentName, addrBar.GetCurrentPropertyValue(30093)));  // UIA_LegacyIAccessibleValuePropertyId: 30093
                    }
                    while ((tab = tw.GetNextSiblingElement(tab)) != null);
                }
                catch (Exception e) { continue; }  // in case any window was minimized, or NULL_pointer_exception , or else ... 
            }
            return tabs;
        }

        private static List<tabUrlObj> getIExplorerTabURLs()
        {
            // did NOT work using UIAutomationCore.dll // something to do with active-windows/tabs ! 
            // which led to null pointer exceptions if a window, for e.g., was not active/focused ... 
            // attempts to set-focus manually did not work !
            // so needed to use a different approach:-  [using SHDocVw]
            // main source idea from: https://www.codeproject.com/questions/500271/howplustoplusgetplusurlsplusfromplusallplustabplus //
            ShellWindows shellWindows = new ShellWindows();
            string filename;
            List<tabUrlObj> tabs = new List<tabUrlObj>();
            foreach (InternetExplorer ie in shellWindows)
            {
                filename = System.IO.Path.GetFileNameWithoutExtension(ie.FullName).ToLower();
                if (filename.Equals("iexplore"))
                    tabs.Add(new tabUrlObj(new HWND(ie.HWND), ie.LocationName, ie.LocationURL));
                // else if (filename.Equals("explorer")) Console.WriteLine(new HWND(ie.HWND) + "\t" + ie.LocationName + "\t" + ie.LocationURL);
                // line above has an ALTERNATIVE way to get all explorer open folders paths (& titles) ...  
            }
            return tabs;
        }

        // NOTE: should SAVE desktop's position (index) before running getAllTabURLs() , in order to move back to original position afterwards
        // because some get{Browser}TabURLs methods move along the tabs , and hence change desktops while collecting urls ... 
        private static IDictionary<Consts.BROWSER, List<HWND>> browsersWindowsLists;
        public static IDictionary<Consts.BROWSER, List<tabUrlObj>> getAllTabURLs()
        {
            browsersWindowsLists = OpenWindowGetter.GetBrowsersWindowsLists(); // to be used by each  get{Browser}TabURLs() method // 
            IDictionary<Consts.BROWSER, List<tabUrlObj>> allTabURLs = new Dictionary<Consts.BROWSER, List<tabUrlObj>>();
            allTabURLs[Consts.BROWSER.FIREFOX] = getFirefoxTabURLs();
            allTabURLs[Consts.BROWSER.MSEDGE] = getMSEdgeTabURLs();
            allTabURLs[Consts.BROWSER.IEXPLORER] = getIExplorerTabURLs();
            allTabURLs[Consts.BROWSER.CHROME] = getChromeTabURLs();
            return allTabURLs;
        }
    }
    // ************************************************************************************************************ //




    // ************************************************************************************************************ //
    // added 2022-03-02 // 
    // copied from https://github.com/MScholtes/VirtualDesktop/blob/master/VirtualDesktop.cs , lines 29-253 //
    // the ENTIRE  COM API  region of code .... could not delete any part of it, seems all are interdependent ! //
    // only needed to use the SetName function in  IVirtualDesktopManagerInternal2 Interface
    // to set-up desktop change-name / remove-name functions correctly !
    // it did not work by only changing values of registry keys directly 
    // because in Task-View: no change of names! .. only desktops-list menu changed! (& registry keys of course!) //
    #region COM API
    internal static class Guids
    {
        public static readonly Guid CLSID_ImmersiveShell = new Guid("C2F03A33-21F5-47FA-B4BB-156362A2F239");
        public static readonly Guid CLSID_VirtualDesktopManagerInternal = new Guid("C5E0CDCA-7B6E-41B2-9FC4-D93975CC467B");
        public static readonly Guid CLSID_VirtualDesktopManager = new Guid("AA509086-5CA9-4C25-8F95-589D3C07B48A");
        public static readonly Guid CLSID_VirtualDesktopPinnedApps = new Guid("B5A399E7-1C87-46B8-88E9-FC5747B171BD");
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Size
    {
        public int X;
        public int Y;
        private int width;
        private int height;

        public Size(int width, int height) : this()
        {
            this.width = width;
            this.height = height;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    internal enum APPLICATION_VIEW_CLOAK_TYPE : int
    {
        AVCT_NONE = 0,
        AVCT_DEFAULT = 1,
        AVCT_VIRTUAL_DESKTOP = 2
    }

    internal enum APPLICATION_VIEW_COMPATIBILITY_POLICY : int
    {
        AVCP_NONE = 0,
        AVCP_SMALL_SCREEN = 1,
        AVCP_TABLET_SMALL_SCREEN = 2,
        AVCP_VERY_SMALL_SCREEN = 3,
        AVCP_HIGH_SCALE_FACTOR = 4
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
    [Guid("372E1D3B-38D3-42E4-A15B-8AB2B178F513")]
    internal interface IApplicationView
    {
        int SetFocus();
        int SwitchTo();
        int TryInvokeBack(IntPtr /* IAsyncCallback* */ callback);
        int GetThumbnailWindow(out IntPtr hwnd);
        int GetMonitor(out IntPtr /* IImmersiveMonitor */ immersiveMonitor);
        int GetVisibility(out int visibility);
        int SetCloak(APPLICATION_VIEW_CLOAK_TYPE cloakType, int unknown);
        int GetPosition(ref Guid guid /* GUID for IApplicationViewPosition */, out IntPtr /* IApplicationViewPosition** */ position);
        int SetPosition(ref IntPtr /* IApplicationViewPosition* */ position);
        int InsertAfterWindow(IntPtr hwnd);
        int GetExtendedFramePosition(out Rect rect);
        int GetAppUserModelId([MarshalAs(UnmanagedType.LPWStr)] out string id);
        int SetAppUserModelId(string id);
        int IsEqualByAppUserModelId(string id, out int result);
        int GetViewState(out uint state);
        int SetViewState(uint state);
        int GetNeediness(out int neediness);
        int GetLastActivationTimestamp(out ulong timestamp);
        int SetLastActivationTimestamp(ulong timestamp);
        int GetVirtualDesktopId(out Guid guid);
        int SetVirtualDesktopId(ref Guid guid);
        int GetShowInSwitchers(out int flag);
        int SetShowInSwitchers(int flag);
        int GetScaleFactor(out int factor);
        int CanReceiveInput(out bool canReceiveInput);
        int GetCompatibilityPolicyType(out APPLICATION_VIEW_COMPATIBILITY_POLICY flags);
        int SetCompatibilityPolicyType(APPLICATION_VIEW_COMPATIBILITY_POLICY flags);
        int GetSizeConstraints(IntPtr /* IImmersiveMonitor* */ monitor, out Size size1, out Size size2);
        int GetSizeConstraintsForDpi(uint uint1, out Size size1, out Size size2);
        int SetSizeConstraintsForDpi(ref uint uint1, ref Size size1, ref Size size2);
        int OnMinSizePreferencesUpdated(IntPtr hwnd);
        int ApplyOperation(IntPtr /* IApplicationViewOperation* */ operation);
        int IsTray(out bool isTray);
        int IsInHighZOrderBand(out bool isInHighZOrderBand);
        int IsSplashScreenPresented(out bool isSplashScreenPresented);
        int Flash();
        int GetRootSwitchableOwner(out IApplicationView rootSwitchableOwner);
        int EnumerateOwnershipTree(out IObjectArray ownershipTree);
        int GetEnterpriseId([MarshalAs(UnmanagedType.LPWStr)] out string enterpriseId);
        int IsMirrored(out bool isMirrored);
        int Unknown1(out int unknown);
        int Unknown2(out int unknown);
        int Unknown3(out int unknown);
        int Unknown4(out int unknown);
        int Unknown5(out int unknown);
        int Unknown6(int unknown);
        int Unknown7();
        int Unknown8(out int unknown);
        int Unknown9(int unknown);
        int Unknown10(int unknownX, int unknownY);
        int Unknown11(int unknown);
        int Unknown12(out Size size1);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("1841C6D7-4F9D-42C0-AF41-8747538F10E5")]
    internal interface IApplicationViewCollection
    {
        int GetViews(out IObjectArray array);
        int GetViewsByZOrder(out IObjectArray array);
        int GetViewsByAppUserModelId(string id, out IObjectArray array);
        int GetViewForHwnd(IntPtr hwnd, out IApplicationView view);
        int GetViewForApplication(object application, out IApplicationView view);
        int GetViewForAppUserModelId(string id, out IApplicationView view);
        int GetViewInFocus(out IntPtr view);
        int Unknown1(out IntPtr view);
        void RefreshCollection();
        int RegisterForApplicationViewChanges(object listener, out int cookie);
        int UnregisterForApplicationViewChanges(int cookie);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("FF72FFDD-BE7E-43FC-9C03-AD81681E88E4")]
    internal interface IVirtualDesktop
    {
        bool IsViewVisible(IApplicationView view);
        Guid GetId();
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("F31574D6-B682-4CDC-BD56-1827860ABEC6")]
    internal interface IVirtualDesktopManagerInternal
    {
        int GetCount();
        void MoveViewToDesktop(IApplicationView view, IVirtualDesktop desktop);
        bool CanViewMoveDesktops(IApplicationView view);
        IVirtualDesktop GetCurrentDesktop();
        void GetDesktops(out IObjectArray desktops);
        [PreserveSig]
        int GetAdjacentDesktop(IVirtualDesktop from, int direction, out IVirtualDesktop desktop);
        void SwitchDesktop(IVirtualDesktop desktop);
        IVirtualDesktop CreateDesktop();
        void RemoveDesktop(IVirtualDesktop desktop, IVirtualDesktop fallback);
        IVirtualDesktop FindDesktop(ref Guid desktopid);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("0F3A72B0-4566-487E-9A33-4ED302F6D6CE")]
    internal interface IVirtualDesktopManagerInternal2
    {
        int GetCount();
        void MoveViewToDesktop(IApplicationView view, IVirtualDesktop desktop);
        bool CanViewMoveDesktops(IApplicationView view);
        IVirtualDesktop GetCurrentDesktop();
        void GetDesktops(out IObjectArray desktops);
        [PreserveSig]
        int GetAdjacentDesktop(IVirtualDesktop from, int direction, out IVirtualDesktop desktop);
        void SwitchDesktop(IVirtualDesktop desktop);
        IVirtualDesktop CreateDesktop();
        void RemoveDesktop(IVirtualDesktop desktop, IVirtualDesktop fallback);
        IVirtualDesktop FindDesktop(ref Guid desktopid);
        void Unknown1(IVirtualDesktop desktop, out IntPtr unknown1, out IntPtr unknown2);
        void SetName(IVirtualDesktop desktop, [MarshalAs(UnmanagedType.HString)] string name);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("A5CD92FF-29BE-454C-8D04-D82879FB3F1B")]
    internal interface IVirtualDesktopManager
    {
        bool IsWindowOnCurrentVirtualDesktop(IntPtr topLevelWindow);
        Guid GetWindowDesktopId(IntPtr topLevelWindow);
        void MoveWindowToDesktop(IntPtr topLevelWindow, ref Guid desktopId);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("4CE81583-1E4C-4632-A621-07A53543148F")]
    internal interface IVirtualDesktopPinnedApps
    {
        bool IsAppIdPinned(string appId);
        void PinAppID(string appId);
        void UnpinAppID(string appId);
        bool IsViewPinned(IApplicationView applicationView);
        void PinView(IApplicationView applicationView);
        void UnpinView(IApplicationView applicationView);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("92CA9DCD-5622-4BBA-A805-5E9F541BD8C9")]
    internal interface IObjectArray
    {
        void GetCount(out int count);
        void GetAt(int index, ref Guid iid, [MarshalAs(UnmanagedType.Interface)]out object obj);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
    internal interface IServiceProvider10
    {
        [return: MarshalAs(UnmanagedType.IUnknown)]
        object QueryService(ref Guid service, ref Guid riid);
    }
    #endregion
    // ************************************************************************************************************ //



    // added 2022-09-19 //
    // uses AutoHotKey.dll , to run a one-time .ahk script when program starts in order to RE-MAP
    // the default windows shortcuts of  CTRL+WIN+RIGHT/LEFT , for desktop transitions 
    // to use msvd.exe (credits/license: https://github.com/MScholtes/VirtualDesktop )
    // so that desktop wrapping is possible when moving from last to first or first to last 
    // ** An EXTRA benefit is that TOUCH-PAD gestures of 4-fingers + right/left is ALSO overriden !!!
    // ** but a slight disadvantage is that default msvd.exe used here, only works for Windows 10 1809 to 21H2
    // >>>> to overcome this , a folder of msvd_bins is provided with all possible win10/win11 versions of msvd.exe
    //  check msvd_bins/README.txt for further details 
    static class AHK
    {
        // use: CTRL+ALT+SHIFT+S ; TO SUSPEND/UNSUSPEND the script ! // at runtime !! // 
        public static void ReMap_DefaultWinCTRL_RightLeft()
        {
            string script = "#NoTrayIcon\n"
                + "; Below line re-maps WIN+CTRL+LEFT-ARROW\n"
                + "#^Left::Run, msvd.exe /Quiet /Wrap /Left , , Hide\n"
                + "return\n"
                + "; Below line re-maps WIN+CTRL+RIGHT-ARROW\n"
                + "#^Right::Run, msvd.exe /Quiet /Wrap /Right , , Hide\n"
                + "return\n"
                + "^!+s::Suspend\n" // CTRL+ALT+SHIFT+S >>> TO SUSPEND/UNSUSPEND the script ! // 2022-09-25
                + "return";

            ahktextdll(script);
        }

        [DllImport("AutoHotkey.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ahkdll")]
        private static extern int ahkdll([MarshalAs(UnmanagedType.LPWStr)] string scriptFilePath,
            [MarshalAs(UnmanagedType.LPWStr)] string parameters = "",
            [MarshalAs(UnmanagedType.LPWStr)] string title = "");

        [DllImport("AutoHotkey.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ahktextdll")]
        private static extern int ahktextdll([MarshalAs(UnmanagedType.LPWStr)] string script,
            [MarshalAs(UnmanagedType.LPWStr)] string parameters = "",
            [MarshalAs(UnmanagedType.LPWStr)] string title = "");

    }
    // ************************************************************************************************************ //

    // added 2022-09-26:  to collect all SaveTo methods (& related) in one class //
    static class SaveTo
    {
        // saves to a TEXT file  // source: https://stackoverflow.com/a/14887022 // 
        public static void SaveToFile(string txt, string filename)
        {
            var saveFileDialog1 = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = string.Format("{0}Text files (*.txt)|*.txt|All files (*.*)|*.*", ""),
                RestoreDirectory = true,
                ShowHelp = false,
                CheckFileExists = false,
                FileName = filename
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                File.WriteAllText(saveFileDialog1.FileName, txt);
        }

        public static void SaveToBatchFile(string content, string filename)
        {
            var saveFileDialog1 = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = string.Format("{0}Batch file (*.bat)|*.bat|All files (*.*)|*.*", ""),
                RestoreDirectory = true,
                ShowHelp = false,
                CheckFileExists = false,
                FileName = filename
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                File.WriteAllText(saveFileDialog1.FileName, content, System.Text.Encoding.UTF8);
        }

        public static void SaveToHTML(string content, string filename)
        {
            var saveFileDialog1 = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = string.Format("{0}HTML file (*.html)|*.html|All files (*.*)|*.*", ""),
                RestoreDirectory = true,
                ShowHelp = false,
                CheckFileExists = false,
                FileName = filename
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                File.WriteAllText(saveFileDialog1.FileName, content, System.Text.Encoding.UTF8);
        }

        public static void FastSave(string txt_data, string batch, string html, string path)
        {
            File.WriteAllText(path + "\\" + "desktops" + ".txt", txt_data);
            File.WriteAllText(path + "\\" + "folders" + ".bat", batch, Encoding.UTF8);
            File.WriteAllText(path + "\\" + "urls" + ".html", html, Encoding.UTF8);
        }
    }

    // added 2022-09-26:  to collect most short helper methods used in Form1.cs in one class //
    static class Helper
    {
        public static string getFormattedDateTime()  // added 2022-09-20
        {
            DateTime now = DateTime.Now;
            string year = now.ToString("yyyy"), month = now.ToString("MM"), day = now.ToString("dd");
            string hour = now.ToString("HH"), minute = now.ToString("mm");
            return year + pad0(month) + pad0(day) + "-" + pad0(hour) + pad0(minute);
        }

        private static string pad0(String input) => (input.Length < 2 ? "0" : "") + input;  // added 2022-09-20

        // added  2022-09-25
        public static string shortenText(string input, int MAX_LENGTH) => input.Length > MAX_LENGTH ? (input.Substring(0, MAX_LENGTH) + "...") : input;
    }

    // ************************************************************************************************************ //


    // added 2022-10-03
    // a helper class to get taskbar coordinates {to use it with mouse-hook for mouse-wheel up/down, to toggle desktops next/prev.}
    // code credits to:  https://stackoverflow.com/questions/29330440/get-precise-location-and-size-of-taskbar
    public static class TaskbarHelper
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        public static RECT Coordinates  // used once, on form1_load 
        {
            get
            {
                IntPtr TaskBarHandle = FindWindow("Shell_traywnd", "");
                GetWindowRect(TaskBarHandle, out RECT rct);
                return rct;
            }
        }

        // added 2022-10-05 // not used in releases; for personal use; depends on each user's desktop/taskbar layout // 
        public static RECT getCoordinates_Trimmed(int percentRight, int percentLeft)  // used once, on form1_load 
        {
            var r = Coordinates;
            r.Left = r.Left + percentLeft * r.Right / 100;
            r.Right = r.Right * (100 - percentRight) / 100;
            return r;
        }



        #region EVEN MORE Failed attempts to fix window-focus issue - 2022-10-07 - v2.4.2.11

        // added 2022-10-07 // trying to solve window-focus issue; more failed approaches :-) // 

        private static HWND taskbarHandle;
        private static IUIAutomationTreeWalker tw;
        private static IUIAutomationElement appBar;


        public static void initTaskbarHandle() // used once ,  on form1_load 
        {
            taskbarHandle = FindWindow("Shell_TrayWnd", "");
            var uiaClassObject = new CUIAutomation();
            IUIAutomationElement root = uiaClassObject.ElementFromHandle(taskbarHandle);
            appBar = root.FindFirst(TreeScope.TreeScope_Descendants, 
                uiaClassObject.CreatePropertyCondition(30012, "MSTaskListWClass")); // UIA_ClassNamePropertyId: 30012
            tw = uiaClassObject.CreateTreeWalker(uiaClassObject.CreatePropertyCondition(30010, true)); // // UIA_IsEnabledPropertyId: 30010 
        }

        public static void focusOnTaskbar() // used with next/prev procedures 
        {
            if (taskbarHandle != null && taskbarHandle != IntPtr.Zero)
            {
                // failed approach ... # 1
                //SetForegroundWindow(taskbarHandle);
                //ShowWindow(taskbarHandle, 5);
                // - - - - -  - - - - -  - - - - -  - - - - -  - - - - - //
                // failed approach ... # 2
                // UIAutomation approach; to get last-app coordinates in appBar -> to simulate click just 1 pixel to its right (empty area) //
                IUIAutomationElement lastApp = tw.GetLastChildElement(appBar);
                tagRECT rect = lastApp.CurrentBoundingRectangle;
                simulateMouseClick(rect.right + 1, (rect.top + rect.bottom) / 2); // assuming horizontally positioned taskbar [ very big assumption; needs adjusting later !! ] //
            }
        }


        // LEFT-click on position x,y // source: https://www.codegrepper.com/code-examples/csharp/c%23+how+to+simulate+mouse+click //
        private static void simulateMouseClick(int x, int y)
        {
            // SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        /*
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow); // nCmdShow: SW_HIDE=0, SW_SHOW=5  ... //
        */

        #endregion
    }
}