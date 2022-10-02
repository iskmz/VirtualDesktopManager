namespace VirtualDesktopManager
{
    partial class DesktopPopUp
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
            this.tb = new System.Windows.Forms.TextBox();
            this.timerSplash = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tb
            // 
            this.tb.BackColor = System.Drawing.Color.Black;
            this.tb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb.ForeColor = System.Drawing.Color.White;
            this.tb.Location = new System.Drawing.Point(9, 9);
            this.tb.Multiline = true;
            this.tb.Name = "tb";
            this.tb.ReadOnly = true;
            this.tb.Size = new System.Drawing.Size(253, 59);
            this.tb.TabIndex = 0;
            this.tb.TabStop = false;
            this.tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // timerSplash
            // 
            this.timerSplash.Enabled = true;
            this.timerSplash.Interval = 2500;
            this.timerSplash.Tick += new System.EventHandler(this.TimerSplash_Tick);
            // 
            // DesktopPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(271, 77);
            this.ControlBox = false;
            this.Controls.Add(this.tb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DesktopPopUp";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DesktopPopUp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb;
        private System.Windows.Forms.Timer timerSplash;
    }
}