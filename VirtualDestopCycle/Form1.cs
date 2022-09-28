using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Runtime.InteropServices;
using HWND = System.IntPtr;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using InputBoxClassLibrary;
using WindowsDesktop;
using GlobalHotKey;
using System.Drawing.Imaging;

namespace VirtualDesktopManager
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", ExactSpelling = true)]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private IList<VirtualDesktop> desktops;
        private IntPtr[] activePrograms;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        private readonly HotKeyManager _rightHotkey;
        private readonly HotKeyManager _leftHotkey;
        private readonly HotKeyManager _numberHotkey;
        private readonly HotKeyManager _PANIC_Hotkey;  // added  2022-09-28
        private readonly HotKeyManager _desktopsList_Hotkey;  // added  2022-09-28
        private readonly HotKeyManager _HotkeysList_Hotkey;  // added  2022-09-28


        private bool closeToTray;
        private bool useAltKeySettings;


        // added 2022-02-19 //
        // two global variables to store current back-color & brush names // 
        private string selectedBackgroundName = Consts.DEFAULT_COLOR;
        private string selectedBrushName = Consts.DEFAULT_BRUSH;


        // added 2022-02-26 //
        // another two global variables to store cycles_amount & cycles_transition_time // 
        private int selectedCycleTransTime = Consts.DEFAULT_TRANS_TIME;
        private int selectedCyclesAmount = Consts.DEFAULT_CYCLES_AMOUNT;
        // flag used when cycling forever , and to stop it ! //
        private bool cycle_forever_flag = false;
        // timer: used when cycling FOREVER {thread.sleep causes issues with keypress-event-handling !} //
        private System.Timers.Timer timer = new System.Timers.Timer(Consts.DEFAULT_TRANS_TIME * 1000);
        // hotkeyManager: used when registering/unregistering combo for stopping cycling FOREVER //
        private HotKeyManager _stopCyclingHotKey = new HotKeyManager();
        // user preferences class to load/save preferences //
        UserPreferences up = new UserPreferences();


        // addded 2022-07-20 //  for the desktop# & title splash //
        DesktopPopUp dpp = new DesktopPopUp();
        private bool splashActive = false; // default value 

        public Form1()
        {
            InitializeComponent();

            handleChangedNumber();

            closeToTray = true;

            _rightHotkey = new HotKeyManager();
            _rightHotkey.KeyPressed += RightKeyManagerPressed;
            _leftHotkey = new HotKeyManager();
            _leftHotkey.KeyPressed += LeftKeyManagerPressed;
            _numberHotkey = new HotKeyManager();
            _numberHotkey.KeyPressed += NumberHotkeyPressed;

            _PANIC_Hotkey = new HotKeyManager();                // added  2022-09-28
            _PANIC_Hotkey.KeyPressed += PANIC_Hotkey_Pressed;  // added  2022-09-28
            _desktopsList_Hotkey = new HotKeyManager();                // added  2022-09-28
            _desktopsList_Hotkey.KeyPressed += desktopsList_Hotkey_Pressed;  // added  2022-09-28
            _HotkeysList_Hotkey = new HotKeyManager();                // added  2022-09-28
            _HotkeysList_Hotkey.KeyPressed += HotkeysList_Hotkey_Pressed;  // added  2022-09-28


            VirtualDesktop.CurrentChanged += VirtualDesktop_CurrentChanged;
            VirtualDesktop.Created += VirtualDesktop_Added;
            VirtualDesktop.Destroyed += VirtualDesktop_Destroyed;

            this.FormClosing += Form1_FormClosing;

            useAltKeySettings = Properties.Settings.Default.AltHotKey;
            checkBox1.Checked = useAltKeySettings;

            listView1.Items.Clear();
            listView1.Columns.Add("File").Width = 400;
            foreach (var file in Properties.Settings.Default.DesktopBackgroundFiles)
            {
                listView1.Items.Add(NewListViewItem(file));
            }

            // added 2022-02-26 //
            loadUserPreferences();
        }



        // ************************************* //  section added 2022-09-28  // *************************************//

        // below method was added to make more logical behavior for Ctrl+Alt+Shift+L  ! //
        private void DesktopsList_DropDownClosed(object sender, EventArgs e) // added  2022-09-28
        {
            isDesktopsListShown = false; 
            for (int i = 0; i < contextMenuStrip1.Items.Count; i++) // to restore visibility, if submenu is closed in any way !
                contextMenuStrip1.Items[i].Visible = true;
        }

        static bool isDesktopsListShown = false;
        private void desktopsList_Hotkey_Pressed(object sender, KeyPressedEventArgs e) // added  2022-09-28
        {
            // NOTE: this code still has ISSUES!
            // sometimes when changing desktops & menu is shown, it loses focus permanently 
            // so even when hiding or showing again using hotkey , still cannot use keyboard arrows to select
            // in the meantime it should be used simply by showing menu and choosing an item , or hiding it 
            // and NOT changing desktops while its shown

            if (!isDesktopsListShown)
            {
                updateContextMenuStrip(); // to simulate what happens on mouse-right-click event 

                // hide All main-menu items except the last one ("DESKTOPS")
                for (int i = 0; i < contextMenuStrip1.Items.Count - 1; i++)
                    contextMenuStrip1.Items[i].Visible = false;

                int width = Screen.AllScreens[0].Bounds.Width;
                int height = Screen.AllScreens[0].Bounds.Height;
                int x = (width / 2) - (width*15 / 100); // i.e. center point - 15% of width to the left
                int y = height/2;
                contextMenuStrip1.Show(new Point(x, y));
                desktopsList.ShowDropDown();
                desktopsList.DropDown.Select();
                desktopsList.DropDown.Focus();
            }
            else // is shown, then hide it 
            {
                contextMenuStrip1.Hide();
                for (int i = 0; i < contextMenuStrip1.Items.Count; i++) // make all items visible again
                    contextMenuStrip1.Items[i].Visible = true;
            }
            isDesktopsListShown = !isDesktopsListShown;
        }

        private void PANIC_Hotkey_Pressed(object sender, KeyPressedEventArgs e) =>  panic.PerformClick();  // added  2022-09-28

        private void HotkeysList_Hotkey_Pressed(object sender, KeyPressedEventArgs e)  // added  2022-09-28
        {
            MessageBox.Show(Consts.HotkeysList_MSG, Consts.HotkeysList_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // *****************************************************************************************************//



        // ************************************* // user preferences load/save section // *************************************//

        private void SaveUserPref_Click(object sender, EventArgs e)
        {
            if (saveUserPreferences())
            {
                MessageBox.Show("Current user preferences were saved successfully", "Preferences Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void loadUserPreferences()  // added 2022-02-26 //
        {
            // load from xml to class //
            XmlSerializer mySerializer = new XmlSerializer(typeof(UserPreferences));
            FileStream myFileStream;
            try
            {
                myFileStream = new FileStream("./up.xml", FileMode.Open);
            }
            catch (Exception err)
            {
                return;  // loads nothing , just uses default values ! //
            }
            up = (UserPreferences)mySerializer.Deserialize(myFileStream);

            // update locals from class // 
            // only if values are valid; ELSE: set up defaults! // changed: 2022-02-28
            if (Consts.isValidColorAndBrush(up.BackColor, up.BrushName))
            {
                selectedBackgroundName = up.BackColor;
                selectedBrushName = up.BrushName;
            }
            else
            {
                selectedBackgroundName = Consts.DEFAULT_COLOR;
                selectedBrushName = Consts.DEFAULT_BRUSH;
            }

            selectedCyclesAmount = Consts.isValidCyclesAmount(up.cyclesAmount) ?
                int.Parse(up.cyclesAmount) : Consts.DEFAULT_CYCLES_AMOUNT;
            selectedCycleTransTime = Consts.isValidTransTime(up.cycleTransTime) ?
                int.Parse(up.cycleTransTime) : Consts.DEFAULT_TRANS_TIME;
        }

        private bool saveUserPreferences()  // added 2022-02-26 //
        {
            // update class fields from locals //
            up.BackColor = selectedBackgroundName;
            up.BrushName = selectedBrushName;
            up.cyclesAmount = "" + selectedCyclesAmount;
            up.cycleTransTime = "" + selectedCycleTransTime;

            // save class to XML //
            XmlSerializer mySerializer = new XmlSerializer(typeof(UserPreferences));
            StreamWriter myWriter;
            try
            {
                myWriter = new StreamWriter("./up.xml");
            }
            catch (Exception err)
            {
                return false; // nothing is saved ! //
            }
            mySerializer.Serialize(myWriter, up);
            myWriter.Close();
            return true;
        }

        // ***************************** // END of user preferences load/save section // *************************************//


        private void NumberHotkeyPressed(object sender, KeyPressedEventArgs e)
        {
            var index = (int)e.HotKey.Key - (int)Key.D0 - 1;
            var currentDesktopIndex = getCurrentDesktopIndex();
            if ((index == currentDesktopIndex) || (index > desktops.Count - 1)) return;
            desktops.ElementAt(index)?.Switch();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closeToTray)
            {
                e.Cancel = true;
                this.Visible = false;
                this.ShowInTaskbar = false;
                notifyIcon1.BalloonTipTitle = "Still Running...";
                notifyIcon1.BalloonTipText = "Right-click on the tray icon to exit.";
                notifyIcon1.ShowBalloonTip(2000);
            }
        }

        private void handleChangedNumber()
        {
            desktops = VirtualDesktop.GetDesktops();
            activePrograms = new IntPtr[desktops.Count];
        }

        private void VirtualDesktop_Added(object sender, VirtualDesktop e) => handleChangedNumber();
        private void VirtualDesktop_Destroyed(object sender, VirtualDesktopDestroyEventArgs e) => handleChangedNumber();

        private void VirtualDesktop_CurrentChanged(object sender, VirtualDesktopChangedEventArgs e)
        {
            // 0 == first
            int currentDesktopIndex = getCurrentDesktopIndex();
            string pictureFile = PickNthFile(currentDesktopIndex);
            if (pictureFile != null) Native.SetBackground(pictureFile);
            restoreApplicationFocus(currentDesktopIndex);
            changeTrayIcon(currentDesktopIndex);
            // added 2022-07-19
            if (splashActive) showSplash();
        }

        // added 2022-07-19
        private void showSplash()
        {
            int i = getCurrentDesktopIndex();
            string msg = "Desktop # " + (i + 1) + desktopNameOrEmpty(i, "\r\n", "");
            dpp.showMe(msg);
        }

        private string PickNthFile(int currentDesktopIndex)
        {
            int n = Properties.Settings.Default.DesktopBackgroundFiles.Count;
            if (n == 0) return null;
            int index = currentDesktopIndex % n;
            return Properties.Settings.Default.DesktopBackgroundFiles[index];
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _rightHotkey.Dispose();
            _leftHotkey.Dispose();
            _numberHotkey.Dispose();

            _PANIC_Hotkey.Dispose();  // added  2022-09-28
            _desktopsList_Hotkey.Dispose();  // added  2022-09-28
            _HotkeysList_Hotkey.Dispose();  // added  2022-09-28


            _stopCyclingHotKey.Dispose(); // added 2022-02-26
            timer.Dispose();  // added 2022-02-26

            saveUserPreferences(); // added 2022-02-26 //

            closeToTray = false;

            this.Close();
        }

        private void normalHotkeys()
        {
            try
            {
                _rightHotkey.Register(Key.Right, Consts.CTRL_ALT);
                _leftHotkey.Register(Key.Left, Consts.CTRL_ALT);
                RegisterNumberHotkeys(Consts.CTRL_ALT);
                Register_PANIC_Hotkey(); // added  2022-09-28
                Register_desktopsList_Hotkey(); // added 2022-09-28
                Register_HotkeysList_Hotkey(); // added 2022-09-28
            }
            catch (Exception err) // catching the error solves the problem and SETS the HotKeys as requested ! //
                                  // error THROWN without need ! ... the problem is with the error thrown ! // 
            {
                // notifyIcon1.BalloonTipTitle = "Error setting hotkeys";
                // notifyIcon1.BalloonTipText = "Could not set hotkeys. Please open settings and try the alternate combination.";
                // notifyIcon1.ShowBalloonTip(2000);
            }
        }

        private void alternateHotkeys()
        {
            try
            {
                _rightHotkey.Register(Key.Right, Consts.ALT_SHIFT);
                _leftHotkey.Register(Key.Left, Consts.ALT_SHIFT);
                RegisterNumberHotkeys(Consts.ALT_SHIFT);
                Register_PANIC_Hotkey(); // added  2022-09-28
                Register_desktopsList_Hotkey(); // added 2022-09-28
                Register_HotkeysList_Hotkey(); // added 2022-09-28
            }
            catch (Exception err) // catching the error solves the problem and SETS the HotKeys as requested ! //
                                  // error THROWN without need ! ... the problem is with the error thrown ! // 
            {
                // notifyIcon1.BalloonTipTitle = "Error setting hotkeys";
                // notifyIcon1.BalloonTipText = "Could not set hotkeys. Please open settings and try the default combination.";
                // notifyIcon1.ShowBalloonTip(2000);
            }
        }

        private void RegisterNumberHotkeys(ModifierKeys modifiers)
        {
            for (var i = Key.D1; i <= Key.D9; i++) _numberHotkey.Register(i, modifiers);
        }


        private void Register_PANIC_Hotkey() => _PANIC_Hotkey.Register(Key.P, Consts.CTRL_ALT_SHIFT);   // added  2022-09-28
        private void Register_desktopsList_Hotkey() => _desktopsList_Hotkey.Register(Key.L, Consts.CTRL_ALT_SHIFT);   // added  2022-09-28
        private void Register_HotkeysList_Hotkey() => _HotkeysList_Hotkey.Register(Key.H, Consts.CTRL_ALT_SHIFT);   // added  2022-09-28



        private void Form1_Load(object sender, EventArgs e)
        {
            labelStatus.Text = "";

            if (!useAltKeySettings) normalHotkeys();
            else alternateHotkeys();

            var desktop = initialDesktopState();
            changeTrayIcon();

            this.Visible = false;

            // added 2022-09-20
            AHK.ReMap_DefaultWinCTRL_RightLeft();
        }

        private int getCurrentDesktopIndex() => desktops.IndexOf(VirtualDesktop.Current);

        private void saveApplicationFocus(int currentDesktopIndex = -1)
        {
            IntPtr activeAppWindow = GetForegroundWindow();
            if (currentDesktopIndex == -1)
                currentDesktopIndex = getCurrentDesktopIndex();
            activePrograms[currentDesktopIndex] = activeAppWindow;
        }

        private void restoreApplicationFocus(int currentDesktopIndex = -1)
        {
            if (currentDesktopIndex == -1)
                currentDesktopIndex = getCurrentDesktopIndex();

            if (activePrograms[currentDesktopIndex] != null && activePrograms[currentDesktopIndex] != IntPtr.Zero)
            {
                SetForegroundWindow(activePrograms[currentDesktopIndex]);
            }
        }

        private void changeTrayIcon(int currentDesktopIndex = -1)
        {
            if (currentDesktopIndex == -1)
                currentDesktopIndex = getCurrentDesktopIndex();

            var desktopNumber = currentDesktopIndex + 1;
            var desktopNumberString = desktopNumber.ToString();

            var fontSize = 250;
            var xPlacement = 5;
            var yPlacement = -40;

            if (desktopNumber > 9)
            {
                desktopNumberString = "+";
            }

            Bitmap newIcon = getCurrentBackground() ?? Properties.Resources.back_Dark_Blue; // changed @ 2022-02-19
            Font desktopNumberFont = new Font("Segoe UI", fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            var gr = Graphics.FromImage(newIcon);

            gr.DrawString(desktopNumberString, desktopNumberFont, getCurrentBrush() ?? Brushes.White, xPlacement, yPlacement); // changed @ 2022-02-19

            Icon numberedIcon = Icon.FromHandle(newIcon.GetHicon());
            notifyIcon1.Icon = numberedIcon;

            DestroyIcon(numberedIcon.Handle);
            desktopNumberFont.Dispose();
            newIcon.Dispose();
            gr.Dispose();
        }

        VirtualDesktop initialDesktopState()
        {
            var desktop = VirtualDesktop.Current;
            int desktopIndex = getCurrentDesktopIndex();
            saveApplicationFocus(desktopIndex);
            return desktop;
        }

        void RightKeyManagerPressed(object sender, KeyPressedEventArgs e) => moveToNext(initialDesktopState());
        void LeftKeyManagerPressed(object sender, KeyPressedEventArgs e) => moveToPrevious(initialDesktopState());
        void RightKeyManagerPressed_WinDefault(object sender, KeyPressedEventArgs e) => moveToNext(initialDesktopState());
        void LeftKeyManagerPressed_WinDefault(object sender, KeyPressedEventArgs e) => moveToPrevious(initialDesktopState());

        private void openSettings()
        {
            this.Visible = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e) => openSettings();

        // removed functionality on 2022-02-18, for it not to conflict with 
        // single left click right desktop switch added at the end of this file !
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            //openSettings(); 
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    ListViewItem selected = listView1.SelectedItems[0];
                    int indx = selected.Index;
                    int totl = listView1.Items.Count;

                    if (indx == 0)
                    {
                        listView1.Items.Remove(selected);
                        listView1.Items.Insert(totl - 1, selected);
                    }
                    else
                    {
                        listView1.Items.Remove(selected);
                        listView1.Items.Insert(indx - 1, selected);
                    }
                }
                else
                {
                    MessageBox.Show("You can only move one item at a time. Please select only one item and try again.",
                        "Item Select", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    ListViewItem selected = listView1.SelectedItems[0];
                    int indx = selected.Index;
                    int totl = listView1.Items.Count;

                    if (indx == totl - 1)
                    {
                        listView1.Items.Remove(selected);
                        listView1.Items.Insert(0, selected);
                    }
                    else
                    {
                        listView1.Items.Remove(selected);
                        listView1.Items.Insert(indx + 1, selected);
                    }
                }
                else
                {
                    MessageBox.Show("You can only move one item at a time. Please select only one item and try again.",
                        "Item Select", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            _rightHotkey.Unregister(Key.Right, Consts.CTRL_ALT);
            _leftHotkey.Unregister(Key.Left, Consts.CTRL_ALT);
            _rightHotkey.Unregister(Key.Right, Consts.ALT_SHIFT);
            _leftHotkey.Unregister(Key.Left, Consts.ALT_SHIFT);

            if (checkBox1.Checked)
            {
                alternateHotkeys();
                Properties.Settings.Default.AltHotKey = true;
            }
            else
            {
                normalHotkeys();
                Properties.Settings.Default.AltHotKey = false;
            }

            Properties.Settings.Default.DesktopBackgroundFiles.Clear();
            foreach (ListViewItem item in listView1.Items)
            {
                Properties.Settings.Default.DesktopBackgroundFiles.Add(item.Tag.ToString());
            }

            Properties.Settings.Default.Save();
            labelStatus.Text = "Changes were successful.";
        }

        private void addFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog1.Multiselect = true;
            openFileDialog1.Title = "Select desktop background image";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                foreach (string file in openFileDialog1.FileNames)
                {
                    listView1.Items.Add(NewListViewItem(file));
                }
            }
        }

        private static ListViewItem NewListViewItem(string file)
        {
            return new ListViewItem()
            {
                Text = Path.GetFileName(file),
                ToolTipText = file,
                Name = Path.GetFileName(file),
                Tag = file
            };
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    ListViewItem selected = listView1.SelectedItems[0];
                    listView1.Items.Remove(selected);
                }
            }
            catch (Exception ex)
            {
            }
        }

        // added on 2022-02-18 , on single-left mouse click >> move to the next desktop
        // if at the end then cycle back to the first one || SHIFT keypress + Left-mouse click >>  to previous desktop 
        private void notifyIcon1_Click(object sender, MouseEventArgs me)
        {
            if (me.Button == MouseButtons.Left)
            {
                var desktop = initialDesktopState();
                if (Control.ModifierKeys == Keys.Shift) moveToPrevious(desktop);
                else moveToNext(desktop);
            }

            if (me.Button == MouseButtons.Right)
            {
                updateContextMenuStrip();  // added on 2022-02-18 // to update desktops # list // 
                                           // changed on 2022-02-26 // to update all sub-lists //
            }
        }


        private void OpenFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        // added on 2022-02-18
        private void nextClick(object sender, EventArgs e) => moveToNext(initialDesktopState());
        private void previousClick(object sender, EventArgs e) => moveToPrevious(initialDesktopState());
        // moves to previous desktop , cycles to last if on first one ...
        private void moveToPrevious(VirtualDesktop desktop) => (desktop.GetLeft() ?? desktops.Last())?.Switch();
        // moves to next desktop , cycles to first if on last one ...
        private void moveToNext(VirtualDesktop desktop) => (desktop.GetRight() ?? desktops.First())?.Switch();

        // added on 2022-02-18 // changed 2022-02-26
        // updates the all sub-menus ....  // called only when RIGHT_CLICKING tray icon ... 
        private void updateContextMenuStrip()
        {
            updateDesktopsList(); // dynamically re-creates all items each time , with formatting updates // 
            updateColorsList();         // only highlights currently selected item //
            updateCyclesAmountList();   // only highlights currently selected item //
            updateTransitionTimeList(); // only highlights currently selected item //
        }

        private void updateDesktopsList()
        {
            desktopsList.DropDownItems.Clear(); // clear Desktops-sub-list 
            var TOTAL_DESKTOPS = desktops.Count;

            // CLOSE:   delete current desktop and move to previous (or next if deleting the 1st) // added 2022-03-01 
            // CLOSE_ALL:   delete all desktops starting from end, except the 1st one of course ! // added 2022-07-09
            if (TOTAL_DESKTOPS > 1)
            {
                add_CloseAllItem();
                add_CloseItem();
                add_Separator();
            }

            // add all current desktops to the menu  (refresh list)
            // changed @ 2022-03-02 to include desktops' names, beside each # ; only if names are non-generic! // 
            // modified 2022-07-10 // more clear code ... 
            for (var k = 1; k <= TOTAL_DESKTOPS; k++) add_DesktopItem(k);

            add_Separator();
            add_AddItem();// added 2022-02-21 // creates new desktop at the end, and moves to it //
            add_AddMultipleItem(); // added 2022-07-09  // Adds Multiple Desktops, as much as user requests [from 1 to 10] // 
        }

        // added 2022-07-10 // code-restructuring // more clear code ...
        private void add_DesktopItem(int k)
        {
            ToolStripItem item = desktopsList.DropDownItems.Add("# " + k);
            item.Text += desktopNameOrEmpty(k - 1, ":  ", ""); // added: 2022-03-02
            // item.Click += // >>  only gets a click , cannot differentiate betweeen right & left << // 
            item.MouseDown += handleDesktopNumberClick; // changed: 2022-03-02 , to add a change-desktop-name on RIGHT-CLICK feature !
            item.Click += desktopNumberEnterPress; // added 2022-09-28 , along with Ctrl+Alt+Shift+L // only 'Click' handles Enter-Key-Press in dropdown! //
            item.ToolTipText = "Right Click to RENAME Desktop"; // added: 2022-03-02
            if ((k - 1) == getCurrentDesktopIndex())
            {
                item.Font = new Font(item.Font, FontStyle.Bold);
                item.ForeColor = Color.White;
                item.BackColor = Color.Black;
            }
        }

        // added 2022-07-09
        private void add_Separator() => desktopsList.DropDownItems.Add(new ToolStripSeparator());

        // added 2022-07-10 // special accessory "generic" method for addition of menu-items // to make code more concise & unified //
        private void add_special_menu_Item(string name, Image icon, EventHandler action, string tooltip)
        {
            ToolStripItem item = desktopsList.DropDownItems.Add(name);
            item.Image = icon;
            item.Click += action;
            item.ToolTipText = tooltip;
            // default style values
            item.Font = new Font(item.Font, FontStyle.Bold);
            item.ForeColor = Color.Black;
            item.BackColor = Color.LightGray;
            item.ImageScaling = ToolStripItemImageScaling.SizeToFit;
        }

        // added 2022-07-09 // modified 2022-07-10 : using the above "add_special_menu_item" // 
        private void add_CloseAllItem() => add_special_menu_Item("Close ALL", Properties.Resources.close_all, closeAllDesktops, "Close All Desktops !");
        private void add_CloseItem() => add_special_menu_Item("Close Current", Properties.Resources.close_desktop, closeCurrentAndMove, "Close Current Desktop");
        private void add_AddItem() => add_special_menu_Item("Add", Properties.Resources.add_new, createNewAndMoveTo, "Add New Desktop");
        private void add_AddMultipleItem() => add_special_menu_Item("Add Multiple", Properties.Resources.add_multiple, addMulti, "Add Multiple Desktops ...");

        // event handler for "CLOSE ALL" item in desktops list // added 2022-07-09 //
        // first moves to last desktop , then starts doing close one by one ... backwards moving ... 
        private void closeAllDesktops(object sender, EventArgs e)
        {
            string msg = "This action will remove all desktops, and move all windows to the first one!\n\n Are you sure ?!";
            DialogResult res = MessageBox.Show(msg, "Close All Desktops !!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            String nameOfFirstDesktop = desktopNameFromIndex(0); // to handle issue of a changed name, when removing multiple desktops !
            if (res == DialogResult.OK)
            {
                desktops.Last().Switch();
                for (int i = desktops.Count; i > 1; i--)
                    VirtualDesktop.Current.Remove();
            }
            Thread.Sleep(2000); // <-- fixes some issue with the ChangeDesktopName() function below: could be a threading conflict 
            // with the remove function above and changing desktop name while removing others (non-blocking methods)
            changeDesktopName(0, nameOfFirstDesktop);
        }

        // event handler for "close" item in desktops list // added 2022-03-01 //
        private void closeCurrentAndMove(object sender, EventArgs e)
        {
            int i = getCurrentDesktopIndex();
            var current = VirtualDesktop.Current;
            string msg = "This action will remove the current desktop # " + (i + 1) + " and move all of its windows ";
            msg += "to the " + (i == 0 ? "next" : "previous") + " desktop.\n\nAre you sure ?!";
            DialogResult res = MessageBox.Show(msg, "Remove Current Desktop!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (res == DialogResult.OK) VirtualDesktop.Current.Remove(i == 0 ? current.GetRight() : current.GetLeft());
        }

        // event handler for ToolStripItem (desktops) CLICK [MOUSE-DOWN!!]  // choose what to do based on LEFT/RIGHT click ... // 
        private void handleDesktopNumberClick(object sender, EventArgs e)
        {
            desktopNumber_MouseDown = true; // temporary indicator, added 2022-09-28 , turns false again after full CLICK is complete // 
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left) gotoDesktopNumber(sender, e);
            else if (me.Button == MouseButtons.Right) changeNameInputBox(sender, e);
        }

        // added 2022-09-28 //
        static bool desktopNumber_MouseDown = false; // indicator when mouse_down event comes before "click" event ; to DISABLE click event //
        // so that the below method used only when ENTER-KEY press on dropdown menu // MouseDown event handles the mouse RIGHT/LEFT clicks // 
        private void desktopNumberEnterPress(object sender, EventArgs e)
        {
            if (desktopNumber_MouseDown) desktopNumber_MouseDown = false;
            else
            {
                contextMenuStrip1.Hide(); // to lose focus; so that desktop transition works smoothly !
                Thread.Sleep(250); // wait a little-bit to give time for menu to HIDE properly // fixes "focus" errors which affect desktop switch ! //
                gotoDesktopNumber(sender, e);
            }
        }

        // event handler for ToolStripItem (desktops) LEFT-MOUSE-CLICK ... // ** NOTE: DEPENDS on each item's TEXT-string ** // 
        private void gotoDesktopNumber(object sender, EventArgs e)
        {
            string txt = ((ToolStripItem)sender).Text;
            int i = txt.IndexOf(":");
            if (i > 0) txt = txt.Substring(0, i); // added: 2022-03-02 , to accomodate for adding desktop-names // 
            int num = int.Parse(txt.Substring(txt.IndexOf(" ") + 1));
            desktops.ElementAt(num - 1)?.Switch();
        }

        // event handler for ToolStripItem (desktops) RIGHT-MOUSE-CLICK ... // ** NOTE: DEPENDS on each item's TEXT-string ** // 
        private void changeNameInputBox(object sender, EventArgs e)
        {
            // pre-processing // 
            string txt = ((ToolStripItem)sender).Text;
            int i = txt.IndexOf(":");
            if (i > 0) txt = txt.Substring(0, i); // extract text before ":" (containing desktop number) 
            int num = int.Parse(txt.Substring(txt.IndexOf(" ") + 1));
            // input-box from VisualBasic: InputBoxClassLibrary.dll //
            string previousName = desktopNameOrEmpty(num - 1);
            string msg = "Change Desktop # " + num + " Name:-\n\n";
            msg += "to remove current name: type \"#\" and press OK";
            string newName = InputBox.Show(msg, "CHANGE DESKTOP NAME", previousName);
            bool OK = !(newName.Equals(null));
            newName = newName.Trim();
            if (OK && !newName.Equals("#")) changeDesktopName(num - 1, newName);
            else if (OK) removeDesktopName(num - 1); // back to generic name // 
        }

        // event handler for "Add" item in desktops list  // added 2022-02-21 //
        private void createNewAndMoveTo(object sender, EventArgs e)
        {
            int prevCount = desktops.Count; // desktop count before addition 
            VirtualDesktop.Create(); // apparently it is A-sync function !
            while (desktops.Count == prevCount) Thread.Sleep(500); // until a new desktop is created
            desktops.Last()?.Switch();
        }

        // event handler for "Add Multiple" item in desktops list   // added: 2022-07-09
        private void addMulti(object sender, EventArgs e)
        {
            // input-box from VisualBasic: InputBoxClassLibrary.dll //
            string msg = "Enter number [1 to 10] of how many new desktops to add ...\n\n";
            string numNewDesktops = InputBox.Show(msg, "Add Multiple Desktops !", "0");
            bool OK = !(numNewDesktops.Equals(null));
            int num = 0;
            if (OK && int.TryParse(numNewDesktops, out num))
            {
                if (num >= 1 && num <= 10)
                {
                    int prevCount = desktops.Count; // desktop count before addition 
                    for (int i = 0; i < num; i++)
                    {
                        VirtualDesktop.Create(); // an A-sync function !
                        while (desktops.Count == prevCount) Thread.Sleep(250); // wait until a new desktop is created
                        prevCount = desktops.Count; // update new count for next iteration .. 
                    }
                }
            }
        }

        private void updateColorsList() // added: 2022-02-26
        {
            clearSubMenuFormatting(colorList); // clear all items formatting from before //
            string itemName = color_resource_to_itemname(selectedBackgroundName, selectedBrushName);
            highlightSelectedItem(colorList.DropDownItems[itemName]); // update current selection formatting //
        }

        private string color_resource_to_itemname(string background, string brush) // added: 2022-02-26
        {
            return background.Equals("Transparent") ?
                background + (brush.Equals("white") ? "White" : "Black") :
                background.Replace("back_", "");
        }

        private void updateCyclesAmountList() // added: 2022-02-26
        {
            clearSubMenuFormatting(cyclesAmountSubMenu); // clear all items formatting from before //
            string itemName = ("cycles_" + selectedCyclesAmount).Replace("-1", "forever");
            highlightSelectedItem(cyclesAmountSubMenu.DropDownItems[itemName]); // update current selection formatting //
        }

        private void updateTransitionTimeList() // added: 2022-02-26
        {
            clearSubMenuFormatting(transTimeSubMenu); // clear all items formatting from before //
            string itemName = "sec" + selectedCycleTransTime;
            highlightSelectedItem(transTimeSubMenu.DropDownItems[itemName]); // update current selection formatting //
        }

        // a helper method for the above update... methods ... //
        private void clearSubMenuFormatting(ToolStripMenuItem subMenu) // added: 2022-02-26
        {
            foreach (ToolStripItem item in subMenu.DropDownItems)
            {
                item.BackColor = DefaultBackColor;
                item.ForeColor = DefaultForeColor;
                item.Font = new Font(item.Font, FontStyle.Regular);
            }
        }

        // a helper method for the above update... methods ... //
        private void highlightSelectedItem(ToolStripItem item) // added: 2022-02-26
        {
            item.BackColor = Color.LightGray;
            item.ForeColor = Color.Black;
            item.Font = new Font(item.Font, FontStyle.Bold | FontStyle.Underline | FontStyle.Italic);
        }


        // added 2022-02-19 // to show Desktop # when mouse hovers over tray icon
        // useful for more than 9 desktops (when "+" sign is shown) 
        // modified 2022-03-02 to include desktops' names 
        private void NotifyIcon1_MouseMove(object sender, MouseEventArgs e)
        {
            int i = getCurrentDesktopIndex();
            notifyIcon1.Text = "Virtual Desktop Manager\n\nDesktop # " + (i + 1) + desktopNameOrEmpty(i, "\n\n", "");
            changeTrayIcon(); // to fix icon disappearing on mouse-hover ... simply re-draw it ... 
        }


        // ****** // all of below were added on 2022-02-19 , to customize background colors and brush-colors // ****************//

        // section 1 : dark colors
        private void BlackToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("back_Black", "white");
        private void BrownToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("back_Brown", "white");
        private void DarkBlueToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("back_Dark_Blue", "white");
        private void DarkGreenToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("back_Dark_Green", "white");
        private void DarkRedToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("back_Dark_Red", "white");
        private void PurpleToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("back_Purple", "white");
        // section 2 : bright colors
        private void PinkToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("back_Pink", "black");
        private void RedToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("back_Red", "black");
        private void GreenToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("back_Green", "black");
        private void BlueToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("back_Blue", "black");
        private void YellowToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("back_Yellow", "black");
        private void WhiteToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("back_White", "black");
        // section 3 : transparent
        private void TransparentWhiteTextToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("Transparent", "white");
        private void TransparentBlackTextToolStripMenuItem_Click(object sender, EventArgs e) => updateColor("Transparent", "black");

        // main method to update color // called from the above 14 methods (on-click selections) //
        private void updateColor(string backgroundColorName, string textColorName)
        {
            selectedBackgroundName = backgroundColorName;
            selectedBrushName = textColorName;
            changeTrayIcon(); // update tray icon after changes 
        }

        private Bitmap getCurrentBackground() => (Bitmap)Properties.Resources.ResourceManager.GetObject(selectedBackgroundName);
        private Brush getCurrentBrush() => (Brush)(selectedBrushName.Equals("white") ? Brushes.White : Brushes.Black);


        // ***************** // end of colors / brushes section // ************************************//


        // ************************************* CYCLES / REVERSE CYCLES SECTION  **********************************************//


        // added 2020-02-20 // cycles through all desktops in order, starting from current
        private void CycleMenuItem_Click(object sender, EventArgs e) => cycle(selectedCycleTransTime, selectedCyclesAmount);
        // added 2020-02-20 // cycles through all desktops in reverse-order, starting from current
        private void ReverseCycleMenuItem_Click(object sender, EventArgs e) => revCycle(selectedCycleTransTime, selectedCyclesAmount);


        // cycles the "numOfCycles" full cycles , with haltTimeSec transition time between desktops cycled //
        // if numOfCycles = -1 , cycles 'forever' until 'special' keyPress // 
        private void cycle(int haltTimeSec = 4, int numOfCycles = 1)
        {
            bool splashWasActive = splashActive; splashDeactivate(); // added 2022-07-20 // to accomodate for splashDesktop#

            if (numOfCycles == -1) cycleForever(haltTimeSec);
            else
            {
                int haltTime = haltTimeSec * 1000; // in millisecs
                int total_moves = desktops.Count; // in each single full cycle !
                for (var k = 0; k < numOfCycles; k++)
                {
                    for (var i = 0; i < total_moves; i++)
                    {
                        if ((k == numOfCycles - 1) && (i == total_moves - 1)) break; // if last move of last cycle
                        moveToNext(initialDesktopState());
                        Thread.Sleep(haltTime);
                    }
                }
                moveToNext(initialDesktopState());
            }

            if (splashWasActive) splashActivate(); // added 2022-07-20 // to accomodate for splashDesktop#
        }

        private void cycleForever(int haltTimeSec) // stop on Any-KeyPress
        {
            if (getMsgResult() == DialogResult.OK) // before cycling: msg-box: confirm/STOP cycling //  in getMsgResult()  //
            {
                RegisterStopCycleCombo(); // registering key-combination & eventhandler to Stop cycling when pressed
                timerSetAndStart(haltTimeSec, cycleOrQuit); // "loop" of timer.Start --> moveToNext (unless keypressed) --> continue --> // 
            }
        }

        private void cycleOrQuit(object source, ElapsedEventArgs e)
        {
            if (cycle_forever_flag) moveToNext(initialDesktopState());
            else { timer.Stop(); timer.Elapsed -= cycleOrQuit; }
        }

        // REVERSE cycles the "numOfCycles" full cycles , with haltTimeSec transition time between desktops cycled //
        // if numOfCycles = -1 , reverse-cycles 'forever' until 'special' keyPress // 
        private void revCycle(int haltTimeSec = 4, int numOfCycles = 1)
        {
            bool splashWasActive = splashActive; splashDeactivate(); // added 2022-07-20 // to accomodate for splashDesktop#

            if (numOfCycles == -1) revCycleForever(haltTimeSec);
            else
            {
                int haltTime = haltTimeSec * 1000; // in millisecs
                int total_moves = desktops.Count; // in each single full cycle !
                for (var k = 0; k < numOfCycles; k++)
                {
                    for (var i = 0; i < total_moves; i++)
                    {
                        if ((k == numOfCycles - 1) && (i == total_moves - 1)) break; // if last move of last cycle
                        moveToPrevious(initialDesktopState());
                        Thread.Sleep(haltTime);
                    }
                }
                moveToPrevious(initialDesktopState());
            }

            if (splashWasActive) splashActivate(); // added 2022-07-20 // to accomodate for splashDesktop#
        }

        private void revCycleForever(int haltTimeSec) // stop on Any-KeyPress
        {
            if (getMsgResult(true) == DialogResult.OK) // before cycling: msg-box: confirm/STOP cycling //  in getMsgResult()  //
            {
                RegisterStopCycleCombo(); // registering key-combination & eventhandler to Stop cycling when pressed
                timerSetAndStart(haltTimeSec, reverseCycleOrQuit); // "loop" of timer.Start --> moveToPrevious (unless keypressed) --> continue --> // 
            }
        }

        private void reverseCycleOrQuit(object source, ElapsedEventArgs e)
        {
            if (cycle_forever_flag) moveToPrevious(initialDesktopState());
            else { timer.Stop(); timer.Elapsed -= reverseCycleOrQuit; }
        }

        // a message warning before cycling (or reverse cycling) FOREVER !! // 
        private DialogResult getMsgResult(bool isReverse = false)
        {
            string reverse = isReverse ? " reverse " : " ";
            string msg = "Pressing OK will cause the desktops to" + reverse + "cycle FOREVER after waiting ";
            msg += selectedCycleTransTime + " seconds before start and in between desktop transitions. \n\n";
            msg += "At any moment, press the key-combination: (Ctrl+Alt+S) to STOP cycling.";
            return MessageBox.Show(msg, reverse.ToUpper() + "Cycling FOREVER !!!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        }

        // register & set event-handler ;  for STOP-cycle combination // CTRL+ALT+S // 
        private void RegisterStopCycleCombo()
        {
            try { _stopCyclingHotKey.Register(Key.S, Consts.CTRL_ALT); } // register key-combo
            catch (Exception err) { } // catching the error solves the problem and SETS the HotKeys as requested ! //
            _stopCyclingHotKey.KeyPressed += StopCyclePressed; // add event-handler
        }

        // sends a message to cycleOrQuit (or revCycleOrQuit) eventHandler-forTimer to stop 
        // by updating a global bool variable: cycle_forever_flag
        // also, removes key-combo and event-handler (itself) after being pressed once 
        private void StopCyclePressed(object sender, KeyPressedEventArgs e)
        {
            cycle_forever_flag = false;
            MessageBox.Show("Key-Combination [CTRL+ALT+S] for Stopping was pressed.", "Cycling Stopped!",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            _stopCyclingHotKey.KeyPressed -= StopCyclePressed; // remove event handler
            _stopCyclingHotKey.Unregister(Key.S, Consts.CTRL_ALT); // unregister key-combo
        }

        // sets timer values: cycle or reverse cycle handler , transition time, and other bools // and starts timer 
        private void timerSetAndStart(int timeSeconds, ElapsedEventHandler e)
        {
            cycle_forever_flag = true;
            timer.Interval = timeSeconds * 1000; // converts to millisecs
            timer.Elapsed += e;
            timer.AutoReset = true;
            timer.Start();
        }

        private void Cycles_1_Click(object sender, EventArgs e) => updateCyclesAmount(1);
        private void Cycles_2_Click(object sender, EventArgs e) => updateCyclesAmount(2);
        private void Cycles_3_Click(object sender, EventArgs e) => updateCyclesAmount(3);
        private void Cycles_4_Click(object sender, EventArgs e) => updateCyclesAmount(4);
        private void Cycles_5_Click(object sender, EventArgs e) => updateCyclesAmount(5);
        private void Cycles_forever_Click(object sender, EventArgs e) => updateCyclesAmount(-1);

        private void updateCyclesAmount(int amount) { if (amount == -1 || (amount >= 1 && amount <= 5)) selectedCyclesAmount = amount; }

        private void Sec2_ToolStripMenuItem_Click(object sender, EventArgs e) => updateCyclesTransTime(2);
        private void Sec4_ToolStripMenuItem_Click(object sender, EventArgs e) => updateCyclesTransTime(4);
        private void Sec6_ToolStripMenuItem_Click(object sender, EventArgs e) => updateCyclesTransTime(6);
        private void Sec8_ToolStripMenuItem_Click(object sender, EventArgs e) => updateCyclesTransTime(8);
        private void Sec10_ToolStripMenuItem_Click(object sender, EventArgs e) => updateCyclesTransTime(10);
        private void Sec12_ToolStripMenuItem_Click(object sender, EventArgs e) => updateCyclesTransTime(12);
        private void Sec14_ToolStripMenuItem_Click(object sender, EventArgs e) => updateCyclesTransTime(14);

        private void updateCyclesTransTime(int t) { if (t % 2 == 0 && (t >= 2 && t <= 14)) selectedCycleTransTime = t; }

        // ************************************* // end of CYCLES section // ********************************************************//



        // added 2022-03-02 // 
        private void GetIDs_Click(object sender, EventArgs e)
        {
            var TOTAL_DESKTOPS = desktops.Count;
            string msg = "";
            for (var i = 0; i < TOTAL_DESKTOPS; i++)
            {
                msg += (i + 1) + ":  " + desktopNameFromIndex(i) + "\n";
                msg += desktops.ElementAt(i).Id.ToString().ToUpper() + "\n\n";
            }
            string shown_msg = msg + "Copy the above data to clipboard ?!";

            DialogResult res = MessageBox.Show(shown_msg, "Desktops GUIDs", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (res == DialogResult.Yes)
            {
                Clipboard.SetText(msg);
                notifyIcon1.BalloonTipTitle = "Desktops GUIDs";
                notifyIcon1.BalloonTipText = "Desktops data was copied to clipboard successfully!";
                notifyIcon1.ShowBalloonTip(2000);
            }
        }

        // added 2022-03-02
        // return name of desktop from index (index = 0..Count-1) or "Desktop n" if it has no name
        // source: https://github.com/MScholtes/VirtualDesktop/blob/master/VirtualDesktop.cs  , lines: 397-414 (as of 2022-03-02) //
        // how it works: extracts the name from REGISTRY (if exists) , by using GUID of current desktop 
        // modified a bit from the original version to use IDs from currently used VirtualDesktop package 
        // instead of adding DesktopManager interface implementation used in original code
        private string desktopNameFromIndex(int index)
        {
            Guid guid = desktops.ElementAt(index).Id;
            // read desktop name in registry
            string desktopName = null;
            try
            {
                desktopName = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VirtualDesktops\\Desktops\\{" + guid.ToString() + "}", "Name", null);
            }
            catch { }
            // no name found, generate generic name
            if (string.IsNullOrEmpty(desktopName))
            { // create name "Desktop n" (n = number starting with 1)
                desktopName = "Desktop " + (index + 1).ToString();
            }
            return desktopName;
        }

        // added 2022-03-02
        public string desktopNameOrEmpty(int index, string prefix = "", string suffix = "")
        {
            string name = desktopNameFromIndex(index);
            if (name.Equals("Desktop " + (index + 1).ToString())) return ""; // returns empty string if name is GENERIC //
            return prefix + name + suffix;
        }

        // added 2022-03-02
        // changes name of desktop at index [0..Count-1] // changes value in registry !! // 
        private bool changeDesktopName(int index, string name)
        {

            // OLD METHOD , of updating registry values directly .. does NOT update TaskView Names //
            /*
            Guid guid = desktops.ElementAt(index).Id;
            string reg_path = "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VirtualDesktops\\Desktops\\{" + guid.ToString() + "}";
            if (!string.IsNullOrEmpty(name)) // NO change to an empty name ! // 
            {
                try
                {
                    Microsoft.Win32.Registry.SetValue(reg_path, "Name", name);
                }
                catch
                {
                    return false; // some ERROR ... desktop name was not changed ! // 
                }
            }
            return true;
            */
            //*******************************************************************************************//

            if (!string.IsNullOrEmpty(name)) // NO change to an empty name ! //
            {
                return SetName(index, name);
            }
            return false; // no change 
        }


        // added 2022-03-02
        private bool removeDesktopName(int index)
        {
            // OLD METHOD // simply sets Name Value to "" (empty string) // cannot remove it ! // 
            /*
            Guid guid = desktops.ElementAt(index).Id;
            string reg_path = "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VirtualDesktops\\Desktops\\{" + guid.ToString() + "}";
            try
            {
                Microsoft.Win32.Registry.SetValue(reg_path, "Name", "");
            }
            catch
            {
                return false; // some ERROR ... desktop name was not removed (set to "") // 
            }
            return true;
            */
            //*******************************************************************************************//

            return SetName(index, ""); // empty string is removed (registry value removed completely)
        }

        // added 2022-03-02
        // uses "IVirtualDesktopManagerInternal2"  and others ; from [COM API] region below at the end 
        // credits again to:  https://github.com/MScholtes/VirtualDesktop/blob/master/VirtualDesktop.cs
        // implementation similar to lines 260-267  && lines 476-482 (in .cs file above) , with few modifications 
        // * * * * * 
        // if name="" (empty string) , then removes name (i.e. removes registry value "Name" COMPLETELY, not just delete its content)
        public bool SetName(int index, string name)
        {
            IVirtualDesktopManagerInternal2 VDMInternal2;
            var shell = (IServiceProvider10)Activator.CreateInstance(Type.GetTypeFromCLSID(Guids.CLSID_ImmersiveShell));
            try
            {
                VDMInternal2 = (IVirtualDesktopManagerInternal2)shell
                    .QueryService(Guids.CLSID_VirtualDesktopManagerInternal, typeof(IVirtualDesktopManagerInternal2).GUID);
            }
            catch
            {
                VDMInternal2 = null; // CANNOT CHANGE DESKTOP NAME .. not compatible win10 version ; need newer // 
            }

            if (VDMInternal2 != null) // only if interface to set name is present
            {
                VDMInternal2.SetName(VDMInternal2.FindDesktop(desktops.ElementAt(index).Id), name);
                return true;
            }
            return false;
        }

        // ************************************* // section added 2022-07-20 // ********************************************************//

        private void SplashItem_Click(object sender, EventArgs e)
        {
            if (splashActive) splashDeactivate(); else splashActivate();
        }

        private void splashDeactivate()
        {
            splashActive = false;
            splashItem.ForeColor = Color.Black;
            splashItem.BackColor = Color.White;
            splashItem.Text = "Splash";
        }

        private void splashActivate()
        {
            splashActive = true;
            splashItem.ForeColor = Color.White;
            splashItem.BackColor = Color.Black;
            splashItem.Text += " (Active)";
        }

        private void GetWindowsList_Click(object sender, EventArgs e)
        {
            string msg = "List of open windows' [handles] & titles in Desktop #" + (getCurrentDesktopIndex() + 1) + "\n\n";
            Guid currentID = desktops[getCurrentDesktopIndex()].Id;
            IDictionary<HWND, string> windows = OpenWindowGetter.GetOpenWindows();
            int i = 1;
            foreach (KeyValuePair<IntPtr, string> window in windows)
            {
                IntPtr handle = window.Key;
                string title = window.Value;
                VirtualDesktop vdFromHandle = VirtualDesktop.FromHwnd(handle);
                if (vdFromHandle != null && vdFromHandle.Id.Equals(currentID))
                {
                    msg += i + ".  [" + handle + "]:\t" + title + "\n\n";
                    i++;
                }

            }
            if (i == 1)
            {
                MessageBox.Show("No open windows in current desktop!", "List of Open Windows", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string shown_msg = msg + "Copy the above data to clipboard ?!";
                DialogResult res = MessageBox.Show(shown_msg, "List of Open Windows", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (res == DialogResult.Yes)
                {
                    Clipboard.SetText(msg);
                    notifyIcon1.BalloonTipTitle = "Open Windows List";
                    notifyIcon1.BalloonTipText = "Open Windows List was copied to clipboard successfully!";
                    notifyIcon1.ShowBalloonTip(2000);
                }
            }
        }

        // modified at 2022-09-26: to include two more lists of All URLs & All open folders' paths ; grouped by desktop # // 
        // also made method much more concise, using some "generate" methods used first with Panic! // 
        private void ExportDesktopsData_Click(object sender, EventArgs e)
        {
            // list 1: headings of desktop #, GUID +/- title //  
            string[] headings = generate_headings();    
            // list 2: windows list, their titles & handle numbers //
            string[] windowsList = generate_windowsList_from(OpenWindowGetter.GetOpenWindows());
            // list 3: folders list -> their full paths
            string[] foldersList = generate_foldersList_from(OpenWindowGetter.getExplorerAddressList());
            // list 4: URLs list: tab titles & their URLs 
            string[] urlsList = new string[desktops.Count];
            int currentDesktopIndex = getCurrentDesktopIndex(); // save for coming back 
            DialogResult r = MessageBox.Show("Desktops might switch back & forth during processing!\n\n"
                + "& Keep all browsers' windows open & NOT minimized !\n\n"
                + "Supported Browsers: Firefox, Chrome, Edge & I.E.\n\n"
                + " \u25CF press OK to continue\n \u25CF press Cancel to proceed without browsers' URLs extraction\n",
                "URLs Extraction WARNING !", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (r == DialogResult.OK)
            {
                IDictionary<Consts.BROWSER, List<BrowsersURLs.tabUrlObj>> urls = BrowsersURLs.getAllTabURLs(); // <- might switch desktops !
                desktops[currentDesktopIndex].Switch(); // go back to current desktop
                while (getCurrentDesktopIndex() != currentDesktopIndex) Thread.Sleep(200); // to wait for Switch() above to work !
                Thread.Sleep(2000); // wait a little bit more ; just to make sure we are back on desktop before dialog shows !
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - //

                urlsList = generate_urlsList_from(urls);
            }
            // save all the above lists, after combining [grouping by desktop#] //
            SaveTo.SaveToFile(combineLists(headings, windowsList, foldersList, urlsList), "desktops@" + Helper.getFormattedDateTime());
        }

        private static string combineLists(string[] headings, string[] windowsList, string[] foldersList, string[] urlsList)
        {
            string result = "";
            for (var i = 0; i < headings.Length; i++)
            {
                result += headings[i];
                result += String.IsNullOrEmpty(windowsList[i]) ? "" : "\twindows-list:-\n" + windowsList[i];
                result += String.IsNullOrEmpty(foldersList[i]) ? "" : "\tfolders-list:-\n" + foldersList[i];
                result += String.IsNullOrEmpty(urlsList[i]) ? "" : "\tURLs-list:-\n" + urlsList[i];
            }
            return result;
        }


        private void AboutMenuItem_Click(object sender, EventArgs e) => (new About()).ShowDialog();


        // ************************************* // section added 2022-09-25 // ********************************************************//


        private void GetURLs_Click(object sender, EventArgs e)
        {
            int currentDesktopIndex = getCurrentDesktopIndex(); // save for coming back 
            DialogResult r = MessageBox.Show("Desktops might switch back & forth during processing!\n\n"
                + "& Keep all browsers' windows open & NOT minimized !\n\n"
                + "Supported Browsers: Firefox, Chrome, Edge & I.E.",
                "URLs Extraction WARNING !", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (r == DialogResult.OK)
            {
                IDictionary<Consts.BROWSER, List<BrowsersURLs.tabUrlObj>> urls = BrowsersURLs.getAllTabURLs(); // <- might switch desktops !
                desktops[currentDesktopIndex].Switch(); // go back to current desktop
                while (getCurrentDesktopIndex() != currentDesktopIndex) Thread.Sleep(200); // to wait for Switch() above to work !
                Thread.Sleep(2000); // wait a little bit more ; just to make sure we are back on desktop before dialog shows !
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - //
                string msg = "List of All URLs for All Tabs in All Desktops\n\n";
                string msg_Clipboard = msg; // for clipboard copy OUTPUT (with full URLs);
                string[] desktopsURLsOutput = new string[desktops.Count]; // urls array, grouped by desktop's index , for MSGBOX output [shortened URL]
                string[] desktopsURLsOutput_Clipboard = new string[desktops.Count]; // similar to above, but keeps FULL URLs , to copy to clipboard !
                int[] desktopsOutputLengths = new int[desktops.Count]; // for later use, to detect desktops with NO URLs ! //
                for (int i = 0; i < desktops.Count; i++) // add headings for each desktop# list
                {
                    desktopsURLsOutput[i] = "Desktop #" + (i + 1) + desktopNameOrEmpty(i, ":  {", "}") + "\n\n";
                    desktopsURLsOutput_Clipboard[i] = desktopsURLsOutput[i];
                    desktopsOutputLengths[i] = desktopsURLsOutput[i].Length;
                }
                foreach (KeyValuePair<Consts.BROWSER, List<BrowsersURLs.tabUrlObj>> browserLst in urls) // go over EACH list (by browser type)
                {
                    foreach (BrowsersURLs.tabUrlObj tab in browserLst.Value) // inside each list, go over each item (tabURL object)
                    {
                        int tabsDesktopIndex = desktops.IndexOf(VirtualDesktop.FromHwnd(tab.getWindowHandle()));
                        string pre = "\u25CF " + tab.getTitle() + " [";
                        string post = "]\n";
                        desktopsURLsOutput[tabsDesktopIndex] += pre + Helper.shortenText(tab.getURL(), 50) + post;
                        desktopsURLsOutput_Clipboard[tabsDesktopIndex] += pre + tab.getURL() + post;
                    }
                }
                for (int i = 0; i < desktops.Count; i++) // go over desktops again to check for ones WITH NO URLs; And compose msg ! //
                {
                    if (desktopsOutputLengths[i] == desktopsURLsOutput[i].Length)
                    {
                        desktopsURLsOutput[i] = "";
                        desktopsURLsOutput_Clipboard[i] = "";
                    }
                    else
                    {
                        desktopsURLsOutput[i] += "\n";
                        desktopsURLsOutput_Clipboard[i] += "\n";
                    }
                    msg += desktopsURLsOutput[i];
                    msg_Clipboard += desktopsURLsOutput_Clipboard[i];
                }
                string shown_msg = msg + "Copy to clipboard {with full URLs} ?!";

                DialogResult res = MessageBox.Show(new Form { TopMost = true }, shown_msg, "List of All URLs",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (res == DialogResult.Yes)
                {
                    Clipboard.SetText(msg_Clipboard);
                    notifyIcon1.BalloonTipTitle = "URLs List";
                    notifyIcon1.BalloonTipText = "URLs List was copied to clipboard successfully!";
                    notifyIcon1.ShowBalloonTip(2000);
                }
            }
        }

        private void GetFolders_Click(object sender, EventArgs e)
        {
            List<OpenWindowGetter.explorerFolderObj> folders = OpenWindowGetter.getExplorerAddressList();
            string msg = "List of All Open Folders on All Desktops\n\n";
            string[] desktopsFoldersOutput = new string[desktops.Count]; // folders array, for output msgbox & clipboard
            int[] desktopsOutputLengths = new int[desktops.Count]; // for later use, to detect desktops with NO Open Folders //
            for (int i = 0; i < desktops.Count; i++) // add headings for each desktop# list
            {
                desktopsFoldersOutput[i] = "Desktop #" + (i + 1) + desktopNameOrEmpty(i, ":  {", "}") + "\n\n";
                desktopsOutputLengths[i] = desktopsFoldersOutput[i].Length;
            }
            foreach (OpenWindowGetter.explorerFolderObj folder in folders)
            {
                int folderDesktopIndex = desktops.IndexOf(VirtualDesktop.FromHwnd(folder.getWindowHandle()));
                desktopsFoldersOutput[folderDesktopIndex] += "\u25CF " + folder.getAddress().Replace("\\", " " + "\\") + "\n";
                // "Replace" thing -> to 'trick' disabled word-wrap in default msg-box
            }
            for (int i = 0; i < desktops.Count; i++) // go over desktops again to check for ones WITH NO open folders; And compose msg ! //
            {
                if (desktopsOutputLengths[i] == desktopsFoldersOutput[i].Length) desktopsFoldersOutput[i] = "";
                else desktopsFoldersOutput[i] += "\n";
                msg += desktopsFoldersOutput[i];
            }
            string shown_msg = msg + "Copy to clipboard ?!";
            DialogResult res = MessageBox.Show(new Form { TopMost = true }, shown_msg, "List of All Open Folders",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (res == DialogResult.Yes)
            {
                Clipboard.SetText(msg.Replace(" " + "\\", "\\"));
                notifyIcon1.BalloonTipTitle = "Folders List";
                notifyIcon1.BalloonTipText = "Folders List was copied to clipboard successfully!";
                notifyIcon1.ShowBalloonTip(2000);
            }

        }

        private void ExportFolders_Click(object sender, EventArgs e)
        {
            List<OpenWindowGetter.explorerFolderObj> folders = OpenWindowGetter.getExplorerAddressList();
            string batch = generateBatch_from(folders);
            SaveTo.SaveToBatchFile(batch, "folders@" + Helper.getFormattedDateTime());
        }

        private string generateBatch_from(List<OpenWindowGetter.explorerFolderObj> folders)
        {            
            string batch = "@echo off\r\n";
            string[] desktopsFoldersOutput = new string[desktops.Count]; // to group folder-paths by Desktop# (even though all will open when run) //
            int[] desktopsOutputLengths = new int[desktops.Count]; // for later use, to detect desktops with NO Open Folders //
            for (int i = 0; i < desktops.Count; i++) // add headings for each desktop# list
            {
                // ::  is for commenting out this line in batch file , because it has no runtime-use besides grouping // 
                desktopsFoldersOutput[i] = "::Desktop#" + (i + 1) + desktopNameOrEmpty(i, ":[", "]") + "\r\n";
                desktopsOutputLengths[i] = desktopsFoldersOutput[i].Length;
            }
            foreach (OpenWindowGetter.explorerFolderObj folder in folders)
            {
                int folderDesktopIndex = desktops.IndexOf(VirtualDesktop.FromHwnd(folder.getWindowHandle()));
                desktopsFoldersOutput[folderDesktopIndex] += "Explorer \"" + folder.getAddress() + "\"\r\n"; // command to open in explorer
            }
            for (int i = 0; i < desktops.Count; i++) // go over desktops again to check for ones WITH NO open folders; And create batch file //
            {
                if (desktopsOutputLengths[i] == desktopsFoldersOutput[i].Length) desktopsFoldersOutput[i] = "";
                batch += desktopsFoldersOutput[i];
            }
            return batch;
        }

        private void ExportHTML_Click(object sender, EventArgs e)
        {
            int currentDesktopIndex = getCurrentDesktopIndex(); // save for coming back 
            DialogResult r = MessageBox.Show("Desktops might switch back & forth during processing!\n\n"
                + "& Keep all browsers' windows open & NOT minimized !\n\n"
                + "Supported Browsers: Firefox, Chrome, Edge & I.E.",
                "URLs Extraction WARNING !", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (r == DialogResult.OK)
            {
                IDictionary<Consts.BROWSER, List<BrowsersURLs.tabUrlObj>> urls = BrowsersURLs.getAllTabURLs(); // <- might switch desktops !
                desktops[currentDesktopIndex].Switch(); // go back to current desktop
                while (getCurrentDesktopIndex() != currentDesktopIndex) Thread.Sleep(200); // to wait for Switch() above to work !
                Thread.Sleep(2000); // wait a little bit more ; just to make sure we are back on desktop before save-dialog for html shows !
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - //

                string title = "urls@" + Helper.getFormattedDateTime();
                string htmlDoc = generateHTML_from(urls,title);
                SaveTo.SaveToHTML(htmlDoc, title);
            }
        }

        private string generateHTML_from(IDictionary<Consts.BROWSER, List<BrowsersURLs.tabUrlObj>> urls,string title)
        {
            string header, body, jscript;
            header = "<!DOCTYPE html><html><head><title>" + title + "</title></head>\n";
            body = "<body>\n";
            string[] desktopsURLsOutput = new string[desktops.Count]; // to build body, grouped by desktop # // 
            int[] desktopsOutputLengths = new int[desktops.Count]; // for later use, to detect desktops with NO URLs ! //
            for (int i = 0; i < desktops.Count; i++) // add headings for each desktop# list
            {
                desktopsURLsOutput[i] = "<h2>" + "Desktop #" + (i + 1) + desktopNameOrEmpty(i, ":  {", "}") + "</h2>" + "\n";
                desktopsOutputLengths[i] = desktopsURLsOutput[i].Length;
            }
            foreach (KeyValuePair<Consts.BROWSER, List<BrowsersURLs.tabUrlObj>> browserLst in urls) // go over EACH list (by browser type)
            {
                foreach (BrowsersURLs.tabUrlObj tab in browserLst.Value) // inside each list, go over each item (tabURL object)
                {
                    int tabsDesktopIndex = desktops.IndexOf(VirtualDesktop.FromHwnd(tab.getWindowHandle()));
                    string hyperlink = "<a href=\"" + tab.getURL() + "\" target=\"_blank\">" + tab.getTitle() + "</a><br/>";
                    desktopsURLsOutput[tabsDesktopIndex] += hyperlink + "\n";
                }
            }
            for (int i = 0; i < desktops.Count; i++) // go over desktops again to check for ones WITH NO URLs ; and build html-body //
            {
                if (desktopsOutputLengths[i] == desktopsURLsOutput[i].Length) desktopsURLsOutput[i] = "";
                body += desktopsURLsOutput[i];
            }
            body += "<br/><br/>\n";
            body += "<input type=\"button\" value=\"Open All URLs!\" onclick=\"open_All()\"><br/><i>allow popups!</i>\n";
            jscript = "<script>\n"
                + "function open_All(){\n"
                + "var myURLs = document.getElementsByTagName(\"a\");\n"
                + "for(var i=0; i<myURLs.length; i++) window.open(myURLs[i].getAttribute('href'),\"_blank\");\n"
                + "}\n"
                + "</script>\n";
            body += jscript + "</body>\n</html>";
            return header + body;
        }

        // ************************************* // section added 2022-09-26 // ********************************************************//

        // source (mainly) from: https://www.c-sharpcorner.com/UploadFile/2d2d83/how-to-capture-a-screen-using-C-Sharp/ //

        private void ScreenshotCurrent_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog
            {
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Description = "choose path to save image captures to ...",
                ShowNewFolderButton = true
            };
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                // start capture image process , save to selected path by user 
                string finalPath = folderBrowserDialog.SelectedPath + @"\\" + "captures@" + Helper.getFormattedDateTime();
                Directory.CreateDirectory(finalPath);
                Thread.Sleep(1000); // to make enough time for folderBrowserDialog to close properly !
                captureCurrentDesktop(finalPath);
            }
        }

        // helper method which captures Current Desktop (including all screens) and saves to given "path" // 
        private void captureCurrentDesktop(string path)
        {
            int desktopIndex = getCurrentDesktopIndex();
            try
            {
                int index = 0;
                foreach (Screen scr in Screen.AllScreens)
                {
                    Rectangle captureRectangle = Screen.AllScreens[index].Bounds;
                    Bitmap captureBitmap = new Bitmap(captureRectangle.Width, captureRectangle.Height, PixelFormat.Format32bppArgb);
                    Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                    captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
                    string imgName = "desktop_" + (desktopIndex + 1) + "_screen_" + index;
                    captureBitmap.Save(path + "\\" + imgName + ".jpg", ImageFormat.Jpeg);
                    index++;
                    captureBitmap.Dispose();
                    captureGraphics.Dispose();
                }
            }
            catch (Exception) { }
        }

        private void ScreenshotAll_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Desktops will cycle ONCE, in order to take screenshots of each one.\n\n"
                + "press OK to proceed or Cancel to abort.\n\n",
                "Desktops Cycle Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (r == DialogResult.OK)
            {
                var folderBrowserDialog = new FolderBrowserDialog
                {
                    SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Description = "choose path to save image captures to ...",
                    ShowNewFolderButton = true
                };
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // start capture image process , save to selected path by user 
                    string finalPath = folderBrowserDialog.SelectedPath + @"\\" + "captures@" + Helper.getFormattedDateTime();
                    Directory.CreateDirectory(finalPath);
                    Thread.Sleep(1000); // to make enough time for folderBrowserDialog to close properly !
                    for (int i = 0; i < desktops.Count; i++) captureAndMove(finalPath, 1500);
                    MessageBox.Show("Done Capturing All Desktops!", "Desktops' Screenshots", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void captureAndMove(string path,int Halt_Time_MSEC)
        {
            captureCurrentDesktop(path);
            moveToNext(initialDesktopState());
            Thread.Sleep(Halt_Time_MSEC); // to make enough time for a COMPLETE desktop transition before capturing ; Increase time if necessary ! //
        }


        // inspired from PanicButton (a google chrome extension) // 
        private void Panic_Click(object sender, EventArgs e)
        {
            // dummy msg-box: will proceed no matter what user clicks ; was made to give tooltip few seconds to disappear 
            MessageBox.Show(new Form { TopMost = true },"It will take a few seconds\n\nJust watch and wait\n\n", "Panic !", 
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            // step 0: prepare path to save everything to
            string DEFAULT_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + "PANIC@" + Helper.getFormattedDateTime();
            Directory.CreateDirectory(DEFAULT_PATH);
            // step 1: do all screenshots round (cycle desktops)
            Thread.Sleep(1000);
            for (int i = 0; i < desktops.Count; i++) captureAndMove(DEFAULT_PATH, 1500);
            // step 2: generate string for batch file (from folders list)
            List<OpenWindowGetter.explorerFolderObj> folders = OpenWindowGetter.getExplorerAddressList();
            string batch = generateBatch_from(folders);
            // step 3: generate string for HTML (from urls list)
            int currentDesktopIndex = getCurrentDesktopIndex();
            IDictionary<Consts.BROWSER, List<BrowsersURLs.tabUrlObj>> urls = BrowsersURLs.getAllTabURLs();
            desktops[currentDesktopIndex].Switch();
            while (getCurrentDesktopIndex() != currentDesktopIndex) Thread.Sleep(250);
            Thread.Sleep(2000);
            string html = generateHTML_from(urls, "urls");
            // step 4: generate string for text file [=all data] (using folders & urls from above)
            string dataTxt = generateDataTxt_from(OpenWindowGetter.GetOpenWindows(), folders, urls);
            // step 5: quick save to all generated strings above (steps 2,3,4) , in one step 
            SaveTo.FastSave(dataTxt, batch, html, DEFAULT_PATH);
            // finally , inform the panicked user ! //
            MessageBox.Show("Done!\n\nEverything was saved to: \n\n" + DEFAULT_PATH, "Don't Panic !", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string generateDataTxt_from(IDictionary<HWND, string> windows, List<OpenWindowGetter.explorerFolderObj> folders, IDictionary<Consts.BROWSER, List<BrowsersURLs.tabUrlObj>> urls)
        {
            string[] headings = generate_headings();    // list 1: headings of desktop #, GUID +/- title //            
            string[] windowsList = generate_windowsList_from(windows);  // list 2: windows list, their titles & handle numbers //
            string[] foldersList = generate_foldersList_from(folders);  // list 3: folders list -> their full paths //
            string[] urlsList = generate_urlsList_from(urls);   // list 4: URLs list: tab titles & their URLs  //
            return combineLists(headings, windowsList, foldersList, urlsList);
        }

        private string[] generate_headings()
        {
            string[] headings = new string[desktops.Count];
            for (var k = 0; k < desktops.Count; k++)
            {
                headings[k] = (k == 0 ? "" : "\n") + "Desktop #" + (k + 1) + "\n";
                headings[k] += desktopNameOrEmpty(k, "\tname/title: ", "\n");
                headings[k] += "\tGUID: " + desktops.ElementAt(k).Id.ToString().ToUpper() + "\n";
            }
            return headings;
        }

        private string[] generate_windowsList_from(IDictionary<HWND, string> windows)
        {
            string[] windowsList = new string[desktops.Count];
            int[] windowsCounters = new int[desktops.Count];
            int currIndex;
            foreach (KeyValuePair<IntPtr, string> window in windows)
            {
                IntPtr handle = window.Key;
                string title = window.Value;
                VirtualDesktop vdFromHandle = VirtualDesktop.FromHwnd(handle);
                if (vdFromHandle != null)
                {
                    currIndex = desktops.IndexOf(vdFromHandle);
                    windowsList[currIndex] += "\t\t" + (windowsCounters[currIndex] + 1) + ".  [" + handle + "]: " + title + "\n";
                    windowsCounters[currIndex]++;
                }
            }
            return windowsList;
        }

        private string[] generate_foldersList_from(List<OpenWindowGetter.explorerFolderObj> folders)
        {
            string[] foldersList = new string[desktops.Count];
            foreach (OpenWindowGetter.explorerFolderObj folder in folders)
            {
                int folderDesktopIndex = desktops.IndexOf(VirtualDesktop.FromHwnd(folder.getWindowHandle()));
                foldersList[folderDesktopIndex] += "\t\t" + "\u25CF " + folder.getAddress() + "\n";
            }
            return foldersList;
        }

        private string[] generate_urlsList_from(IDictionary<Consts.BROWSER, List<BrowsersURLs.tabUrlObj>> urls)
        {
            string[] urlsList = new string[desktops.Count];
            foreach (KeyValuePair<Consts.BROWSER, List<BrowsersURLs.tabUrlObj>> browserLst in urls)
            {
                foreach (BrowsersURLs.tabUrlObj tab in browserLst.Value)
                {
                    int tabsDesktopIndex = desktops.IndexOf(VirtualDesktop.FromHwnd(tab.getWindowHandle()));
                    urlsList[tabsDesktopIndex] += "\t\t" + "\u25CF " + tab.getTitle() + " [" + tab.getURL() + "]\n";
                }
            }
            return urlsList;
        }






        // ************************************* // ************************ // ********************************************************//
    }
}