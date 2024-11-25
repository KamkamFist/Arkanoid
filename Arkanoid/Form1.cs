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
        private int playerWidth = 50;
        int znajdzkaszansa = 50;
        private int ballSpeed = 5;
        private Panel[] panels = new Panel[20];
        private int destroyedBlocks = 0;
        private Panel[] znajdzki = new Panel[2];
        private int[] znajdzkiY = new int[2];
        private bool[] znajdzkiActive = new bool[2];
        private DateTime[] effectStartTime = new DateTime[2];
        private bool[] effectExpired = new bool[2];
        int licznik = 3;

        public Form1()
        {
            InitializeComponent();
            LevelSelection();
            gameTimer.Interval = 20;
            gameTimer.Tick += gameTimer_Tick;
            gameTimer.Start();
            ResetBall();
            InitializePanels();
            InitializeZnajdzki();
            znajdzkaTimer.Interval = 100;
            znajdzkaTimer.Tick += znajdzkaTimer_Tick;
            znajdzkaTimer.Start();
        }

        private void LevelSelection()
        {
            Form2 levelForm = new Form2();
            if (levelForm.ShowDialog() == DialogResult.OK)
            {
                ballSpeed = levelForm.bspeed;
                playerWidth = levelForm.pwidth;
                znajdzkaszansa = levelForm.z;
                userPalette.Width = playerWidth;
            }
        }

        private void InitializePanels()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i] = (Panel)this.Controls["panel" + (i + 1)];
            }
        }

        private void InitializeZnajdzki()
        {
            znajdzki[0] = (Panel)this.Controls["Znajdzka1"];
            znajdzki[1] = (Panel)this.Controls["Znajdzka2"];

            foreach (var znajdzka in znajdzki)
            {
                znajdzka.Visible = false;
            }

            for (int i = 0; i < znajdzki.Length; i++)
            {
                znajdzkiActive[i] = false;
                effectExpired[i] = false;
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
            Ballz.Left += ballX * ballSpeed / 10;
            Ballz.Top += ballY * ballSpeed / 10;

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
                    destroyedBlocks++;

                    SpawnZnajdzka(panel.Left, panel.Top);

                    licznik++;
                    label1.Text = licznik.ToString();
                    if (destroyedBlocks == panels.Length)
                    {
                        MessageBox.Show("Wygra³eœ! Zniszczy³eœ wszystkie bloki!", "Gratulacje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetGame();
                    }

                    break;
                }
            }

            MoveZnajdzki();
            CheckZnajdzkiCollision();

            if (Ballz.Bottom >= ClientSize.Height)
            {
                CheckLose();
                ResetBall();
                label1.Text = licznik.ToString();
            }
        }

        void CheckLose()
        {
            licznik -= 1;
            if (licznik <= 0)
            {
                MessageBox.Show("przegra³eœ misiak");
                licznik = 3;
                ResetGame();
                label1.Text = licznik.ToString();
            }
        }

        private void SpawnZnajdzka(int blockX, int blockY)
        {
            Random rand = new Random();
            int randomValue = rand.Next(1, 101);

            if (randomValue < znajdzkaszansa)
            {
                int znajdzkaIndex = rand.Next(2);

                if (!znajdzki[znajdzkaIndex].Visible)
                {
                    znajdzki[znajdzkaIndex].Left = blockX;
                    znajdzki[znajdzkaIndex].Top = blockY;
                    znajdzki[znajdzkaIndex].Visible = true;
                    znajdzkiY[znajdzkaIndex] = blockY;
                    effectStartTime[znajdzkaIndex] = DateTime.Now;
                    znajdzkiActive[znajdzkaIndex] = true;
                    effectExpired[znajdzkaIndex] = false;
                }
            }
        }

        private void MoveZnajdzki()
        {
            for (int i = 0; i < znajdzki.Length; i++)
            {
                if (znajdzki[i].Visible)
                {
                    znajdzki[i].Top += 5;

                    if (znajdzki[i].Top + znajdzki[i].Height >= ClientSize.Height)
                    {
                        znajdzki[i].Visible = false;
                        znajdzkiActive[i] = false;
                    }
                }
            }
        }

        private void CheckZnajdzkiCollision()
        {
            for (int i = 0; i < znajdzki.Length; i++)
            {
                if (znajdzki[i].Visible && znajdzki[i].Bounds.IntersectsWith(userPalette.Bounds))
                {
                    switch (i)
                    {
                        case 0:
                            ballX /= 2;
                            ballY /= 2;
                            break;
                        case 1:
                            userPalette.Width += 20;
                            break;
                    }

                    znajdzki[i].Visible = false;
                    effectExpired[i] = false;
                }
            }
        }

        private void znajdzkaTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < znajdzki.Length; i++)
            {
                if (znajdzkiActive[i] && !effectExpired[i])
                {
                    if ((DateTime.Now - effectStartTime[i]).TotalSeconds >= 5)
                    {
                        DeactivateEffect(i);
                    }
                }
            }
        }

        private void DeactivateEffect(int znajdzkaIndex)
        {
            switch (znajdzkaIndex)
            {
                case 0:
                    ballX *= 2;
                    ballY *= 2;
                    break;
                case 1:
                    userPalette.Width -= 20;
                    break;
            }

            znajdzkiActive[znajdzkaIndex] = false;
            effectExpired[znajdzkaIndex] = true;
        }

        private void ResetBall()
        {
            Ballz.Left = (ClientSize.Width / 2) - (Ballz.Width / 2);
            Ballz.Top = (ClientSize.Height / 2) - (Ballz.Height / 2);
            Random rand = new Random();
            ballX = rand.Next(2) == 0 ? -Math.Abs(ballX) : Math.Abs(ballX);
            ballY = rand.Next(2) == 0 ? -Math.Abs(ballY) : Math.Abs(ballY);
        }

        private void ResetGame()
        {
            destroyedBlocks = 0;
            foreach (var panel in panels)
            {
                panel.Visible = true;
            }
            ResetBall();
            InitializeZnajdzki();
        }
    }
}
