﻿using EasyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoHookSomeAPI;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;

namespace InjectDLL
{
    public class Main: IEntryPoint
    {
        FileMonInterface Interface;
        LocalHook LocalTestHook;        
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
                LocalTestHook = LocalHook.Create(
                    LocalHook.GetProcAddress("kernel32.dll", "WriteFile"),
                    new DWriteFile(WriteFile_Hooked),
                    this);

                LocalTestHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            }
            catch (Exception ExtInfo)
            {
                Interface.ReportException(ExtInfo);

                return;
            }

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

            return SetClipboardData(uFormat,hMem);
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
                    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + ":" +
                        RemoteHooking.GetCurrentThreadId() + "]: \"" + InFileName + "\"");
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
            
            try
            {

                Main This = (Main)HookRuntimeInfo.Callback;
                lock (This.Queue)
                {
                    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + ":" +
                        RemoteHooking.GetCurrentThreadId() + "]:" + bytes.Length.ToString() +"-" +GetFileNameFromHandle(hFile));

                }
            }
            catch
            {
            }

            // call original API...
            return WriteFile(hFile, bytes, nNumberOfBytesToWrite, out lpNumberOfBytesWritten, ref lpOverlapped);
        }

        #endregion


        #region FindPathHelper
        [DllImport("kernel32.dll")]
        static extern uint GetFileSize(IntPtr hFile, IntPtr lpFileSizeHigh);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFileMapping(
            IntPtr hFile,
            IntPtr lpFileMappingAttributes,
            FileMapProtection flProtect,
            uint dwMaximumSizeHigh,
            uint dwMaximumSizeLow,
            [MarshalAs(UnmanagedType.LPTStr)]string lpName);

        [Flags]
        public enum FileMapProtection : uint
        {
            PageReadonly = 0x02,
            PageReadWrite = 0x04,
            PageWriteCopy = 0x08,
            PageExecuteRead = 0x20,
            PageExecuteReadWrite = 0x40,
            SectionCommit = 0x8000000,
            SectionImage = 0x1000000,
            SectionNoCache = 0x10000000,
            SectionReserve = 0x4000000,
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr MapViewOfFile(
            IntPtr hFileMappingObject,
            FileMapAccess dwDesiredAccess,
            uint dwFileOffsetHigh,
            uint dwFileOffsetLow,
            uint dwNumberOfBytesToMap);

        [Flags]
        public enum FileMapAccess : uint
        {
            FileMapCopy = 0x0001,
            FileMapWrite = 0x0002,
            FileMapRead = 0x0004,
            FileMapAllAccess = 0x001f,
            fileMapExecute = 0x0020,
        }

        [DllImport("psapi.dll", SetLastError = true)]
        public static extern uint GetMappedFileName(IntPtr m_hProcess, IntPtr lpv, StringBuilder
                lpFilename, uint nSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        public static string GetFileNameFromHandle(IntPtr FileHandle)
        {
            string fileName = String.Empty;
            IntPtr fileMap = IntPtr.Zero, fileSizeHi = IntPtr.Zero;
            UInt32 fileSizeLo = 0;

            fileSizeLo = GetFileSize(FileHandle, fileSizeHi);

            if (fileSizeLo == 0)
            {
                // cannot map an 0 byte file
                return "Empty file.";
            }

            fileMap = CreateFileMapping(FileHandle, IntPtr.Zero, FileMapProtection.PageReadonly, 0, 1, null);

            if (fileMap != IntPtr.Zero)
            {
                IntPtr pMem = MapViewOfFile(fileMap, FileMapAccess.FileMapRead, 0, 0, 1);
                if (pMem != IntPtr.Zero)
                {
                    StringBuilder fn = new StringBuilder(250);
                    GetMappedFileName(System.Diagnostics.Process.GetCurrentProcess().Handle, pMem, fn, 250);
                    if (fn.Length > 0)
                    {
                        UnmapViewOfFile(pMem);
                        CloseHandle(FileHandle);
                        return fn.ToString();
                    }
                    else
                    {
                        UnmapViewOfFile(pMem);
                        CloseHandle(FileHandle);
                        return "Empty filename.";
                    }
                }
            }

            return "Empty filemap handle.";
        }
        #endregion
    }
}

