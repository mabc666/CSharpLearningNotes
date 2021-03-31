// 该程序演示了并行计算类(Parallel)的用法和特性
// 1、启动一个并行任务
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ParallelDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //----------------1、启动Parallel任务----------------//
            // 直接开启的并行任务不能保证执行顺序,但是不论是用for、foreach、Invoke调用的工作都会等待任务完成才会往下走
            // Tips:要确保任务是能够并行执行的
            Parallel.For(0, 100, i => DoWork(i));
            //Parallel.Invoke(
            //    ()=> DoWork(0),
            //    () => DoWork(1),
            //    ()=> DoWork(2));
            Console.WriteLine("节点1");
            Console.ReadKey();

        }
        public static void DoWork(int i) 
        {
            Console.WriteLine(i);        
        }

    }
}
