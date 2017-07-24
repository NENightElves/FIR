using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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


    interface FIR_core_V2_Func_List
    {
        int GetAllShape(int[,] board, int X, int Y);
        int GetLineShape(int[,] board, int X, int Y, int Direction);
        void GetLineStart(ref int X, ref int Y, int Direction);
        void GetLineEnd(ref int X, ref int Y, int Direction);
        string GenerateLine(int[,] board, int StartX, int StartY, int EndX, int EndY);
        int FindShapeOnLine(string line, string shape, int n);
        int PositionOnLine(int StartX, int StartY, int EndX, int EndY, int X, int Y);
        bool IsPositionOnLine(int Start, int End, int Position);
        bool IsOnBoard(int X, int Y, int size);
    }

    public class FIR_core_V2
    {
        #region const
        const int SIZE = 15;

        const int NoShapeBase = 0;
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

        const int NoShape = 0;
        const int ShapeFive = 1;
        const int ShapeLiveFour = 2;
        const int ShapeDoubleRushFour = 3;
        const int ShapeRushFourLiveThree = 4;
        const int ShapeDoubleLiveThree = 5;
        const int ShapeLiveThreeSleepThree = 6;
        const int ShapeRushFour = 7;
        const int ShapeLiveThree = 8;
        const int ShapeDoubleLiveTwo = 9;
        const int ShapeSleepThree = 10;
        const int ShapeLiveTwoSleepTwo = 11;
        const int ShapeLiveTwo = 12;
        const int ShapeSleepTwo = 13;
        const int ShapeDieFour = 14;
        const int ShapeDieThree = 15;
        const int ShapeDieTwo = 16;

        const int NoScore = 0;
        const int ScoreFive = 100000;
        const int ScoreLiveFour = 10000;
        const int ScoreDoubleRushFour = 10000;
        const int ScoreRushFourLiveThree = 10000;
        const int ScoreDoubleLiveThree = 10000;
        const int ScoreLiveThreeSleepThree = 1000;
        const int ScoreRushFour = 500;
        const int ScoreLiveThree = 200;
        const int ScoreDoubleLiveTwo = 100;
        const int ScoreSleepThree = 50;
        const int ScoreLiveTwoSleepTwo = 10;
        const int ScoreLiveTwo = 5;
        const int ScoreSleepTwo = 3;
        const int ScoreDieFour = -5;
        const int ScoreDieThree = -5;
        const int ScoreDieTwo = -5;
        const int ScoreMin = -10000000;

        const int DirectionLine0 = 1;
        const int DirectionLine45 = 2;
        const int DirectionLine90 = 3;
        const int DirectionLine135 = 4;

        const int AlphaBetaMax = 10000;
        const int AlpahBetaMin = -10000;

        string[,] ShapeBase = new string[20, 20];
        int[] ScoreShape = new int[20];
        int[] ShapeBaseLength = new int[20];
        int[,] imp_board = new int[SIZE + 1, SIZE + 1];
        int OUT_WIDTH, WIDTH, DEPTH, NUM_THREAD;
        bool[] THREAD_FLAG;
        Task[] MYTHREAD;
        #endregion

        public struct StructSortBoard
        {
            public int num;
            public int X;
            public int Y;
        }
        public struct StructAlphaBetaSearch
        {
            public int imp;
            public int X;
            public int Y;
            public int max_depth;
        }
        public struct StructThreadAlphaBetaSearch
        {
            public int[,] board;
            public int max;
            public int min;
            public int depth_count;
            public int index;
        }

        public FIR_core_V2(int out_width, int width, int depth, int num_thread)
        {
            //depth是偶数
            int i, j, k;
            #region const
            int tmp;
            string tmps;
            int half_size;
            THREAD_FLAG = new bool[num_thread + 1];
            MYTHREAD = new Task[num_thread + 1];
            OUT_WIDTH = out_width;
            WIDTH = width;
            DEPTH = depth;
            NUM_THREAD = num_thread;
            for (i = 0; i <= 19; i++)
            {
                for (j = 0; j <= 19; j++)
                {
                    ShapeBase[i, j] = "";
                }
                ScoreShape[i] = 0;
                ShapeBaseLength[i] = 0;
            }
            ShapeBaseLength[Five] = 1;
            ShapeBaseLength[LiveFour] = 1;
            ShapeBaseLength[RushFour] = 3;
            ShapeBaseLength[LiveThree] = 2;
            ShapeBaseLength[SleepThree] = 6;
            ShapeBaseLength[LiveTwo] = 3;
            ShapeBaseLength[SleepTwo] = 6;
            ShapeBaseLength[DieFour] = 1;
            ShapeBaseLength[DieThree] = 1;
            ShapeBaseLength[DieTwo] = 1;

            ShapeBase[Five, 1] = "11111";
            ShapeBase[LiveFour, 1] = "011110";
            ShapeBase[RushFour, 1] = "011112";
            ShapeBase[RushFour, 2] = "0101110";
            ShapeBase[RushFour, 3] = "0110110";
            ShapeBase[LiveThree, 1] = "01110";
            ShapeBase[LiveThree, 2] = "010110";
            ShapeBase[SleepThree, 1] = "001112";
            ShapeBase[SleepThree, 2] = "010112";
            ShapeBase[SleepThree, 3] = "011012";
            ShapeBase[SleepThree, 4] = "10011";
            ShapeBase[SleepThree, 5] = "10101";
            ShapeBase[SleepThree, 6] = "2011102";
            ShapeBase[LiveTwo, 1] = "00110";
            ShapeBase[LiveTwo, 2] = "01010";
            ShapeBase[LiveTwo, 3] = "010010";
            ShapeBase[SleepTwo, 1] = "000112";
            ShapeBase[SleepTwo, 2] = "001012";
            ShapeBase[SleepTwo, 3] = "010012";
            ShapeBase[SleepTwo, 4] = "10001";
            ShapeBase[SleepTwo, 5] = "2010102";
            ShapeBase[SleepTwo, 6] = "2011002";
            ShapeBase[DieFour, 1] = "211112";
            ShapeBase[DieThree, 1] = "21112";
            ShapeBase[DieTwo, 1] = "2112";

            for (i = 1; i <= 10; i++)
            {
                tmp = ShapeBaseLength[i];
                for (j = 1; j <= tmp; j++)
                {
                    tmps = "";
                    for (k = ShapeBase[i, j].Length - 1; k >= 0; k--)
                        tmps += ShapeBase[i, j][k];
                    if (tmps != ShapeBase[i, j]) { ShapeBaseLength[i]++; ShapeBase[i, ShapeBaseLength[i]] = tmps; }
                }
            }

            ScoreShape[ShapeFive] = 100000;
            ScoreShape[ShapeLiveFour] = 10000;
            ScoreShape[ShapeDoubleRushFour] = 10000;
            ScoreShape[ShapeRushFourLiveThree] = 10000;
            ScoreShape[ShapeDoubleLiveThree] = 5000;
            ScoreShape[ShapeLiveThreeSleepThree] = 1000;
            ScoreShape[ShapeRushFour] = 500;
            ScoreShape[ShapeLiveThree] = 200;
            ScoreShape[ShapeDoubleLiveTwo] = 100;
            ScoreShape[ShapeSleepThree] = 50;
            ScoreShape[ShapeLiveTwoSleepTwo] = 10;
            ScoreShape[ShapeLiveTwo] = 5;
            ScoreShape[ShapeSleepTwo] = 3;
            ScoreShape[ShapeDieFour] = -5;
            ScoreShape[ShapeDieThree] = -5;
            ScoreShape[ShapeDieTwo] = -5;

            half_size = (SIZE % 2 == 1) ? (SIZE + 1) / 2 : SIZE / 2;
            for (i = 1; i <= half_size; i++)
                for (j = 1; j <= half_size; j++)
                    imp_board[i, j] = ((i < j) ? i : j) - 1;
            for (i = 1; i <= half_size; i++)
                for (j = half_size + 1; j <= SIZE; j++)
                    imp_board[i, j] = imp_board[i, SIZE + 1 - j];
            for (i = half_size + 1; i <= SIZE; i++)
                for (j = 1; j <= SIZE; j++)
                    imp_board[i, j] = imp_board[SIZE + 1 - i, j];
            #endregion
        }

        public static StructAlphaBetaSearch[] sort_imp_alpha_beta_search = new StructAlphaBetaSearch[100];
        public static int[,] ChangeBoard(int[,] board)
        {
            int i, j;
            int[,] tmp_board = new int[SIZE + 1, SIZE + 1];
            CopyArray(board, tmp_board);
            for (i = 1; i <= SIZE; i++)
                for (j = 1; j <= SIZE; j++)
                    if (tmp_board[i, j] == 1) tmp_board[i, j] = 2; else if (tmp_board[i, j] == 2) tmp_board[i, j] = 1;
            return tmp_board;
        }
        public static bool IsOnBoard(int X, int Y, int size)
        {
            if ((X >= 1) && (X <= size) && (Y >= 1) && (Y <= size)) return true; else return false;
        }
        public static void CopyArray(int[,] a, int[,] b)
        {
            int i, j;
            for (i = 1; i <= SIZE; i++)
                for (j = 1; j <= SIZE; j++)
                    b[i, j] = a[i, j];
        }
        public static void CopyArray(StructSortBoard[] a, StructSortBoard[] b)
        {
            int i;
            for (i = 1; i <= SIZE * SIZE; i++)
                b[i] = a[i];
        }

        #region ShapeJudgment
        bool IsShapeFive(int[] a)
        {
            int i, s = 0;
            for (i = 1; i <= 4; i++)
                if (a[i] == Five) s++;
            if (s == 1) return true; else return false;
        }
        bool IsShapeLiveFour(int[] a)
        {
            int i, s = 0;
            for (i = 1; i <= 4; i++)
                if (a[i] == LiveFour) s++;
            if (s == 1) return true; else return false;
        }
        bool IsShapeDoubleRushFour(int[] a)
        {
            int i, s = 0;
            for (i = 1; i <= 4; i++)
                if (a[i] == RushFour) s++;
            if (s == 2) return true; else return false;
        }
        bool IsShapeRushFourLiveThree(int[] a)
        {
            int i;
            bool x = false, y = false;
            for (i = 1; i <= 4; i++)
                if (a[i] == RushFour) x = true; else if (a[i] == LiveThree) y = true;
            if (x && y) return true; else return false;
        }
        bool IsShapeDoubleLiveThree(int[] a)
        {
            int i, s = 0;
            for (i = 1; i <= 4; i++)
                if (a[i] == LiveThree) s++;
            if (s == 2) return true; else return false;
        }
        bool IsShapeLiveThreeSleepThree(int[] a)
        {
            int i;
            bool x = false, y = false;
            for (i = 1; i <= 4; i++)
                if (a[i] == LiveThree) x = true; else if (a[i] == SleepThree) y = true;
            if (x && y) return true; else return false;
        }
        bool IsShapeRushFour(int[] a)
        {
            int i, s = 0;
            for (i = 1; i <= 4; i++)
                if (a[i] == RushFour) s++;
            if (s == 1) return true; else return false;
        }
        bool IsShapeLiveThree(int[] a)
        {
            int i, s = 0;
            for (i = 1; i <= 4; i++)
                if (a[i] == LiveThree) s++;
            if (s == 1) return true; else return false;
        }
        bool IsShapeDoubleLiveTwo(int[] a)
        {
            int i, s = 0;
            for (i = 1; i <= 4; i++)
                if (a[i] == LiveTwo) s++;
            if (s == 2) return true; else return false;
        }
        bool IsShapeSleepThree(int[] a)
        {
            int i, s = 0;
            for (i = 1; i <= 4; i++)
                if (a[i] == SleepThree) s++;
            if (s == 1) return true; else return false;
        }
        bool IsShapeLiveTwoSleepTwo(int[] a)
        {
            int i;
            bool x = false, y = false;
            for (i = 1; i <= 4; i++)
                if (a[i] == LiveTwo) x = true; else if (a[i] == SleepTwo) y = true;
            if (x && y) return true; else return false;
        }
        bool IsShapeLiveTwo(int[] a)
        {
            int i, s = 0;
            for (i = 1; i <= 4; i++)
                if (a[i] == LiveTwo) s++;
            if (s == 1) return true; else return false;
        }
        bool IsShapeSleepTwo(int[] a)
        {
            int i, s = 0;
            for (i = 1; i <= 4; i++)
                if (a[i] == SleepTwo) s++;
            if (s == 1) return true; else return false;
        }
        bool IsShapeDieFour(int[] a)
        {
            int i, s = 0;
            for (i = 1; i <= 4; i++)
                if (a[i] == DieFour) s++;
            if (s == 1) return true; else return false;
        }
        bool IsShapeDieThree(int[] a)
        {
            int i, s = 0;
            for (i = 1; i <= 4; i++)
                if (a[i] == DieThree) s++;
            if (s == 1) return true; else return false;
        }
        bool IsShapeDieTwo(int[] a)
        {
            int i, s = 0;
            for (i = 1; i <= 4; i++)
                if (a[i] == DieTwo) s++;
            if (s == 1) return true; else return false;
        }
        #endregion

        bool IsNabourhooded(int[,] board, int X, int Y)
        {
            ///应保证board[X,Y]为0
            int i, j;
            for (i = -1; i <= 1; i++)
                for (j = -1; j <= 1; j++)
                {
                    if (IsOnBoard(X + i, Y + i, SIZE))
                        if (board[X + i, Y + j] != 0) return true;
                }
            return false;
        }
        void GetLineStart(ref int X, ref int Y, int Direction)
        {
            switch (Direction)
            {
                case DirectionLine0:
                    Y = 1;
                    break;
                case DirectionLine45:
                    if (X > SIZE - Y) { X = X + Y - SIZE; Y = SIZE; } else { Y = X + Y - 1; X = 1; }
                    break;
                case DirectionLine90:
                    X = 1;
                    break;
                case DirectionLine135:
                    if (X > Y) { X = X - Y + 1; Y = 1; } else { Y = Y - X + 1; X = 1; }
                    break;
            }
        }
        void GetLineEnd(ref int X, ref int Y, int Direction)
        {
            switch (Direction)
            {
                case DirectionLine0:
                    Y = SIZE;
                    break;
                case DirectionLine45:
                    if (SIZE - X > Y) { X = X + Y - 1; Y = 1; } else { Y = X + Y - SIZE; X = SIZE; }
                    break;
                case DirectionLine90:
                    X = SIZE;
                    break;
                case DirectionLine135:
                    if (X < Y) { X = X + SIZE - Y; Y = SIZE; } else { Y = Y + SIZE - X; X = SIZE; }
                    break;
            }

        }
        string GenerateLine(int[,] board, int StartX, int StartY, int EndX, int EndY)
        {
            int StepX, StepY;
            int i, j;
            string s;
            s = "";
            if (Math.Abs(EndX - StartX) != (Math.Abs(EndX - StartX))) return s;
            StepX = (EndX - StartX > 0) ? 1 : (EndX - StartX < 0) ? -1 : 0;
            StepY = (EndY - StartY > 0) ? 1 : (EndY - StartY < 0) ? -1 : 0;
            EndX += StepX;
            EndY += StepY;
            for (i = StartX, j = StartY; (i != EndX || j != EndY); i += StepX, j += StepY)
            {
                s += board[i, j];
            }
            return s;
        }
        int PositionOnLine(int StartX, int StartY, int EndX, int EndY, int X, int Y)
        {
            int result;
            if (EndX - StartX == 0) return Y;
            if (EndY - StartY == 0) return X;
            result = (EndX - StartX > 0) ? 1 : -1;
            result = (X - StartX) / result + 1;
            return result;
        }
        int FindShapeOnLine(string line, string shape, int n)
        {
            int i;
            string tmp;
            for (i = n; i <= line.Length - shape.Length + 1; i++)
            {
                tmp = line.Substring(i - 1, shape.Length);
                if (tmp == shape) return i;
            }
            return 0;
        }
        bool IsPositionOnLine(int Start, int End, int Position)
        {
            if ((Position >= Start) && (Position <= End)) return true; else return false;
        }
        int GetLineShape(int[,] board, int X, int Y, int Direction)
        {
            int start_x, start_y, end_x, end_y;
            int[,] tmp_board = new int[SIZE + 1, SIZE + 1];
            int position, position_l, position_r;
            string s;
            int i, j;

            start_x = X;
            start_y = Y;
            end_x = X;
            end_y = Y;

            CopyArray(board, tmp_board);
            GetLineStart(ref start_x, ref start_y, Direction);
            GetLineEnd(ref end_x, ref end_y, Direction);
            s = GenerateLine(tmp_board, start_x, start_y, end_x, end_y);
            position = PositionOnLine(start_x, start_y, end_x, end_y, X, Y);
            position_l = 0;
            for (i = 1; i <= 10; i++)
            {
                for (j = 1; j <= ShapeBaseLength[i]; j++)
                {
                    do
                    {
                        position_l++;
                        position_r = ShapeBase[i, j].Length;
                        position_l = FindShapeOnLine(s, ShapeBase[i, j], position_l);
                        position_r = position_r + position_l - 1;
                    } while (!(IsPositionOnLine(position_l, position_r, position) || (position_l == 0)));

                    if (position_l != 0)
                    {
                        return i;
                    }
                }
            }
            return 0;
        }
        public int GetAllShape(int[,] board, int X, int Y)
        {
            int[,] tmp_board = new int[SIZE + 1, SIZE + 1];
            int[] shape = new int[5];
            CopyArray(board, tmp_board);
            shape[1] = GetLineShape(tmp_board, X, Y, DirectionLine0);
            shape[2] = GetLineShape(tmp_board, X, Y, DirectionLine45);
            shape[3] = GetLineShape(tmp_board, X, Y, DirectionLine90);
            shape[4] = GetLineShape(tmp_board, X, Y, DirectionLine135);
            if (IsShapeFive(shape)) return ShapeFive;
            if (IsShapeLiveFour(shape)) return ShapeLiveFour;
            if (IsShapeDoubleRushFour(shape)) return ShapeDoubleRushFour;
            if (IsShapeRushFourLiveThree(shape)) return ShapeRushFourLiveThree;
            if (IsShapeDoubleLiveThree(shape)) return ShapeDoubleLiveThree;
            if (IsShapeLiveThreeSleepThree(shape)) return ShapeLiveThreeSleepThree;
            if (IsShapeRushFour(shape)) return ShapeRushFour;
            if (IsShapeLiveThree(shape)) return ShapeLiveThree;
            if (IsShapeDoubleLiveTwo(shape)) return ShapeDoubleLiveTwo;
            if (IsShapeSleepThree(shape)) return ShapeSleepThree;
            if (IsShapeLiveTwoSleepTwo(shape)) return ShapeLiveTwoSleepTwo;
            if (IsShapeLiveTwo(shape)) return ShapeLiveTwo;
            if (IsShapeSleepTwo(shape)) return ShapeSleepTwo;
            if (IsShapeDieFour(shape)) return ShapeDieFour;
            if (IsShapeDieThree(shape)) return ShapeDieThree;
            if (IsShapeDieTwo(shape)) return ShapeDieTwo;
            return NoShape;
        }
        public int GetImpX(int[,] board, int X, int Y)
        {
            int[,] tmp_board = new int[SIZE + 1, SIZE + 1];
            if (board[X, Y] == 0)
            {
                CopyArray(board, tmp_board);
                tmp_board[X, Y] = 1;
                return ScoreShape[GetAllShape(tmp_board, X, Y)];
            }
            else
            {
                return ScoreMin;
            }
        }
        public int GetImpY(int[,] board, int X, int Y)
        {
            int[,] tmp_board = new int[SIZE + 1, SIZE + 1];
            if (board[X, Y] == 0)
            {
                tmp_board = ChangeBoard(board);
                tmp_board[X, Y] = 1;
                return ScoreShape[GetAllShape(tmp_board, X, Y)];
            }
            else
            {
                return ScoreMin;
            }
        }
        public int[,] GetAllImpX(int[,] board)
        {
            int[,] tmp_board = new int[SIZE + 1, SIZE + 1];
            int[,] result = new int[SIZE + 1, SIZE + 1];
            int i, j;
            for (i = 1; i <= SIZE; i++)
                for (j = 1; j <= SIZE; j++)
                    if (board[i, j] == 0)
                    {
                        CopyArray(board, tmp_board);
                        tmp_board[i, j] = 1;
                        result[i, j] = ScoreShape[GetAllShape(tmp_board, i, j)];
                    }
                    else
                    {
                        result[i, j] = ScoreMin;
                    }
            return result;
        }
        public int[,] GetAllImpY(int[,] board)
        {
            int[,] tmp_board = new int[SIZE + 1, SIZE + 1];
            int[,] result = new int[SIZE + 1, SIZE + 1];
            int i, j;
            for (i = 1; i <= SIZE; i++)
                for (j = 1; j <= SIZE; j++)
                    if (board[i, j] == 0)
                    {
                        tmp_board = ChangeBoard(board);
                        tmp_board[i, j] = 1;
                        result[i, j] = ScoreShape[GetAllShape(tmp_board, i, j)];
                    }
                    else
                    {
                        result[i, j] = ScoreMin;
                    }
            return result;
        }
        public int[,] GetAllImp(int[,] board)
        {
            int i, j;
            int[,] board1 = new int[SIZE + 1, SIZE + 1], board2 = new int[SIZE + 1, SIZE + 1];
            int x, y;
            int[,] imp_board = new int[SIZE + 1, SIZE + 1];
            for (i = 1; i <= SIZE; i++)
            {
                for (j = 1; j <= SIZE; j++)
                {
                    if (board[i, j] == 0)
                    {
                        CopyArray(board, board1);
                        board2 = FIR_core_V2.ChangeBoard(board);
                        board1[i, j] = 1;
                        board2[i, j] = 1;
                        x = GetAllShape(board1, i, j);
                        y = GetAllShape(board2, i, j);
                        imp_board[i, j] = ScoreShape[x] + ScoreShape[y];
                    }
                    else
                    {
                        imp_board[i, j] = ScoreMin;
                    }
                }
            }
            return imp_board;
        }
        public int[,] GetAllImpWithBoard(int[,] board)
        {
            int[,] tmp_board = new int[SIZE + 1, SIZE + 1];
            int i, j;
            tmp_board = GetAllImp(board);
            for (i = 1; i <= SIZE; i++)
                for (j = 1; j <= SIZE; j++)
                    if (tmp_board[i, j] != ScoreMin) tmp_board[i, j] += imp_board[i, j];
            return tmp_board;
        }
        public StructSortBoard[] SortBoardImp(int[,] imp_board)
        {
            int i, j, k;
            StructSortBoard[] sort_imp_board = new StructSortBoard[(SIZE + 1) * (SIZE + 1)];
            StructSortBoard tmp;
            k = 0;
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                {
                    k++;
                    sort_imp_board[k].num = imp_board[i, j];
                    sort_imp_board[k].X = i;
                    sort_imp_board[k].Y = j;
                }
            for (i = 1; i <= SIZE * SIZE - 1; i++)
                for (j = i + 1; j <= SIZE * SIZE; j++)
                    if (sort_imp_board[i].num < sort_imp_board[j].num)
                    {
                        tmp = sort_imp_board[i];
                        sort_imp_board[i] = sort_imp_board[j];
                        sort_imp_board[j] = tmp;
                    }
            return sort_imp_board;
        }
        public int ScoreOfBoardForComputer(int[,] board)
        {
            int x, y;
            int result;
            int[,] imp_board;
            int[,] board1 = new int[SIZE + 1, SIZE + 1], board2 = new int[SIZE + 1, SIZE + 1];
            StructSortBoard[] sort_imp_board;
            imp_board = GetAllImpWithBoard(board);
            sort_imp_board = SortBoardImp(imp_board);
            CopyArray(board, board1);
            board2 = FIR_core_V2.ChangeBoard(board);
            board1[sort_imp_board[1].X, sort_imp_board[1].Y] = 1;
            board2[sort_imp_board[1].X, sort_imp_board[1].Y] = 1;
            x = GetAllShape(board1, sort_imp_board[1].X, sort_imp_board[1].Y);
            y = GetAllShape(board2, sort_imp_board[1].X, sort_imp_board[1].Y);
            result = ScoreShape[x] - ScoreShape[y];
            ////test
            //Console.WriteLine($"{sort_imp_board[1].X},{sort_imp_board[1].Y}          {result}");
            ////test
            return result;
        }
        public int AlphaBetaSearch(int[,] board, int max, int min, int depth_count, int index_depth_max)
        {
            int i, j;
            int[,] imp_board = new int[SIZE + 1, SIZE + 1];
            int[,] tmp_board = new int[SIZE + 1, SIZE + 1];
            int[,] ttmp_board = new int[SIZE + 1, SIZE + 1];
            int value = 0;
            StructSortBoard[] sort_imp_board = new StructSortBoard[(SIZE + 1) * (SIZE + 1)];
            if (depth_count == DEPTH)
            {
                ////test
                //for (j = 1; j <= depth_count; j++) Console.Write("|");
                ////test

                sort_imp_alpha_beta_search[index_depth_max].max_depth = DEPTH;
                return ScoreOfBoardForComputer(board);
            }
            else
            {
                if (depth_count % 2 == 0)
                {
                    //max
                    imp_board = GetAllImpWithBoard(board);
                    sort_imp_board = SortBoardImp(imp_board);
                    for (i = 1; i <= WIDTH; i++)
                    {
                        //特殊情况直接返回
                        if (GetImpX(board, sort_imp_board[i].X, sort_imp_board[i].Y) >= ScoreShape[ShapeDoubleLiveThree])
                            { max = AlphaBetaMax; sort_imp_alpha_beta_search[index_depth_max].max_depth = depth_count; break; }
                        //if (GetImpY(board, sort_imp_board[i].X, sort_imp_board[i].Y) >= ScoreShape[ShapeLiveFour]) { max = AlpahBetaMin; depth_max = depth_count; break; }
                        //特殊情况直接返回
                        CopyArray(board, tmp_board);
                        tmp_board[sort_imp_board[i].X, sort_imp_board[i].Y] = 1;
                        value = AlphaBetaSearch(tmp_board, max, min, depth_count + 1, index_depth_max);
                        //test
                        for (j = 1; j <= depth_count; j++) Console.Write("|");
                        Console.WriteLine($"{sort_imp_board[i].X},{sort_imp_board[i].Y}|{sort_imp_board[i].num}|【{value}】");
                        //test
                        if (value > max) max = value; else break;
                    }
                    return max;
                }
                else
                {
                    //min
                    ttmp_board = ChangeBoard(board);
                    imp_board = GetAllImpWithBoard(ttmp_board);
                    sort_imp_board = SortBoardImp(imp_board);
                    for (i = 1; i <= WIDTH; i++)
                    {
                        //特殊情况直接返回
                        //if (GetImpX(ttmp_board, sort_imp_board[i].X, sort_imp_board[i].Y) >= ScoreShape[ShapeLiveFour]) { min = AlphaBetaMax; depth_max = depth_count; break; }
                        if (GetImpY(ttmp_board, sort_imp_board[i].X, sort_imp_board[i].Y) >= ScoreShape[ShapeDoubleLiveThree])
                            { min = AlpahBetaMin; sort_imp_alpha_beta_search[index_depth_max].max_depth = depth_count; break; }
                        //特殊情况直接返回
                        CopyArray(board, tmp_board);
                        tmp_board[sort_imp_board[i].X, sort_imp_board[i].Y] = 2;
                        value = AlphaBetaSearch(tmp_board, max, min, depth_count + 1, index_depth_max);
                        //test
                        for (j = 1; j <= depth_count; j++) Console.Write("|");
                        Console.WriteLine($"{sort_imp_board[i].X},{sort_imp_board[i].Y}|{sort_imp_board[i].num}|【{value}】");
                        //test
                        if (value < min) min = value; else break;
                    }
                    return min;
                }
            }
        }
        public int CallAlphaBataSearch(int[,] board)
        {
            int i, j;
            int[,] tmp_board = new int[SIZE + 1, SIZE + 1];
            int[,] imp_board;
            MYTHREAD = new Task[100];
            StructSortBoard[] sort_imp_board;
            StructThreadAlphaBetaSearch[] xxx = new StructThreadAlphaBetaSearch[OUT_WIDTH + 1];
            StructAlphaBetaSearch tmp;
            imp_board = GetAllImpWithBoard(board);
            sort_imp_board = SortBoardImp(imp_board);
            for (i = 1; i <= OUT_WIDTH; i++)
            {
                CopyArray(board, tmp_board);
                tmp_board[sort_imp_board[i].X, sort_imp_board[i].Y] = 1;                      
                sort_imp_alpha_beta_search[i].X = sort_imp_board[i].X;
                sort_imp_alpha_beta_search[i].Y = sort_imp_board[i].Y;
                //封装
                xxx[i].board = new int[SIZE + 1, SIZE + 1];
                CopyArray(tmp_board, xxx[i].board);
                xxx[i].max = int.MinValue;
                xxx[i].min = int.MaxValue;
                xxx[i].depth_count = 1;
                xxx[i].index = i;
                if (i <= NUM_THREAD)
                {
                    MYTHREAD[i] = new Task(() => { ThreadAlphaBetaSearch(xxx[i]); });
                    MYTHREAD[i].Start();
                    Thread.Sleep(500);
                }
                else
                {
                    ThreadAlphaBetaSearch(xxx[i]);
                }
            }
            for (i = 1; i <= NUM_THREAD; i++)
            {
                MYTHREAD[i].Wait();
            }
            for (i = 1; i <= OUT_WIDTH; i++)
            {
                //test
                Console.WriteLine($"{sort_imp_board[i].X},{sort_imp_board[i].Y}|{sort_imp_board[i].num}|【{sort_imp_alpha_beta_search[i].imp}】");
                //test
                //特殊情况加权重
                if (sort_imp_board[i].num >= ScoreShape[ShapeDoubleLiveThree])
                {
                    sort_imp_alpha_beta_search[i].imp += AlphaBetaMax;
                }
                if (sort_imp_board[i].num >= 5 * ScoreShape[ShapeDoubleRushFour]) sort_imp_alpha_beta_search[i].imp += ScoreShape[ShapeFive];
                //if (!IsNabourhooded(board, sort_imp_alpha_beta_search[i].X, sort_imp_alpha_beta_search[i].Y)) sort_imp_alpha_beta_search[i].imp -= 300;
                //特殊情况加权重
                //处理相同权值
                //sort_imp_alpha_beta_search[i].imp += sort_imp_board[i].num;
                //处理相同权值
            }
            for (i = 1; i <= OUT_WIDTH - 1; i++)
                for (j = i + 1; j <= OUT_WIDTH; j++)
                    if (sort_imp_alpha_beta_search[i].imp < sort_imp_alpha_beta_search[j].imp)
                    {
                        tmp = sort_imp_alpha_beta_search[i];
                        sort_imp_alpha_beta_search[i] = sort_imp_alpha_beta_search[j];
                        sort_imp_alpha_beta_search[j] = tmp;
                    }
            for (i = 1; i <= OUT_WIDTH; i++)
                if (sort_imp_alpha_beta_search[i].max_depth != DEPTH) break;
            //if (i == OUT_WIDTH + 1)
            //{
            //    //test
            //    Console.WriteLine($"###{sort_imp_board[1].X},{sort_imp_board[1].Y}|{sort_imp_board[1].num}###");
            //    //test
            //    return sort_imp_board[1].X * 100 + sort_imp_board[1].Y;
            //}
            //for (i = 1; i <= WIDTH - 1; i++)
            //    for (j = i + 1; j <= WIDTH; j++)
            //        if (sort_imp_alpha_beta_search[i].max_depth < sort_imp_alpha_beta_search[j].max_depth)
            //        {
            //            tmp = sort_imp_alpha_beta_search[i];
            //            sort_imp_alpha_beta_search[i] = sort_imp_alpha_beta_search[j];
            //            sort_imp_alpha_beta_search[j] = tmp;
            //        }
            //test
            for (i = 1; i <= OUT_WIDTH; i++)
                Console.WriteLine($"{sort_imp_alpha_beta_search[i].X},{sort_imp_alpha_beta_search[i].Y}  {sort_imp_alpha_beta_search[i].imp} " +
                    $"{sort_imp_alpha_beta_search[i].max_depth}");
            Console.WriteLine("\r\n");
            //test

            return sort_imp_alpha_beta_search[1].X * 100 + sort_imp_alpha_beta_search[1].Y;
        }
        public void ThreadAlphaBetaSearch(StructThreadAlphaBetaSearch x)
        {
            int[,] tmp_board = new int[SIZE + 1, SIZE + 1];
            int max, min, depth_count, index;
            tmp_board = x.board;
            min = x.min;
            max = x.max;
            depth_count = x.depth_count;
            index = x.index;
            sort_imp_alpha_beta_search[index].imp=AlphaBetaSearch(tmp_board, max, min, depth_count, index);
        }
    }



    public class FIR_user_V2 : FIR_core_V2
    {
        public int[,] board = new int[16, 16];
        int[,] imp_board = new int[16, 16];
        StructSortBoard[] sort = new StructSortBoard[300];

        public FIR_user_V2(int out_width, int width, int depth, int num_thread) : base(out_width, width, depth, num_thread) {; }
        public int FindTarget()
        {       
            int[,] tmp_board = new int[16, 16];
            //imp_board = GetAllImpWithBoard(board);
            //sort = SortBoardImp(imp_board);
            //print_boardimp(imp_board);
            //Console.WriteLine(sort[1].X * 100 + sort[1].Y);
            CopyArray(board, tmp_board);
            return CallAlphaBataSearch(tmp_board);
        }
        void print_boardimp(int[,] boardimp)
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
    }
}



