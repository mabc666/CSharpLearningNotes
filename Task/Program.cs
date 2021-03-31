// 该程序演示了Task类中的一些特性
// 1、启动一个Task任务
// 2、取消Task任务
// 3、任务完成后的后续任务启动
// 4、任务状态描述
// 5、任务工厂
// 6、任务调度器
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //----------------1、启动Task任务----------------//
            // 第一个参数是一个委托
            // 第二个参数是CancellationToken
            // 第三个参数是TaskCreationOptions,该参数涉及到任务的调度具体查阅资料
            CancellationTokenSource cts1 = new CancellationTokenSource();
            Task task1 = new Task(() => {
                Console.WriteLine("Task1 run");
            }, cts1.Token,(TaskCreationOptions.LongRunning | TaskCreationOptions.DenyChildAttach));
            task1.Start();

            Task.Run(() =>
            {
                Console.WriteLine("Task2 run");
            });

            //-------------2、取消Task任务------------------//
            CancellationTokenSource cts2 = new CancellationTokenSource();
            Task<String> taskWithReturn = new Task<String>(() => {
                Thread.Sleep(5000);
                cts2.Token.ThrowIfCancellationRequested();
                return "Task2 End";
            },cts2.Token);
            cts2.Cancel();
            try
            {
                var temp = taskWithReturn.Result;
            } catch (AggregateException ex)
            {
                // Handle方法会对方法进行过滤如果不是指定的方法则会继续抛出异常
                ex.Handle(s => s is OperationCanceledException);
                foreach (var temp in ex.InnerExceptions)
                {
                    Console.WriteLine(temp.Message);
                }
                Console.WriteLine("Task is Cancel");
            }


            //-------------3、任务完成后的后续任务启动------------------//
            Task<String>.Run(() => {
                Console.WriteLine("Task3 Run");
                Thread.Sleep(2000);
                return "ABC";
            }).ContinueWith(s => {
                Console.WriteLine($"The Task3 Result is {s.Result}");
            });

            // Task任务完成后可以执行一个任务集合
            var task2 = new Task(()=> {
                Console.WriteLine("Task4 Run");
            });
            task2.ContinueWith(task => { Console.WriteLine("Task4 Completion Successful"); },TaskContinuationOptions.OnlyOnRanToCompletion);
            task2.ContinueWith(task => { Console.WriteLine("Task4 Fault"); }, TaskContinuationOptions.OnlyOnFaulted);
            task2.ContinueWith(task => { Console.WriteLine("Task4 Cancel"); }, TaskContinuationOptions.OnlyOnCanceled);
            task2.Start();
           

            // 创建父子任务
            Task<int[]> parent = new Task<int[]>(()=> {
                int[] result = new int[3];
                new Task(() => {
                    Console.WriteLine("Son1 Run");
                    result[0] = 0;
                },TaskCreationOptions.AttachedToParent).Start();
                new Task(() => {
                    Console.WriteLine("Son2 Run");
                    result[1] = 1;
                }, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => {
                    Console.WriteLine("Son3 Run");
                    result[2] = 2;
                }, TaskCreationOptions.AttachedToParent).Start();
                return result;
            });

            parent.ContinueWith(task => {
                Console.WriteLine("All Task Completion");
                Array.ForEach(task.Result, Console.WriteLine);
            });
            parent.Start();

            //-------------4、任务状态描述------------------//
            // Created - 任务已显示创建，可以手动Start()这个任务
            // WaitingForActivation - 任务已隐式创建,会自动开始
            // WaitingToRun - 任务已调度，但未运行
            // Running - 任务正在运行
            // WaitingForChildrenToComplete - 任务正在等待其子任务完成
            // RanToCompletion - 任务正常完成
            // Canceled - 任务被取消
            // Faulted - 任务错误

            // Task对象提供以下几种测试任务完成的方法
            // task1.Status = TaskStatus.Faulted
            // task1.IsCanceled

            //----------------5、任务工厂----------------//
            // 任务工厂主要解决避免重复配置相同属性的任务
            var cts3 = new CancellationTokenSource();
            var taskFactory = new TaskFactory(
                cts3.Token,
                TaskCreationOptions.AttachedToParent,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default
                );
            var childTasks = new[] {
                taskFactory.StartNew(()=> {
                    Console.WriteLine("TaskFactory Son1 Run");
                }),
                taskFactory.StartNew(()=> {
                    Console.WriteLine("TaskFactory Son2 Run");
                }),
                taskFactory.StartNew(()=> {
                    Console.WriteLine("TaskFactory Son3 Run");
                })
            };

            for (int i = 0; i < childTasks.Length; i++)
            {
                childTasks[i].ContinueWith(task => cts3.Cancel(), TaskContinuationOptions.OnlyOnFaulted);
            }

            taskFactory.ContinueWhenAll(
                childTasks,
                completedTasks =>
                {
                    int i = 0;
                    int j = 0;
                    int k = 0;
                    foreach (var temp in completedTasks)
                    {
                        if (temp.IsCompleted)
                        {
                            i++;
                        }
                        if (temp.IsFaulted)
                        {
                            j++;
                        }
                        if (temp.IsCanceled)
                        {
                            k++;
                        }
                    }
                    Console.WriteLine($"正常完成{i}个任务");
                    Console.WriteLine($"{j}个任务发生错误");
                    Console.WriteLine($"{k}个任务被取消");
                },CancellationToken.None
            );

            //----------------5、任务调度器----------------//
            // 任务调度器有两种类型线:程池任务调度器、同步上下文任务调度器
            // 默认情况所有应用都是调用的线程池任务调度器
            // 同步上下文调度器适合提供了图形用户界面的程序，它将所有的任务都调度给GUI线程，使得所有的任务代码都可以更新UI组件。
            // 此处无法展示更新GUI的操作
            //var SyncTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            //Task.Run(()=> { }).ContinueWith(task => {
            //    Console.WriteLine("更新UI操作");
            //},SyncTaskScheduler);
            
            Console.ReadKey();

        }
    }
}
