using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManagedDll
{
    public class Class1
    {
        public static CancellationTokenSource cts = new CancellationTokenSource();
        public static CancellationToken token = CancellationToken.None;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr DelegateFunc([MarshalAs(UnmanagedType.LPStr)] string data1, int data2);

        //***注意不能用return: MarshalAs(UnmanagedType.LPStr)方式返回字符串到C++
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.LPStr)]
        //public delegate string DelegateFunc([MarshalAs(UnmanagedType.LPStr)] string data1, int data2);

        public static DelegateFunc GetResult = null;

        [DllExport("CallbackFunction", CallingConvention = CallingConvention.Cdecl)]
        public static IntPtr CallbackFunction(IntPtr callback)
        {
            DelegateFunc myFunc = (DelegateFunc)Marshal.GetDelegateForFunctionPointer(callback, typeof(DelegateFunc));
            GetResult = myFunc;
            return Marshal.StringToHGlobalAnsi("已成功收到回调指针");
        }


        
        [DllExport("FuncTest", CallingConvention = CallingConvention.Cdecl)]
        public static string FuncTest([MarshalAs(UnmanagedType.LPStr)] string data1, int data2)
        {

            if (data1== "测试数据1")
            {
                token = cts.Token;
                Task.Factory.StartNew(() =>
                {
                    int i = 0;
                    while (!token.IsCancellationRequested)
                    {
                        i = i + 1;
                        GetResult("\r\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\r\n测试\r\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\r\n" + DateTime.Now.ToString(), i);
                        Thread.Sleep(500);
                        if (i== data2)
                        {                            
                            cts.Cancel();
                            cts.Dispose();
                            cts = new CancellationTokenSource();
                            break;
                        }
                    }
                }, token);
            }          
           
            return "函数调用成功";
        }

    }
}
