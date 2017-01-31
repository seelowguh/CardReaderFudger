using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CardReaderFudger.Classes;

namespace CardReaderFudger
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (
                System.Diagnostics.Process.GetProcessesByName(
                    System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location))
                    .Count() > 1)
                System.Diagnostics.Process.GetCurrentProcess().Kill();


            // Show the system tray icon.					
            using (ProcessOption pi = new ProcessOption())
            {
                pi.Display();

                // Make sure the application runs!
                Application.Run();
            }
        }
    }
}
