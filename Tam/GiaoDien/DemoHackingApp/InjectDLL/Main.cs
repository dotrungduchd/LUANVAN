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
        public const string metadataFileName = @"data.tam";
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

        static bool WriteFile_Hooked(
            IntPtr hFile, IntPtr lpBuffer,
            uint nNumberOfBytesToWrite, 
            IntPtr lpNumberOfBytesWritten,
            [In] ref System.Threading.NativeOverlapped lpOverlapped)
        {

            StringBuilder fnPath = new StringBuilder((int)MAX_PATH);
            GetFinalPathNameByHandle(hFile, fnPath, MAX_PATH, FILE_NAME_NORMALIZED);
            string filePath = fnPath.ToString();
            Main This = (Main)HookRuntimeInfo.Callback;
            string[] ext = This.Interface.getExtensions();
            Process process = Process.GetProcessById(RemoteHooking.GetCurrentProcessId());

            if (!string.IsNullOrEmpty(filePath) && !filePath.Contains("Temporary") && 
                !filePath.Contains("AppData") && CheckExtension(filePath, ext) && !filePath.Contains("~$"))
            {
                try
                {
                    OutputDebugString("W " + filePath);

                    int fileSize = (int)GetFileSize(hFile, IntPtr.Zero);
                    byte[] bytes = ReadDataFromPointer(lpBuffer, nNumberOfBytesToWrite);
                    string hashKey02 = This.Interface.getHashKey02();
                    string usbDrive = CheckPath(filePath, This.Interface.getUSBDrives());

                    #region Get IV
                    byte[] IV = This.Interface.getIV(filePath);

                    if (IV == null)
                    {
                        bool hasIV = false;

                        // Generate IV and block data to encrypt
                        #region Generate IV

                        This.aes.GenerateIV();
                        string IVstring = "";
                        for (int i = 0; i < This.aes.IV.Length; i++)
                        {
                            IVstring += (This.aes.IV[i].ToString() + " ");
                        }

                        hasIV = true;
                        #endregion

                        // Save IV into file
                        #region Save File info to Metadata File
                        string metadataFilePath = "";
                        string filePathWritten = "";

                        if (!string.IsNullOrEmpty(usbDrive))
                        {
                            filePathWritten = filePath.Substring(filePath.IndexOf(usbDrive) + 2);
                            metadataFilePath = usbDrive + metadataFileName;
                            OutputDebugString(usbDrive);
                        }
                        else
                        {
                            filePathWritten = filePath;
                            metadataFilePath = This.currentDir + metadataFileName;
                        }

                        // Save file info
                        List<string> allLines = new List<string>();
                        allLines.Add(filePathWritten);
                        allLines.Add(IVstring);
                        allLines.Add(hashKey02);

                        FileHelper.SaveFileInfo(metadataFilePath, allLines);

                        #endregion


                        // Call original Writefile function if can not get IV
                        if (!hasIV)
                            return WriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, lpNumberOfBytesWritten, ref lpOverlapped);
                    }
                    else

                    #endregion 

                    This.aes.IV = IV;
                    This.Interface.addIV(filePath, This.aes.IV);


                    // Encrypte data if IV not null
                    #region Encrypt data

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
                    Marshal.Copy(resultBlock, 0, lpBuffer, (int)nNumberOfBytesToWrite);

                    #endregion
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


        private DReadFile ReadFileFunc;
        static bool ReadFile_Hooked(
            IntPtr hFile, 
            IntPtr lpBuffer,
            uint nNumberOfBytesToRead, 
            IntPtr lpNumberOfBytesRead, 
            [In] ref System.Threading.NativeOverlapped lpOverlapped)
        {

            StringBuilder fnPath = new StringBuilder((int)MAX_PATH);
            GetFinalPathNameByHandle(hFile, fnPath, MAX_PATH, FILE_NAME_NORMALIZED);
            bool ReturnValue = false;
            string filePath = fnPath.ToString();
            Main This = (Main)HookRuntimeInfo.Callback;
            string[] exts = This.Interface.getExtensions();
            Process process = Process.GetProcessById(RemoteHooking.GetCurrentProcessId());

            if ((!CheckExtension(filePath, exts)))
            {
                return ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, ref lpOverlapped);
            }
            else
            {
                OutputDebugString("R " + filePath);

                string hashKey02 = This.Interface.getHashKey02();
                string usbDrive = CheckPath(filePath, This.Interface.getUSBDrives());

                try
                {
                    // Get IV from file and generate block data to decrypt
                    #region Get IV
                    byte[] IV = This.Interface.getIV(filePath);
                    if (IV == null)
                    {
                        bool hasIV = false;

                        // Read info from metadata
                        #region Read File Info

                        string metadataFilePath;
                        string filePathToRead = "";

                        if (!string.IsNullOrEmpty(usbDrive))
                            metadataFilePath = usbDrive + metadataFileName;
                        else
                            metadataFilePath = This.currentDir + metadataFileName;

                        string[] data = FileHelper.ReadFileInfo(metadataFilePath, filePathToRead).ToArray();

                        // Check data
                        if (data != null && data.Length == 3 && filePathToRead.Contains(data[0]))
                        {
                            // Authen hash key02
                            if (!hashKey02.Contains(data[2]))
                            {
                                return ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, ref lpOverlapped);
                            }

                            // Get IV
                            IV = new byte[This.aes.BlockSize / 8];
                            string[] IVnumbers = data[1].Split(' ');
                            for (int j = 0; j < IV.Length; j++)
                            {
                                IV[j] = (byte)int.Parse(IVnumbers[j]);
                            }

                            hasIV = true;
                        }

                        // Call original ReadFile if can not get IV
                        if (!hasIV)
                            return ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, ref lpOverlapped);
                        #endregion

                    }

                    #endregion

                    This.aes.IV = IV;
                    This.Interface.addIV(filePath, IV);


                    #region Decrypt data
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
                    int lpDistanceMoveHigh = 0;
                    int currentOffset = 0;
                    currentOffset = (int)SetFilePointer(hFile, 0, out lpDistanceMoveHigh, EMoveMethod.Current);
                    try
                    {
                        // if lpOverlapped is a null it will throw an exception
                        startPos = lpOverlapped.OffsetLow;
                    }
                    catch (Exception)
                    {
                        startPos = currentOffset;
                    }

                    // Call original API to get buffer
                    ReturnValue = This.ReadFileFunc(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, ref lpOverlapped);
                    
                    // Get number of bytes read
                    int bytesCount = (int)nNumberOfBytesToRead;
                    if (!lpNumberOfBytesRead.Equals(IntPtr.Zero) && Marshal.ReadInt32(lpNumberOfBytesRead) != 0)
                        bytesCount = Marshal.ReadInt32(lpNumberOfBytesRead);
                    else
                        bytesCount = lpOverlapped.InternalHigh.ToInt32();

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

                    #endregion
                }
                catch (Exception ex)
                {
                    OutputDebugString(ex.ToString());
                }
                return ReturnValue;
            }
        }

        static bool CreateProcess_Hooked(
           string lpApplicationName,
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


        static bool ReplaceFile_Hooked(
           string lpReplacedFileName,
           string lpReplacementFileName, 
           string lpBackupFileName,
           ReplaceFileFlags dwReplaceFlags, 
           IntPtr lpExclude, 
           IntPtr lpReserved)
        {
            Main This = (Main)HookRuntimeInfo.Callback;
            string[] exts = This.Interface.getExtensions();
            try
            {
                if (CheckExtension(lpReplacedFileName, exts) && !lpReplacementFileName.Contains("~$"))
                {
                    OutputDebugString("RP " + lpReplacedFileName + lpReplacementFileName);

                    string metadataFilePath;
                    string filePathToReplace = lpReplacedFileName;
                    string hashKey02 = This.Interface.getHashKey02();
                    string usbDrive = CheckPath(lpReplacedFileName, This.Interface.getUSBDrives());

                    #region Get IV
                    byte[] IV = This.Interface.getIV(lpReplacedFileName);

                    if (IV == null)
                    {
                        bool hasIV = false;

                        #region Get IV from metadata File
                        if (!string.IsNullOrEmpty(usbDrive))
                            metadataFilePath = usbDrive + metadataFileName;
                        else
                            metadataFilePath = This.currentDir + metadataFileName;

                        string[] data = FileHelper.ReadFileInfo(metadataFilePath, filePathToReplace).ToArray();

                        // Check filename
                        if (data != null && data.Length == 3 && filePathToReplace.Contains(data[0]))
                        {
                            // Authen hash key02
                            if (!hashKey02.Contains(data[2]))
                            {
                                return ReplaceFile(lpReplacedFileName, lpReplacementFileName, lpBackupFileName, 
                                    dwReplaceFlags, lpExclude, lpReserved);
                            }

                            // Parse IV
                            IV = new byte[This.aes.BlockSize / 8];
                            string[] IVnumbers = data[1].Split(' ');
                            for (int j = 0; j < IV.Length; j++)
                            {
                                IV[j] = (byte)int.Parse(IVnumbers[j]);
                            }

                            hasIV = true;
                        }

                        #endregion

                        // Generate IV if IV not exist
                        #region Generate IV
                        if (!hasIV)
                        {
                            OutputDebugString("Not Exist IV");

                            This.aes.GenerateIV();
                            string IVstring = "";
                            for (int i = 0; i < This.aes.IV.Length; i++)
                            {
                                IVstring += (This.aes.IV[i].ToString() + " ");
                            }

                            hasIV = true;

                            // Save IV into file      
                            List<string> tmpData = new List<string>();
                            tmpData.Add(filePathToReplace);
                            tmpData.Add(IVstring);
                            tmpData.Add(hashKey02);
                            FileHelper.SaveFileInfo(metadataFilePath, tmpData);
                        }
                        #endregion

                        // Call original ReplaceFile if not get IV
                        if (!hasIV)
                            return ReplaceFile(lpReplacedFileName, lpReplacementFileName, lpBackupFileName, dwReplaceFlags, lpExclude, lpReserved);

                    }
                    #endregion

                    This.aes.IV = IV;
                    This.Interface.addIV(lpReplacedFileName, IV);

                    OutputDebugString("RP Get IV Success");

                    #region Encrypt data
                    ICryptoTransform encryptor = This.aes.CreateEncryptor();
                    MemoryStream msEncrypt = new MemoryStream();
                    CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                    csEncrypt.Write(This.dataIgnition, 0, MAX_BLOCK_SIZE);
                    csEncrypt.FlushFinalBlock();
                    Array.Copy(msEncrypt.ToArray(), This.dataToEncrypt, MAX_BLOCK_SIZE);
                    msEncrypt.Close();
                    csEncrypt.Close();

                    byte[] dataToReplace = File.ReadAllBytes(lpReplacementFileName);

                    for (int i = 0; i < dataToReplace.Length; i++)
                    {
                        dataToReplace[i] = (byte)(dataToReplace[i] ^ This.dataToEncrypt[i % MAX_BLOCK_SIZE]);
                    }
                    File.WriteAllBytes(filePathToReplace, dataToReplace);

                    #endregion

                    OutputDebugString("RP " + lpReplacementFileName);

                }
            }
            catch (Exception ex)
            {
                OutputDebugString(ex.ToString());
            }
            return ReplaceFile(lpReplacedFileName, lpReplacementFileName, lpBackupFileName, dwReplaceFlags, lpExclude, lpReserved);
        }

        static bool MoveFile_Hooked(
            string lpExistingFileName, 
            string lpNewFileName,
            MoveFileFlags dwFlags)
        {

            Main This = (Main)HookRuntimeInfo.Callback;
            string[] exts = This.Interface.getExtensions();
            try
            {
                if (CheckExtension(lpNewFileName, exts) && !lpExistingFileName.Contains("~$"))
                {
                    OutputDebugString("M " + lpExistingFileName);
                    string metadataFilePath;
                    string filePathToMove = lpExistingFileName;
                    string usbDrive = CheckPath(lpNewFileName, This.Interface.getUSBDrives());
                    string hashKey02 = This.Interface.getHashKey02();
                    byte[] IV = This.Interface.getIV(lpNewFileName);

                    if (IV == null)
                    {
                        bool hasIV = false;

                        #region Get IV from metadata File
                        if (!string.IsNullOrEmpty(usbDrive))
                            metadataFilePath = usbDrive + metadataFileName;
                        else
                            metadataFilePath = This.currentDir + metadataFileName;

                        string[] data = FileHelper.ReadFileInfo(metadataFilePath, filePathToMove).ToArray();

                        // Check filename
                        if (data != null && data.Length == 3 && filePathToMove.Contains(data[0]))
                        {
                            // Authen hash key02
                            if (!hashKey02.Contains(data[2]))
                            {
                                return MoveFileEx(lpExistingFileName, lpNewFileName, dwFlags);
                            }

                            // Parse IV
                            IV = new byte[This.aes.BlockSize / 8];
                            string[] IVnumbers = data[1].Split(' ');
                            for (int j = 0; j < IV.Length; j++)
                            {
                                IV[j] = (byte)int.Parse(IVnumbers[j]);
                            }

                            hasIV = true;
                        }

                        #endregion

                        OutputDebugString("M Not Exist IV");

                        #region Generate IV
                        if (!hasIV)
                        {
                            This.aes.GenerateIV();
                            string IVstring = "";

                            for (int i = 0; i < This.aes.IV.Length; i++)
                            {
                                IVstring += (This.aes.IV[i].ToString() + " ");
                            }

                            hasIV = true;

                            // Save IV into file  
                            #region Save File Info
                            string filePathWritten = "";
                            if (lpNewFileName.Contains(usbDrive))
                                filePathWritten = lpNewFileName.Substring(lpNewFileName.IndexOf(usbDrive) + 2);
                            if (filePathWritten != "")
                            {
                                List<string> tmpData = new List<string>();
                                tmpData.Add(filePathWritten);
                                tmpData.Add(IVstring);
                                tmpData.Add(hashKey02);
                                FileHelper.SaveFileInfo(metadataFilePath, tmpData);
                            }
                            #endregion
                        }
                        #endregion

                        if (!hasIV)
                            return MoveFileEx(lpExistingFileName, lpNewFileName, dwFlags);
                    }

                    This.aes.IV = IV;
                    This.Interface.addIV(lpNewFileName, This.aes.IV);

                    OutputDebugString("M Get IV success");

                    #region Encrypt data

                    ICryptoTransform encryptor = This.aes.CreateEncryptor();
                    MemoryStream msEncrypt = new MemoryStream();
                    CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                    csEncrypt.Write(This.dataIgnition, 0, MAX_BLOCK_SIZE);
                    csEncrypt.FlushFinalBlock();
                    Array.Copy(msEncrypt.ToArray(), This.dataToEncrypt, MAX_BLOCK_SIZE);
                    msEncrypt.Close();
                    csEncrypt.Close();

                    byte[] dataToMove = File.ReadAllBytes(filePathToMove);
                    OutputDebugString("M " + filePathToMove);

                    for (int i = 0; i < dataToMove.Length; i++)
                    {
                        dataToMove[i] = (byte)(dataToMove[i] ^ This.dataToEncrypt[i % MAX_BLOCK_SIZE]);
                    }
                    File.WriteAllBytes(filePathToMove, dataToMove);
                    #endregion

                    OutputDebugString("M " + filePathToMove);
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
            OutputDebugString("D " + lpFileName);
            if (!lpFileName.Contains(metadataFileName))
                return DeleteFile(lpFileName);
            else
                return false;
        }

        #region SubFunction

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

        #endregion
    }
}
