typedef struct _API_HOOK
{
	BOOL Hooked;
	PVOID FunctionAddress;
	PVOID Hook;
	char jmp[6];
	char OrigBytes[6];
	PVOID OrigFunction;
}API_HOOK,*PAPI_HOOK;

BOOL WINAPI InitAPIHook(PAPI_HOOK Hook,char* szModuleName,char* szFunctionName,PVOID HookFunction)
{
	HMODULE hModule;
	ULONG OrigFunction,FunctionAddress;
	char opcodes[]={0x90,0x90,0x90,0x90,0x90,0xe9,0x00,0x00,0x00,0x00};

	if(Hook->Hooked)
	{
		return FALSE;
	}

	hModule=GetModuleHandle(szModuleName);

	if(hModule==NULL)
	{
		Hook->Hooked=FALSE;
		return FALSE;
	}

	Hook->FunctionAddress=GetProcAddress(hModule,szFunctionName);

	if(Hook->FunctionAddress==NULL)
	{
		Hook->Hooked=FALSE;
		return FALSE;
	}

	Hook->jmp[0]=0xe9;
	*(PULONG)&Hook->jmp[1]=(ULONG)HookFunction-(ULONG)Hook->FunctionAddress-5;

	memcpy(Hook->OrigBytes,Hook->FunctionAddress,5);
	
	Hook->OrigFunction=VirtualAlloc(NULL,4096,MEM_COMMIT|MEM_RESERVE,PAGE_EXECUTE_READWRITE);

	if(Hook->OrigFunction==NULL)
	{
		return FALSE;
	}

	memcpy(Hook->OrigFunction,Hook->OrigBytes,5);

	OrigFunction=(ULONG)Hook->OrigFunction+5;
	FunctionAddress=(ULONG)Hook->FunctionAddress+5;

	*(LPBYTE)((LPBYTE)Hook->OrigFunction+5)=0xe9;
	*(PULONG)((LPBYTE)Hook->OrigFunction+6)=(ULONG)FunctionAddress-(ULONG)OrigFunction-5;

	Hook->Hooked=TRUE;
	return TRUE;
}

BOOL WINAPI InitAPIHookByAddress(PAPI_HOOK Hook,PVOID Address,PVOID HookFunction)
{
	ULONG OrigFunction,FunctionAddress;
	char opcodes[]={0x90,0x90,0x90,0x90,0x90,0xe9,0x00,0x00,0x00,0x00};

	if(Hook->Hooked)
	{
		return FALSE;
	}

	Hook->FunctionAddress=Address;

	Hook->jmp[0]=0xe9;
	*(PULONG)&Hook->jmp[1]=(ULONG)HookFunction-(ULONG)Hook->FunctionAddress-5;

	memcpy(Hook->OrigBytes,Hook->FunctionAddress,5);
	
	Hook->OrigFunction=VirtualAlloc(NULL,4096,MEM_COMMIT|MEM_RESERVE,PAGE_EXECUTE_READWRITE);

	memcpy(Hook->OrigFunction,Hook->OrigBytes,5);

	OrigFunction=(ULONG)Hook->OrigFunction+5;
	FunctionAddress=(ULONG)Hook->FunctionAddress+5;

	*(LPBYTE)((LPBYTE)Hook->OrigFunction+5)=0xe9;
	*(PULONG)((LPBYTE)Hook->OrigFunction+6)=(ULONG)FunctionAddress-(ULONG)OrigFunction-5;

	Hook->Hooked=TRUE;
	return TRUE;
}

BOOL WINAPI StartAPIHook(PAPI_HOOK Hook)
{
	DWORD op;

	if(!Hook->Hooked)
	{
		return FALSE;
	}
	
	VirtualProtect(Hook->FunctionAddress,5,PAGE_EXECUTE_READWRITE,&op);
	memcpy(Hook->FunctionAddress,Hook->jmp,5);
	VirtualProtect(Hook->FunctionAddress,5,op,&op);

	return TRUE;
}

BOOL WINAPI UnhookAPIHook(PAPI_HOOK Hook)
{
	DWORD op;

	if(!Hook->Hooked)
	{
		return FALSE;
	}

	VirtualProtect(Hook->FunctionAddress,5,PAGE_EXECUTE_READWRITE,&op);
	memcpy(Hook->FunctionAddress,Hook->OrigBytes,5);
	VirtualProtect(Hook->FunctionAddress,5,op,&op);

	Hook->Hooked=FALSE;
	return TRUE;
}

BOOL WINAPI RemoveAPIHook(PAPI_HOOK Hook)
{
	if(Hook->Hooked)
	{
		return FALSE;
	}
	
	VirtualFree(Hook->OrigFunction,0,MEM_RELEASE);
	memset(Hook,0,sizeof(API_HOOK));
	return TRUE;
}