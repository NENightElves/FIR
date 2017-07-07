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
            xxx.board[7, 6] = 1;
            xxx.board[6, 7] = 1;
            xxx.board[7, 8] = 1;
            xxx.board[8, 8] = 1;
            xxx.board[9, 10] = 1;
            xxx.board[10, 7] = 1;

            xxx.board[9, 7] = 2;
            xxx.board[8, 7] = 2;
            xxx.board[7, 7] = 2;
            xxx.board[9, 8] = 2;
            xxx.board[9, 9] = 2;
            xxx.board[8, 9] = 2;
            xxx.board[10, 9] = 2;


            x = 9;
            y = 4;
            //Console.WriteLine(xxx.imp(x, y, 1));
            //Console.WriteLine(xxx.imp(x, y, 2));
            //Console.WriteLine(xxx.imp_collect_calc(1, x, y, 1, 1));
            //Console.WriteLine(xxx.imp_collect_calc(1, x, y, 1, 0));
            //Console.WriteLine(xxx.imp_collect_calc(1, x, y, 0, 1));
            //Console.WriteLine(xxx.imp_collect_calc(1, x, y, 1, -1));
            //Console.WriteLine(xxx.imp_collect_calc(2, x, y, 1, 1));
            //Console.WriteLine(xxx.imp_collect_calc(2, x, y, 1, 0));
            Console.WriteLine(xxx.imp_collect_calc(2, x, y, 0, 1));
            Console.WriteLine(xxx.imp_collect_calc(2, x, y, 1, -1));
        }
    }
}
