//compile with: /unsafe
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
using System.Diagnostics;

namespace InjectDLL
{
    public class Main : IEntryPoint
    {
        FileMonInterface Interface;
        LocalHook WriteFileHook, CreateFileHook, ReadFileHook, CreateProcessHook;
        Stack<String> Queue = new Stack<String>();
        Stack<Int32> pIdQueue = new Stack<Int32>();
        Dictionary<string, object> cpParams = new Dictionary<string, object>();
        IntPtr tmpThread = IntPtr.Zero;
        ThreadPriorityLevel thrOldLvl;
        int oldThrId = 0;
        String myChannelName;

        public Main(
             RemoteHooking.IContext InContext,
             String InChannelName)
        {
            // connect to host...
            Interface = RemoteHooking.IpcConnectClient<FileMonInterface>(InChannelName);            
            myChannelName = InChannelName;
            Interface.Ping();
        }

        public void Run(
            RemoteHooking.IContext InContext,
            String InChannelName)
        {
            // install hook...
            try
            {
                
                WriteFileHook = LocalHook.Create(
                    LocalHook.GetProcAddress("kernel32.dll", "WriteFile"),
                    new DWriteFile(WriteFile_Hooked),
                    this);

                WriteFileHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                //CreateFileHook = LocalHook.Create(
                //    LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"),
                //    new DCreateFile(CreateFile_Hooked),
                //    this);

                ReadFileHook = LocalHook.Create(LocalHook.GetProcAddress("kernel32.dll", "ReadFile"),
                    new DReadFile(ReadFile_Hooked),
                    this);

                ReadFileHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                CreateProcessHook = LocalHook.Create(LocalHook.GetProcAddress("kernel32.dll", "CreateProcessW"),
                    new DCreateProcess(CreateProcess_Hooked),
                    this);

                CreateProcessHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
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

                    if (pIdQueue.Count > 0)
                    {
                        Int32[] Package = null;
                        lock (pIdQueue)
                        {
                            Package = pIdQueue.ToArray();
                            pIdQueue.Clear();
                        }
                        Interface.OnCreateProcess(RemoteHooking.GetCurrentProcessId(), Package,oldThrId,thrOldLvl ,this.myChannelName);
                    }
                    //if (cpParams.Count > 2)
                    //{
                    //    string lpApplicationName = "";
                    //    string lpCommandline = "";
                    //    uint dwCreationFlags = 0;
                    //    lock (cpParams)
                    //    {
                    //        lpApplicationName = (string)cpParams["ApplicationName"];
                    //        lpCommandline = (string)cpParams["CommandLine"];
                    //        dwCreationFlags = (uint)cpParams["CreationFlags"];
                    //        cpParams.Clear();
                    //    }
                    //    if (!string.IsNullOrEmpty(lpApplicationName))
                    //    {
                    //        Interface.OnCreateProcess(RemoteHooking.GetCurrentProcessId(), lpApplicationName, lpCommandline, this.myChannelName);
                    //    }
                    //}


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
            catch(Exception ex)
            {
                Interface.ReportException(ex);
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
        delegate bool DWriteFile(IntPtr hFile, IntPtr lpBuffer,
   uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten,
   [In] ref System.Threading.NativeOverlapped lpOverlapped);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate bool DReadFile(IntPtr hFile, IntPtr lpBuffer,
           uint nNumberOfBytesToRead, IntPtr lpNumberOfBytesRead, [In] ref System.Threading.NativeOverlapped lpOverlapped);


        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate bool DCreateProcess(string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

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
        public static extern bool WriteFile(IntPtr hFile, IntPtr lpBuffer,
   uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten,
   [In] ref System.Threading.NativeOverlapped lpOverlapped);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint GetFinalPathNameByHandle(IntPtr hFile, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszFilePath, uint cchFilePath, uint dwFlags);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        static extern bool ReadFile(IntPtr hFile, IntPtr lpBuffer,
           uint nNumberOfBytesToRead, IntPtr lpNumberOfBytesRead, [In] ref System.Threading.NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize,
           uint flNewProtect, out uint lpflOldProtect);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern void OutputDebugString(string message);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool CreateProcess(
           string lpApplicationName,
           string lpCommandLine,
           IntPtr lpProcessAttributes,
           IntPtr lpThreadAttributes,
           bool bInheritHandles,
           uint dwCreationFlags,
           IntPtr lpEnvironment,
           string lpCurrentDirectory,
           [In] ref STARTUPINFO lpStartupInfo,
           out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern uint ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

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
        public enum Protection
        {
            PAGE_NOACCESS = 0x01,
            PAGE_READONLY = 0x02,
            PAGE_READWRITE = 0x04,
            PAGE_WRITECOPY = 0x08,
            PAGE_EXECUTE = 0x10,
            PAGE_EXECUTE_READ = 0x20,
            PAGE_EXECUTE_READWRITE = 0x40,
            PAGE_EXECUTE_WRITECOPY = 0x80,
            PAGE_GUARD = 0x100,
            PAGE_NOCACHE = 0x200,
            PAGE_WRITECOMBINE = 0x400
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFOEX
        {
            public STARTUPINFO StartupInfo;
            public IntPtr lpAttributeList;
        }

        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

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

        static bool WriteFile_Hooked(IntPtr hFile, IntPtr lpBuffer,
                uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten,
                [In] ref System.Threading.NativeOverlapped lpOverlapped)
        {

            byte[] bytes = ReadDataFromPointer(lpBuffer, nNumberOfBytesToWrite);
            Console.WriteLine(nNumberOfBytesToWrite);
            StringBuilder fnPath = new StringBuilder((int)MAX_PATH);
            GetFinalPathNameByHandle(hFile, fnPath, MAX_PATH, FILE_NAME_NORMALIZED);
            string filePath = fnPath.ToString();
            try
            {

                Main This = (Main)HookRuntimeInfo.Callback;
                lock (This.Queue)
                {
                    if (filePath.Contains("Encryption"))
                    {
                        for (uint i = 0; i < nNumberOfBytesToWrite; i++)
                        {
                            bytes[i] += 1;
                        }
                        Marshal.Copy(bytes, 0, lpBuffer, (int)nNumberOfBytesToWrite);
                        This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + "-WriteFile:" +
                        RemoteHooking.GetCurrentThreadId() + "]:" + bytes.Length.ToString() + "-" + fnPath);
                    }
                }
            }
            catch
            {
            }


            // call original API...
            return WriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, out lpNumberOfBytesWritten, ref lpOverlapped);
        }


        // Read data from pointer
        private static byte[] ReadDataFromPointer(IntPtr lpBuffer, uint nNumberOfBytesToWrite)
        {
            byte[] bytes = new byte[nNumberOfBytesToWrite];
            for (uint i = 0; i < nNumberOfBytesToWrite; i++)
            {
                bytes[i] = Marshal.ReadByte(lpBuffer, (int)i);
            }
            return bytes;
        }


        private DReadFile ReadFileFunc;
        static bool ReadFile_Hooked(IntPtr hFile, IntPtr lpBuffer,
           uint nNumberOfBytesToRead, IntPtr lpNumberOfBytesRead, [In] ref System.Threading.NativeOverlapped lpOverlapped)
        {

            StringBuilder fnPath = new StringBuilder((int)MAX_PATH);
            GetFinalPathNameByHandle(hFile, fnPath, MAX_PATH, FILE_NAME_NORMALIZED);
            bool ReturnValue = false;
            string filePath = fnPath.ToString();            
            Main This = (Main)HookRuntimeInfo.Callback;
            string[] exts = This.Interface.getExtensions();
            if ((!filePath.Contains("Encryption")) || (!CheckExtension(filePath,exts)))
            {
                return ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, ref lpOverlapped);
            }
            else
            {
                try
                {
                    
                    ReturnValue = This.ReadFileFunc(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, ref lpOverlapped);                    
                    int bytesCount = (int)nNumberOfBytesToRead;
                    if (!lpNumberOfBytesRead.Equals(IntPtr.Zero) && Marshal.ReadInt32(lpNumberOfBytesRead)!=0)
                    {
                        bytesCount = Marshal.ReadInt32(lpNumberOfBytesRead);                        
                    }
                    else
                    {
                        bytesCount = lpOverlapped.InternalHigh.ToInt32();
                        OutputDebugString("Co overlapped -"+bytesCount);
                    }
                    byte[] bytes = ReadDataFromPointer(lpBuffer, (uint)bytesCount);                    
                    bool ValidExtension = CheckExtension(filePath,exts);
                    if (ValidExtension)
                    {
                        for (uint i = 0; i < bytesCount; i++)
                        {
                            bytes[i] -= 1;
                        }
                        Marshal.Copy(bytes, 0, lpBuffer, (int)bytesCount);
                    }                    
                    lock (This.Queue)
                    {
                        if (ValidExtension)
                        {
                            DateTime now = DateTime.Now;                            
                            This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + "-ReadFile:" +
                                RemoteHooking.GetCurrentThreadId() + ":" + bytesCount + "]:" + "-" + fnPath + "\n");
                        }
                    }
                }
                catch (Exception ex)
                {
                    OutputDebugString(ex.Message);
                }
                return ReturnValue;
            }
        }

        static bool CreateProcess_Hooked(string lpApplicationName,
           string lpCommandLine,
           IntPtr lpProcessAttributes,
           IntPtr lpThreadAttributes,
           bool bInheritHandles,
           uint dwCreationFlags,
           IntPtr lpEnvironment,
           string lpCurrentDirectory,
           [In] ref STARTUPINFO lpStartupInfo,
           out PROCESS_INFORMATION lpProcessInformation)
        {
            PROCESS_INFORMATION proInformation = new PROCESS_INFORMATION();
            bool ReturnValue = false;
            int processId = 0;
            try
            {
                
                Main This = (Main)HookRuntimeInfo.Callback;                
                
                string InLibraryPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(FileMonInterface).Assembly.Location), "InjectDLL.dll");
                //dwCreationFlags = dwCreationFlags | 0x00000004;
                
                ReturnValue = CreateProcess(lpApplicationName, lpCommandLine, lpProcessAttributes, lpThreadAttributes, false,dwCreationFlags, lpEnvironment, null, ref lpStartupInfo, out proInformation);
                Process pro = Process.GetProcessById(proInformation.dwProcessId);
                This.oldThrId = proInformation.dwThreadId;
                foreach (ProcessThread thread in pro.Threads)
                {
                    if (thread.Id == proInformation.dwThreadId)
                    {
                        This.thrOldLvl = thread.PriorityLevel;
                        thread.PriorityLevel = ThreadPriorityLevel.Idle;
                    }
                }
                


                //OutputDebugString(InLibraryPath);
                //RemoteHooking.CreateAndInject(lpApplicationName, lpCommandLine, (int)dwCreationFlags,
                //    InLibraryPath, // 32-bit version (the same because AnyCPU)
                //    InLibraryPath,
                //    out processId,
                //    This.myChannelName);
                
                //HelperServiceInterface helper = new HelperServiceInterface();
                //helper.InjectEx(
                //    NativeAPI.GetCurrentProcessId(),
                //    proInformation.dwProcessId,
                //    proInformation.dwThreadId,
                //    0x20000000,
                //    "InjectDLL.dll",
                //    "InjectDLL.dll",
                //    true,
                //    false,
                //    false,
                //    This.myChannelName);
                //SuspendThread(proInformation.hThread); 
                lock (This.Queue)
                {
                    DateTime now = DateTime.Now;
                    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + "-CreateProcess:" +
                        RemoteHooking.GetCurrentThreadId() + "]:" + "-" + lpApplicationName + proInformation.dwProcessId + "\n");

                }
                lock (This.pIdQueue)
                {
                    This.pIdQueue.Push(proInformation.dwProcessId);
                }
                //lock (This.cpParams)
                //{
                //    if (!This.cpParams.Keys.Contains("ApplicationName"))
                //    {
                //        This.cpParams.Add("ApplicationName", lpApplicationName);
                //    }
                //    if (!This.cpParams.Keys.Contains("CommandLine"))
                //    {
                //        This.cpParams.Add("CommandLine", lpCommandLine);
                //    }
                //    if (!This.cpParams.Keys.Contains("CreationFlags"))
                //    {
                //        This.cpParams.Add("CreationFlags", dwCreationFlags);
                //    }
                //}                


            }
            catch (Exception ex)
            {
                OutputDebugString(ex.Message);
            }
            lpProcessInformation = new PROCESS_INFORMATION();
            lpProcessInformation.dwProcessId = proInformation.dwProcessId;
            lpProcessInformation.dwThreadId = proInformation.dwThreadId;
            lpProcessInformation.hProcess = proInformation.hProcess;
            lpProcessInformation.hThread = proInformation.hThread;            
            //ReturnValue = processId != 0;

            return ReturnValue;

        }
        static string[] strExtensions = { ".rtf", ".txt",".docx",".doc",".mp3",".png",".jpg",".bmp",".flv",".jpeg",".exe",".mkv",".pdf",
                                   ".xls",".cs",".xlsx"};

        private static bool CheckExtension(string filePath,string[] exts)
        {               
            foreach (var ext in exts)
            {
                
                if (filePath.Contains(ext))
                    return true;
            }
            return false;
        }

        #endregion
    }
}

