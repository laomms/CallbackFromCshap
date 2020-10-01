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
        public delegate bool DelegateFunc([MarshalAs(UnmanagedType.LPStr)] string data1, int data2);

        public static DelegateFunc GetResult = null;
        [DllExport("CallbackFunction", CallingConvention = CallingConvention.Cdecl)]
        public static bool CallbackFunction(IntPtr callback)
        {
            DelegateFunc myFunc = (DelegateFunc)Marshal.GetDelegateForFunctionPointer(callback, typeof(DelegateFunc));
            GetResult = myFunc;
            return true;
        }


        
        [DllExport("FuncTest", CallingConvention = CallingConvention.Cdecl)]
        public static bool FuncTest([MarshalAs(UnmanagedType.LPStr)] string data1, int data2)
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
                        Thread.Sleep(200);
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
           
           
            return true;
        }

    }
}
