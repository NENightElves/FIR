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
        const int width = 5;
        const int depth = 4;

        PictureBox[,] btn = new PictureBox[20, 20];
        int mode = 2;
        Image player_color = FIR_V2.Properties.Resources.circle_black;
        Image computer_color = FIR_V2.Properties.Resources.circle_write;
        Image cross = FIR_V2.Properties.Resources.cross;
        Image tmp_color;
        int n;
        FIR_user_V2[] step_record = new FIR_user_V2[300];
        bool IsEnd;

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

        private bool _IsWin(int x, int y, Image xy)
        {
            int i, j;
            int ii, jj;
            int n;
            for (i = 1; i <= 15; i++)
                for (j = 1; j <= 15; j++)
                {
                    n = 0;
                    ii = i;
                    jj = j;
                    while (btn[ii, jj].Image == xy)
                    {
                        ii += x;
                        jj += y;
                        n++;
                    }
                    if (n == 5) { return true; }
                }
            return false;
        }
        public bool IsWin()
        {
            if ((_IsWin(1, 1, player_color)) || (_IsWin(1, 0, player_color)) || (_IsWin(0, 1, player_color)) || (_IsWin(-1, 1, player_color)))
            {
                MessageBox.Show("玩家胜利");
                return true;
            }
            if ((_IsWin(1, 1, computer_color)) || (_IsWin(1, 0, computer_color)) || (_IsWin(0, 1, computer_color)) || (_IsWin(-1, 1, computer_color)))
            {
                MessageBox.Show("电脑胜利");
                return true;
            }
            return false;
        }

        void btn_event(object sender, EventArgs e)
        {
            int x, y;
            if (n == 0) { IsEnd = false; button1.Text = "重开"; }
            if ((IsEnd == false) && ((sender as PictureBox).Image == cross))
            {
                n++;
                (sender as PictureBox).Image = player_color;
                if (IsWin()) { IsEnd = true; return; }
                copy_data();
                x = step_record[n].FindTarget();
                y = x % 100;
                x = x / 100;
                btn[x, y].Image = computer_color;
                if (IsWin()) { IsEnd = true; return; }
                n++;
                copy_data();
            }
            return;
        }

        void btn_mouse_enter(object sender, EventArgs e)
        {
            if ((sender as PictureBox).Image == null) (sender as PictureBox).Image = cross;
        }

        void btn_mouse_leave(object sender, EventArgs e)
        {
            if ((sender as PictureBox).Image == cross) (sender as PictureBox).Image = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i, j;
            n = 0;
            button2.Visible = false;
            button3.Visible = false;
            textBox1.Visible = false;
            for (i = 1; i <= 299; i++)
            {
                step_record[i] = new FIR_user_V2(width, depth);
            }
            for (i = 0; i <= BOARD_SIZE_X + 1; i++)
                for (j = 0; j <= BOARD_SIZE_Y + 1; j++)
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
            for (i = 0; i <= BOARD_SIZE_X + 1; i++)
            {
                btn[i, 0].Visible = false;
                btn[0, i].Visible = false;
                btn[i, BOARD_SIZE_X + 1].Visible = false;
                btn[BOARD_SIZE_X + 1, i].Visible = false;
                btn[i, 0].Enabled = false;
                btn[0, i].Enabled = false;
                btn[i, BOARD_SIZE_X + 1].Enabled = false;
                btn[BOARD_SIZE_X + 1, i].Enabled = false;
            }
            this.Size = new Size(416, 490);
            button1.Location = new Point(5, 410);
            button2.Location = new Point(70, 410);
            button3.Location = new Point(146, 410);
            textBox1.Location = new Point(230, 411);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i, j;
            if (button1.Text == "电脑先")
            {
                tmp_color = player_color;
                player_color = computer_color;
                computer_color = tmp_color;
                Random ran = new Random();
                btn[8,8].Image = computer_color;
                n++;
                IsEnd = false;
                copy_data();
                mode = 1;
                button1.Text = "重开";
            }
            else
            {
                player_color = FIR_V2.Properties.Resources.circle_black;
                computer_color = FIR_V2.Properties.Resources.circle_write;
                n = 0;
                for (i = 1; i <= 299; i++)
                {
                    step_record[i] = new FIR_user_V2(width, depth);
                }
                for (i = 1; i <= BOARD_SIZE_X; i++)
                    for (j = 1; j <= BOARD_SIZE_Y; j++)
                    {
                        btn[i, j].Image = null;
                    }
                button1.Text = "电脑先";
            }
        }

        //test
        private void button2_Click(object sender, EventArgs e)
        {
            int i, j;
            FIR_user_V2 xxx = new FIR_user_V2(width, depth);
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
