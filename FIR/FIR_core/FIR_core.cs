using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIR
{
    public class FIR_core
    {
        const int ROW_me_1 = 1;
        const int ROW_me_2 = 5;
        const int ROW_me_3 = 21;
        const int ROW_me_4 = 105;
        const int ROW_me_5 = 50000;
        //const int ROW_you_1 = 5;
        //const int ROW_you_2 = 21;
        //const int ROW_zero_in_1 = 5;
        //const int ROW_zero_out_1 = 1;

        //static int[,] totalimp = new int[20, 20];

        public int[,] board = new int[20, 20];
        int[,] boardimp = new int[20, 20];
        //int[,] boardimp1 = new int[20, 20];
        //int[,] boardimp2 = new int[20, 20];
        int num_you, num_me, num_zero_in, num_zero_out, num_zero_d_out;
        int[] ROW_me = new int[6];

        public FIR_core()
        {
            int i, j;
            for (i = 1; i <= 15; i++)
            {
                for (j = 1; j <= 15; j++)
                {
                    board[i, j] = 0;
                    boardimp[i, j] = 0;
                }
            }
            ROW_me[1] = ROW_me_1;
            ROW_me[2] = ROW_me_2;
            ROW_me[3] = ROW_me_3;
            ROW_me[4] = ROW_me_4;
            ROW_me[5] = ROW_me_5;
            initial_num();
        }

        bool on_board(int x, int y)
        ///判断点x,y是否在棋盘上
        {
            if (((x >= 1) && (x <= 15)) && ((y >= 1) && (y <= 15))) return true; else return false;
        }
        void initial_num()
        ///初始化
        {
            num_you = 0;
            num_me = 0;
            num_zero_in = 0;
            num_zero_out = 0;
            num_zero_d_out = 0;
        }
        public void imp_collect_row(int mode, int i, int j, int stepx, int stepy)
        ///针对[i,j]格点尝试向stepx,stepy方向统计各变量的值,mode说明了此步是我方还是敌方
        {
            int xxx = i, yyy = j;
            int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
            bool f;
            initial_num();
            board[xxx, yyy] = mode;

            while (true)
            {
                if (on_board(i, j))
                {
                    if (board[i, j] == mode) { num_me++; }
                    if (board[i, j] == 0) num_zero_in++;
                }
                if (((board[i, j] != mode) && (board[i, j] != 0)) || !on_board(i, j))
                {
                    f = on_board(i, j);
                    i -= stepx;
                    j -= stepy;
                    while (board[i, j] != mode)
                    {
                        i -= stepx;
                        j -= stepy;
                        num_zero_d_out++;
                        if (f)
                        {
                            num_zero_out++;
                            f = false;
                        }
                    }
                    x2 = i;
                    y2 = j;
                    break;
                }
                i += stepx;
                j += stepy;
            }

            i = xxx; j = yyy;
            num_me--;

            while (true)
            {
                if (on_board(i, j))
                {
                    if (board[i, j] == mode) { num_me++; }
                    if (board[i, j] == 0) num_zero_in++;
                }
                if (((board[i, j] != mode) && (board[i, j] != 0)) || !on_board(i, j))
                {
                    f = on_board(i, j);
                    i += stepx;
                    j += stepy;
                    while (board[i, j] != mode)
                    {
                        i += stepx;
                        j += stepy;
                        num_zero_d_out++;
                        if (f)
                        {
                            num_zero_out++;
                            f = false;
                        }
                    }
                    x1 = i;
                    y1 = j;
                    break;
                }
                i -= stepx;
                j -= stepy;
            }

            num_zero_in -= num_zero_d_out;
            //if (num_zero_in > (5 - num_me)) num_zero_in = 0;
            if ((board[x1 - stepx, y1 - stepy] != mode) && (board[x1 - stepx, y1 - stepy] != 0)) num_you++;
            if ((board[x2 + stepx, y2 + stepy] != mode) && (board[x2 + stepx, y2 + stepy] != 0)) num_you++;

            board[xxx, yyy] = 0;
        }

        public int imp_collect_calc(int mode, int x, int y, int stepx, int stepy)
        ///针对统计的变量计算[x,y]权重，仅限于某个方向
        {
            int sum = 0;
            imp_collect_row(mode, x, y, stepx, stepy);
            sum += ROW_me[num_me];
            sum -= num_you * ROW_me[num_me-1];
            sum -= num_zero_in * ROW_me[num_me-1];
            //switch (num_you)
            //{
            //    case 1:
            //        sum -= ROW_you_1;
            //        break;
            //    case 2:
            //        sum -= ROW_you_2;
            //        break;
            //}
            //switch (num_zero_in)
            //{
            //    case 1:
            //        sum -= 1 * ROW_me[num_me];
            //        break;
            //    case 2:
            //        sum -= 2 * ROW_me[num_me];
            //        break;
            //}
            //switch (num_zero_out)
            //{
            //    case 1:
            //        sum += 1 * ROW_zero_out_1;
            //        break;
            //    case 2:
            //        sum += 2 * ROW_zero_out_1;
            //        break;
            //}
            if ((num_me == 5) && (num_zero_in == 0)) sum += ROW_me_5;
            return sum;
        }

        public int imp(int x, int y, int mode)
        ///计算总量权重
        {
            int importance = 0;
            importance += imp_collect_calc(mode, x, y, 1, 1);
            importance += imp_collect_calc(mode, x, y, 1, 0);
            importance += imp_collect_calc(mode, x, y, 0, 1);
            importance += imp_collect_calc(mode, x, y, 1, -1);
            return importance;
        }

        void boardimp_form()
        ///对棋盘上所有格子生成权重
        {
            int i, j;
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                    if (board[i, j] == 0) boardimp[i, j] = imp(i, j, 1) + imp(i, j, 2); else boardimp[i, j] = -9999 - board[i, j];
        }

        public int FindTarget()
        ///寻找下一个着棋位置
        {
            int i, j;
            int max;
            int[] a = new int[1000];
            int aa = 0;
            max = int.MinValue;
            boardimp_form();

            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                    if (boardimp[i, j] > max) max = boardimp[i, j];

            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                    if (boardimp[i, j] == max) a[++aa] = i * 100 + j;
            Random ran = new Random();

            max = ran.Next(1, aa);

            //test
            print_boardimp();
            Console.Write(a[max]);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            //test

            return a[max];
        }

        //testFunction
        void print_boardimp()
        {
            int i, j;
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
                    Console.Write("{0,-7}", boardimp[i, j]);
                }
                Console.WriteLine();
            }
        }
        //testFunction


    }


}



