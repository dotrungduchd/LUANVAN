﻿using DemoHackingApp;
using EasyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace InjectDLL
{
    public class Main : IEntryPoint
    {
        public const int MAX_BUFFER_METADATA = 256;
        public const int MAX_BLOCK_SIZE = 1024;
        public const string path = @"data.tam";
        public string currentDir = "";
        public const int FINAL_BLOCK = -1;
        byte[] dataIgnition, dataToEncrypt;
        ProcessInterface Interface;
        LocalHook WriteFileHook, ReadFileHook, CreateProcessHook, ReplaceFileHook, MoveFileExWHook, DeleteFileHook;
        Stack<String> Queue = new Stack<String>();
        String myChannelName;
        const string Password = "testpassdine12345678";
        const string Salt = "salt ne an di";
        public SymmetricAlgorithm aes;

        public Main(
             RemoteHooking.IContext InContext,
             String InChannelName)
        {
            // connect to host...
            Interface = RemoteHooking.IpcConnectClient<ProcessInterface>(InChannelName);
            myChannelName = InChannelName;
            aes = new AesCryptoServiceProvider();
            dataIgnition = new byte[MAX_BLOCK_SIZE];
            for (int i = 0; i < MAX_BLOCK_SIZE; i++)
            {
                dataIgnition[i] = (byte)i;
            }
            dataToEncrypt = new byte[MAX_BLOCK_SIZE];
            OutputDebugString(Encoding.ASCII.GetString(dataIgnition));
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(Password, Encoding.ASCII.GetBytes(Salt));
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.Padding = PaddingMode.Zeros;
            aes.Mode = CipherMode.CFB;
            currentDir = Interface.GetCurrentDirectory() + "\\";
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

                ReplaceFileHook = LocalHook.Create(LocalHook.GetProcAddress("kernel32.dll", "ReplaceFileW"),
                    new DReplaceFile(ReplaceFile_Hooked),
                    this);

                ReplaceFileHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                MoveFileExWHook = LocalHook.Create(LocalHook.GetProcAddress("kernel32.dll", "MoveFileExW"),
                    new DMoveFile(MoveFile_Hooked),
                    this);

                MoveFileExWHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

                DeleteFileHook = LocalHook.Create(LocalHook.GetProcAddress("kernel32.dll", "DeleteFileW"),
                    new DDeleteFile(DeleteFile_Hooked),
                    this);

                DeleteFileHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

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

                    Interface.Ping();
                }

            }
            catch (Exception ex)
            {

                Interface.ReportException(ex);
                // Ping() will raise an exception if host is unreachable
            }

        }
        #region KhaiBaoDelegate

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
   uint nNumberOfBytesToWrite, IntPtr lpNumberOfBytesWritten,
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

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate bool DReplaceFile(string lpReplacedFileName,
           string lpReplacementFileName, string lpBackupFileName,
           ReplaceFileFlags dwReplaceFlags, IntPtr lpExclude, IntPtr lpReserved);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           SetLastError = true)]
        delegate bool DMoveFile(string lpExistingFileName, string lpNewFileName, MoveFileFlags dwFlags);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate bool DDeleteFile(string lpFileName);
        #endregion

        #region KhaiBaoAPI
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteFile(string lpFileName);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName,
           MoveFileFlags dwFlags);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool ReplaceFile(string lpReplacedFileName,
           string lpReplacementFileName, string lpBackupFileName,
           ReplaceFileFlags dwReplaceFlags, IntPtr lpExclude, IntPtr lpReserved);

        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, string lpString);

        // just use a P-Invoke implementation to get native API access from C# (this step is not necessary for C++.NET)
        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern bool CloseClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        static extern IntPtr GetClipboardData(uint uFormat);

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
   uint nNumberOfBytesToWrite, IntPtr lpNumberOfBytesWritten,
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

        [DllImport("kernel32.dll")]
        static extern uint GetFileSize(IntPtr hFile, IntPtr lpFileSizeHigh);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetOverlappedResult(IntPtr hFile,
           [In] ref System.Threading.NativeOverlapped lpOverlapped,
           out uint lpNumberOfBytesTransferred, bool bWait);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint SetFilePointer(
            [In] IntPtr hFile,
            [In] int lDistanceToMove,
            [Out] out int lpDistanceToMoveHigh,
            [In] EMoveMethod dwMoveMethod);

        #endregion

        #region KhaiBaoEnumHoacStruct
        [Flags]
        enum MoveFileFlags
        {
            MOVEFILE_REPLACE_EXISTING = 0x00000001,
            MOVEFILE_COPY_ALLOWED = 0x00000002,
            MOVEFILE_DELAY_UNTIL_REBOOT = 0x00000004,
            MOVEFILE_WRITE_THROUGH = 0x00000008,
            MOVEFILE_CREATE_HARDLINK = 0x00000010,
            MOVEFILE_FAIL_IF_NOT_TRACKABLE = 0x00000020
        }

        [Flags]
        enum ReplaceFileFlags : uint
        {
            REPLACEFILE_WRITE_THROUGH = 0x00000001,
            REPLACEFILE_IGNORE_MERGE_ERRORS = 0x00000002
        }

        public enum EMoveMethod : uint
        {
            Begin = 0,
            Current = 1,
            End = 2
        }

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

        private const uint FILE_NAME_NORMALIZED = 0x0;
        private const uint MAX_PATH = 260;
        [Flags]
        public enum Protection : uint
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
                uint nNumberOfBytesToWrite, IntPtr lpNumberOfBytesWritten,
                [In] ref System.Threading.NativeOverlapped lpOverlapped)
        {

            StringBuilder fnPath = new StringBuilder((int)MAX_PATH);
            GetFinalPathNameByHandle(hFile, fnPath, MAX_PATH, FILE_NAME_NORMALIZED);
            string filePath = fnPath.ToString();
            Main This = (Main)HookRuntimeInfo.Callback;
            bool result = false;
            int extra_size = 0;
            string[] ext = This.Interface.getExtensions();
            string usbDrive = CheckPath(filePath, This.Interface.getUSBDrives());
            OutputDebugString(filePath);
            Process process = Process.GetProcessById(RemoteHooking.GetCurrentProcessId());
            if (!string.IsNullOrEmpty(filePath) && !filePath.Contains("Temporary") && !filePath.Contains("AppData") && CheckExtension(filePath, ext) && !filePath.Contains("~$"))
            {
                try
                {
                    int fileSize = (int)GetFileSize(hFile, IntPtr.Zero);
                    byte[] bytes = ReadDataFromPointer(lpBuffer, nNumberOfBytesToWrite);
                    byte[] IV = This.Interface.getIV(filePath);
                    string key02 = This.Interface.getKey02();
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(key02, Encoding.ASCII.GetBytes(Salt));
                    This.aes.Key = key.GetBytes(This.aes.KeySize / 8);
                    if (IV == null)
                    {
                        // Generate IV and block data to encrypt
                        This.aes.GenerateIV();
                        This.Interface.addIV(filePath, This.aes.IV);
                        string IVstring = "";
                        for (int i = 0; i < This.aes.IV.Length; i++)
                        {
                            IVstring += (This.aes.IV[i].ToString() + " ");
                        }
                        // Save IV into file
                        List<string> allLines = new List<string>();
                        string fileData = "";
                        string filePathToSave = "";
                        OutputDebugString(usbDrive);
                        if (!string.IsNullOrEmpty(usbDrive))
                        {
                            filePathToSave = filePath.Substring(filePath.IndexOf(usbDrive) + 2);
                            fileData = usbDrive + path;
                        }
                        else
                        {
                            filePathToSave = filePath;
                            fileData = This.currentDir + path;
                        }
                        allLines.Add(filePathToSave);
                        allLines.Add(IVstring);
                        allLines.Add(key02);
                        File.AppendAllLines(fileData, allLines);
                        File.SetAttributes(fileData, FileAttributes.Hidden);
                    }
                    else
                    {
                        This.aes.IV = IV;
                    }
                    ICryptoTransform encryptor = This.aes.CreateEncryptor();
                    MemoryStream msEncrypt = new MemoryStream();
                    CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                    csEncrypt.Write(This.dataIgnition, 0, MAX_BLOCK_SIZE);
                    csEncrypt.FlushFinalBlock();
                    Array.Copy(msEncrypt.ToArray(), This.dataToEncrypt, MAX_BLOCK_SIZE);
                    msEncrypt.Close();
                    csEncrypt.Close();
                    byte[] resultBlock;
                    //Encrypt data block by block
                    resultBlock = new byte[nNumberOfBytesToWrite];
                    for (int i = 0; i < nNumberOfBytesToWrite; i = i + MAX_BLOCK_SIZE)
                    {
                        int nBytesToEncrypt = MAX_BLOCK_SIZE;
                        if (nNumberOfBytesToWrite - i < MAX_BLOCK_SIZE)
                            nBytesToEncrypt = (int)nNumberOfBytesToWrite - i;
                        byte[] tmpData = new byte[nBytesToEncrypt];
                        Array.Copy(bytes, i, tmpData, 0, nBytesToEncrypt);
                        byte[] tmpRes = new byte[nBytesToEncrypt];
                        for (int j = 0; j < nBytesToEncrypt; j++)
                        {
                            tmpRes[j] = (byte)(tmpData[j] ^ This.dataToEncrypt[j]);
                        }
                        Array.Copy(tmpRes, 0, resultBlock, i, nBytesToEncrypt);
                    }
                    // Copy data in byte array into buffer
                    uint oldProtection = 0;
                    Marshal.Copy(resultBlock, 0, lpBuffer, (int)nNumberOfBytesToWrite);
                }
                catch (Exception ex)
                {
                    OutputDebugString(ex.ToString());
                    OutputDebugString(filePath);
                }
                return WriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, lpNumberOfBytesWritten, ref lpOverlapped);
            }
            // call original API...
            else
            {
                return WriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, lpNumberOfBytesWritten, ref lpOverlapped);
            }
        }

        private static string CheckPath(string filePath, string[] usbDrives)
        {
            if (usbDrives == null)
                return null;
            for (int i = 0; i < usbDrives.Length; i++)
            {
                if (filePath.Contains(usbDrives[i]))
                    return usbDrives[i];
            }
            return null;
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
            string key02 = This.Interface.getKey02();
            //int fileSize = (int)GetFileSize(hFile, IntPtr.Zero);            
            int offset = 0;
            string usbDrive = CheckPath(filePath, This.Interface.getUSBDrives());
            Process process = Process.GetProcessById(RemoteHooking.GetCurrentProcessId());
            if ((!CheckExtension(filePath, exts)))
            {
                return ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, ref lpOverlapped);
            }
            else
            {
                try
                {
                    // Get IV from file and generate block data to decrypt
                    byte[] IV = This.Interface.getIV(filePath);
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(key02, Encoding.ASCII.GetBytes(Salt));
                    This.aes.Key = key.GetBytes(This.aes.KeySize / 8);
                    if (IV == null)
                    {
                        string[] data;
                        if (!string.IsNullOrEmpty(usbDrive))
                        {
                            data = File.ReadAllLines(usbDrive + path);
                        }
                        else
                        {
                            data = File.ReadAllLines(This.currentDir + path);
                        }
                        for (int i = data.Length - 1; i >= 0; i--)
                        {
                            if (filePath.Contains(data[i]))
                            {
                                IV = new byte[This.aes.BlockSize / 8];
                                string[] IVnumbers = data[i + 1].Split(' ');
                                if (!key02.Contains(data[i + 2]))
                                {
                                    return ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, ref lpOverlapped);
                                }
                                for (int j = 0; j < IV.Length; j++)
                                {
                                    IV[j] = (byte)int.Parse(IVnumbers[j]);
                                }
                                break;
                            }
                        }
                        if (IV == null)
                            return ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, ref lpOverlapped);
                        This.Interface.addIV(filePath, IV);
                    }
                    This.aes.IV = IV;
                    ICryptoTransform encryptor = This.aes.CreateEncryptor();
                    MemoryStream msEncrypt = new MemoryStream();
                    CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                    csEncrypt.Write(This.dataIgnition, 0, MAX_BLOCK_SIZE);
                    csEncrypt.FlushFinalBlock();
                    Array.Copy(msEncrypt.ToArray(), This.dataToEncrypt, MAX_BLOCK_SIZE);
                    msEncrypt.Close();
                    csEncrypt.Close();

                    // Get the current offset
                    int startPos = 0;
                    int oldOffset = -1;
                    int lpDistanceMoveHigh = 0;
                    int currentOffset = 0;
                    currentOffset = (int)SetFilePointer(hFile, 0, out lpDistanceMoveHigh, EMoveMethod.Current);
                    try
                    {
                        // if lpOverlapped is a null it will throw an exception
                        startPos = lpOverlapped.OffsetLow;
                    }
                    catch (Exception ex)
                    {
                        startPos = currentOffset;
                    }
                    // Call original API to get buffer
                    ReturnValue = This.ReadFileFunc(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, ref lpOverlapped);
                    // Get number of bytes read
                    int bytesCount = (int)nNumberOfBytesToRead;
                    if (!lpNumberOfBytesRead.Equals(IntPtr.Zero) && Marshal.ReadInt32(lpNumberOfBytesRead) != 0)
                    {
                        bytesCount = Marshal.ReadInt32(lpNumberOfBytesRead);
                    }

                    else
                    {
                        bytesCount = lpOverlapped.InternalHigh.ToInt32();
                    }
                    // Read data from buffer 
                    byte[] bytes = ReadDataFromPointer(lpBuffer, (uint)bytesCount);

                    // Decrypt data and copy it to buffer (replace the encrypted data)
                    // ---- Cach 2 ----
                    byte[] resultBlock = new byte[bytesCount];
                    OutputDebugString("Start Pos: " + startPos);
                    startPos = startPos % MAX_BLOCK_SIZE;
                    for (int i = 0; i < bytesCount; i++)
                    {
                        resultBlock[i] = (byte)(bytes[i] ^ This.dataToEncrypt[(i + startPos) % MAX_BLOCK_SIZE]);
                    }
                    Marshal.Copy(resultBlock, 0, lpBuffer, bytesCount);
                    // ---- Cach 2 ----

                    //lock (This.Queue)
                    //{
                    //    DateTime now = DateTime.Now;
                    //    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + "-ReadFile:" +
                    //        RemoteHooking.GetCurrentThreadId() + ":" + bytesCount + "]:" + "-" + fnPath + "-" + currentOffset + "\n");
                    //}

                }
                catch (Exception ex)
                {
                    OutputDebugString(ex.ToString());
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
            bool ReturnValue = true;
            Main This = (Main)HookRuntimeInfo.Callback;
            Process process = Process.GetProcessById(RemoteHooking.GetCurrentProcessId());
            string[] defaultPrograms = This.Interface.getDefaultPrograms();
            if ((process.ProcessName.Contains("xplorer")) && CheckProgram(lpApplicationName, defaultPrograms))
            {
                try
                {
                    dwCreationFlags = dwCreationFlags | 0x00000004;
                    ReturnValue = CreateProcess(lpApplicationName, lpCommandLine, lpProcessAttributes, lpThreadAttributes, false, dwCreationFlags, lpEnvironment, lpCurrentDirectory, ref lpStartupInfo, out proInformation);
                    string InLibraryPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(ProcessInterface).Assembly.Location), "InjectDLL.dll");
                    while (true)
                    {
                        try
                        {
                            RemoteHooking.Inject(proInformation.dwProcessId, InLibraryPath, InLibraryPath, This.myChannelName);
                            break;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }

                    //lock (This.Queue)
                    //{
                    //    DateTime now = DateTime.Now;
                    //    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + "-CreateProcess:" +
                    //        RemoteHooking.GetCurrentThreadId() + "]:" + "-" + lpCommandLine + "-" + proInformation.dwProcessId + "\n");

                    //}
                    OutputDebugString("Create Process: " + lpCommandLine + "-" + proInformation.dwProcessId);
                }
                catch (Exception ex)
                {
                    OutputDebugString(ex.Message);
                    This.Interface.ReportException(ex);
                }
                lpProcessInformation = new PROCESS_INFORMATION();
                lpProcessInformation.dwProcessId = proInformation.dwProcessId;
                lpProcessInformation.dwThreadId = proInformation.dwThreadId;
                lpProcessInformation.hProcess = proInformation.hProcess;
                lpProcessInformation.hThread = proInformation.hThread;


                return ReturnValue;
            }
            else
            {
                return CreateProcess(lpApplicationName, lpCommandLine, lpProcessAttributes, lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment, lpCurrentDirectory, ref lpStartupInfo, out lpProcessInformation);
            }

        }


        static bool ReplaceFile_Hooked(string lpReplacedFileName,
           string lpReplacementFileName, string lpBackupFileName,
           ReplaceFileFlags dwReplaceFlags, IntPtr lpExclude, IntPtr lpReserved)
        {
            Main This = (Main)HookRuntimeInfo.Callback;
            string[] exts = This.Interface.getExtensions();
            string usbDrive = CheckPath(lpReplacedFileName, This.Interface.getUSBDrives());
            try
            {
                OutputDebugString("Replace!!!" + lpReplacedFileName + lpReplacementFileName);
                if (CheckExtension(lpReplacedFileName, exts) && !lpReplacementFileName.Contains("~$"))
                {
                    string dataFile;
                    string key02 = This.Interface.getKey02();
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(key02, Encoding.ASCII.GetBytes(Salt));
                    This.aes.Key = key.GetBytes(This.aes.KeySize / 8);
                    byte[] IV = This.Interface.getIV(lpReplacedFileName);
                    if (IV == null)
                    {
                        if (!string.IsNullOrEmpty(usbDrive))
                        {
                            dataFile = usbDrive + path;
                        }
                        else
                        {
                            dataFile = This.currentDir + path;
                        }
                        bool hasIV = false;
                        IV = new byte[This.aes.BlockSize / 8];
                        string[] data = File.ReadAllLines(dataFile);
                        for (int i = data.Length - 1; i >= 0; i--)
                        {
                            if (lpReplacedFileName.Contains(data[i]))
                            {
                                string[] IVnumbers = data[i + 1].Split(' ');
                                for (int j = 0; j < IV.Length; j++)
                                {
                                    IV[j] = (byte)int.Parse(IVnumbers[j]);
                                }
                                hasIV = true;
                                break;
                            }
                        }
                        if (!hasIV)
                        {
                            OutputDebugString("Chua co IV");
                            This.aes.GenerateIV();
                            string IVstring = "";
                            for (int i = 0; i < This.aes.IV.Length; i++)
                            {
                                IVstring += (This.aes.IV[i].ToString() + " ");
                            }
                            List<string> tmpData = new List<string>();
                            // Save IV into file                      
                            tmpData.Add(lpReplacedFileName);
                            tmpData.Add(IVstring);
                            tmpData.Add(key02);
                            File.AppendAllLines(dataFile, tmpData);
                            File.SetAttributes(dataFile, FileAttributes.Hidden);
                        }
                        else
                        {
                            This.aes.IV = IV;
                        }
                        This.Interface.addIV(lpReplacedFileName, This.aes.IV);
                    }
                    OutputDebugString("Get IV");
                    ICryptoTransform encryptor = This.aes.CreateEncryptor();
                    MemoryStream msEncrypt = new MemoryStream();
                    CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                    csEncrypt.Write(This.dataIgnition, 0, MAX_BLOCK_SIZE);
                    csEncrypt.FlushFinalBlock();
                    Array.Copy(msEncrypt.ToArray(), This.dataToEncrypt, MAX_BLOCK_SIZE);
                    msEncrypt.Close();
                    csEncrypt.Close();
                    byte[] dataToReplace = File.ReadAllBytes(lpReplacementFileName);
                    OutputDebugString("Replace Read-" + lpReplacedFileName);
                    for (int i = 0; i < dataToReplace.Length; i++)
                    {
                        dataToReplace[i] = (byte)(dataToReplace[i] ^ This.dataToEncrypt[i % MAX_BLOCK_SIZE]);
                    }
                    File.WriteAllBytes(lpReplacementFileName, dataToReplace);
                    OutputDebugString("Replace Write-" + lpReplacementFileName);
                }
            }
            catch (Exception ex)
            {
                OutputDebugString(ex.ToString());
            }
            return ReplaceFile(lpReplacedFileName, lpReplacementFileName, lpBackupFileName, dwReplaceFlags, lpExclude, lpReserved);
        }

        static bool MoveFile_Hooked(string lpExistingFileName, string lpNewFileName,
           MoveFileFlags dwFlags)
        {

            Main This = (Main)HookRuntimeInfo.Callback;
            string[] exts = This.Interface.getExtensions();
            string usbDrive = CheckPath(lpNewFileName, This.Interface.getUSBDrives());
            try
            {
                if (CheckExtension(lpNewFileName, exts) && !lpExistingFileName.Contains("~$"))
                {
                    string dataFile;
                    string key02 = This.Interface.getKey02();
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(key02, Encoding.ASCII.GetBytes(Salt));
                    This.aes.Key = key.GetBytes(This.aes.KeySize / 8);
                    byte[] IV = This.Interface.getIV(lpNewFileName);
                    if (IV == null)
                    {
                        if (!string.IsNullOrEmpty(usbDrive))
                        {
                            dataFile = usbDrive + path;
                        }
                        else
                        {
                            dataFile = This.currentDir + path;
                        }
                        bool hasIV = false;
                        IV = new byte[This.aes.BlockSize / 8];
                        if (File.Exists(dataFile))
                        {
                            string[] data = File.ReadAllLines(dataFile);
                            for (int i = data.Length - 1; i >= 0; i--)
                            {
                                if (lpNewFileName.Contains(data[i]))
                                {
                                    string[] IVnumbers = data[i + 1].Split(' ');
                                    for (int j = 0; j < IV.Length; j++)
                                    {
                                        IV[j] = (byte)int.Parse(IVnumbers[j]);
                                    }
                                    hasIV = true;
                                    break;
                                }
                            }
                        }
                        if (!hasIV)
                        {
                            OutputDebugString("Chua co IV");
                            This.aes.GenerateIV();
                            string IVstring = "";
                            for (int i = 0; i < This.aes.IV.Length; i++)
                            {
                                IVstring += (This.aes.IV[i].ToString() + " ");
                            }
                            List<string> tmpData = new List<string>();
                            // Save IV into file     
                            if (lpNewFileName.Contains(usbDrive))
                            {
                                lpNewFileName = lpNewFileName.Substring(lpNewFileName.IndexOf(usbDrive) + 2);
                            }
                            tmpData.Add(lpNewFileName);
                            tmpData.Add(IVstring);
                            tmpData.Add(key02);
                            File.AppendAllLines(dataFile, tmpData);
                            File.SetAttributes(dataFile, FileAttributes.Hidden);
                        }
                        else
                        {
                            This.aes.IV = IV;
                        }
                        This.Interface.addIV(lpNewFileName, This.aes.IV);
                    }
                    OutputDebugString("Get IV");
                    ICryptoTransform encryptor = This.aes.CreateEncryptor();
                    MemoryStream msEncrypt = new MemoryStream();
                    CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                    csEncrypt.Write(This.dataIgnition, 0, MAX_BLOCK_SIZE);
                    csEncrypt.FlushFinalBlock();
                    Array.Copy(msEncrypt.ToArray(), This.dataToEncrypt, MAX_BLOCK_SIZE);
                    msEncrypt.Close();
                    csEncrypt.Close();
                    byte[] dataToReplace = File.ReadAllBytes(lpExistingFileName);
                    OutputDebugString("Replace Read-" + lpNewFileName);
                    for (int i = 0; i < dataToReplace.Length; i++)
                    {
                        dataToReplace[i] = (byte)(dataToReplace[i] ^ This.dataToEncrypt[i % MAX_BLOCK_SIZE]);
                    }
                    File.WriteAllBytes(lpExistingFileName, dataToReplace);
                    OutputDebugString("Replace Write-" + lpExistingFileName);
                }
            }
            catch (Exception ex)
            {
                OutputDebugString(ex.ToString());
            }
            return MoveFileEx(lpExistingFileName, lpNewFileName, dwFlags);
        }


        static bool DeleteFile_Hooked(string lpFileName)
        {
            if (!lpFileName.Contains(path))
                return DeleteFile(lpFileName);
            else
                return false;
        }

        private static bool CheckExtension(string filePath, string[] exts)
        {
            foreach (var ext in exts)
            {
                if (filePath.Contains(ext))
                    return true;
            }
            return false;
        }

        private static bool CheckProgram(string lpApplicationName, string[] defaultPrograms)
        {
            for (int i = 0; i < defaultPrograms.Length; i++)
            {
                if (defaultPrograms[i].Contains(lpApplicationName))
                    return true;
            }
            return false;
        }

        #endregion
    }
}
