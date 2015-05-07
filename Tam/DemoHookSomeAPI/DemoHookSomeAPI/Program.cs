using EasyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace DemoHookSomeAPI
{
    public class FileMonInterface : MarshalByRefObject
    {
        public void IsInstalled(Int32 InClientPID)
        {
            Console.WriteLine("FileMon has been installed in target {0}.\r\n", InClientPID);
        }

        public void OnWriteFile(Int32 InClientPID, String[] InFileNames)
        {
            for (int i = 0; i < InFileNames.Length; i++)
            {
                Console.WriteLine(InFileNames[i]);
            }
        }

        public void ReportException(Exception InInfo)
        {
            Console.WriteLine("The target process has reported an error:\r\n" + InInfo.ToString());
        }

        public void OnCreateProcess(Int32 InClientPID, Int32[] pIds,int oldThrId,ThreadPriorityLevel oldThrLvl ,string ChannelName)
        {
            for (int i = 0; i < pIds.Length; i++)
            {
                try
                {
                    RemoteHooking.Inject(pIds[i],
                        "InjectDLL.dll",
                        "InjectDLL.dll",
                        ChannelName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Process pro = Process.GetProcessById(pIds[i]);
                foreach (ProcessThread thread in pro.Threads)
                {
                    if(thread.Id == oldThrId)
                        thread.PriorityLevel = oldThrLvl;
                }
            }
        }

        public string[] getExtensions()
        {
            return Global.extensions;
        }

        //public void OnCreateProcess(Int32 InClientPID, string lpApplicationName, string lpCommandLine, string ChannelName)
        //{
        //    PROCESS_INFORMATION pInfo = new PROCESS_INFORMATION();
        //    STARTUPINFO sInfo = new STARTUPINFO();
        //    SECURITY_ATTRIBUTES pSec = new SECURITY_ATTRIBUTES();
        //    SECURITY_ATTRIBUTES tSec = new SECURITY_ATTRIBUTES();
        //    try
        //    {
        //        //bool retValue = GlobalHelper.CreateProcess(lpApplicationName, lpCommandLine,
        //        //ref pSec, ref tSec, false, 0x00000004,
        //        //IntPtr.Zero, null, ref sInfo, out pInfo);
        //        int outProcessID = 0;
        //        RemoteHooking.CreateAndInject(lpApplicationName, lpCommandLine, (int)0x00000004, "InjectDLL.dll", "InjectDLL.dll", out outProcessID, ChannelName);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }
        //}
          
        public void Ping()
        {
        }

    }
    public class Global
    {
        public static string[] extensions = { ".rtf", ".txt",".docx",".doc",".mp3",".png",".jpg",".bmp",".flv",".jpeg",".exe",".mkv",".pdf",
                                   ".xls",".cs",".xlsx",".ppt",".pptx"};
    }
    class Program
    {
        static String ChannelName = null;        
        
        static void Main(string[] args)
        {
            
            Int32 TargetPID = 0;

            if ((args.Length != 1) || !Int32.TryParse(args[0], out TargetPID))
            {
                Console.WriteLine();
                Console.WriteLine("Usage: FileMon %PID%");
                Console.WriteLine();

                return;
            }

            try
            {
                Config.Register(
                    "A DemoHookWriteFile like demo application.",
                    "DemoHookSomeAPI.exe",
                    "InjectDLL.dll");

                RemoteHooking.IpcCreateServer<FileMonInterface>(ref ChannelName, WellKnownObjectMode.Singleton);

                //RemoteHooking.InstallSupportDriver();                

                RemoteHooking.Inject(
                    TargetPID,
                    "InjectDLL.dll",
                    "InjectDLL.dll",
                    ChannelName);
                
                Console.ReadLine();
            }
            catch (Exception ExtInfo)
            {
                Console.WriteLine("There was an error while connecting to target:\r\n{0}", ExtInfo.ToString());
            }
        }
    }
}
