using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIR
{
    public partial class Form1 : Form
    {
        const int LEFT_MARGIN = 13;
        const int UP_MARGIN = 13;
        const int RIGHT_MARGIN = 75;
        const int DOWN_MARGIN = 100;
        const int BUTTON_SIZE_X = 23;
        const int BUTTON_SIZE_Y = 23;
        const int BUTTON_DISTANCE_X = 25;
        const int BUTTON_DISTANCE_Y = 25;
        const int BOARD_SIZE_X = 15;
        const int BOARD_SIZE_Y = 15;

        PictureBox[,] btn = new PictureBox[20, 20];
        int mode = 2;
        Image player_color = Properties.Resources.circle_black;
        Image computer_color = Properties.Resources.circle_write;
        Image cross = Properties.Resources.cross;
        Image tmp_color;
        int n = 0;
        FIR_core[] step_record = new FIR_core[200];

        public Form1()
        {
            InitializeComponent();
        }

        //用于获取按钮与棋盘格点的对应坐标
        public int GetX(int n)
        {
            return ((n - LEFT_MARGIN) / BUTTON_SIZE_X + 1);
        }
        public int GetY(int n)
        {
            return ((n - UP_MARGIN) / BUTTON_SIZE_Y + 1);
        }
        //用于获取按钮与棋盘格点的对应坐标

        public void copy_data()
        {
            int i, j;
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                {
                    if (btn[i, j].Image == cross) step_record[n].board[i, j] = 0;
                    if (btn[i, j].Image == player_color) step_record[n].board[i, j] = 2;
                    if (btn[i, j].Image == computer_color) step_record[n].board[i, j] = 1;
                }
        }

        void btn_event(object sender, EventArgs e)
        {
            int x, y;
            if ((sender as PictureBox).Image == cross)
            {
                n++;
                (sender as PictureBox).Image = player_color;
                copy_data();
                x = step_record[n].FindTarget();
                y = x % 100;
                x = x / 100;
                btn[x, y].Image = computer_color;
                n++;
                copy_data();
            }
            else
                return;
        }

        void btn_mouse_enter(object sender, EventArgs e)
        {
            if ((sender as PictureBox).Image==null) (sender as PictureBox).Image = cross;
        }

        void btn_mouse_leave(object sender, EventArgs e)
        {
            if ((sender as PictureBox).Image == cross) (sender as PictureBox).Image = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i, j;
            for (i = 1; i <= 199; i++)
            {
                step_record[i] = new FIR_core();
            }
            for (i = 1; i <= BOARD_SIZE_X; i++)
                for (j = 1; j <= BOARD_SIZE_Y; j++)
                {
                    btn[i, j] = new PictureBox();
                    btn[i, j].Size = new Size(BUTTON_SIZE_X, BUTTON_SIZE_Y);
                    btn[i, j].Location = new Point(UP_MARGIN + (j - 1) * BUTTON_DISTANCE_Y, LEFT_MARGIN + (i - 1) * BUTTON_DISTANCE_X);
                    btn[i, j].BackColor = Color.Transparent;
                    btn[i, j].Image = null;
                    btn[i, j].MouseEnter += btn_mouse_enter;
                    btn[i, j].MouseLeave += btn_mouse_leave;
                    btn[i, j].Click += btn_event;
                    this.Controls.Add(btn[i, j]);
                }
            //this.Size = new Size(LEFT_MARGIN + RIGHT_MARGIN + BOARD_SIZE_X * BUTTON_SIZE_X, UP_MARGIN + DOWN_MARGIN + BOARD_SIZE_Y * BUTTON_SIZE_Y);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            tmp_color = player_color;
            player_color = computer_color;
            computer_color = tmp_color;
            Random ran = new Random();
            btn[ran.Next(7, 10), ran.Next(7, 10)].BackColor = Color.Black;
            n++;
            copy_data();
            mode = 1;
            button1.Enabled = false;
        }

        //test
        private void button2_Click(object sender, EventArgs e)
        {
            int i, j;
            FIR_core xxx = new FIR_core();
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
            Console.Write(xxx.FindTarget());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i, j;
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                {
                    //if (btn[i, j].BackColor == Color.White) textBox1.Text+="xxx.board[" + i + ", " + j + "] = 0";
                    if (btn[i, j].Image == player_color) textBox1.Text += "xxx.board[" + i + ", " + j + "] = 2;\r\n";
                    if (btn[i, j].Image == computer_color) textBox1.Text += "xxx.board[" + i + ", " + j + "] = 1;\r\n";
                }
        }
        //test
    }
}
