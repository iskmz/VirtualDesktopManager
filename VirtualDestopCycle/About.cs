using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VirtualDesktopManager
{
    partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }


        private void About_Load(object sender, EventArgs e) { }

        private void About_Paint(object sender, PaintEventArgs e) { }


        private void Labeld2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            labeld2.LinkVisited = true;
            System.Diagnostics.Process.Start("https://github.com/iskmz/VirtualDesktopManager");
        }

        private void Labeld4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            labeld4.LinkVisited = true;
            System.Diagnostics.Process.Start("https://github.com/m0ngr31/VirtualDesktopManager");
        }

        private void Labeld6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            labeld6.LinkVisited = true;
            System.Diagnostics.Process.Start("https://icons8.com");
        }

        // credits/source: https://dotnetthoughts.net/how-to-apply-border-color-for-tablelayoutpanel/ //
        private void TableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            var panel = sender as TableLayoutPanel;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            var rectangle = e.CellBounds;
            using (var pen = new Pen(Color.White, 1))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                if (e.Row == (panel.RowCount - 1)) rectangle.Height -= 1;
                if (e.Column == (panel.ColumnCount - 1)) rectangle.Width -= 1;
                if (e.Row == 0 && e.Column == 0) e.Graphics.DrawRectangle(pen, rectangle);
                if (e.Row == 1 && e.Column == 1) e.Graphics.DrawRectangle(pen, rectangle);
            }
        }

    }
}
