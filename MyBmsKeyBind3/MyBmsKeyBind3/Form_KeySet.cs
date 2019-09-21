using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MyBmsKeyBind3
{
    public partial class Form_KeySet : Form
    {
        private MyRow row;

        internal MyRow Row { get => row; set => row = value; }
        int dxkey = -1;

        MyDeviceManager dmanager = new MyDeviceManager();

        public Form_KeySet()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            if (textBox1.Text.Length > 0)
            {
                int test = Convert.ToInt32(textBox1.Text);
                if (dxkey != test)
                {
                    dxkey = test;
                }
            }

            if (dxkey == -1)
            {
                Row.Dx = null;
            }
            else
            {
                MyRow res = ((Form1)Owner).FindDx(dxkey);
                if (res != null)
                {
                    MessageBox.Show("Duplicate key: " + res.Name());
                    return;
                }

                if (Row.Dx == null)
                {
                    Row.Dx = new MyRowDx();
                    Row.Dx.Function = Row.Function;
                }

                Row.Dx.SetKey(dxkey, checkShift.Checked);
            }

            Close();
        }

        private void Form_KeySet_Load(object sender, EventArgs e)
        {
            label1.Text = Row.Name();
            textBox1.Text = "";
            dxkey = -1;
            checkShift.Checked = false;

            if (Row.Dx != null)
            {
                dxkey = Row.Dx.DxCode();
                checkShift.Checked = Row.Dx.IsShift();

                textBox1.Text = dxkey.ToString();
            }

            dmanager.DeviceInit();
            
            timer1.Start();
        }

        private void Form_KeySet_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dxkey = dmanager.PollButton();
            if (dxkey >= 0)
            {
                textBox1.Text = dxkey.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dxkey = -1;
            textBox1.Text = "";
            checkShift.Checked = false;
        }
    }
}
