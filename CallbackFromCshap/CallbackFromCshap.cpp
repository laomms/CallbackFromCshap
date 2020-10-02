#include <Windows.h>
#include <iostream>
#include <string> 

typedef const char* (*FuncTest)(const char* data1, int data2);
typedef const char* (*functionPointer)(const char* data1, int data2);
typedef const char* (*CallbackFunction)(functionPointer callback);


const char* realFunction(const char* data1, int data2)
{
    std::cout << data1  << "\n";
    std::cout << std::to_string(data2) << "\n";
    return data1;
}


int main()
{
    HMODULE hDll = LoadLibrary(L".\\ManagedDll.dll");
    if (hDll != NULL)
    {
        CallbackFunction DLLFuncPtr = (CallbackFunction)GetProcAddress(hDll, "CallbackFunction");
        if (DLLFuncPtr != NULL)
        {
            const char* ret =DLLFuncPtr(&realFunction);
            std::cout << ret << "\n";
        }

        FuncTest pFuncTest = (FuncTest)GetProcAddress(hDll, "FuncTest");
        if (pFuncTest != NULL)
        {
            const char*  res =pFuncTest("测试数据1", 500);
            std::cout << res << "\n";
        }
    }
    system("pause");
    return 0;
}
