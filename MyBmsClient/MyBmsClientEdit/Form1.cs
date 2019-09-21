using MyBmsClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyBmsClientEdit
{
    public partial class Form1 : Form
    {
        KeySaveLoader keysaveloader = new KeySaveLoader();

        List<MyButton> buttons = new List<MyButton>();
        List<MyButton> selected = new List<MyButton>();
        int nextIndex = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;

            pictureBox1.BackgroundImage = Bitmap.FromFile("background.png");
            pictureBox1.BackgroundImageLayout = ImageLayout.None;

            keysaveloader.LoadData(buttons);
            nextIndex = buttons.Count;
            UpdateButtonListView();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            keysaveloader.SaveData(buttons);
        }

        enum _Mode
        {
            mode_None,
            mode_Select,
            mode_Add,
            mode_Move,
        };

        _Mode cur_mode = _Mode.mode_Select;

        private void SetMode(_Mode mode)
        {
            if (mode != _Mode.mode_Move)
            {
                selected.Clear();
            }

            cur_mode = mode;
            toolStripStatusLabel1.Text = "mode: " + cur_mode.ToString();
            pictureBox1.Invalidate();
        }

        private void selectButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMode(_Mode.mode_Select);
        }

        private void addButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMode(_Mode.mode_Add);
        }

        private void moveButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMode(_Mode.mode_Move);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (MyButton b in selected)
            {
                buttons.Remove(b);
            }
            UpdateButtonListView();

            selected.Clear();

            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Pen p = Pens.YellowGreen;
            if (cur_mode == _Mode.mode_Add)
            {
                p = Pens.Red;
            }

            foreach (var b in buttons)
            {
                g.DrawRectangle(p, b.R);
            }

            if (cur_mode == _Mode.mode_Select)
            {
                foreach (var b in selected)
                {
                    g.DrawRectangle(Pens.WhiteSmoke, b.R);
                }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (cur_mode == _Mode.mode_Add)
            {
                AddMode_Click(e.Location);
            }

            if (cur_mode == _Mode.mode_Select)
            {
                SelectMode_Click(e.Location);
            }

            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (MyButton b in buttons)
            {
                if (b.R.Contains(e.Location))
                {
                    SetupKeyValue(b);
                    break;
                }
            }
        }

        private void ListViewSelect(MyButton b)
        {
            string index = b.Index.ToString();

            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text == index)
                {
                    item.Selected = true;
                    break;
                }
            }
        }

        private void SetupKeyValue(MyButton b)
        {

            FormSetupKeyValue formSetup = new FormSetupKeyValue();
            formSetup.Button = b;
            formSetup.ShowDialog(this);
            UpdateButtonListView();
        }

        private void AddMode_Click(Point p)
        {
            MyButton b = new MyButton();
            b.Index = nextIndex;
            nextIndex++;
            b.R = new Rectangle(p, new Size(40, 40));

            buttons.Add(b);
            UpdateButtonListView();
        }

        private void SelectMode_Click(Point p)
        {
            foreach (MyButton b in buttons)
            {
                if (b.R.Contains(p))
                {
                    selected.Add(b);
                    ListViewSelect(b);
                    break;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C)
            {
                SetMode(_Mode.mode_None);
            }

            if (e.KeyCode == Keys.S)
            {
                SetMode(_Mode.mode_Select);
            }

            if (e.KeyCode == Keys.Left)
            {
                MoveSelectedX(-1);
            }
            if (e.KeyCode == Keys.Right)
            {
                MoveSelectedX(1);
            }
            if (e.KeyCode == Keys.Up)
            {
                MoveSelectedY(-1);
            }
            if (e.KeyCode == Keys.Down)
            {
                MoveSelectedY(1);
            }

            if (e.KeyCode == Keys.O)
            {
                SizeUp(-1);
            }
            if (e.KeyCode == Keys.P)
            {
                SizeUp(1);
            }

            pictureBox1.Invalidate();
        }

        private void SizeUp(int u)
        {
            foreach (MyButton b in selected)
            {
                Rectangle r = b.R;
                r.Width += u;
                r.Height += u;

                b.R = r;
            }
        }

        private void MoveSelectedX(int x)
        {
            foreach (MyButton b in selected)
            {
                Rectangle r = b.R;
                r.Offset(x, 0);
                b.R = r;
            }
        }

        private void MoveSelectedY(int y)
        {
            foreach (MyButton b in selected)
            {
                Rectangle r = b.R;
                r.Offset(0, y);
                b.R = r;
            }
        }

        private void xAlignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int alignY = 10000;

            foreach (MyButton b in selected)
            {
                alignY = Math.Min(b.R.Y, alignY);
            }

            foreach (MyButton b in selected)
            {
                Rectangle r = b.R;
                r.Offset(0, alignY - r.Y);

                b.R = r;
            }

            pictureBox1.Invalidate();
        }

        private void yAlignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int alignX = 10000;

            foreach (MyButton b in selected)
            {
                alignX = Math.Min(b.R.X, alignX);
            }

            foreach (MyButton b in selected)
            {
                Rectangle r = b.R;
                r.Offset(alignX - r.X, 0);

                b.R = r;
            }

            pictureBox1.Invalidate();
        }

        private void UpdateButtonListView()
        {
            listView1.Items.Clear();

            foreach (MyButton b in buttons)
            {
                ListViewItem item = new ListViewItem(b.Index.ToString());
                item.SubItems.Add(b.Key);
                listView1.Items.Add(item);
            }
        }

        private void sizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Size resize = selected[0].R.Size;

            foreach (MyButton b in selected)
            {
                Rectangle r = b.R;
                r.Size = resize;

                b.R = r;
            }

            pictureBox1.Invalidate();
        }
    }
}
