using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Net;

namespace InfaBGInfo
{
    class WindowsSystemInfo
    {

        string[] sArrayOfSysProps = { "Boot Time", "CPU", "Default Gateway"
                                        , "DCHP Server", "DNS Server", "Free Space", "MAC Address", "Machine Domain"
                                        , "Memory"
                                        , "OS Version", "Service Pack", "System Type", "User Name", "Volumes"};


        private string sSystemInfo = String.Empty;
        private StringBuilder sb = new StringBuilder();

        public WindowsSystemInfo() { }


        public string GetSystemInfoString()
        {
            //GetStuff("Win32_OperatingSystem");
            windowsNetworkInfo();

            return sb.ToString();
        }


        private void windowsSystemInfoBase()
        {
            string tmpReturn = String.Empty;
            // Add each property of the SystemInformation class to the list box.
            Type t = typeof(System.Windows.Forms.SystemInformation);            
            PropertyInfo[] pi = t.GetProperties();

            
            for (int i = 0; i < pi.Length; i++)
            {
                foreach (string s in sArrayOfSysProps)
                {
                    if (s.ToLower() == pi[i].Name.ToLower())
                    {
                        object propval = pi[i].GetValue(null, null);

                        tmpReturn += pi[i].Name + ":".PadRight(16, ' ') + "\t" + propval.ToString();
                        tmpReturn += "\n";

                        continue;
                    }
                }
            }

            sb.AppendLine(tmpReturn);
        }


        private void windowsNetworkInfo()
        {
            string tmpReturn = String.Empty;

            // Get host name
            String strHostName = Dns.GetHostName();
            tmpReturn += "Host Name:".PadRight(16, ' ') + "\t" + strHostName;
            tmpReturn += "\n";

            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostByName(strHostName);

            // Enumerate IP addresses
            int nIP = 0;
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                tmpReturn += "IP #" + ++nIP + ":".PadRight(16, ' ') + "\t" + ipaddress.ToString();
            }
            tmpReturn += "\n";

            tmpReturn += "Machine:".PadRight(16, ' ') + "\t" + System.Environment.MachineName.ToString();
            tmpReturn += "\n";
            tmpReturn += "OS Version:".PadRight(16, ' ') + "\t" + System.Environment.OSVersion.ToString();
            tmpReturn += "\n";
            tmpReturn += "# of Processors:".PadRight(16, ' ') + "\t" + System.Environment.ProcessorCount.ToString();
            tmpReturn += "\n";
            tmpReturn += "User Name:".PadRight(16, ' ') + "\t" + System.Environment.UserName.ToString();
            
            sb.Append(tmpReturn);
        }


        public void GetStuff(string queryObject)
        {
            string tmpReturn = String.Empty;
            ManagementObjectSearcher searcher;
            int i = 0;
            ArrayList hd = new ArrayList();
            try
            {
                searcher = new ManagementObjectSearcher("SELECT * FROM " + queryObject);

                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    i++;
                    PropertyDataCollection searcherProperties = wmi_HD.Properties;
                    foreach (PropertyData sp in searcherProperties)
                    {
                        tmpReturn += sp.Name + ":".PadRight(20, ' ') + "\t" + sp.Value.ToString();
                        tmpReturn += "\n";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sb.AppendLine(tmpReturn);
            }
            //return hd;
        }
    }
}
