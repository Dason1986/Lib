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

        string _serialNumber;
        string _driveLetter;

        public static string GetSerialNumber(string drive)
        {
            var usb = new USBDeiver();
            return usb.getSerialNumberFromDriveLetter(drive);
        }
        string getSerialNumberFromDriveLetter(string driveLetter)
        {
            this._driveLetter = driveLetter.ToUpper();

            if (!this._driveLetter.Contains(":"))
            {
                this._driveLetter += ":";
            }

            matchDriveLetterWithSerial();

            return this._serialNumber;
        }

        private void matchDriveLetterWithSerial()
        {
            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDiskToPartition");
            foreach (ManagementObject dm in searcher1.Get())
            {
                string[] diskArray = null;
                var driveLetter = getValueInQuotes(dm["Dependent"].ToString());
                diskArray = getValueInQuotes(dm["Antecedent"].ToString()).Split(',');
                var driveNumber = diskArray[0].Remove(0, 6).Trim();
                //foreach (var property in dm.Properties)
                //{
                //    Console.WriteLine("{0}:{1}", property.Name, getValueInQuotes(dm[property.Name].ToString()));
                //}
                //Console.WriteLine();
                if (driveLetter != this._driveLetter) continue;
                /* This is where we get the drive serial */
                ManagementObjectSearcher disks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject disk in disks.Get())
                {
                    //foreach (var property in disk.Properties)
                    //{
                    //    Console.WriteLine("{1}:{0}", disk.GetPropertyValue(property.Name), property.Name);

                    //}


                    Console.WriteLine();
                    if (disk["Name"].ToString() == (@"\\.\PHYSICALDRIVE" + driveNumber) & disk["InterfaceType"].ToString() == "USB")
                    {
                        var sn = disk.GetPropertyValue("SerialNumber") as string;
                        this._serialNumber = sn ?? parseSerialFromDeviceID(disk["PNPDeviceID"].ToString());
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

            int posFoundStart = 0;
            int posFoundEnd = 0;

            posFoundStart = inValue.IndexOf("\"");
            posFoundEnd = inValue.IndexOf("\"", posFoundStart + 1);
            if (posFoundStart == -1 || posFoundEnd == -1) return inValue;
            parsedValue = inValue.Substring(posFoundStart + 1, (posFoundEnd - posFoundStart) - 1);

            return parsedValue;
        }

    }
}