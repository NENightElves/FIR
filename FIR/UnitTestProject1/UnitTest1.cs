﻿using System;
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
            int[,] a = new int[16, 16];
            int[,] imp = new int[16, 16];
            int i, j;
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                    a[i, j] = 0;
            a[2, 2] = 1; a[3, 3] = 1; a[4, 4] = 1; a[5, 5] = 1; a[6, 6] = 0;
            a[2, 3] = 1; a[2, 4] = 1; a[2, 5] = 2;

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
    }
}
