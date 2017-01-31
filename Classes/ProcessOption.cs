using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CardReaderFudger.Forms;
using CardReaderFudger.Properties;

namespace CardReaderFudger.Classes
{
    internal class ProcessOption : IDisposable
    {
        private NotifyIcon ni;

        public ProcessOption()
        {
            ni = new NotifyIcon();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool DestroyIcon(IntPtr handle);

        public void Display()
        {
            IntPtr hIcon = Resources.bolt.GetHicon();
            Icon formIcon = Icon.FromHandle(hIcon);

            ni.MouseDoubleClick += new MouseEventHandler(ni_MouseDoubleClick);
            ni.Text = "RF Options";
            ni.Icon = formIcon;
            ni.Visible = true;

            DestroyIcon(formIcon.Handle);

            ni.ContextMenuStrip = new Classes.Menu().Create();
        }

        private void ni_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //  Log task
                ProcessID PO = new ProcessID();
                PO.Show();
            }
        }

        public void Dispose()
        {
            ni.Dispose();
        }
    }
}