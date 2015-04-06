using EasyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoHookSomeAPI;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;

namespace InjectDLL
{
    public class Main : IEntryPoint
    {
        FileMonInterface Interface;
        LocalHook WriteFileHook, CreateFileHook, ReadFileHook;
        Stack<String> Queue = new Stack<String>();

        public Main(
             RemoteHooking.IContext InContext,
             String InChannelName)
        {
            // connect to host...
            Interface = RemoteHooking.IpcConnectClient<FileMonInterface>(InChannelName);

            Interface.Ping();
        }

        public void Run(
            RemoteHooking.IContext InContext,
            String InChannelName)
        {
            // install hook...
            try
            {
                //WriteFileHook = LocalHook.Create(
                //    LocalHook.GetProcAddress("kernel32.dll", "WriteFile"),
                //    new DWriteFile(WriteFile_Hooked),
                //    this);

                //CreateFileHook = LocalHook.Create(
                //    LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"),
                //    new DCreateFile(CreateFile_Hooked),
                //    this);

                ReadFileHook = LocalHook.Create(LocalHook.GetProcAddress("kernel32.dll", "ReadFile"),
                    new DReadFile(ReadFile_Hooked),
                    this);

                ReadFileHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                //WriteFileHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                //CreateFileHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            }
            catch (Exception ExtInfo)
            {
                Interface.ReportException(ExtInfo);

                return;
            }
            ReadFileFunc = LocalHook.GetProcDelegate<DReadFile>("kernel32.dll", "ReadFile");

            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());

            RemoteHooking.WakeUpProcess();

            // wait for host process termination...
            try
            {
                while (true)
                {
                    Thread.Sleep(500);

                    // transmit newly monitored file accesses...
                    if (Queue.Count > 0)
                    {
                        String[] Package = null;

                        lock (Queue)
                        {
                            Package = Queue.ToArray();

                            Queue.Clear();
                        }

                        Interface.OnWriteFile(RemoteHooking.GetCurrentProcessId(), Package);
                    }
                    else
                        Interface.Ping();
                }
            }
            catch
            {
                // Ping() will raise an exception if host is unreachable
            }
        }
        #region KhaiBaoDelegate
        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Auto,
            SetLastError = true)]
        delegate int DDrawText(IntPtr hDC, string lpString, int nCount, ref RECT lpRect, uint uFormat);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
           CharSet = CharSet.Auto,
           SetLastError = true)]
        delegate bool DSetWindowText(IntPtr hwnd, string lpString);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
           CharSet = CharSet.Auto,
           SetLastError = true)]
        delegate int DSetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
           CharSet = CharSet.Auto,
           SetLastError = true)]
        delegate bool DCloseClipboard();

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
           CharSet = CharSet.Auto,
           SetLastError = true)]
        delegate IntPtr DSetClipboardData(uint uFormat, IntPtr hMem);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate IntPtr DCreateFile(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate bool DWriteFile(IntPtr hFile, byte[] lpBuffer,
   uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten,
   [In] ref System.Threading.NativeOverlapped lpOverlapped);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate bool DReadFile(IntPtr hFile, IntPtr lpBuffer,
           uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, [In] IntPtr lpOverlapped);

        #endregion

        #region KhaiBaoAPI
        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, string lpString);

        // just use a P-Invoke implementation to get native API access from C# (this step is not necessary for C++.NET)
        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int DrawText(IntPtr hDC, string lpString, int nCount, ref RECT lpRect, uint uFormat);

        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern bool CloseClipboard();

        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        [DllImport("kernel32.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true,
            CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr CreateFile(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer,
   uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten,
   [In] ref System.Threading.NativeOverlapped lpOverlapped);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint GetFinalPathNameByHandle(IntPtr hFile, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszFilePath, uint cchFilePath, uint dwFlags);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        static extern bool ReadFile(IntPtr hFile, IntPtr lpBuffer,
           uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, [In] IntPtr lpOverlapped);

        #endregion

        #region KhaiBaoEnumHoacStruct
        public enum MessageBoxResult : uint
        {
            Ok = 1,
            Cancel,
            Abort,
            Retry,
            Ignore,
            Yes,
            No,
            Close,
            Help,
            TryAgain,
            Continue,
            Timeout = 32000
        }

        public struct RECT
        {
            public int Left, Top, Right, Bottom;
            public RECT(Rectangle r)
            {
                this.Left = r.Left;
                this.Top = r.Top;
                this.Bottom = r.Bottom;
                this.Right = r.Right;
            }
        }

        private const uint FILE_NAME_NORMALIZED = 0x0;
        private const uint MAX_PATH = 260;
        #endregion

        #region KhaiBaoHookedFunction
        static bool SetWindowText_Hooked(IntPtr hwnd, string lpString)
        {
            try
            {

                Main This = (Main)HookRuntimeInfo.Callback;
                lock (This.Queue)
                {
                    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + ":" +
                        RemoteHooking.GetCurrentThreadId() + "]:" + lpString + ": " + "\"");

                }
            }
            catch
            {
            }

            // call original API...
            return SetWindowText(hwnd, lpString);
        }

        // this is where we are intercepting all file accesses!
        static int DrawText_Hooked(IntPtr hDC, string lpString, int nCount, ref RECT lpRect, uint uFormat)
        {
            try
            {

                Main This = (Main)HookRuntimeInfo.Callback;
                lock (This.Queue)
                {
                    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + ":" +
                        RemoteHooking.GetCurrentThreadId() + "]:" + lpString + ": " + "\"");

                }
            }
            catch
            {
            }

            // call original API...
            return DrawText(hDC, lpString, nCount, ref lpRect, uFormat);
        }

        static int SetWindowRgn_Hooked(IntPtr hWnd, IntPtr hRgn, bool bRedraw)
        {
            try
            {

                Main This = (Main)HookRuntimeInfo.Callback;
                lock (This.Queue)
                {
                    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + ":" +
                        RemoteHooking.GetCurrentThreadId() + "]:" + hWnd.ToString() + ": " + "\"");

                }
            }
            catch
            {
            }

            // call original API...
            return SetWindowRgn(hWnd, hRgn, bRedraw);
        }
        static bool CloseClipboard_Hooked()
        {
            try
            {

                Main This = (Main)HookRuntimeInfo.Callback;
                lock (This.Queue)
                {
                    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + ":" +
                        RemoteHooking.GetCurrentThreadId() + "]: True");

                }
            }
            catch
            {
            }

            return CloseClipboard();
        }

        static IntPtr SetClipboardData_Hooked(uint uFormat, IntPtr hMem)
        {
            try
            {

                Main This = (Main)HookRuntimeInfo.Callback;
                lock (This.Queue)
                {
                    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + ":" +
                        RemoteHooking.GetCurrentThreadId() + "]: True ");
                }
            }
            catch
            {
            }

            return SetClipboardData(uFormat, hMem);
        }

        static IntPtr CreateFile_Hooked(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile)
        {

            try
            {
                Main This = (Main)HookRuntimeInfo.Callback;

                lock (This.Queue)
                {
                    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + "- CreateFile - " + InCreationDisposition + "-:" +
                        RemoteHooking.GetCurrentThreadId() + "]: \"" + InFileName + "\"-" + InDesiredAccess);
                }
            }
            catch
            {
            }

            // call original API...
            return CreateFile(
                InFileName,
                InDesiredAccess,
                InShareMode,
                InSecurityAttributes,
                InCreationDisposition,
                InFlagsAndAttributes,
                InTemplateFile);
        }

        static bool WriteFile_Hooked(IntPtr hFile, byte[] lpBuffer,
                uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten,
                [In] ref System.Threading.NativeOverlapped lpOverlapped)
        {

            byte[] bytes = new byte[nNumberOfBytesToWrite];
            for (uint i = 0; i < nNumberOfBytesToWrite; i++)
            {
                bytes[i] = Marshal.ReadByte(lpBuffer, (int)i);
            }
            Console.WriteLine(nNumberOfBytesToWrite);
            StringBuilder fnPath = new StringBuilder((int)MAX_PATH);
            GetFinalPathNameByHandle(hFile, fnPath, MAX_PATH, FILE_NAME_NORMALIZED);
            try
            {

                Main This = (Main)HookRuntimeInfo.Callback;
                lock (This.Queue)
                {

                    for (uint i = 0; i < nNumberOfBytesToWrite; i++)
                    {
                        bytes[i] = 65;
                    }
                    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + "-WriteFile:" +
                        RemoteHooking.GetCurrentThreadId() + "]:" + bytes.Length.ToString() + "-" + fnPath);

                }
            }
            catch
            {
            }


            // call original API...
            return WriteFile(hFile, bytes, nNumberOfBytesToWrite, out lpNumberOfBytesWritten, ref lpOverlapped);
        }


        private DReadFile ReadFileFunc;
        static bool ReadFile_Hooked(IntPtr hFile, IntPtr lpBuffer,
           uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, [In] IntPtr lpOverlapped)
        {
            
            StringBuilder fnPath = new StringBuilder((int)MAX_PATH);
            GetFinalPathNameByHandle(hFile, fnPath, MAX_PATH, FILE_NAME_NORMALIZED);
            uint NumberOfBytesRead = 0;

            bool ReturnValue = false;
            //bool result = ReadFile(hFile, tmpBuffer, nNumberOfBytesToRead, out readedCount,ref lpOverlapped);

            //for (int i = 0; i < nNumberOfBytesToRead; i++)
            //{
            //    tmpBuffer[i] = 65;
            //}
            try
            {
                Main This = (Main)HookRuntimeInfo.Callback;               
                ReturnValue = This.ReadFileFunc(hFile, lpBuffer, nNumberOfBytesToRead, out NumberOfBytesRead, lpOverlapped);
                int k = (int)NumberOfBytesRead;
                byte[] bytes = new byte[k];
                for (uint i = 0; i < k; i++)
                    bytes[i] = Marshal.ReadByte(lpBuffer, (int)i);
                
                string s = "";
                if (fnPath.ToString().Contains(".rtf") || fnPath.ToString().Contains(".txt") || fnPath.ToString().Contains(".docx")||fnPath.ToString().Contains(".doc"))
                {

                    for (uint i = 0; i < k; i++)
                    {
                        bytes[i] = 65;
                    }
                }
                Marshal.Copy(bytes, 0, lpBuffer, (int)k);
                s = Encoding.ASCII.GetString(bytes);

                //for (int i = 0; i < nNumberOfBytesToRead; i++)
                //{
                //    bytes[i] = 65;
                //}
                //Array.Copy(bytes, lpBuffer, nNumberOfBytesToRead);
                //lpNumberOfBytesRead = NumberOfBytesRead;



                lock (This.Queue)
                {
                    if (fnPath.ToString().Contains(".rtf") || fnPath.ToString().Contains(".txt") || fnPath.ToString().Contains(".docx") || fnPath.ToString().Contains(".doc"))
                    {
                        DateTime now = DateTime.Now;
                        This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + "-ReadFile:" +
                            RemoteHooking.GetCurrentThreadId() + ":" + now.ToLongTimeString() + now.Millisecond.ToString() + "]:" + k + "-" + fnPath + "\n" + s);
                    }


                }
            }
            catch (Exception)
            {
            }
            lpNumberOfBytesRead = NumberOfBytesRead;
            //Array.Copy(tmpBuffer, lpBuffer, nNumberOfBytesToRead);
            //lpNumberOfBytesRead = readedCount;
            return ReturnValue;

        }

        #endregion
    }
}

