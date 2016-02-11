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
            Win32_BaseBoard,//����
            Win32_Battery,//���
            Win32_BIOS,//BIOS
            Win32_Bus,//ϵͳ����
            Win32_CDROMDrive,//CD����
            Win32_DiskDrive,//Ӳ������
            Win32_DMAChannel,//�ڴ����ͨ��
            Win32_Fan,//����
            Win32_FloppyController,//���̿�����
            Win32_FloppyDrive,//��������
            Win32_IDEController,//���Ӽ���������
            Win32_IRQResource,//�ж�������Դ
            Win32_Keyboard,//����
            Win32_MemoryDevice,//�ڴ�
            Win32_NetworkAdapter,//����������
            Win32_NetworkAdapterConfiguration,//��������������
            Win32_OnBoardDevice,//��������
            Win32_ParallelPort,//���ж˿�
            Win32_PCMCIController,//�洢��
            Win32_PhysicalMedia,//����ý��
            Win32_PhysicalMemory,//�����ڴ�
            Win32_PortConnector,//�˿�����
            Win32_PortResource,//�˿���Դ
            Win32_Processor,//������
            Win32_SCSIController,//ϵͳ�ӿڿ�����
            Win32_SerialPort,//����
            Win32_SerialPortConfiguration,//��������
            Win32_SoundDevice,//��������
            Win32_SystemEnclosure,//ϵͳ����
            Win32_TapeDrive,//�Ŵ�����
            Win32_TemperatureProbe,//�¶�̽����
            Win32_UninterruptiblePowerSupply,//��ع�Ӧ
            Win32_USBController,//USB������
            Win32_USBHub,//ͨ�ô������ߣ�һ�ֿ��Խ�һ��USB�ӿ���չΪ���
            Win32_VideoController,//��Ƶ������
            Win32_VoltageProbe//��ѹ̽����
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
                //MessageBox.Show ( "����" + RomoteIp + "����������ϢΪ��" + ee.Message ,"���ִ���" ) ;
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