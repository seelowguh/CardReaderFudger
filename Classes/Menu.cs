using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CardReaderFudger.Forms;
using CardReaderFudger.Properties;

namespace CardReaderFudger.Classes
{
    class Menu
    {
        public ContextMenuStrip Create()
        {
            // Add the default menu options.
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;

            // Configure
            item = new ToolStripMenuItem();
            item.Text = "Configuration";
            item.Click += new EventHandler(Configuration_Click);
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Exit.
            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new System.EventHandler(Exit_Click);
            menu.Items.Add(item);

            return menu;
        }

        private void Configuration_Click(object sender, EventArgs e)
        {
            //  Log task
            Configuration Config = new Configuration();
            Config.Show();
        }

        void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
