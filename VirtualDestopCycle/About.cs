using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace VirtualDesktopManager
{
    partial class About : Form
    {
        int ii, vv;
        public About() { InitializeComponent(); }
        private void About_Load(object sender, EventArgs e) { ii = 0; }
        private void Labeld2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => clickLink(labeld2, "https://github.com/iskmz/VirtualDesktopManager");
        private void Labeld4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => clickLink(labeld4, "https://github.com/m0ngr31/VirtualDesktopManager");
        private void Labeld6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => clickLink(labeld6, "https://icons8.com");
        private void clickLink(LinkLabel lbl, string linkStr) { lbl.LinkVisited = true; System.Diagnostics.Process.Start(linkStr); }

        // credits/source: https://dotnetthoughts.net/how-to-apply-border-color-for-tablelayoutpanel/ //
        private void TableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            var panel = sender as TableLayoutPanel;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            var rectangle = e.CellBounds;
            using (var pen = new Pen(Color.White, 1))
            {
                pen.Alignment = PenAlignment.Center;
                pen.DashStyle = DashStyle.Solid;
                if (e.Row == (panel.RowCount - 1)) rectangle.Height -= 1;
                if (e.Column == (panel.ColumnCount - 1)) rectangle.Width -= 1;
                if ((e.Row == 0 || e.Row==1) && e.Column == e.Row) e.Graphics.DrawRectangle(pen, rectangle);
            }
            vv = int.Parse(labelVersion.Text.Substring(labelVersion.Text.LastIndexOf(".") + 1));
        }

        private void BtnHotkeysList_Click(object sender, EventArgs e)
        {
            string dot = "\n\n\u25CF ";

            string msg = ""
                + dot + "Ctrl+Alt+Right/Left: move to right/left desktop , with wraping/cycling when reaching edges; "
                + "alternate combination is Alt+Shift+Right/Left , to be chosen in Settings."
                + dot + "Ctrl+Alt+Digit[1-9]: move to desktop with digit selected; "
                + "alternate combination is Alt+Shift+Digit[1-9] , to be chosen in Settings."
                + dot + "special combination only when cycling (or reverse cycling) "
                + "FOREVER, in order to stop cycling is: Ctrl+Alt+S"
                + dot + "as of version 2.4, "
                + "default windows combination: Ctrl+Winkey+Right/Left "
                + "is overriden to make desktops wrap/cycle when reaching edges "
                + "{also touchpad-4-fingers swipe Right/Left is overriden}; "
                + "in order to Deactivate/Activate this override feature use "
                + "the following combination: Ctrl+Alt+Shift+S\n\n";

            MessageBox.Show(msg, "Hotkeys List", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void Pb1_Click(object sender, EventArgs e)
        {
            ii++;
            if (ii == 11)
            {
                ii = 0;
                if (vv == 11)
                {
                    pb1.Image = Properties.Resources.IM_; pb1.Refresh();
                    await Task.Delay(1111);
                    pb1.Image = Properties.Resources.IM; pb1.Refresh();
                }
            }
        }
    }
}