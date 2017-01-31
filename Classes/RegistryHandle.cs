using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace CardReaderFudger.Classes
{
    internal class RegistryHandle
    {
        private RegistryKey regKey = Registry.CurrentConfig.CreateSubKey("Sensata").CreateSubKey("RFIDCardReader");

        internal class Configuration
        {
            internal string EventName;
            internal string Installation;
            internal string QueueOptions;
            internal string Execution;
            internal int QueueNumber;
            internal string Location;
            internal string OtherEvent;
            internal bool UseReader;

            internal Configuration(string e, string i, string q, string ex, int qn, string L, string OE)
            {
                EventName = e;
                Installation = i;
                QueueOptions = q;
                Execution = ex;
                QueueNumber = qn;
                Location = L;
                OtherEvent = OE;
                UseReader = false;
            }

            internal Configuration(string e, string i, string q, string ex, int qn, string L, string OE, bool UR)
            {
                EventName = e;
                Installation = i;
                QueueOptions = q;
                Execution = ex;
                QueueNumber = qn;
                Location = L;
                OtherEvent = OE;
                UseReader = UR;
            }
        }

        internal void WriteConfig(Configuration x)
        {
            try
            {
                writeReg("EventName", x.EventName);
                writeReg("Installation", x.Installation);
                writeReg("QueueType", x.QueueOptions);
                writeReg("Execution", x.Execution);
                writeReg("QueueNumber", x.QueueNumber.ToString());
                writeReg("Location", x.Location);
                writeReg("OtherEvent", x.OtherEvent);
                writeReg("UseCardReader", x.UseReader.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal Configuration ReadConfig()
        {
            Configuration c = null;
            try
            {
                if (readReg("EventName").ToString() == string.Empty)
                    c = new Configuration("", "", "", "", 0, "", "");
                else
                {
                    bool UseReader;
                    if (!Boolean.TryParse(readReg("UseCardReader").ToString(), out UseReader))
                        UseReader = false;

                     c = new Configuration(readReg("EventName").ToString(), readReg("Installation").ToString()
                        , readReg("QueueType").ToString(), readReg("Execution").ToString(), Convert.ToInt32(readReg("QueueNumber"))
                        , readReg("Location").ToString(), readReg("OtherEvent").ToString(), UseReader);
                }
                   
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return c;
        }

        private void writeReg(string Key, string Value)
        {
            regKey.SetValue(Key,Value);
        }

        private object readReg(string Key)
        {
            return regKey.GetValue(Key, "");
        }
    }
}
