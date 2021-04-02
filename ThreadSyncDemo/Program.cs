// 该程序演示了并发的任务如何使用AutoResetEvent进行顺序控制
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSyncDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoResetEvent _second = new AutoResetEvent(false);
            AutoResetEvent _three = new AutoResetEvent(false);
            Task.Run(() =>
            {
                Console.WriteLine("first thread");
                _second.Set();
            });
            Task.Run(() =>
            {
                _second.WaitOne();
                Console.WriteLine("second thread");
                _three.Set();
            });
            Task.Run(() =>
            {
                _three.WaitOne();
                Console.WriteLine("thrid thread");

            });
            Console.ReadKey();

        }
    }
}
