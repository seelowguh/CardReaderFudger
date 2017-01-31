using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CardReaderFudger.Classes;

namespace CardReaderFudger.Forms
{
    public partial class Configuration : Form
    {
        public Configuration()
        {
            InitializeComponent();
        }

        private Rectangle GetScreen()
        {
            return Screen.PrimaryScreen.Bounds;
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.Manual;
            int x = GetScreen().Width - (this.Width + 20);
            int y = GetScreen().Height - (this.Height + 80);
            this.Location = this.PointToScreen(new Point(x, y));
            this.KeyPreview = true;

            try
            {
                RegistryHandle reg = new RegistryHandle();
                RegistryHandle.Configuration config = reg.ReadConfig();

                txtEventName.Text = config.EventName;
                txtInstallation.Text = config.Installation;
                txtQueueType.Text = config.QueueOptions;
                txtQueueNumber.Text = config.QueueNumber.ToString();
                txtExecution.Text = config.Execution;
                txtLocation.Text = config.Location;
                txtOtherEvent.Text = config.OtherEvent;
                txtUseReader.Text = config.UseReader.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void Configuration_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                RegistryHandle reg = new RegistryHandle();
                RegistryHandle.Configuration config = null;
                int QueueNum;
                bool UseReader;

                config = new RegistryHandle.Configuration(txtEventName.Text, txtInstallation.Text, txtQueueType.Text,
                    txtExecution.Text, int.TryParse(txtQueueNumber.Text, out QueueNum) ? QueueNum : 1, txtLocation.Text, txtOtherEvent.Text, Boolean.TryParse(txtUseReader.Text, out UseReader) ? UseReader : false);

                reg.WriteConfig(config);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
