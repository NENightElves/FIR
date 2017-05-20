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
        const int ROW_me_2 = 2;
        const int ROW_me_3 = 5;
        const int ROW_me_4 = 10;
        const int ROW_me_5 = 50000;
        const int ROW_you_1 = 5;
        const int ROW_you_2 = 10;
        const int ROW_zero_in_1 = 2;
        const int ROW_zero_out_1 = 3;
        
        public int[,] board = new int[20, 20];
        int[,] boardimp = new int[20, 20];
        int[,] boardimp1 = new int[20, 20];
        int[,] boardimp2 = new int[20, 20];
        int importance;
        int num_you, num_me, num_zero_in, num_zero_out;

        bool on_board(int x, int y)
        {
            if (((x >= 1) && (x <= 15)) && ((y >= 1) && (y <= 15))) return true; else return false;
        }
        void initial_num()
        {
            num_you = 0;
            num_me = 0;
            num_zero_in = 0;
            num_zero_out = 0;
        }
        void imp_collect_row(int mode, int i, int j, int stepx, int stepy)
        {
            int xxx = i, yyy = j;
            int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
            initial_num();
            board[i, j] = mode;

            while (true)
            {
                if (on_board(i,j))
                {
                    if (board[i, j] == mode) { x2 = i; y2 = j; }
                    if (board[i, j] == 0) num_zero_in++;
                }
                if (((board[i, j] != mode) && (board[i, j] != 0)) || !on_board(i, j))
                {
                    while (board[x2, y2] != mode)
                    {
                        x2 -= stepx;
                        y2 -= stepy;
                        num_zero_out++;
                    }
                    break;
                }
                i += stepx;
                j += stepy;
            }

            i = xxx;j = yyy;

            while (true)
            {
                if (on_board(i, j))
                {
                    if (board[i, j] == mode) { x1 = i; y1 = j; }
                    if (board[i, j] == 0) num_zero_in++;
                }
                if (((board[i, j] != mode) && (board[i, j] != 0)) || !on_board(i, j))
                {
                    while (board[x1,y1] != mode)
                    {
                        x1 += stepx;
                        y1 += stepy;
                        num_zero_out++;
                    }
                    break;
                }
                i -= stepx;
                j -= stepy;
            }

            num_zero_in -= num_zero_out;
            num_me--;
            if ((board[x1 - stepx, y1 - stepy] != mode) && (board[x1 - stepx, y1 - stepy] != 0)) num_you++;
            if ((board[x2 + stepx, y2 + stepy] != mode) && (board[x2 + stepx, y2 + stepy] != 0)) num_you++;

            board[i, j] = 0;
        }

        int imp_collect_calc(int mode, int x, int y, int stepx, int stepy)
        {
            int sum = 0;
            imp_collect_row(mode, x, y, stepx, stepy);
            switch (num_me)
            {
                case 1:
                    sum += ROW_me_1;
                    break;
                case 2:
                    sum += ROW_me_2;
                    break;
                case 3:
                    sum += ROW_me_3;
                    break;
                case 4:
                    sum += ROW_me_4;
                    break;
                case 5:
                    sum += ROW_me_5;
                    break;
            }
            switch (num_you)
            {
                case 1:
                    sum -= ROW_you_1;
                    break;
                case 2:
                    sum -= ROW_you_2;
                    break;
            }
            switch (num_zero_in)
            {
                case 1:
                    sum -= 1 * ROW_zero_in_1;
                    break;
                case 2:
                    sum -= 2 * ROW_zero_in_1;
                    break;
            }
            switch (num_zero_out)
            {
                case 1:
                    sum += 1 * ROW_zero_out_1;
                    break;
                case 2:
                    sum += 2 * ROW_zero_out_1;
                    break;
            }
            return sum;
        }

        int imp(int x, int y, int mode)
        {
            importance = 0;
            importance += imp_collect_calc(mode, x, y, 1, 1);
            importance += imp_collect_calc(mode, x, y, 1, 0);
            importance += imp_collect_calc(mode, x, y, 0, 1);
            importance += imp_collect_calc(mode, x, y, 1, -1);
            return importance;
        }

        void boardimp_form()
        {
            int i, j;
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                    if (board[i, j] == 0) boardimp[i, j] = imp(i, j, 1) - imp(i, j, 2); else boardimp[i, j] = int.MinValue;
        }

        public int FindTarget()
        {
            int i, j;
            int max;
            int[] a = new int[1000];
            int aa=0;
            max = int.MinValue;
            boardimp_form();
            
            //test
            print_boardimp();
            //test

            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                    if (boardimp[i, j] > max) max = boardimp[i, j];

            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                    if (boardimp[i, j] == max) a[++aa]=i*100+j;
            Random ran = new Random();
            return a[ran.Next(1,aa)];
        }

        //testFunction
        void print_boardimp()
        {
            int i, j;
            for (i = 1; i <= 15; i++)
            {
                for (j = 1; j <= 15; j++)
                {
                    Console.Write(boardimp[i, j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
        //testFunction


    }
}
