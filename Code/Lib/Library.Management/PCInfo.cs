using Library.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace Library.Management
{
    public class PCInfo
    {
        private enum Win32Hardware
        {
            Win32_BaseBoard,//主板
            Win32_Battery,//电池
            Win32_BIOS,//BIOS
            Win32_Bus,//系统总线
            Win32_CDROMDrive,//CD驱动
            Win32_DiskDrive,//硬盘驱动
            Win32_DMAChannel,//内存访问通道
            Win32_Fan,//风扇
            Win32_FloppyController,//软盘控制器
            Win32_FloppyDrive,//软盘驱动
            Win32_IDEController,//电子集成驱动器
            Win32_IRQResource,//中断请求资源
            Win32_Keyboard,//键盘
            Win32_MemoryDevice,//内存
            Win32_NetworkAdapter,//网络适配器
            Win32_NetworkAdapterConfiguration,//网络适配器配置
            Win32_OnBoardDevice,//主板驱动
            Win32_ParallelPort,//并行端口
            Win32_PCMCIController,//存储卡
            Win32_PhysicalMedia,//物理媒体
            Win32_PhysicalMemory,//物理内存
            Win32_PortConnector,//端口连接
            Win32_PortResource,//端口资源
            Win32_Processor,//处理器
            Win32_SCSIController,//系统接口控制器
            Win32_SerialPort,//串口
            Win32_SerialPortConfiguration,//串口配置
            Win32_SoundDevice,//声卡驱动
            Win32_SystemEnclosure,//系统类型
            Win32_TapeDrive,//磁带驱动
            Win32_TemperatureProbe,//温度探测器
            Win32_UninterruptiblePowerSupply,//电池供应
            Win32_USBController,//USB控制器
            Win32_USBHub,//通用串行总线，一种可以将一个USB接口扩展为多个
            Win32_VideoController,//视频控制器
            Win32_VoltageProbe//电压探测器
        }

        public PCInfo([NotNull] string romoteIp, [NotNull] string adminName, [NotNull] string password)
        {
            if (string.IsNullOrEmpty(romoteIp)) throw new ArgumentNullException("romoteIp");
            if (string.IsNullOrEmpty(adminName)) throw new ArgumentNullException("adminName");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            this.AdminName = adminName;
            this.Password = password;
            this.RomoteIp = romoteIp;
        }

        public PCInfo()
        {
        }

        public DiskDeiver[] GetDiskDeivers()
        {
            ManagementObjectSearcher disks = new ManagementObjectSearcher(Connection(), new ObjectQuery("SELECT * FROM Win32_DiskDrive"));
            List<DiskDeiver> list = new List<DiskDeiver>();
            foreach (ManagementObject disk in disks.Get())
            {
                var diskdev = new DiskDeiver();
                list.Add(diskdev);

                var sn = disk.GetPropertyValue("SerialNumber") as string;
                diskdev.SerialNumber = string.IsNullOrEmpty(sn) ? parseSerialFromDeviceID(disk["PNPDeviceID"].ToString()) : sn;
                diskdev.Model = disk.GetPropertyValue("Model") as string;
                diskdev.InterfaceType = disk.GetPropertyValue("InterfaceType") as string;

                if (!string.IsNullOrEmpty(diskdev.SerialNumber)) diskdev.SerialNumber = diskdev.SerialNumber.Trim();
                if (!string.IsNullOrEmpty(diskdev.Model)) diskdev.Model = diskdev.Model.Trim();
                if (!string.IsNullOrEmpty(diskdev.InterfaceType)) diskdev.InterfaceType = diskdev.InterfaceType.Trim();
            }
            return list.ToArray();
        }

        public string AdminName { get; protected set; }
        protected string Password { get; set; }
        public string RomoteIp { get; protected set; }

        private ManagementScope Connection()
        {
            if (string.IsNullOrEmpty(AdminName) && string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(RomoteIp)) return null;
            try
            {
                ConnectionOptions Conn = new ConnectionOptions
                {
                    Username = AdminName,
                    Password = Password
                };

                var ms = new ManagementScope(@"\\" + RomoteIp + @"\root\cimv2", Conn);
                ms.Connect();
                if (ms.IsConnected)
                    return ms;
                return null;
            }
            catch (Exception ee)
            {
                Trace.TraceError(ee.Message);
                //MessageBox.Show ( "连接" + RomoteIp + "出错，出错信息为：" + ee.Message ,"出现错误！" ) ;
                return null;
            }
        }

        public NetworkDeiver[] GetNetworkDeivers()
        {
            List<NetworkDeiver> mak = new List<NetworkDeiver>();
            ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher(Connection(), new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration where IPEnabled=True"));

            foreach (ManagementObject mo in objectSearcher.Get())
            {
                var item = new NetworkDeiver();
                mak.Add(item);
                item.MacAddress = mo["MacAddress"] as string;
                item.DHCPEnabled = (Boolean)mo["DHCPEnabled"];
                item.IPAddress = mo["IPAddress"] as string[];
                item.DefaultIPGateway = mo["DefaultIPGateway"] as string[];
                item.DHCPServer = mo["DHCPServer"] as string;
                item.Description = mo["Description"] as string;
                item.ServiceName = mo["ServiceName"] as string;
                item.Caption = mo["Caption"] as string;
            }
            return mak.ToArray();
        }

        private static string parseSerialFromDeviceID(string deviceId)
        {
            string[] splitDeviceId = deviceId.Split('\\');
            int arrayLen = splitDeviceId.Length - 1;

            var serialArray = splitDeviceId[arrayLen].Split('&');
            var serial = serialArray[0];

            return serial;
        }
    }
}