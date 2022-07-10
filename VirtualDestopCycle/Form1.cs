using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Runtime.InteropServices;
using WindowsDesktop;
using GlobalHotKey;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using InputBoxClassLibrary;

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

        private void Form1_Load(object sender, EventArgs e)
        {
            labelStatus.Text = "";

            if (!useAltKeySettings) normalHotkeys();
            else alternateHotkeys();

            var desktop = initialDesktopState();
            changeTrayIcon();

            this.Visible = false;
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
            item.MouseUp += handleDesktopNumberClick; // changed: 2022-03-02 , to add a change-desktop-name on RIGHT-CLICK feature !
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
        private void add_special_menu_Item(string name,Image icon,EventHandler action,string tooltip)
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

        // event handler for ToolStripItem (desktops) CLICK // choose what to do based on LEFT/RIGHT click ... // 
        private void handleDesktopNumberClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left) gotoDesktopNumber(sender, e);
            else if (me.Button == MouseButtons.Right) changeNameInputBox(sender, e);
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
        private string desktopNameOrEmpty(int index, string prefix = "", string suffix = "")
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

    }

    static class Consts
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
}