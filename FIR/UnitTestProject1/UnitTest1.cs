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
            FIR_core xxx = new FIR_core(7);
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                {
                    xxx.board[i, j] = 0;
                }

            xxx.board[4, 6] = 1;
            xxx.board[5, 7] = 1;
            xxx.board[6, 7] = 2;
            xxx.board[6, 8] = 1;
            xxx.board[7, 7] = 2;
            xxx.board[7, 8] = 1;
            xxx.board[7, 9] = 2;
            xxx.board[8, 7] = 2;
            xxx.board[8, 8] = 2;


            x = 8;
            y = 9;
            Console.WriteLine(xxx.imp(x, y, 1));
            Console.WriteLine(xxx.imp(x, y, 2));
            Console.WriteLine(xxx.imp_collect_calc(1, x, y, 1, 1));
            Console.WriteLine(xxx.imp_collect_calc(1, x, y, 1, 0));
            Console.WriteLine(xxx.imp_collect_calc(1, x, y, 0, 1));
            Console.WriteLine(xxx.imp_collect_calc(1, x, y, 1, -1));
            Console.WriteLine(xxx.imp_collect_calc(2, x, y, 1, 1));
            Console.WriteLine(xxx.imp_collect_calc(2, x, y, 1, 0));
            Console.WriteLine(xxx.imp_collect_calc(2, x, y, 0, 1));
            Console.WriteLine(xxx.imp_collect_calc(2, x, y, 1, -1));

            Console.WriteLine();

            x = 9;
            y = 7;
            Console.WriteLine(xxx.imp(x, y, 1));
            Console.WriteLine(xxx.imp(x, y, 2));
            Console.WriteLine(xxx.imp_collect_calc(1, x, y, 1, 1));
            Console.WriteLine(xxx.imp_collect_calc(1, x, y, 1, 0));
            Console.WriteLine(xxx.imp_collect_calc(1, x, y, 0, 1));
            Console.WriteLine(xxx.imp_collect_calc(1, x, y, 1, -1));
            Console.WriteLine(xxx.imp_collect_calc(2, x, y, 1, 1));
            Console.WriteLine(xxx.imp_collect_calc(2, x, y, 1, 0));
            Console.WriteLine(xxx.imp_collect_calc(2, x, y, 0, 1));
            Console.WriteLine(xxx.imp_collect_calc(2, x, y, 1, -1));
        }

        [TestMethod]
        public void TestMethod2()
        {
            FIR_core_V2 xxx = new FIR_core_V2();
            int x, y;
            int[,] a = new int[8, 8];
            int i, j;
            for (i = 1; i <= 7; i++)
                for (j = 1; j <= 7; j++)
                    a[i, j] = 0;
            a[1, 1] = 1;a[1, 2] = 1;a[1, 3] = 1;a[1, 4] = 1;a[1, 5] = 1;
            Console.WriteLine(xxx.GetLineShape(a, 1, 2, 1));



        }
    }
}
