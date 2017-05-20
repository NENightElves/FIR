using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIR_core
{
    public class FIR_core
    {
        int[,] board = new int[16, 16];
        int[,] boardimp = new int[16, 16];
        int mode;

        int num_you, num_me, num_zero_in, num_zero_out;

        //int FindTarget()
        //void FindTarget1()
        //void FindTarget2()
        //void combine()
        //int imp()
        bool on_board(int x, int y)
        {
            if (((x >= 1) && (x <= 15)) || ((y >= 1) && (y <= 15))) return true else return false;
        }
        void imp_collect_row(int mode,int stepx,int stepy)
        {
            int i, j;
            while (on_board(i,j))
            {


            }


        }
        

    }
}
