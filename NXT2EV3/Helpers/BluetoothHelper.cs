using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace NXT2EV3.Helpers
{
    struct BluetoothDevice
    {
        public string Name;
        public string Port;

        public override string ToString()
        {
            return $"{Name} ({Port})";
        }
    }

    static class BluetoothHelper
    {
        public static List<BluetoothDevice> GetBluetoothPort()
        {
            Regex regexPortName = new Regex(@"(COM\d+)");

            List<BluetoothDevice> portList = new List<BluetoothDevice>();

            ManagementObjectSearcher searchSerial = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");

            foreach (ManagementObject obj in searchSerial.Get())
            {
                string name = obj["Name"] as string;
                string classGuid = obj["ClassGuid"] as string;
                string deviceID = obj["DeviceID"] as string;

                if (classGuid != null && deviceID != null)
                {
                    if (String.Equals(classGuid, "{4d36e978-e325-11ce-bfc1-08002be10318}", StringComparison.InvariantCulture))
                    {
                        string[] tokens = deviceID.Split('&');

                        if (tokens.Length >= 4)
                        {
                            string[] addressToken = tokens[4].Split('_');
                            string bluetoothAddress = addressToken[0];

                            Match m = regexPortName.Match(name);
                            string comPortNumber = "";
                            if (m.Success)
                            {
                                comPortNumber = m.Groups[1].ToString();
                            }

                            if (Convert.ToUInt64(bluetoothAddress, 16) > 0)
                            {
                                string bluetoothName = GetBluetoothRegistryName(bluetoothAddress).Replace("\0", "");
                                portList.Add(new BluetoothDevice {Name = bluetoothName, Port = comPortNumber});
                            }
                        }
                    }
                }
            }

            return portList;
        }

        private static string GetBluetoothRegistryName(string address)
        {
            string deviceName = "";

            string registryPath = @"SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices";
            string devicePath = String.Format(@"{0}\{1}", registryPath, address);

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(devicePath))
            {
                if (key != null)
                {
                    Object o = key.GetValue("Name");

                    byte[] raw = o as byte[];

                    if (raw != null)
                    {
                        deviceName = Encoding.ASCII.GetString(raw);
                    }
                }
            }

            return deviceName;
        }
    }
}
