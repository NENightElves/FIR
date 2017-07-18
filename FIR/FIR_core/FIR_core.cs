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
        const int ROW_me_4 = 85;
        const int ROW_me_5 = 50000;
        const int stack_goal = 7;
        //const int ROW_you_1 = 5;
        //const int ROW_you_2 = 21;
        //const int ROW_zero_in_1 = 5;
        //const int ROW_zero_out_1 = 1;

        public struct BoardImpSort
        {
            public int num;
            public int X;
            public int Y;
        }

        static int[,] totalimp = new int[20, 20];
        static int stack_num;

        public int[,] board = new int[20, 20];
        int[,] boardimp = new int[20, 20];
        BoardImpSort[] boardimpsort = new BoardImpSort[300];
        //int[,] boardimp1 = new int[20, 20];
        //int[,] boardimp2 = new int[20, 20];
        int num_you, num_me, num_zero_in, num_zero_out, num_zero_d_out;
        int[] ROW_me = new int[200];
        int stepX, stepY;

        private int GetIndex(int n)
        {
            return n + 50;
        }
        public FIR_core(int n)
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
            ROW_me[GetIndex(1)] = ROW_me_1;
            for (i = 2; i <= 10; i++) ROW_me[GetIndex(i)] = ROW_me[GetIndex(i - 1)] * 4 + 1;
            ROW_me[GetIndex(0)] = 0;
            for (i = 1; i <= 10; i++) ROW_me[GetIndex(-i)] = -ROW_me[GetIndex(i)];
            stack_num = n;
            stepX = 0;
            stepY = 0;
            initial_num();
        }

        bool on_board(int x, int y)
        ///判断点x,y是否在棋盘上
        {
            if (((x >= 1) && (x <= 15)) && ((y >= 1) && (y <= 15))) return true; else return false;
        }

        bool IsComputer(int x)
        {
            if (x % 2 == 0) return false; else return true;
        }

        public void setstep(int x, int y)
        {
            stepX = x;
            stepY = y;
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
            //if (num_zero_in > 3) num_zero_in = 0;
            if ((board[x1 - stepx, y1 - stepy] != mode) && (board[x1 - stepx, y1 - stepy] != 0)) num_you++;
            if ((board[x2 + stepx, y2 + stepy] != mode) && (board[x2 + stepx, y2 + stepy] != 0)) num_you++;

            board[xxx, yyy] = 0;
        }

        public int imp_collect_calc(int mode, int x, int y, int stepx, int stepy)
        ///针对统计的变量计算[x,y]权重，仅限于某个方向
        {
            int sum = 0;
            imp_collect_row(mode, x, y, stepx, stepy);
            sum += ROW_me[GetIndex(num_me - num_zero_in)];
            sum -= num_you * ROW_me[GetIndex(num_me - 1)] * 2;
            if ((num_me == 5) && (num_zero_in == 0))
            {
                if (mode == 1) sum += ROW_me_5 * 2; else sum += ROW_me_5;
            }
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
            int i, j, k;
            int stack_num_tmp;

            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                    if (board[i, j] == 0) boardimp[i, j] = imp(i, j, 1) + imp(i, j, 2); else boardimp[i, j] = -9999 - board[i, j];
            boardimp_sort();

            if (stack_num > 0)
            {
                if (stack_num == stack_goal)
                {
                    for (i = 1; i <= 15; i++)
                        for (j = 1; j <= 15; j++)
                            totalimp[i, j] = boardimp[i, j];
                }
                else
                {
                    if (stepX != 0)
                    {
                        if (IsComputer(stack_num))
                        {
                            totalimp[stepX, stepY] += boardimpsort[1].num;
                        }
                        else
                        {
                            totalimp[boardimpsort[1].X, boardimpsort[1].Y] += boardimpsort[1].num;
                        }
                    }
                    stack_num_tmp = stack_num - 1;
                    FIR_core FIR_core_stack = new FIR_core(stack_num_tmp);
                    for (k = 1; k <= 5; k++)
                    {
                        for (i = 1; i <= 15; i++)
                            for (j = 1; j <= 15; j++)
                                FIR_core_stack.board[i, j] = board[i, j];
                        setstep(boardimpsort[k].X, boardimpsort[k].Y);
                        if (IsComputer(stack_num))
                            FIR_core_stack.board[boardimpsort[k].X, boardimpsort[k].Y] = 1;
                        else
                            FIR_core_stack.board[boardimpsort[k].X, boardimpsort[k].Y] = 2;
                        FIR_core_stack.boardimp_form();
                    }
                }
            }
            else
            {
                return;
            }

        }

        void boardimp_sort()
        {
            int i, j, n;
            BoardImpSort tmp;
            n = 0;
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                {
                    n++;
                    boardimpsort[n].num = board[i, j];
                    boardimpsort[n].X = i;
                    boardimpsort[n].Y = j;
                }
            for (i = 1; i <= 224; i++)
                for (j = i + 1; j <= 225; j++)
                {
                    if (boardimpsort[i].num < boardimpsort[j].num)
                    {
                        tmp = boardimpsort[i];
                        boardimpsort[i] = boardimpsort[j];
                        boardimpsort[j] = tmp;
                    }
                }
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
                    if (totalimp[i, j] > max) max = totalimp[i, j];

            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                    if (totalimp[i, j] == max) a[++aa] = i * 100 + j;
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


    interface FIR_core_V2_Func
    {
        int GetAllShape(int[][] board, int X, int Y);
        int GetLineShape(int[][] board, int X, int Y, int StepX, int StepY);
        int GetLineStart(ref int X, ref int Y);
        int GetLineEnd(ref int X, ref int Y);
        string GenerateLine(int[][] board, int StartX, int StartY,int EndX,int EndY,int StepX,int StepY);
        int PositionOnLine(int StartX, int StartY, int EndX, int EndY, int X, int Y);

    }

    public class FIR_core_V2 : FIR_core_V2_Func
    {
        #region const
        const int Five = 1;
        const int LiveFour = 2;
        const int RushFour = 3;
        const int LiveThree = 4;
        const int SleepThree = 5;
        const int LiveTwo = 6;
        const int SleepTwo = 7;
        const int DieFour = 8;
        const int DieThree = 9;
        const int DieTwo = 10;

        const int ShapeFive = 101;
        const int ShapeLiveFour = 102;
        const int ShapeDoubleRushFour = 103;
        const int ShapeRushFourLiveThree = 104;
        const int ShapeDoubleLiveThree = 105;
        const int ShapeLiveThreeSleepThree = 106;
        const int ShapeSleepFour = 107;
        const int ShapeLiveThree = 108;
        const int ShapeDoubleLiveTwo = 109;
        const int ShapeSleepThree = 110;
        const int ShapeLiveTwoSleepTwo = 111;
        const int ShapeLiveTwo = 112;
        const int ShapeSleepTwo = 113;
        const int ShapeDieFour = 114;
        const int ShapeDieThree = 115;
        const int ShapeDieTwo = 116;

        const int ScoreFive = 100000;
        const int ScoreLiveFour = 10000;
        const int ScoreDoubleRushFour = 10000;
        const int ScoreRushFourLiveThree = 10000;
        const int ScoreDoubleLiveThree = 5000;
        const int ScoreLiveThreeSleepThree = 1000;
        const int ScoreSleepFour = 500;
        const int ScoreLiveThree = 200;
        const int ScoreDoubleLiveTwo = 100;
        const int ScoreSleepThree = 50;
        const int ScoreLiveTwoSleepTwo = 10;
        const int ScoreLiveTwo = 5;
        const int ScoreSleepTwo = 3;
        const int ScoreDieFour = -5;
        const int ScoreDieThree = -5;
        const int ScoreDieTwo = -5;
        #endregion


    }


}



