#include <Windows.h>
#include <iostream>
#include <string> 

typedef bool(*FuncTest)(const char* data1, int data2);
typedef bool(*functionPointer)(const char* data1, int data2);
typedef bool(*CallbackFunction)(functionPointer callback);


bool realFunction(const char* data1, int data2)
{
    std::cout << data1  << "\n";
    std::cout << std::to_string(data2) << "\n";
    return true;
}


int main()
{
    HMODULE hDll = LoadLibrary(L".\\ManagedDll.dll");
    if (hDll != NULL)
    {
        CallbackFunction DLLFuncPtr = (CallbackFunction)GetProcAddress(hDll, "CallbackFunction");
        if (DLLFuncPtr != NULL)
        {
            DLLFuncPtr(&realFunction);
        }

        FuncTest pFuncTest = (FuncTest)GetProcAddress(hDll, "FuncTest");
        if (pFuncTest != NULL)
        {
            pFuncTest("测试数据1", 500);
        }
    }
    system("pause");
    return 0;
}
