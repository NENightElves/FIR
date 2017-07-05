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
            int x, y;
            FIR_core xxx = new FIR_core();
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                {
                    xxx.board[i, j] = 0;
                }
            xxx.board[8, 8] = 2;
            xxx.board[7, 8] = 2;
            xxx.board[9, 8] = 1;
            

            x = 5;
            y = 8;
            //Console.WriteLine(xxx.imp(x, y, 1));
            //Console.WriteLine(xxx.imp(x, y, 2));
            //Console.WriteLine(xxx.imp_collect_calc(1, x, y, 1, 1));
            //Console.WriteLine(xxx.imp_collect_calc(1, x, y, 1, 0));
            //Console.WriteLine(xxx.imp_collect_calc(1, x, y, 0, 1));
            //Console.WriteLine(xxx.imp_collect_calc(1, x, y, 1, -1));
            //Console.WriteLine(xxx.imp_collect_calc(2, x, y, 1, 1));
            Console.WriteLine(xxx.imp_collect_calc(2, x, y, 1, 0));
            //Console.WriteLine(xxx.imp_collect_calc(2, x, y, 0, 1));
            //Console.WriteLine(xxx.imp_collect_calc(2, x, y, 1, -1));
        }
    }
}
