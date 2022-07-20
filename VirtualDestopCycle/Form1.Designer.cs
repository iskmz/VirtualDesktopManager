namespace VirtualDesktopManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator8 = new System.Windows.Forms.ToolStripSeparator();
            this.exportDesktopsData = new System.Windows.Forms.ToolStripMenuItem();
            this.getWindowsList = new System.Windows.Forms.ToolStripMenuItem();
            this.getIDs = new System.Windows.Forms.ToolStripMenuItem();
            this.separator7 = new System.Windows.Forms.ToolStripSeparator();
            this.saveUserPref = new System.Windows.Forms.ToolStripMenuItem();
            this.separator6 = new System.Windows.Forms.ToolStripSeparator();
            this.splashItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator5 = new System.Windows.Forms.ToolStripSeparator();
            this.transTimeSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.sec2 = new System.Windows.Forms.ToolStripMenuItem();
            this.sec4 = new System.Windows.Forms.ToolStripMenuItem();
            this.sec6 = new System.Windows.Forms.ToolStripMenuItem();
            this.sec8 = new System.Windows.Forms.ToolStripMenuItem();
            this.sec10 = new System.Windows.Forms.ToolStripMenuItem();
            this.sec12 = new System.Windows.Forms.ToolStripMenuItem();
            this.sec14 = new System.Windows.Forms.ToolStripMenuItem();
            this.cyclesAmountSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.cycles_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cycles_2 = new System.Windows.Forms.ToolStripMenuItem();
            this.cycles_3 = new System.Windows.Forms.ToolStripMenuItem();
            this.cycles_4 = new System.Windows.Forms.ToolStripMenuItem();
            this.cycles_5 = new System.Windows.Forms.ToolStripMenuItem();
            this.cycles_forever = new System.Windows.Forms.ToolStripMenuItem();
            this.reverseCycleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cycleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator4 = new System.Windows.Forms.ToolStripSeparator();
            this.colorList = new System.Windows.Forms.ToolStripMenuItem();
            this.Black = new System.Windows.Forms.ToolStripMenuItem();
            this.Brown = new System.Windows.Forms.ToolStripMenuItem();
            this.Dark_Blue = new System.Windows.Forms.ToolStripMenuItem();
            this.Dark_Green = new System.Windows.Forms.ToolStripMenuItem();
            this.Dark_Red = new System.Windows.Forms.ToolStripMenuItem();
            this.Purple = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Pink = new System.Windows.Forms.ToolStripMenuItem();
            this.Red = new System.Windows.Forms.ToolStripMenuItem();
            this.Green = new System.Windows.Forms.ToolStripMenuItem();
            this.Blue = new System.Windows.Forms.ToolStripMenuItem();
            this.Yellow = new System.Windows.Forms.ToolStripMenuItem();
            this.White = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TransparentWhite = new System.Windows.Forms.ToolStripMenuItem();
            this.TransparentBlack = new System.Windows.Forms.ToolStripMenuItem();
            this.separator3 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.nextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.desktopsList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.removeButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.addFileButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = global::VirtualDesktopManager.Properties.Resources.mainIco;
            this.notifyIcon1.Text = "Virtual Desktop Manager";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon1_MouseMove);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutMenuItem,
            this.separator8,
            this.exportDesktopsData,
            this.getWindowsList,
            this.getIDs,
            this.separator7,
            this.saveUserPref,
            this.separator6,
            this.splashItem,
            this.separator5,
            this.transTimeSubMenu,
            this.cyclesAmountSubMenu,
            this.reverseCycleMenuItem,
            this.cycleMenuItem,
            this.separator4,
            this.colorList,
            this.separator3,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem,
            this.separator2,
            this.nextToolStripMenuItem,
            this.previousToolStripMenuItem1,
            this.separator1,
            this.desktopsList});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(193, 618);
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.icons8_info_100;
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(192, 34);
            this.aboutMenuItem.Text = "About";
            this.aboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // separator8
            // 
            this.separator8.Name = "separator8";
            this.separator8.Size = new System.Drawing.Size(189, 6);
            // 
            // exportDesktopsData
            // 
            this.exportDesktopsData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportDesktopsData.Name = "exportDesktopsData";
            this.exportDesktopsData.Size = new System.Drawing.Size(192, 34);
            this.exportDesktopsData.Text = "export data";
            this.exportDesktopsData.ToolTipText = "Export desktop data: desktops, their titles, GUIDs & windows-list , to a text fil" +
    "e";
            this.exportDesktopsData.Click += new System.EventHandler(this.ExportDesktopsData_Click);
            // 
            // getWindowsList
            // 
            this.getWindowsList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.getWindowsList.Name = "getWindowsList";
            this.getWindowsList.Size = new System.Drawing.Size(192, 34);
            this.getWindowsList.Text = "list windows";
            this.getWindowsList.ToolTipText = "show a list of all windows in the current desktop: each window\'s handle # & title" +
    "";
            this.getWindowsList.Click += new System.EventHandler(this.GetWindowsList_Click);
            // 
            // getIDs
            // 
            this.getIDs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.getIDs.Name = "getIDs";
            this.getIDs.Size = new System.Drawing.Size(192, 34);
            this.getIDs.Text = "desktops GUIDs";
            this.getIDs.ToolTipText = "show a list of all desktops , their names and their GUIDs";
            this.getIDs.Click += new System.EventHandler(this.GetIDs_Click);
            // 
            // separator7
            // 
            this.separator7.Name = "separator7";
            this.separator7.Size = new System.Drawing.Size(189, 6);
            // 
            // saveUserPref
            // 
            this.saveUserPref.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.saveUserPref.Image = global::VirtualDesktopManager.Properties.Resources.icons8_save_100;
            this.saveUserPref.Name = "saveUserPref";
            this.saveUserPref.Size = new System.Drawing.Size(192, 34);
            this.saveUserPref.Text = "Save Preferences";
            this.saveUserPref.ToolTipText = "save current preferences: colors, transition time, cycles amount";
            this.saveUserPref.Click += new System.EventHandler(this.SaveUserPref_Click);
            // 
            // separator6
            // 
            this.separator6.Name = "separator6";
            this.separator6.Size = new System.Drawing.Size(189, 6);
            // 
            // splashItem
            // 
            this.splashItem.ForeColor = System.Drawing.SystemColors.ControlText;
            this.splashItem.Image = global::VirtualDesktopManager.Properties.Resources.icons8_splash_100;
            this.splashItem.Name = "splashItem";
            this.splashItem.Size = new System.Drawing.Size(192, 34);
            this.splashItem.Text = "Splash";
            this.splashItem.ToolTipText = "Click to Activate/Deactivate";
            this.splashItem.Click += new System.EventHandler(this.SplashItem_Click);
            // 
            // separator5
            // 
            this.separator5.Name = "separator5";
            this.separator5.Size = new System.Drawing.Size(189, 6);
            // 
            // transTimeSubMenu
            // 
            this.transTimeSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sec2,
            this.sec4,
            this.sec6,
            this.sec8,
            this.sec10,
            this.sec12,
            this.sec14});
            this.transTimeSubMenu.Name = "transTimeSubMenu";
            this.transTimeSubMenu.Size = new System.Drawing.Size(192, 34);
            this.transTimeSubMenu.Text = "transition time";
            // 
            // sec2
            // 
            this.sec2.Name = "sec2";
            this.sec2.Size = new System.Drawing.Size(106, 22);
            this.sec2.Text = "2 sec";
            this.sec2.Click += new System.EventHandler(this.Sec2_ToolStripMenuItem_Click);
            // 
            // sec4
            // 
            this.sec4.Name = "sec4";
            this.sec4.Size = new System.Drawing.Size(106, 22);
            this.sec4.Text = "4 sec";
            this.sec4.Click += new System.EventHandler(this.Sec4_ToolStripMenuItem_Click);
            // 
            // sec6
            // 
            this.sec6.Name = "sec6";
            this.sec6.Size = new System.Drawing.Size(106, 22);
            this.sec6.Text = "6 sec";
            this.sec6.Click += new System.EventHandler(this.Sec6_ToolStripMenuItem_Click);
            // 
            // sec8
            // 
            this.sec8.Name = "sec8";
            this.sec8.Size = new System.Drawing.Size(106, 22);
            this.sec8.Text = "8 sec";
            this.sec8.Click += new System.EventHandler(this.Sec8_ToolStripMenuItem_Click);
            // 
            // sec10
            // 
            this.sec10.Name = "sec10";
            this.sec10.Size = new System.Drawing.Size(106, 22);
            this.sec10.Text = "10 sec";
            this.sec10.Click += new System.EventHandler(this.Sec10_ToolStripMenuItem_Click);
            // 
            // sec12
            // 
            this.sec12.Name = "sec12";
            this.sec12.Size = new System.Drawing.Size(106, 22);
            this.sec12.Text = "12 sec";
            this.sec12.Click += new System.EventHandler(this.Sec12_ToolStripMenuItem_Click);
            // 
            // sec14
            // 
            this.sec14.Name = "sec14";
            this.sec14.Size = new System.Drawing.Size(106, 22);
            this.sec14.Text = "14 sec";
            this.sec14.Click += new System.EventHandler(this.Sec14_ToolStripMenuItem_Click);
            // 
            // cyclesAmountSubMenu
            // 
            this.cyclesAmountSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cycles_1,
            this.cycles_2,
            this.cycles_3,
            this.cycles_4,
            this.cycles_5,
            this.cycles_forever});
            this.cyclesAmountSubMenu.Name = "cyclesAmountSubMenu";
            this.cyclesAmountSubMenu.Size = new System.Drawing.Size(192, 34);
            this.cyclesAmountSubMenu.Text = "cycles amount";
            // 
            // cycles_1
            // 
            this.cycles_1.Name = "cycles_1";
            this.cycles_1.Size = new System.Drawing.Size(114, 22);
            this.cycles_1.Text = "1";
            this.cycles_1.Click += new System.EventHandler(this.Cycles_1_Click);
            // 
            // cycles_2
            // 
            this.cycles_2.Name = "cycles_2";
            this.cycles_2.Size = new System.Drawing.Size(114, 22);
            this.cycles_2.Text = "2";
            this.cycles_2.Click += new System.EventHandler(this.Cycles_2_Click);
            // 
            // cycles_3
            // 
            this.cycles_3.Name = "cycles_3";
            this.cycles_3.Size = new System.Drawing.Size(114, 22);
            this.cycles_3.Text = "3";
            this.cycles_3.Click += new System.EventHandler(this.Cycles_3_Click);
            // 
            // cycles_4
            // 
            this.cycles_4.Name = "cycles_4";
            this.cycles_4.Size = new System.Drawing.Size(114, 22);
            this.cycles_4.Text = "4";
            this.cycles_4.Click += new System.EventHandler(this.Cycles_4_Click);
            // 
            // cycles_5
            // 
            this.cycles_5.Name = "cycles_5";
            this.cycles_5.Size = new System.Drawing.Size(114, 22);
            this.cycles_5.Text = "5";
            this.cycles_5.Click += new System.EventHandler(this.Cycles_5_Click);
            // 
            // cycles_forever
            // 
            this.cycles_forever.Name = "cycles_forever";
            this.cycles_forever.Size = new System.Drawing.Size(114, 22);
            this.cycles_forever.Text = "forever!";
            this.cycles_forever.Click += new System.EventHandler(this.Cycles_forever_Click);
            // 
            // reverseCycleMenuItem
            // 
            this.reverseCycleMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.icons8_reset_64__rev_cycle;
            this.reverseCycleMenuItem.Name = "reverseCycleMenuItem";
            this.reverseCycleMenuItem.Size = new System.Drawing.Size(192, 34);
            this.reverseCycleMenuItem.Text = "Reverse Cycle !";
            this.reverseCycleMenuItem.ToolTipText = "cycle all desktops in reverse order; as specified in the prameters above";
            this.reverseCycleMenuItem.Click += new System.EventHandler(this.ReverseCycleMenuItem_Click);
            // 
            // cycleMenuItem
            // 
            this.cycleMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.icons8_reset_64__cycle;
            this.cycleMenuItem.Name = "cycleMenuItem";
            this.cycleMenuItem.Size = new System.Drawing.Size(192, 34);
            this.cycleMenuItem.Text = "Cycle !";
            this.cycleMenuItem.ToolTipText = "cycle all desktops in order; as specified in the prameters above";
            this.cycleMenuItem.Click += new System.EventHandler(this.CycleMenuItem_Click);
            // 
            // separator4
            // 
            this.separator4.Name = "separator4";
            this.separator4.Size = new System.Drawing.Size(189, 6);
            // 
            // colorList
            // 
            this.colorList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Black,
            this.Brown,
            this.Dark_Blue,
            this.Dark_Green,
            this.Dark_Red,
            this.Purple,
            this.toolStripSeparator1,
            this.Pink,
            this.Red,
            this.Green,
            this.Blue,
            this.Yellow,
            this.White,
            this.toolStripSeparator2,
            this.TransparentWhite,
            this.TransparentBlack});
            this.colorList.Image = global::VirtualDesktopManager.Properties.Resources.icons8_paint_100;
            this.colorList.Name = "colorList";
            this.colorList.Size = new System.Drawing.Size(192, 34);
            this.colorList.Text = "Colors ...";
            this.colorList.ToolTipText = "change color of tray icon";
            // 
            // Black
            // 
            this.Black.Image = global::VirtualDesktopManager.Properties.Resources.back_Black;
            this.Black.Name = "Black";
            this.Black.Size = new System.Drawing.Size(213, 34);
            this.Black.Text = "Black";
            this.Black.Click += new System.EventHandler(this.BlackToolStripMenuItem_Click);
            // 
            // Brown
            // 
            this.Brown.Image = global::VirtualDesktopManager.Properties.Resources.back_Brown;
            this.Brown.Name = "Brown";
            this.Brown.Size = new System.Drawing.Size(213, 34);
            this.Brown.Text = "Brown";
            this.Brown.Click += new System.EventHandler(this.BrownToolStripMenuItem_Click);
            // 
            // Dark_Blue
            // 
            this.Dark_Blue.Image = global::VirtualDesktopManager.Properties.Resources.back_Dark_Blue;
            this.Dark_Blue.Name = "Dark_Blue";
            this.Dark_Blue.Size = new System.Drawing.Size(213, 34);
            this.Dark_Blue.Text = "Dark Blue";
            this.Dark_Blue.Click += new System.EventHandler(this.DarkBlueToolStripMenuItem_Click);
            // 
            // Dark_Green
            // 
            this.Dark_Green.Image = global::VirtualDesktopManager.Properties.Resources.back_Dark_Green;
            this.Dark_Green.Name = "Dark_Green";
            this.Dark_Green.Size = new System.Drawing.Size(213, 34);
            this.Dark_Green.Text = "Dark Green";
            this.Dark_Green.Click += new System.EventHandler(this.DarkGreenToolStripMenuItem_Click);
            // 
            // Dark_Red
            // 
            this.Dark_Red.Image = global::VirtualDesktopManager.Properties.Resources.back_Dark_Red;
            this.Dark_Red.Name = "Dark_Red";
            this.Dark_Red.Size = new System.Drawing.Size(213, 34);
            this.Dark_Red.Text = "Dark Red";
            this.Dark_Red.Click += new System.EventHandler(this.DarkRedToolStripMenuItem_Click);
            // 
            // Purple
            // 
            this.Purple.Image = global::VirtualDesktopManager.Properties.Resources.back_Purple;
            this.Purple.Name = "Purple";
            this.Purple.Size = new System.Drawing.Size(213, 34);
            this.Purple.Text = "Purple";
            this.Purple.Click += new System.EventHandler(this.PurpleToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(210, 6);
            // 
            // Pink
            // 
            this.Pink.Image = global::VirtualDesktopManager.Properties.Resources.back_Pink;
            this.Pink.Name = "Pink";
            this.Pink.Size = new System.Drawing.Size(213, 34);
            this.Pink.Text = "Pink";
            this.Pink.Click += new System.EventHandler(this.PinkToolStripMenuItem_Click);
            // 
            // Red
            // 
            this.Red.Image = global::VirtualDesktopManager.Properties.Resources.back_Red;
            this.Red.Name = "Red";
            this.Red.Size = new System.Drawing.Size(213, 34);
            this.Red.Text = "Red";
            this.Red.Click += new System.EventHandler(this.RedToolStripMenuItem_Click);
            // 
            // Green
            // 
            this.Green.Image = global::VirtualDesktopManager.Properties.Resources.back_Green;
            this.Green.Name = "Green";
            this.Green.Size = new System.Drawing.Size(213, 34);
            this.Green.Text = "Green";
            this.Green.Click += new System.EventHandler(this.GreenToolStripMenuItem_Click);
            // 
            // Blue
            // 
            this.Blue.Image = global::VirtualDesktopManager.Properties.Resources.back_Blue;
            this.Blue.Name = "Blue";
            this.Blue.Size = new System.Drawing.Size(213, 34);
            this.Blue.Text = "Blue";
            this.Blue.Click += new System.EventHandler(this.BlueToolStripMenuItem_Click);
            // 
            // Yellow
            // 
            this.Yellow.Image = global::VirtualDesktopManager.Properties.Resources.back_Yellow;
            this.Yellow.Name = "Yellow";
            this.Yellow.Size = new System.Drawing.Size(213, 34);
            this.Yellow.Text = "Yellow";
            this.Yellow.Click += new System.EventHandler(this.YellowToolStripMenuItem_Click);
            // 
            // White
            // 
            this.White.Image = global::VirtualDesktopManager.Properties.Resources.back_White;
            this.White.Name = "White";
            this.White.Size = new System.Drawing.Size(213, 34);
            this.White.Text = "White";
            this.White.Click += new System.EventHandler(this.WhiteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(210, 6);
            // 
            // TransparentWhite
            // 
            this.TransparentWhite.Name = "TransparentWhite";
            this.TransparentWhite.Size = new System.Drawing.Size(213, 34);
            this.TransparentWhite.Text = "Transparent (White Text)";
            this.TransparentWhite.Click += new System.EventHandler(this.TransparentWhiteTextToolStripMenuItem_Click);
            // 
            // TransparentBlack
            // 
            this.TransparentBlack.Name = "TransparentBlack";
            this.TransparentBlack.Size = new System.Drawing.Size(213, 34);
            this.TransparentBlack.Text = "Transparent (Black Text)";
            this.TransparentBlack.Click += new System.EventHandler(this.TransparentBlackTextToolStripMenuItem_Click);
            // 
            // separator3
            // 
            this.separator3.Name = "separator3";
            this.separator3.Size = new System.Drawing.Size(189, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.icons8_settings_50;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(192, 34);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.ToolTipText = "open settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.icons8_close_50;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(192, 34);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.ToolTipText = "Exit V.D.M.";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(189, 6);
            // 
            // nextToolStripMenuItem
            // 
            this.nextToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.icons8_right_64;
            this.nextToolStripMenuItem.Name = "nextToolStripMenuItem";
            this.nextToolStripMenuItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nextToolStripMenuItem.Size = new System.Drawing.Size(192, 34);
            this.nextToolStripMenuItem.Text = ">> Next >>";
            this.nextToolStripMenuItem.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.nextToolStripMenuItem.ToolTipText = "go to next desktop";
            this.nextToolStripMenuItem.Click += new System.EventHandler(this.nextClick);
            // 
            // previousToolStripMenuItem1
            // 
            this.previousToolStripMenuItem1.Image = global::VirtualDesktopManager.Properties.Resources.icons8_left_64;
            this.previousToolStripMenuItem1.Name = "previousToolStripMenuItem1";
            this.previousToolStripMenuItem1.Size = new System.Drawing.Size(192, 34);
            this.previousToolStripMenuItem1.Text = "<< Previous <<";
            this.previousToolStripMenuItem1.ToolTipText = "go to previous desktop";
            this.previousToolStripMenuItem1.Click += new System.EventHandler(this.previousClick);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(189, 6);
            // 
            // desktopsList
            // 
            this.desktopsList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.desktopsList.Image = global::VirtualDesktopManager.Properties.Resources.icons8_matrix_desktop_96;
            this.desktopsList.Name = "desktopsList";
            this.desktopsList.Size = new System.Drawing.Size(192, 34);
            this.desktopsList.Text = "Desktops ...";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(90, 22);
            this.toolStripMenuItem2.Text = "# 1";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.ForeColor = System.Drawing.Color.White;
            this.checkBox1.Location = new System.Drawing.Point(14, 54);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(353, 21);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Use alternate key combination (Shift+Alt+Left/Right)";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Black;
            this.groupBox1.Controls.Add(this.removeButton);
            this.groupBox1.Controls.Add(this.downButton);
            this.groupBox1.Controls.Add(this.upButton);
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Controls.Add(this.addFileButton);
            this.groupBox1.Controls.Add(this.saveButton);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(8, 9);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Size = new System.Drawing.Size(593, 347);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton.BackColor = System.Drawing.Color.Black;
            this.removeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeButton.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.removeButton.ForeColor = System.Drawing.Color.White;
            this.removeButton.Location = new System.Drawing.Point(175, 282);
            this.removeButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(144, 37);
            this.removeButton.TabIndex = 7;
            this.removeButton.Text = "Remove file";
            this.removeButton.UseVisualStyleBackColor = false;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.BackColor = System.Drawing.Color.Black;
            this.downButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downButton.Font = new System.Drawing.Font("Wingdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.downButton.ForeColor = System.Drawing.Color.White;
            this.downButton.Location = new System.Drawing.Point(539, 188);
            this.downButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(41, 37);
            this.downButton.TabIndex = 6;
            this.downButton.Text = "â";
            this.downButton.UseVisualStyleBackColor = false;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.BackColor = System.Drawing.Color.Black;
            this.upButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.upButton.Font = new System.Drawing.Font("Wingdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.upButton.ForeColor = System.Drawing.Color.White;
            this.upButton.Location = new System.Drawing.Point(539, 128);
            this.upButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(41, 37);
            this.upButton.TabIndex = 5;
            this.upButton.Text = "á";
            this.upButton.UseVisualStyleBackColor = false;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(14, 98);
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(508, 168);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // addFileButton
            // 
            this.addFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addFileButton.BackColor = System.Drawing.Color.Black;
            this.addFileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addFileButton.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.addFileButton.ForeColor = System.Drawing.Color.White;
            this.addFileButton.Location = new System.Drawing.Point(14, 282);
            this.addFileButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.addFileButton.Name = "addFileButton";
            this.addFileButton.Size = new System.Drawing.Size(144, 37);
            this.addFileButton.TabIndex = 3;
            this.addFileButton.Text = "Add background";
            this.addFileButton.UseVisualStyleBackColor = false;
            this.addFileButton.Click += new System.EventHandler(this.addFileButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.BackColor = System.Drawing.Color.Black;
            this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveButton.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.saveButton.ForeColor = System.Drawing.Color.White;
            this.saveButton.Location = new System.Drawing.Point(477, 45);
            this.saveButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(103, 37);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Location = new System.Drawing.Point(8, 363);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(171, 76);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.labelStatus.Location = new System.Drawing.Point(184, 363);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(0, 17);
            this.labelStatus.TabIndex = 5;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog1_FileOk);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(609, 451);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Virtual Desktop Manager";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion



        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button addFileButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.ToolStripMenuItem nextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previousToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator separator1;
        private System.Windows.Forms.ToolStripSeparator separator2;
        private System.Windows.Forms.ToolStripMenuItem desktopsList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator separator3;
        private System.Windows.Forms.ToolStripMenuItem colorList;
        private System.Windows.Forms.ToolStripMenuItem Black;
        private System.Windows.Forms.ToolStripMenuItem Brown;
        private System.Windows.Forms.ToolStripMenuItem Dark_Blue;
        private System.Windows.Forms.ToolStripMenuItem Dark_Green;
        private System.Windows.Forms.ToolStripMenuItem Dark_Red;
        private System.Windows.Forms.ToolStripMenuItem Purple;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Pink;
        private System.Windows.Forms.ToolStripMenuItem Red;
        private System.Windows.Forms.ToolStripMenuItem Green;
        private System.Windows.Forms.ToolStripMenuItem Blue;
        private System.Windows.Forms.ToolStripMenuItem Yellow;
        private System.Windows.Forms.ToolStripMenuItem White;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem TransparentWhite;
        private System.Windows.Forms.ToolStripMenuItem TransparentBlack;
        private System.Windows.Forms.ToolStripMenuItem reverseCycleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cycleMenuItem;
        private System.Windows.Forms.ToolStripSeparator separator4;
        private System.Windows.Forms.ToolStripMenuItem transTimeSubMenu;
        private System.Windows.Forms.ToolStripMenuItem cyclesAmountSubMenu;
        private System.Windows.Forms.ToolStripMenuItem sec2;
        private System.Windows.Forms.ToolStripMenuItem sec4;
        private System.Windows.Forms.ToolStripMenuItem sec6;
        private System.Windows.Forms.ToolStripMenuItem sec8;
        private System.Windows.Forms.ToolStripMenuItem sec10;
        private System.Windows.Forms.ToolStripMenuItem sec12;
        private System.Windows.Forms.ToolStripMenuItem sec14;
        private System.Windows.Forms.ToolStripMenuItem cycles_1;
        private System.Windows.Forms.ToolStripMenuItem cycles_2;
        private System.Windows.Forms.ToolStripMenuItem cycles_3;
        private System.Windows.Forms.ToolStripMenuItem cycles_4;
        private System.Windows.Forms.ToolStripMenuItem cycles_5;
        private System.Windows.Forms.ToolStripMenuItem cycles_forever;
        private System.Windows.Forms.ToolStripSeparator separator5;
        private System.Windows.Forms.ToolStripMenuItem saveUserPref;
        private System.Windows.Forms.ToolStripMenuItem getIDs;
        private System.Windows.Forms.ToolStripSeparator separator6;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
        private System.Windows.Forms.ToolStripSeparator separator8;
        private System.Windows.Forms.ToolStripMenuItem exportDesktopsData;
        private System.Windows.Forms.ToolStripMenuItem getWindowsList;
        private System.Windows.Forms.ToolStripSeparator separator7;
        private System.Windows.Forms.ToolStripMenuItem splashItem;
    }
}

