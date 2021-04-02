// 该程序展示了如何正确的重写一个Equals方法,Object实现的Equals不满足相等性。
using System;

namespace RedefineEqualsDemo
{
    public class A
    {

    }   
    
    public class RedefineEquals : IEquatable<RedefineEquals>
    {
        int x;
        int y;
        // 若要重定义Equals，必须满足最基本的两个条件：同一性和相等性
        // 下面展示了如何正确的自定义一个相等函数
        // 下面定义的Equals是覆写Object的，覆写的有一个缺陷就是无法比较内部变量的值
        public override bool Equals(object obj)
        {
            bool result = true;
            if (null == obj) // 1、验证输入是否为空
            {
                result = false;
            }
            else if (!Object.ReferenceEquals(this, obj))// 2、验证是否指向同一个对象,这里不直接比较是因为该类有可能重载==导致测试异常
            {
                result = false;
            }
            else if (this.GetType() != obj.GetType()) // 3、判断类型是否相同
            {
                result = false;
            }
            return (base.Equals(obj) && result); //4、判断基类是否相同
        }

        // 下面定义的Equals是实现了IEquatable接口，实现该接口后可以对内部变量进行比较
        public bool Equals(RedefineEquals other)
        {
            bool result = true;
            if (null == other) // 1、验证输入是否为空
            {
                result = false;
            }
            else if (!Object.ReferenceEquals(this, other))// 2、验证是否指向同一个对象,这里不直接比较是因为该类有可能重载==导致测试异常
            {
                result = false;
            }
            else if (this.GetType() != other.GetType()) // 3、判断类型是否相同
            {
                result = false;
            }
            else if(other.x != x && other.y != y) // 4、判断类的字段是否相同
            {
                result = false;
            }
            return (base.Equals(other) && result); // 5、判断基类是否相同
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
