using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionDemo
{
    class Program
    {
        static void Main(string[] args)
        {
         
            Expression <Func<int, bool>> expression = i => i > 100 && i < 150 && i%2 == 0;
        }
    }
}
