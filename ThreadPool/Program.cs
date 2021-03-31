// 该演示程序演示了线程池中的一些概念
// 1、将任务交给线程池完成
// 2、阻止上下文的传递
// 3、任务超时取消
// 更详尽的本地代码调用知识见https://docs.microsoft.com/zh-cn/dotnet/standard/native-interop/
// 更多的本地代码的调用可以查看https://www.pinvoke.net/
using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace ThreadPoolDemo

{
    class Program
    {
        static void Main(string[] args)
        {
            //---------------------1、线程池任务的启动-----------------//
            // 该方法接受一个WaitCallback的带一个参数、无返回值的委托方法，该方法有一个重载的函数自动将后面参数置为空了
            ThreadPool.QueueUserWorkItem(WaitCallBackInstance, "Value");

            //---------------------2、阻止上下文的传递-----------------//
            // 将内容传入上下文中保存
            CallContext.LogicalSetData("Key",5);
            ThreadPool.QueueUserWorkItem(s => {
                Console.WriteLine($"Before Suppress {CallContext.LogicalGetData("Key")}");
            },null);
            // 阻止上下文的流动
            ExecutionContext.SuppressFlow();
            ThreadPool.QueueUserWorkItem(s => {
                Console.WriteLine($"After Suppress {CallContext.LogicalGetData("Key")}");
            }, null);

            // 恢复上下文的流动
            ExecutionContext.RestoreFlow();
            ThreadPool.QueueUserWorkItem(s => {
                Console.WriteLine($"Restore {CallContext.LogicalGetData("Key")}");
            }, null);

            //---------------------3、任务的超时取消-----------------//
            // 该对象包含了管理取消的所有状态
            CancellationTokenSource cts = new CancellationTokenSource();
            // 我们可以给取消Token附加多个取消后的回调，该回调执行顺序为先进后出
            // Register方法的第二个参数决定回调函数到底由谁执行，为false的话会由调用Cancel的线程调用，如果为true的话则回调会被send给已捕捉的SynchronizationContext
            cts.Token.Register(() => {
                Console.WriteLine($"Task Cancel callback1_{Thread.CurrentThread.ManagedThreadId}");
            }, false);
            cts.Token.Register(() => {
                Console.WriteLine($"Task Cancel callback2_{Thread.CurrentThread.ManagedThreadId}");
            }, false);
            cts.Token.Register(() => {
                Console.WriteLine($"Task Cancel callback3_{Thread.CurrentThread.ManagedThreadId}");    
            }, false);
            ThreadPool.QueueUserWorkItem(s => {
                OverTimeTask(cts.Token);
                // 下面是传入的是一种特殊的Token该Token的IsCancellationRequested永远是false
                //OverTimeTask(CancellationToken.None);
            });
            // 延迟2s，通知任务超时
            Thread.Sleep(2000);
            Console.WriteLine($"Task OverTime And Cancel it{Thread.CurrentThread.ManagedThreadId}");
            cts.Cancel();

            // 关联Token，可以用CreateLinkedTokenSource创建一个与其他Token关联的Token，任何一个关联的取消都会导致该Token被取消
            CancellationTokenSource ctsA = new CancellationTokenSource();
            CancellationTokenSource ctsB = new CancellationTokenSource();
            var SeriesToken = CancellationTokenSource.CreateLinkedTokenSource(ctsA.Token, ctsB.Token);
            Console.WriteLine($"Is SeriesToken Cancel{SeriesToken.IsCancellationRequested}");
            ctsA.Cancel();
            Console.WriteLine($"Is SeriesToken Cancel{SeriesToken.IsCancellationRequested}");
            ctsB.Cancel();
            Console.WriteLine($"Is SeriesToken Cancel{SeriesToken.IsCancellationRequested}");

            // 创建一个定时取消的任务，无论任务是否被完成,在服务器中比较常用
            CancellationTokenSource ctsTimer = new CancellationTokenSource();
            ctsTimer.Token.Register(()=> {
                Console.WriteLine("Task is cancelled by timer");
            });
            ThreadPool.QueueUserWorkItem(s => {
                Thread.Sleep(1000);
            }, ctsTimer);
            ctsTimer.CancelAfter(2000);


            Console.ReadKey();
        }

        public static void WaitCallBackInstance(object obj)
        {
            Console.WriteLine($"{obj.ToString()}");
        }

        public static void OverTimeTask(CancellationToken ctsToken)
        {
            Console.WriteLine($"OverTimeTask Start{Thread.CurrentThread.ManagedThreadId}");
           
            for (int i = 0; i < 5; i++)
            {
                if (ctsToken.IsCancellationRequested)
                {
                    Console.WriteLine("OverTimeTask is cancelled");
                    break;
                }
                Thread.Sleep(1000);
            }
            Console.WriteLine("OverTimeTask End");
        }

    }
}
