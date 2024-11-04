using System;
using System.Drawing;
using System.Windows.Forms;

namespace Arkanoid
{
    public partial class Form1 : Form
    {
        int ballX = 10;
        int ballY = 10;
        int playerSpeed = 20;

        private Panel[] panels = new Panel[20]; 

        public Form1()
        {
            InitializeComponent();
            gameTimer.Interval = 20; 
            gameTimer.Tick += gameTimer_Tick;      
            gameTimer.Start();          
            ResetBall();
            InitializePanels();
        }

        private void InitializePanels()
        {
     
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i] = (Panel)this.Controls["panel" + (i + 1)];
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && userPalette.Left > 0)
            {
                userPalette.Left -= playerSpeed;
            }
            if (e.KeyCode == Keys.D && userPalette.Right < ClientSize.Width)
            {
                userPalette.Left += playerSpeed;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            Ballz.Left += ballX;
            Ballz.Top += ballY;

         
            if (Ballz.Top <= 0 || Ballz.Bottom >= ClientSize.Height)
            {
                ballY = -ballY;
            }
            if (Ballz.Right >= ClientSize.Width || Ballz.Left <= 0)
            {
                ballX = -ballX;
            }

            if (Ballz.Bounds.IntersectsWith(userPalette.Bounds))
            {
                ballY = -ballY;
            }


            foreach (var panel in panels)
            {
                if (panel.Visible && Ballz.Bounds.IntersectsWith(panel.Bounds))
                {
       
                    ballY = -ballY;
                    panel.Visible = false; 
                    break; 
                }
            }


            if (Ballz.Bottom >= ClientSize.Height)
            {
                ResetBall(); 
            }
        }

        private void ResetBall()
        {
            Ballz.Left = (ClientSize.Width / 2) - (Ballz.Width / 2);
            Ballz.Top = (ClientSize.Height / 2) - (Ballz.Height / 2);
            Random rand = new Random();
            ballX = rand.Next(2) == 0 ? -Math.Abs(ballX) : Math.Abs(ballX); 
            ballY = rand.Next(2) == 0 ? -Math.Abs(ballY) : Math.Abs(ballY);        
        }
    }
}
