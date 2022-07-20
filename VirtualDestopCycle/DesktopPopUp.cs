using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualDesktopManager
{
    public partial class DesktopPopUp : Form
    {
        public TextBox txtBox { get => tb; }

        public DesktopPopUp()
        {
            InitializeComponent();
        }

        private void DesktopPopUp_Load(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        public void showMe(string txt)
        {
            this.txtBox.Text = txt;
            this.ShowDialog();
            timerSplash.Start();
        }

        private void TimerSplash_Tick(object sender, EventArgs e)
        {
            if (Visible)
            {
                Invoke(new Action(() => { this.Hide(); }));
            }
        }
    }
}
