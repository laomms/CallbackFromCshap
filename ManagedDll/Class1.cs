using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDll
{
    public class Class1
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool DelegateFunc([MarshalAs(UnmanagedType.LPStr)] string data1, [MarshalAs(UnmanagedType.LPStr)] string data2);

        public static DelegateFunc GetResult = null;
        [DllExport("CallbackFunction", CallingConvention = CallingConvention.Cdecl)]
        public static bool CallbackFunction(IntPtr callback)
        {
            DelegateFunc myFunc = (DelegateFunc)Marshal.GetDelegateForFunctionPointer(callback, typeof(DelegateFunc));
            GetResult = myFunc;
            return true;
        }


        
        [DllExport("FuncTest", CallingConvention = CallingConvention.Cdecl)]
        public static bool FuncTest([MarshalAs(UnmanagedType.LPStr)] string data1, [MarshalAs(UnmanagedType.LPStr)] string data2)
        {
            GetResult(data2,data1);
            return true;
        }

    }
}
