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
            xxx.board[7, 8] = 2;
            xxx.board[9, 8] = 2;
            Console.WriteLine(xxx.imp(13, 12, 1));
            Console.WriteLine(xxx.imp(13, 12, 2));
            Console.WriteLine(xxx.imp_collect_calc(1, 13, 12, 1, 1));
            Console.WriteLine(xxx.imp_collect_calc(1, 13, 12, 1, 0));
            Console.WriteLine(xxx.imp_collect_calc(1, 13, 12, 0, 1));
            Console.WriteLine(xxx.imp_collect_calc(1, 13, 12, 1, -1));
        }
    }
}
