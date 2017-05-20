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


        public Form1()
        {
            InitializeComponent();
        }

        void btn_event(object sender, EventArgs e)
        {
            if ((sender as Button).BackColor == Color.White)
                (sender as Button).BackColor = player_color;
            else
                return;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i, j;
            for (i = 1; i <= BOARD_SIZE_X; i++)
                for (j = 1; j <= BOARD_SIZE_Y; j++)
                {
                    btn[i, j] = new Button();
                    btn[i, j].Size = new Size(BUTTON_SIZE_X, BUTTON_SIZE_Y);
                    btn[i, j].Location = new Point(LEFT_MARGIN + (i - 1) * BUTTON_SIZE_X, UP_MARGIN + (j - 1) * BUTTON_SIZE_Y);
                    btn[i, j].BackColor = Color.White;
                    btn[i, j].Click += btn_event;
                    this.Controls.Add(btn[i, j]);
                }
            this.Size = new Size(LEFT_MARGIN + RIGHT_MARGIN + BOARD_SIZE_X * BUTTON_SIZE_X, UP_MARGIN + DOWN_MARGIN + BOARD_SIZE_Y * BUTTON_SIZE_Y);

            btn[1, 1].BackColor = Color.Black;
            btn[1, 2].BackColor = Color.Gray;


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

        }
        //test
    }
}
