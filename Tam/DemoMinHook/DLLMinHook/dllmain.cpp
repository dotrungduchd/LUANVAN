// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include "MinHook.h"

typedef int (WINAPI *MESSAGEBOXW)(HWND, LPCWSTR, LPCWSTR, UINT);
typedef HANDLE(WINAPI *pCreateFileW)(LPCTSTR, DWORD, DWORD, LPSECURITY_ATTRIBUTES,
	DWORD, DWORD, HANDLE);
pCreateFileW fnCreateFileW = NULL;
HINSTANCE hInst;
// Pointer for calling original MessageBoxW.
MESSAGEBOXW fpMessageBoxW = NULL;
// Detour function which overrides MessageBoxW.

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
	
	TCHAR buffer[1024];
	wsprintf(buffer,L"CreateFile: %ls",lpFileName);
	OutputDebugString(buffer);
	//OutputDebugString(L"Tam dep trai");
	return fnCreateFileW(lpFileName, dwDesiredAccess, dwShareMode, lpSecurityAttributes,
		dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile);
}

int WINAPI DetourMessageBoxW(HWND hWnd, LPCWSTR lpText, LPCWSTR lpCaption, UINT uType)
{
	OutputDebugStringW(lpText);
	return fpMessageBoxW(hWnd, L"Tam is Hooking!", lpCaption, uType);
}


extern "C" __declspec(dllexport) LRESULT CALLBACK CallWndProc(int nCode, WPARAM wParam, LPARAM lParam)
{
	return CallNextHookEx(NULL, nCode, wParam, lParam);
}

extern "C" __declspec(dllexport) void SetHook()
{
	SetWindowsHookEx(WH_CALLWNDPROC, CallWndProc, hInst, 0);
	Sleep(INFINITE);
}
TCHAR szModuleName[265];
BOOL WINAPI DllMain( HMODULE hModule,
					  DWORD  ul_reason_for_call,
					  LPVOID lpReserved
					  )
{
	hInst = hModule;
	switch (ul_reason_for_call)
	{
	
	case DLL_PROCESS_ATTACH:
		{			
			//MessageBox(NULL,L"Da attach",L"Hehe",MB_OK);
			//GetModuleFileName(NULL,szModuleName,260);
			//if(wcsstr(szModuleName,L"xplorer.") == NULL) break;
			MH_Initialize();			
			LPVOID pfn = GetProcAddress(GetModuleHandle(L"KERNEL32.dll"),"CreateFileW");
			// Create a hook for MessageBoxW, in disabled state.
			MH_CreateHook(pfn, &HookCreateFile, 
				reinterpret_cast<void**>(&fnCreateFileW));
			MH_EnableHook(&CreateFileW);
			OutputDebugString(L"DLL attached");
			//MessageBox(NULL,L"Hooked !!!",L"Tam",MB_OK);
		}
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		{
			MH_DisableHook(&MessageBoxW);		
			MH_Uninitialize();
		}
		break;
	}
	return TRUE;
}

