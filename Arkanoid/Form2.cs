using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Arkanoid
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public int bspeed = 0;
        public int pwidth = 0;
        public int z = 0;
        public bool coop = false;

        private void button1_Click(object sender, EventArgs e)
        {
            bspeed = 10;
            pwidth = 350;
            z = 50;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bspeed = 20;
            pwidth = 150;
            z = 50;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bspeed = 15;
            pwidth = 5;
            z = 50;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            bspeed = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            pwidth = (int)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            z = (int)numericUpDown3.Value;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bspeed = (int)numericUpDown1.Value;
            pwidth = (int)numericUpDown2.Value;
            z = (int)numericUpDown3.Value;


            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            coop = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            coop = false;
        }
    }
}
