﻿using System;
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
        const int LEFT_MARGIN = 50;
        const int UP_MARGIN = 50;
        const int RIGHT_MARGIN = 75;
        const int DOWN_MARGIN = 100;
        const int BUTTON_SIZE_X = 25;
        const int BUTTON_SIZE_Y = 25;
        const int BOARD_SIZE_X = 15;
        const int BOARD_SIZE_Y = 15;

        Button[,] btn = new Button[20, 20];
        int mode = 2;
        Color player_color = Color.Black;
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
                    if (btn[i, j].BackColor == Color.White) step_record[n].board[i, j] = 0;
                    if (btn[i, j].BackColor == Color.Black) step_record[n].board[i, j] = 2;
                    if (btn[i, j].BackColor == Color.Gray) step_record[n].board[i, j] = 1;
                }
        }

        void btn_event(object sender, EventArgs e)
        {
            int x, y;
            if ((sender as Button).BackColor == Color.White)
            {
                n++;
                (sender as Button).BackColor = player_color;
                copy_data();
                x=step_record[n].FindTarget();
                y = x % 100;
                x = x / 100;
                btn[x, y].BackColor = Color.Gray;
                n++;
                copy_data();
            }
            else
                return;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i, j;
            for (i=1;i<=199;i++)
            {
                step_record[i] = new FIR_core();
            }
            for (i = 0; i <= BOARD_SIZE_X; i++)
                for (j = 0; j <= BOARD_SIZE_Y; j++)
                {
                    btn[i, j] = new Button();
                    btn[i, j].Size = new Size(BUTTON_SIZE_X, BUTTON_SIZE_Y);
                    btn[i, j].Location = new Point(LEFT_MARGIN + (i - 1) * BUTTON_SIZE_X, UP_MARGIN + (j - 1) * BUTTON_SIZE_Y);
                    btn[i, j].BackColor = Color.White;
                    btn[i, j].Click += btn_event;
                    this.Controls.Add(btn[i, j]);
                    if (i == 0) btn[i, j].Text = Convert.ToString(j%10);
                    if (j == 0) btn[i, j].Text = Convert.ToString(i%10);
                }
            this.Size = new Size(LEFT_MARGIN + RIGHT_MARGIN + BOARD_SIZE_X * BUTTON_SIZE_X, UP_MARGIN + DOWN_MARGIN + BOARD_SIZE_Y * BUTTON_SIZE_Y);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            player_color = Color.Gray;
            btn[8, 8].BackColor = Color.Black;
            mode = 1;
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
            xxx.board[8, 8] = 2;
            Console.Write(xxx.FindTarget());
        }
        //test
    }
}
