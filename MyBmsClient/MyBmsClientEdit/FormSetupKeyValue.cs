using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyBmsClient;

namespace MyBmsClientEdit
{
    public partial class FormSetupKeyValue : Form
    {
        MyBmsClient.MyButton button;

        internal MyButton Button { get => button; set => button = value; }
        

        public FormSetupKeyValue()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button.Key = textBox1.Text;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSetupKeyValue_Load(object sender, EventArgs e)
        {
            textBox1.Text = button.Key;
        }
    }
}
