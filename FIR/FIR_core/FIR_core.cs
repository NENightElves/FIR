using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIR_core
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


        int[,] board = new int[20, 20];
        int[,] boardimp = new int[20, 20];
        int[,] boardimp1 = new int[20, 20];
        int[,] boardimp2 = new int[20, 20];
        int mode;
        int importance;


        int num_you, num_me, num_zero_in, num_zero_out;

        //int FindTarget()
        //void FindTarget1()
        //void FindTarget2()
        //void combine()
        //int imp();
        bool on_board(int x, int y)
        {
            if (((x >= 1) && (x <= 15)) || ((y >= 1) && (y <= 15))) return true; else return false;
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
            int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
            initial_num();

            while (true)
            {
                i += stepx;
                j += stepy;
                if (board[i, j] == mode) { x2 = i; y2 = j; }
                if (board[i, j] == 0) num_zero_in++;
                if (((board[i, j] != mode) && (board[i, j] != 0)) || on_board(i, j))
                {
                    while (board[i, j] != mode)
                    {
                        x2 -= stepx;
                        y2 -= stepy;
                        num_zero_out++;
                    }
                    break;
                }
            }

            while (true)
            {
                i -= stepx;
                j -= stepy;
                if (board[i, j] == mode) { x1 = i; y1 = j; }
                if (board[i, j] == 0) num_zero_in++;
                if (((board[i, j] != mode) && (board[i, j] != 0)) || on_board(i, j))
                {
                    while (board[i, j] != mode)
                    {
                        x1 += stepx;
                        y1 += stepy;
                        num_zero_out++;
                    }
                    break;
                }
            }

            num_zero_in -= num_zero_out;
            num_me += 1;
            if ((board[x1 - stepx, y1 - stepy] != mode) && (board[x1 - stepx, y1 - stepy] != 0)) num_you++;
            if ((board[x2 + stepx, y2 + stepy] != mode) && (board[x2 + stepx, y2 + stepy] != 0)) num_you++;
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
                    boardimp[i, j] = imp(i, j, 1) - imp(i, j, 2);
        }

        int FindTarget()
        {
            int i, j;
            int ii = 0, jj = 0;
            int max;
            max = int.MinValue;
            boardimp_form();
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                    if (boardimp[i, j] > max) { ii = i; jj = j; max = boardimp[i, j]; }
            return ii * 10 + jj;
        }
    }
}
