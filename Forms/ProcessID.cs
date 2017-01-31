using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CardReaderFudger.Classes;
using SchraderElectronics.SEL_InterfaceLibrary;
using Timer = System.Timers.Timer;
using System.Xml;

namespace CardReaderFudger.Forms
{
    public partial class ProcessID : Form
    {

        public ProcessID()
        {
            InitializeComponent();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (CheckID(txtUsername.Text))
            {
                ViewOptions(txtUsername.Text);
                this.Close();
            }
            else
            {
                MessageBox.Show("Username is Invalid");
                this.Close();
            }
        }

        private bool CheckID(string ID)
        {
            //  Commented back out to enforce sensata ids
//            return true;

            string leftChar = ID.Substring(0, 1).ToLower();
            if (leftChar == "a" || leftChar == "x")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Process button
        private void ViewOptions(string ID)
        {
            string message = string.Empty;
            RegistryHandle reg = new RegistryHandle();
            RegistryHandle.Configuration config = reg.ReadConfig();

            if (config.EventName == string.Empty)
            {
                MessageBox.Show("No configuration setup");
                this.Close();
                return;
            }

            /*
            if (!CheckRouting(config.Installation, config.EventName))
            {
                MessageBox.Show("Routing is incorrect.");
                this.Close();
                return;
            }
            */

            message = string.Format("<Message><CUMS_AuthorityRFID><SEL_ID>{0}</SEL_ID><STATION_NAME>{1}</STATION_NAME>" +
                                    "<LOCAL_USER>{2}</LOCAL_USER><TIME_STAMP>{3}</TIME_STAMP><LOCATION>{4}</LOCATION>" +
                                    "<OTHER_EVENT>{5}</OTHER_EVENT></CUMS_AuthorityRFID></Message>", ID, Environment.MachineName, 
                                    Environment.UserName, DateTime.Now, config.Location, config.OtherEvent);

            if (clsClarifi.CheckAgentStatus() != clsClarifi.AgentStatus.Started && message != String.Empty)
            {
                MessageBox.Show("Clarif-i Agent Stopped");
                this.Close();
                return;
            }
            else
            {
                clsClarifi.TriggerAgent(config.EventName, config.Installation, message, config.Execution,
                    config.QueueOptions, config.QueueNumber);
                this.Close();
                return;
            }
        }

        private Rectangle GetScreen()
        {
            return Screen.PrimaryScreen.Bounds;
        }

        private void ProcessID_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.Manual;
            int x = GetScreen().Width - (this.Width + 20);
            int y = GetScreen().Height - (this.Height + 80);
            this.Location = this.PointToScreen(new Point(x, y));
            this.KeyPreview = true;

            txtUsername.Focus();
            Application.DoEvents();
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnProcess.PerformClick();
            }
        }

        private int TimerCount = 0;
        private void timClose_Tick(object sender, EventArgs e)
        {
            TimerCount ++;
            if (TimerCount >= 20)
            {
                this.Close();
            }
        }

        private bool CheckRouting(string Installation, string EventName)
        {
            bool EventFound = false;
            string RoutingPath = @"C:\Clarif-i\ADOData\RoutingTable.xml";
            XDocument xdoc = null;
            
            if (!File.Exists(RoutingPath))
                return EventFound;

            using (StreamReader sr = new StreamReader(RoutingPath))
            {
                xdoc = XDocument.Load(sr.BaseStream);
                sr.Close();
                sr.Dispose();
            }

            if (xdoc != null)
            {
                string TXID = xdoc.Descendants("xml")
                    .Where(x => (string) x.Attribute("EventName") == EventName
                                && (string) x.Attribute("Installation") == Installation)
                    .Select(x => (string) x.Attribute("TXID")).FirstOrDefault();

                if(TXID != null)
                    EventFound = true;
            }
                
            return EventFound;
        }

    }
}
