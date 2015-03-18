#include <stdio.h>
#include <Windows.h>
#include <WinInet.h>
#include "apihook.h"
#include "winternl.h"

//typedef HINTERNET (WINAPI *pInternetConnectW)(HINTERNET,LPCWSTR,INTERNET_PORT,LPCWSTR,LPCWSTR,DWORD,DWORD,DWORD_PTR);

//pInternetConnectW fnInternetConnectW;
HANDLE handle, hdFile;

HINSTANCE hInst;
API_HOOK Hook, Hook2;
wchar_t szModuleName[260], str[1024];

extern "C" __declspec(dllexport) LRESULT CALLBACK CallWndProc(int nCode, WPARAM wParam, LPARAM lParam)
{
	return CallNextHookEx(NULL, nCode, wParam, lParam);
}

extern "C" __declspec(dllexport) void SetHook()
{
	SetWindowsHookEx(WH_CALLWNDPROC, CallWndProc, hInst, 0);
	Sleep(INFINITE);
}

//HINTERNET WINAPI HookInternetConnectW(HINTERNET hInternet,LPCWSTR ServerName,INTERNET_PORT InternetPort,LPCWSTR UserName,LPCWSTR Password,DWORD dwService,DWORD dwFlags,DWORD_PTR dwContext)
//{
//	if(wcsstr(ServerName,L"google"))
//	{
//		OutputDebugString("Your request to access Google has been denied!");
//		SetLastError(ERROR_ACCESS_DENIED);
//		return NULL;
//	}
//
//	return fnInternetConnectW(hInternet,ServerName,InternetPort,UserName,Password,dwService,dwFlags,dwContext);
//}

typedef HANDLE(WINAPI *pCreateFileW)(LPCTSTR, DWORD, DWORD, LPSECURITY_ATTRIBUTES,
	DWORD, DWORD, HANDLE);
pCreateFileW fnCreateFileW;

HANDLE WINAPI HookCreateFile(
	LPCTSTR lpFileName,
	DWORD dwDesiredAccess,
	DWORD dwShareMode,
	LPSECURITY_ATTRIBUTES lpSecurityAttributes,
	DWORD dwCreationDisposition,
	DWORD dwFlagsAndAttributes,
	HANDLE hTemplateFile
	)
{
	/*hdFile =  fnCreateFileW(lpFileName, dwDesiredAccess, dwShareMode, lpSecurityAttributes, 
		dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile);*/
	TCHAR buffer[1024];
	wsprintf(buffer, L"CreateFileW: %ls", lpFileName);
	OutputDebugString(L"Hook");
	return fnCreateFileW(lpFileName, dwDesiredAccess, dwShareMode, lpSecurityAttributes,
		dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile);
}

//int WINAPI MyCreateFile(PHANDLE handle, ACCESS_MASK  access, POBJECT_ATTRIBUTES attr,
//	PIO_STATUS_BLOCK io, PLARGE_INTEGER alloc_size, ULONG  attributes, ULONG sharing,
//	ULONG  disposition, ULONG options, PVOID ea_buffer, ULONG ea_length)
//{
//	MessageBox(0, LPWSTR("NtDll"), LPWSTR("NtCreateFile"),
//		MB_ICONINFORMATION);
//	int(WINAPI *pCreateFile)(PHANDLE handle, ACCESS_MASK  access, POBJECT_ATTRIBUTES attr,
//		PIO_STATUS_BLOCK io, PLARGE_INTEGER alloc_size, ULONG  attributes, ULONG sharing,
//		ULONG  disposition, ULONG options, PVOID ea_buffer, ULONG ea_length);
//
//	/*pCreateFile = (int (WINAPI *)(PHANDLE, ACCESS_MASK, POBJECT_ATTRIBUTES, PIO_STATUS_BLOCK, PLARGE_INTEGER,
//	ULONG, ULONG, ULONG, ULONG, PVOID, ULONG))GetOriginalFunction((ULONG_PTR)MyCreateFile);*/
//
//
//	return pCreateFile(handle, access, attr, io, alloc_size, attributes, sharing, disposition, options, ea_buffer, ea_length);
//	//return NULL;
//}


//int HookWriteFile
//(
//_In_         HANDLE hFile,
//_In_         LPCVOID lpBuffer,
//_In_         DWORD nNumberOfBytesToWrite,
//_Out_opt_    LPDWORD lpNumberOfBytesWritten,
//_Inout_opt_  LPOVERLAPPED lpOverlapped
//)
//{
//	MessageBox(0, LPSTR("Kernel32"), LPSTR("WriteFile"),
//		MB_ICONINFORMATION);
//	return WriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, lpNumberOfBytesWritten, lpOverlapped);
//}

//BOOL WINAPI HookWriteFileEx(
//	_In_      HANDLE hFile,
//	_In_opt_  LPCVOID lpBuffer,
//	_In_      DWORD nNumberOfBytesToWrite,
//	_Inout_   LPOVERLAPPED lpOverlapped,
//	_In_      LPOVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine
//	)
//{
//	MessageBox(0, LPSTR("Kernel32"), LPSTR("WriteFileEx"),
//		MB_ICONINFORMATION);
//	return WriteFileEx(hFile, lpBuffer, nNumberOfBytesToWrite, lpOverlapped, lpCompletionRoutine);
//}

//BOOL WINAPI HookCopyFile(
//	_In_  LPCTSTR lpExistingFileName,
//	_In_  LPCTSTR lpNewFileName,
//	_In_  BOOL bFailIfExists
//	)
//{
//	MessageBox(0, LPSTR("Kernel32"), LPSTR("CopyFile"),
//		MB_ICONINFORMATION);
//	return NULL;
//
//}

//BOOL WINAPI HookCopyFileTransacted(
//	_In_      LPCTSTR lpExistingFileName,
//	_In_      LPCTSTR lpNewFileName,
//	_In_opt_  LPPROGRESS_ROUTINE lpProgressRoutine,
//	_In_opt_  LPVOID lpData,
//	_In_opt_  LPBOOL pbCancel,
//	_In_      DWORD dwCopyFlags,
//	_In_      HANDLE hTransaction
//	)
//{
//	MessageBox(0, LPSTR("Kernel32"), LPSTR("CopyFileTransacted"),
//		MB_ICONINFORMATION);
//	return NULL;
//}

//BOOL WINAPI HookCopyFileEx(
//	_In_      LPCTSTR lpExistingFileName,
//	_In_      LPCTSTR lpNewFileName,
//	_In_opt_  LPPROGRESS_ROUTINE lpProgressRoutine,
//	_In_opt_  LPVOID lpData,
//	_In_opt_  LPBOOL pbCancel,
//	_In_      DWORD dwCopyFlags
//	)
//{
//	MessageBox(0, LPSTR("Kernel32"), LPSTR("CopyFileEx"),
//		MB_ICONINFORMATION);
//	return NULL;
//}

//NTSTATUS HookZwWriteFile(
//	_In_      HANDLE FileHandle,
//	_In_opt_  HANDLE Event,
//	_In_opt_  PIO_APC_ROUTINE ApcRoutine,
//	_In_opt_  PVOID ApcContext,
//	_Out_     PIO_STATUS_BLOCK IoStatusBlock,
//	_In_      PVOID Buffer,
//	_In_      ULONG Length,
//	_In_opt_  PLARGE_INTEGER ByteOffset,
//	_In_opt_  PULONG Key
//	)
//{
//	MessageBox(0, LPSTR("NtDll"), LPSTR("ZwWriteFile"),
//		MB_ICONINFORMATION);
//	return NULL;
//}

//BOOL MyTextOut(
//	_In_  HDC hdc,
//	_In_  int nXStart,
//	_In_  int nYStart,
//	_In_  LPCTSTR lpString,
//	_In_  int cchString
//	)
//{
//	MessageBox(0, LPSTR("Gdi32.dll"), LPSTR("TextOut"),
//		MB_ICONINFORMATION);
//	return false;
//}

//int WINAPI MyGetWindowText(
//	_In_   HWND hWnd,
//	_Out_  LPTSTR lpString,
//	_In_   int nMaxCount
//	)
//{
//	MessageBox(0, LPSTR("User32.dll"), LPSTR("GetWindowText"),
//		MB_ICONINFORMATION);
//	return NULL;
//}

//typedef HANDLE (WINAPI* pMessageBoxW)(HWND, LPCTSTR, LPCTSTR, UINT);
//pMessageBoxW fnMessageBoxW;
//
//HANDLE WINAPI MyMessageBox(
//	_In_opt_  HWND hWnd,
//	_In_opt_  LPCTSTR lpText,
//	_In_opt_  LPCTSTR lpCaption,
//	_In_      UINT uType
//	)
//{
//	OutputDebugString("Hook");
//	return fnMessageBoxW(hWnd, lpText, lpCaption, uType);
//}

//int MyDrawText(
//	_In_     HDC hDC,
//	_Inout_  LPCTSTR lpchText,
//	_In_     int nCount,
//	_Inout_  LPRECT lpRect,
//	_In_     UINT uFormat
//	)
//{
//	MessageBox(0, LPSTR("User32.dll"), LPSTR("DrawText"),
//		MB_ICONINFORMATION);
//	return NULL;
//}

//int WINAPI MyMessageBoxEx(
//	_In_opt_  HWND hWnd,
//	_In_opt_  LPCTSTR lpText,
//	_In_opt_  LPCTSTR lpCaption,
//	_In_      UINT uType,
//	_In_      WORD wLanguageId
//	)
//{
//	MessageBox(0, LPSTR("User32.dll"), LPSTR("MessageBoxEx"),
//		MB_ICONINFORMATION);
//	return NULL;
//}

//typedef bool (WINAPI *pWriteFile)(HANDLE, LPCVOID, DWORD, LPDWORD, LPOVERLAPPED);
//pWriteFile fnWriteFile;
//
//BOOL WINAPI MyWriteFile(
//	_In_         HANDLE hFile,
//	_In_         LPCVOID lpBuffer,
//	_In_         DWORD nNumberOfBytesToWrite,
//	_Out_opt_    LPDWORD lpNumberOfBytesWritten,
//	_Inout_opt_  LPOVERLAPPED lpOverlapped
//	)
//{
//	TCHAR buffer[1024];
//	sprintf_s(buffer, 1024, "WriteFile: %ls + %ls", hFile, lpBuffer);
//	//if (strstr(szModuleName, "xplorer.") != NULL)
//		OutputDebugString(buffer);
//		/*byte* buffers = new byte[nNumberOfBytesToWrite];
//		memcpy(buffers, lpBuffer, nNumberOfBytesToWrite);*/
//	return fnWriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, lpNumberOfBytesWritten, lpOverlapped);
//}

BOOL WINAPI DllMain(HMODULE hModule, DWORD dwReason, LPVOID lpReserved)
{
	hInst = hModule;
	TCHAR tchar[34];

	switch (dwReason)
	{
	case DLL_PROCESS_ATTACH:
		GetModuleFileName(NULL, szModuleName, 260);


		wsprintf(str, L"Hook DLL loaded into process %s (%d)", szModuleName, GetCurrentProcessId());

		OutputDebugString(str);

		if (wcsstr(szModuleName, L"xplorer+.") == NULL) break;

		//InitAPIHook(&Hook,"wininet.dll","InternetConnectW",HookInternetConnectW);

		InitAPIHook(&Hook, L"Kernel32.dll", "CreateFileW", HookCreateFile);
		fnCreateFileW = (pCreateFileW)Hook.OrigFunction;

		/*InitAPIHook(&Hook, "Kernel32.dll", "WriteFile", MyWriteFile);
		fnWriteFile = (pWriteFile)Hook.OrigFunction;*/

		//InitAPIHook(&Hook, "NtDll.dll", "NtCreateFileW", MyCreateFile);

		//InitAPIHook(&Hook, "Kernel32.dll", "WriteFileEx", HookWriteFileEx);

		//InitAPIHook(&Hook, "Kernel32.dll", "CopyFileExW", HookCopyFileEx);

		//InitAPIHook(&Hook, "NtDll.dll", "ZwWriteFile", HookZwWriteFile);

		//InitAPIHook(&Hook, "Gdi32.dll", "TextOutA", MyTextOut);

		//InitAPIHook(&Hook, "User32.dll", "GetWindowTextW", MyGetWindowText);

		/*InitAPIHook(&Hook, "User32.dll", "MessageBoxW", MyMessageBox);
		fnMessageBoxW = (pMessageBoxW)Hook.OrigFunction;*/
		
		//InitAPIHook(&Hook, "User32.dll", "DrawTextW", MyDrawText);

		//InitAPIHook(&Hook, "User32.dll", "MessageBoxExW", MyMessageBox);

		//handle = Hook.OrigFunction;


		//fnInternetConnectW=(pInternetConnectW)Hook.OrigFunction;
		StartAPIHook(&Hook);
		//StartAPIHook(&Hook2);
		
		break;

	case DLL_PROCESS_DETACH:
		UnhookAPIHook(&Hook);
		RemoveAPIHook(&Hook);
		/*UnhookAPIHook(&Hook2);
		RemoveAPIHook(&Hook2);*/
		break;
	}

	return TRUE;
}