using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using ObooltNet;


namespace MyBmsClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        KeySaveLoader keysaveloader = new KeySaveLoader();
        List<MyButton> buttons = new List<MyButton>();


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var b in buttons)
            {
                if (b.R.Contains(e.Location))
                {

                    toolStripStatusLabel4.Text = "Click : " + b.R.ToString();

                    this.Send(b.Key);
                    break;
                }
            }
        }

        
        private void UpdateLabel()
        {
            toolStripStatusLabel1.Text = "Connect: " + connected.ToString();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            UpdateLabel();

            Graphics g = e.Graphics;

            Pen p = Pens.YellowGreen;

            foreach (var b in buttons)
            {
                g.DrawRectangle(p, b.R);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Bitmap.FromFile("background.png");
            this.BackgroundImageLayout = ImageLayout.None;

            keysaveloader.LoadData(buttons);
            Client_init();
        }
    }
}
