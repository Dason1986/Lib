using System;
using System.Management;

namespace Library.Management
{
    /*
    model base
    source: http://www.cfdan.com/posts/Retrieving_Non-Volatile_USB_Serial_Number_Using_C_Sharp.cfm
     */

    public class USBDeiver
    {
        public string SerialNumber { get; protected set; }
        public string Model { get; protected set; }

        public string DriveLetter { get; protected set; }

        private USBDeiver(string drive)
        {
            var driveLetter = drive.ToUpper();

            if (!driveLetter.Contains(":"))
            {
                driveLetter += ":";
            }
            DriveLetter = driveLetter;
        }

        public static USBDeiver GetInfo(string drive)
        {
            if (drive == null) throw new ArgumentNullException("drive");

            var usb = new USBDeiver(drive);

            usb.GetInfo();
            return usb;
        }

        private void GetInfo()
        {
            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDiskToPartition");
            foreach (ManagementObject dm in searcher1.Get())
            {
                string[] diskArray = null;
                var driveLetter = getValueInQuotes(dm["Dependent"].ToString());
                diskArray = getValueInQuotes(dm["Antecedent"].ToString()).Split(',');
                var driveNumber = diskArray[0].Remove(0, 6).Trim();

                if (driveLetter != DriveLetter) continue;
                ManagementObjectSearcher disks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject disk in disks.Get())
                {
                    if (disk["Name"].ToString() == (@"\\.\PHYSICALDRIVE" + driveNumber) & disk["InterfaceType"].ToString() == "USB")
                    {
                        var sn = disk.GetPropertyValue("SerialNumber") as string;
                        SerialNumber = string.IsNullOrEmpty(sn) ? parseSerialFromDeviceID(disk["PNPDeviceID"].ToString()) : sn;
                        Model = disk.GetPropertyValue("Model") as string;
                        if (!string.IsNullOrEmpty(SerialNumber)) SerialNumber = SerialNumber.Trim();
                        if (!string.IsNullOrEmpty(Model)) Model = Model.Trim();

                        return;
                    }
                }
            }
        }

        private static string parseSerialFromDeviceID(string deviceId)
        {
            string[] splitDeviceId = deviceId.Split('\\');
            int arrayLen = splitDeviceId.Length - 1;

            var serialArray = splitDeviceId[arrayLen].Split('&');
            var serial = serialArray[0];

            return serial;
        }

        private static string getValueInQuotes(string inValue)
        {
            string parsedValue = "";

            var posFoundStart = inValue.IndexOf("\"", StringComparison.Ordinal);
            var posFoundEnd = inValue.IndexOf("\"", posFoundStart + 1, StringComparison.Ordinal);
            if (posFoundStart == -1 || posFoundEnd == -1) return inValue;
            parsedValue = inValue.Substring(posFoundStart + 1, (posFoundEnd - posFoundStart) - 1);

            return parsedValue;
        }
    }
}