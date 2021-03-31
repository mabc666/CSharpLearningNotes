// 该程序演示了如何通过托管代码去调用本地代码，以及展示了某些本地代码执行完如何进行回调的。
// 更详尽的本地代码调用知识见https://docs.microsoft.com/zh-cn/dotnet/standard/native-interop/
// 更多的本地代码的调用可以查看https://www.pinvoke.net/
using System;
using System.Runtime.InteropServices;

namespace PInvoke
{
    class Program
    {
        //-------------------------------托管代码调用本机代码-------------------------------------------//
        // 引入本地代码，声明函数接口，接口形式与本机代码一致
        // 调用MessageBox窗口
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);
        //-------------------------------托管代码调用本机代码-------------------------------------------//







        //-------------------------------本机代码的回调------------------------------------------------//
        // 为本机代码声明回调的接口
        private delegate bool EnumWC(IntPtr hwnd, IntPtr lParam);
        // 引入本机代码，获取所有窗口的枚举值
        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWC lpEnumFunc, IntPtr lParam);

        // 本机代码的回调函数
        public static bool OutputWindows(IntPtr hwnd, IntPtr lParam)
        {
            Console.WriteLine(hwnd.ToInt64());
            return true;
        }
        //-------------------------------本机代码的回调------------------------------------------------//




        static void Main(string[] args)
        {
            //-------------------------------托管代码调用本机代码-------------------------------------------//
            MessageBox(IntPtr.Zero, "Command-line message box", "Attention!", 0);
            //-------------------------------托管代码调用本机代码-------------------------------------------//





            //-------------------------------本机代码的回调------------------------------------------------//
            EnumWindows(OutputWindows, IntPtr.Zero);
            //-------------------------------本机代码的回调------------------------------------------------//
        }
    }
}
