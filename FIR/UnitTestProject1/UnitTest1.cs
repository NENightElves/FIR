using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FIR;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int i, j;
            FIR_core xxx = new FIR_core();
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                {
                    xxx.board[i, j] = 0;
                }
            xxx.board[8, 8] = 2;
            Console.WriteLine(xxx.imp(7, 7, 1));
            Console.WriteLine(xxx.imp(7, 7, 2));
            Console.WriteLine(xxx.imp(9, 7, 1));
            Console.WriteLine(xxx.imp(9, 7, 2));
            Console.WriteLine(xxx.imp_collect_calc(1, 7, 7, 1, 1));
            Console.WriteLine(xxx.imp_collect_calc(1, 7, 7, 1, 0));
            Console.WriteLine(xxx.imp_collect_calc(1, 7, 7, 0, 1));
            Console.WriteLine(xxx.imp_collect_calc(1, 7, 7, 1, -1));
            Console.WriteLine(xxx.imp_collect_calc(1, 9, 7, 1, 1));
            Console.WriteLine(xxx.imp_collect_calc(1, 9, 7, 1, 0));
            Console.WriteLine(xxx.imp_collect_calc(1, 9, 7, 0, 1));
            Console.WriteLine(xxx.imp_collect_calc(1, 9, 7, 1, -1));
        }
    }
}
