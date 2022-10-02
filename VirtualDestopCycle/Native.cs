using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDesktopManager
{
    internal static class Native
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SystemParametersInfo(
            uint uAction,
            uint uParam,
            string lpvParam,
            uint fuWinIni);

        public static void SetBackground(string pictureFile)
        {
            const uint SPI_SETDESKWALLPAPER = 0x0014;
            const uint SPIF_UPDATEINIFILE = 1;
            const uint SPIF_SENDWININICHANGE = 2;

            SystemParametersInfo(
                SPI_SETDESKWALLPAPER,
                0,
                pictureFile,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
