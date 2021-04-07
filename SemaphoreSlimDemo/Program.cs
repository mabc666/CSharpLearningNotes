using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreSlimDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // 下面该程序展示了如何使用信号量去控制共享资源的使用
            // 假设我们有一座桥只能过5个人，现在有10个人要过桥我们要对其进行控制
            var semaphore = new SemaphoreSlim(5);
            for (int i = 1; i <= 20; i++)
            {
                Thread.Sleep(100); // 排队上桥
                var index = i; // 定义index 避免出现闭包的问题
                Task.Run(() =>
                {
                    semaphore.Wait();

                    try
                    {
                        Console.WriteLine($"第{index}个人正在过桥。");
                        Thread.Sleep(5000);
                    }
                    finally
                    {
                        Console.WriteLine($"第{index}个人已经过桥。");
                        semaphore.Release();
                    }
                });
            }
            Console.ReadKey();


        }
    }
}
