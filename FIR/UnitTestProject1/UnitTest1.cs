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
            FIR_core_V2 xxx = new FIR_core_V2(10,2);
            int[,] a = new int[16, 16];
            int[,] imp = new int[16, 16];
            int i, j;
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                    a[i, j] = 0;
            a[6, 8] = 1;
            a[7, 7] = 1;
            a[7, 8] = 2;
            a[8, 6] = 2;
            a[8, 7] = 1;
            a[8, 9] = 2;
            a[9, 6] = 2;
            a[9, 7] = 2;
            a[10, 6] = 2;
            a[10, 8] = 1;
            a[11, 5] = 1;
            a = FIR_core_V2.ChangeBoard(a);
            a[12, 6] = 1;
            i = xxx.GetAllShape(a, 12, 6);
            imp = xxx.GetAllImpWithBoard(a);

            for (j = 0; j <= 15; j++)
            {
                Console.Write("[{0,-2}]   ", j);
            }
            Console.WriteLine();
            for (i = 1; i <= 15; i++)
            {
                Console.Write("[{0,-2}]   ", i);
                for (j = 1; j <= 15; j++)
                {
                    Console.Write("{0,-7}", imp[i, j]);
                }
                Console.WriteLine();
            }
        }


        [TestMethod]
        public void TestMethod3()
        {
            FIR_core_V2 xxx = new FIR_core_V2(3, 6);
            int[,] a = new int[16, 16];
            int[,] imp = new int[16, 16];
            int i, j;
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                    a[i, j] = 0;
            a[6, 6] = 1;
            a[6, 7] = 2;
            a[6, 8] = 2;
            a[7, 7] = 2;
            a[7, 9] = 2;
            a[8, 6] = 1;
            a[8, 7] = 2;
            a[8, 8] = 1;
            a[9, 7] = 1;

            xxx.CallAlphaBataSearch(a);

        }
    }
}
