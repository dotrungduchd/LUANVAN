using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Management;
using System.IO;
using System.Xml.Linq;


namespace DemoUSBRemovalDetection
{
    // Source Reference: http://www.codeproject.com/Articles/63878/Enumerate-and-Auto-Detect-USB-Drives
    // Source Reference: http://stackoverflow.com/questions/620144/detecting-usb-drive-insertion-and-removal-using-windows-service-and-c-sharp
    class Program
    {
        static List<string> drives = new List<string>();
        static void Main(string[] args)
        {
            var tmp = new ManagementObjectSearcher("select Description,Name from Win32_LogicalDisk").Get();
            foreach (var drive in tmp)
            {
                if (drive["Description"].ToString().Contains("Removable"))
                    drives.Add(drive["Name"].ToString());
            }

            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
            insertWatcher.Start();

            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            ManagementEventWatcher removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler(DeviceRemovedEvent);
            removeWatcher.Start();
            Console.WriteLine("Press any key to close");
            Console.ReadLine();
        }
        private static void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            Console.Write("Insert - ");
            
            //foreach (var property in instance.Properties)
            //{
            //    Console.WriteLine(property.Name + " = " + property.Value);
            //}
            //foreach (var property in instance.SystemProperties)
            //{
            //    Console.WriteLine(property.Name + " = " + property.Value);
            //}
            //Console.WriteLine(instance["VolumeName"].ToString());
            string name = GetDriveLetter(instance);
            drives.Add(name);
            Console.WriteLine(name);            
        }

        static void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            Console.Write("Remove - ");
            var tmpQuery = new ManagementObjectSearcher("select Description,Name from Win32_LogicalDisk").Get();
            var currDrives = new List<string>();
            foreach (var drive in tmpQuery)
            {
                if (drive["Description"].ToString().Contains("Removable"))
                {
                    currDrives.Add(drive["Name"].ToString());
                }
            }
            for (int i = 0; i < drives.Count; i++)
            {
                if (currDrives.IndexOf(drives[i]) == -1)
                {
                    Console.WriteLine(drives[i]);                    
                    //drives.RemoveAt(i);
                    drives.Clear();
                    drives.AddRange(currDrives);
                    break;
                }
            }
            
            
        }
        public static string GetDriveLetter(ManagementBaseObject instance)
        {
            var tmp = new ManagementObjectSearcher("select * from Win32_DiskDrive where InterfaceType='USB'").Get();
            var deviceid = instance["DeviceID"].ToString();
            foreach (var drive in tmp)
            {
                if(drive["PNPDeviceID"].ToString().Contains(deviceid.Remove(0,deviceid.LastIndexOf("\\")+1)))
                {
                    ManagementObject partition = new ManagementObjectSearcher(String.Format(
            "associators of {{Win32_DiskDrive.DeviceID='{0}'}} " +
                  "where AssocClass = Win32_DiskDriveToDiskPartition",
            drive["DeviceID"])).First();

                    if (partition != null)
                    {
                        // associate partitions with logical disks (drive letter volumes)
                        ManagementObject logical = new ManagementObjectSearcher(String.Format(
                            "associators of {{Win32_DiskPartition.DeviceID='{0}'}} " +
                                "where AssocClass= Win32_LogicalDiskToPartition",
                            partition["DeviceID"])).First();

                        if (logical != null)
                        {
                            // finally find the logical disk entry                          

                            return logical["Name"].ToString();
                        }
                    }                   
                }
            }
            return "NotFound";
        }
        
    }
}
