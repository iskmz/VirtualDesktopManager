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
            this.colorList = new System.Windows.Forms.ToolStripMenuItem();
            this.blackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkBlueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkGreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkRedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.purpleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.pinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yellowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.transparentWhiteTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transparentBlackTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorTop = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorMiddle = new System.Windows.Forms.ToolStripSeparator();
            this.nextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.separator = new System.Windows.Forms.ToolStripSeparator();
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
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon1_MouseDoubleClick);
            this.notifyIcon1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon1_MouseMove);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorList,
            this.separatorTop,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem,
            this.separatorMiddle,
            this.nextToolStripMenuItem,
            this.previousToolStripMenuItem1,
            this.separator,
            this.desktopsList});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 176);
            // 
            // colorList
            // 
            this.colorList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blackToolStripMenuItem,
            this.brownToolStripMenuItem,
            this.darkBlueToolStripMenuItem,
            this.darkGreenToolStripMenuItem,
            this.darkRedToolStripMenuItem,
            this.purpleToolStripMenuItem,
            this.toolStripSeparator1,
            this.pinkToolStripMenuItem,
            this.redToolStripMenuItem,
            this.greenToolStripMenuItem,
            this.blueToolStripMenuItem,
            this.yellowToolStripMenuItem,
            this.whiteToolStripMenuItem,
            this.toolStripSeparator2,
            this.transparentWhiteTextToolStripMenuItem,
            this.transparentBlackTextToolStripMenuItem});
            this.colorList.Name = "colorList";
            this.colorList.Size = new System.Drawing.Size(180, 22);
            this.colorList.Text = "Colors ...";
            // 
            // blackToolStripMenuItem
            // 
            this.blackToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.back_Black;
            this.blackToolStripMenuItem.Name = "blackToolStripMenuItem";
            this.blackToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.blackToolStripMenuItem.Text = "Black";
            this.blackToolStripMenuItem.Click += new System.EventHandler(this.BlackToolStripMenuItem_Click);
            // 
            // brownToolStripMenuItem
            // 
            this.brownToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.back_Brown;
            this.brownToolStripMenuItem.Name = "brownToolStripMenuItem";
            this.brownToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.brownToolStripMenuItem.Text = "Brown";
            this.brownToolStripMenuItem.Click += new System.EventHandler(this.BrownToolStripMenuItem_Click);
            // 
            // darkBlueToolStripMenuItem
            // 
            this.darkBlueToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.back_Dark_Blue;
            this.darkBlueToolStripMenuItem.Name = "darkBlueToolStripMenuItem";
            this.darkBlueToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.darkBlueToolStripMenuItem.Text = "Dark Blue";
            this.darkBlueToolStripMenuItem.Click += new System.EventHandler(this.DarkBlueToolStripMenuItem_Click);
            // 
            // darkGreenToolStripMenuItem
            // 
            this.darkGreenToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.back_Dark_Green;
            this.darkGreenToolStripMenuItem.Name = "darkGreenToolStripMenuItem";
            this.darkGreenToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.darkGreenToolStripMenuItem.Text = "Dark Green";
            this.darkGreenToolStripMenuItem.Click += new System.EventHandler(this.DarkGreenToolStripMenuItem_Click);
            // 
            // darkRedToolStripMenuItem
            // 
            this.darkRedToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.back_Dark_Red;
            this.darkRedToolStripMenuItem.Name = "darkRedToolStripMenuItem";
            this.darkRedToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.darkRedToolStripMenuItem.Text = "Dark Red";
            this.darkRedToolStripMenuItem.Click += new System.EventHandler(this.DarkRedToolStripMenuItem_Click);
            // 
            // purpleToolStripMenuItem
            // 
            this.purpleToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.back_Purple;
            this.purpleToolStripMenuItem.Name = "purpleToolStripMenuItem";
            this.purpleToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.purpleToolStripMenuItem.Text = "Purple";
            this.purpleToolStripMenuItem.Click += new System.EventHandler(this.PurpleToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(210, 6);
            // 
            // pinkToolStripMenuItem
            // 
            this.pinkToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.back_Pink;
            this.pinkToolStripMenuItem.Name = "pinkToolStripMenuItem";
            this.pinkToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.pinkToolStripMenuItem.Text = "Pink";
            this.pinkToolStripMenuItem.Click += new System.EventHandler(this.PinkToolStripMenuItem_Click);
            // 
            // redToolStripMenuItem
            // 
            this.redToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.back_Red;
            this.redToolStripMenuItem.Name = "redToolStripMenuItem";
            this.redToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.redToolStripMenuItem.Text = "Red";
            this.redToolStripMenuItem.Click += new System.EventHandler(this.RedToolStripMenuItem_Click);
            // 
            // greenToolStripMenuItem
            // 
            this.greenToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.back_Green;
            this.greenToolStripMenuItem.Name = "greenToolStripMenuItem";
            this.greenToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.greenToolStripMenuItem.Text = "Green";
            this.greenToolStripMenuItem.Click += new System.EventHandler(this.GreenToolStripMenuItem_Click);
            // 
            // blueToolStripMenuItem
            // 
            this.blueToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.back_Blue;
            this.blueToolStripMenuItem.Name = "blueToolStripMenuItem";
            this.blueToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.blueToolStripMenuItem.Text = "Blue";
            this.blueToolStripMenuItem.Click += new System.EventHandler(this.BlueToolStripMenuItem_Click);
            // 
            // yellowToolStripMenuItem
            // 
            this.yellowToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.back_Yellow;
            this.yellowToolStripMenuItem.Name = "yellowToolStripMenuItem";
            this.yellowToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.yellowToolStripMenuItem.Text = "Yellow";
            this.yellowToolStripMenuItem.Click += new System.EventHandler(this.YellowToolStripMenuItem_Click);
            // 
            // whiteToolStripMenuItem
            // 
            this.whiteToolStripMenuItem.Image = global::VirtualDesktopManager.Properties.Resources.back_White;
            this.whiteToolStripMenuItem.Name = "whiteToolStripMenuItem";
            this.whiteToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.whiteToolStripMenuItem.Text = "White";
            this.whiteToolStripMenuItem.Click += new System.EventHandler(this.WhiteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(210, 6);
            // 
            // transparentWhiteTextToolStripMenuItem
            // 
            this.transparentWhiteTextToolStripMenuItem.Name = "transparentWhiteTextToolStripMenuItem";
            this.transparentWhiteTextToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.transparentWhiteTextToolStripMenuItem.Text = "Transparent (White Text)";
            this.transparentWhiteTextToolStripMenuItem.Click += new System.EventHandler(this.TransparentWhiteTextToolStripMenuItem_Click);
            // 
            // transparentBlackTextToolStripMenuItem
            // 
            this.transparentBlackTextToolStripMenuItem.Name = "transparentBlackTextToolStripMenuItem";
            this.transparentBlackTextToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.transparentBlackTextToolStripMenuItem.Text = "Transparent (Black Text)";
            this.transparentBlackTextToolStripMenuItem.Click += new System.EventHandler(this.TransparentBlackTextToolStripMenuItem_Click);
            // 
            // separatorTop
            // 
            this.separatorTop.Name = "separatorTop";
            this.separatorTop.Size = new System.Drawing.Size(177, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // separatorMiddle
            // 
            this.separatorMiddle.Name = "separatorMiddle";
            this.separatorMiddle.Size = new System.Drawing.Size(177, 6);
            // 
            // nextToolStripMenuItem
            // 
            this.nextToolStripMenuItem.Name = "nextToolStripMenuItem";
            this.nextToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.nextToolStripMenuItem.Text = ">> Next >>";
            this.nextToolStripMenuItem.Click += new System.EventHandler(this.nextClick);
            // 
            // previousToolStripMenuItem1
            // 
            this.previousToolStripMenuItem1.Name = "previousToolStripMenuItem1";
            this.previousToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.previousToolStripMenuItem1.Text = "<< Previous <<";
            this.previousToolStripMenuItem1.Click += new System.EventHandler(this.previousClick);
            // 
            // separator
            // 
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(177, 6);
            // 
            // desktopsList
            // 
            this.desktopsList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.desktopsList.Name = "desktopsList";
            this.desktopsList.Size = new System.Drawing.Size(180, 22);
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
        private System.Windows.Forms.ToolStripSeparator separator;
        private System.Windows.Forms.ToolStripSeparator separatorMiddle;
        private System.Windows.Forms.ToolStripMenuItem desktopsList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator separatorTop;
        private System.Windows.Forms.ToolStripMenuItem colorList;
        private System.Windows.Forms.ToolStripMenuItem blackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem darkBlueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem darkGreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem darkRedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem purpleToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem pinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yellowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem whiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem transparentWhiteTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transparentBlackTextToolStripMenuItem;
    }
}

