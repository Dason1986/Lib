using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            int flag = 1|2|4;
            Console.WriteLine((flag&8));
            Console.WriteLine((flag & 2) == 2);
            Console.WriteLine((flag & 4) == 4);
            //3,5,6,7
           
            Console.Read();
        }
    }
}
