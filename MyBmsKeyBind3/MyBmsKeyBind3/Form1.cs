using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace MyBmsKeyBind3
{
    public partial class Form1 : Form
    {
        internal List<MyRow> Rows { get; set; } = new List<MyRow>();

        public Form1()
        {
            InitializeComponent();
            this.Text = "MyBmsKeyBind";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigLoad();

            listView1.Columns.Add("#", 30);
            listView1.Columns.Add("Func", 200);
            listView1.Columns.Add("Name", 400);
            listView1.Columns.Add("Key", 200);
            listView1.Columns.Add("Dx", 200);

            timer1.Start();

            if (File.Exists(openFileDialog1.FileName))
            {
                OpenFile();
            }

            dmanager.DeviceInit();
        }

        private void UpdateListview()
        {
            int prePosition = 0;
            if (listView1.TopItem != null)
            {
                prePosition = listView1.TopItem.Index;
            }

            listView1.Items.Clear();

            string[] attr = new string[5];

            int i = 0;
            foreach (MyRow r in Rows)
            {
                attr[0] = i.ToString();
                i++;
                attr[1] = r.Function;
                attr[2] = r.Name();
                attr[3] = r.KeyToHString();
                attr[4] = r.DxToHString();

                ListViewItem item = new ListViewItem(attr);
                if (r.IsTitle())
                {
                    item.ForeColor = Color.Red;
                }
                else if (r.IsChangable() == false)
                {
                    item.ForeColor = Color.Gray;
                }

                listView1.Items.Add(item);
            }

            listView1.TopItem = listView1.Items[prePosition];
        }

        Form_KeySet frmKeySet = new Form_KeySet();

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listView1.SelectedItems[0];
            int index = int.Parse(item.Text);

            var target = Rows[index];
            if (target.IsChangable() == false)
            {
                return;
            }

            frmKeySet.Row = target;
            if (frmKeySet.ShowDialog(this) == DialogResult.OK)
            {
                item.SubItems[4].Text = target.DxToHString();
                DocumentChangeState(true);
            }
        }

        private void DocumentChangeState(bool isChange)
        {
            string title = openFileDialog1.FileName;
            if (isChange)
            {
                title += " *";
            }
            UpdateFormTitle(title);
        }

        bool textbox1Changed = false;
        DateTime textbox1ChangedTime;
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textbox1Changed = true;
            textbox1ChangedTime = DateTime.Now;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (textbox1Changed)
            {
                TimeSpan ts = DateTime.Now - textbox1ChangedTime;
                if (ts.TotalSeconds > 2)
                {
                    textbox1Changed = false;
                    FindName(textBox1.Text);
                }
            }

            int pressedButton = dmanager.PollButton();
            if (pressedButton >= 0)
            {
                Console.WriteLine(pressedButton.ToString());
                MyRow res = FindDx(pressedButton);
                if (res != null)
                {
                    FindColumn(2, res.Name(), false);
                }
            }
        }

        int findItemCurrent = 0;
        List<ListViewItem> findItems = new List<ListViewItem>();

        private void FindColumn(int col, string name, bool allowSubstring = true)
        {
            if (listView1.Items.Count < 1)
            {
                return;
            }

            if (col < 0 || col > listView1.Items[0].SubItems.Count)
            {
                return;
            }

            foreach (var f in findItems)
            {
                f.BackColor = Color.White;
            }
            findItems.Clear();
            findItemCurrent = 0;

            name = name.ToLower();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                ListViewItem item = listView1.Items[i];

                string temp = item.SubItems[col].Text.ToLower();

                if (allowSubstring)
                {
                    if (temp.Contains(name))
                    {
                        findItems.Add(item);
                    }
                }
                else
                {
                    if (temp == name)
                    {
                        findItems.Add(item);
                    }
                }
            }

            ScrollToFindNext(1);
        }

        private void FindName(string name)
        {
            FindColumn(2, name);
        }

        internal MyRow FindDx(int dxbutton)
        {
            List<MyRow> findRow = new List<MyRow>();

            foreach (MyRow r in Rows)
            {
                if (r.Dx == null) continue;

                if (r.Dx.Dxkey == dxbutton)
                {
                    findRow.Add(r);
                }
            }

            if (findRow.Count > 1)
            {
                string msg = "Duplicate Key: ";
                foreach (var d in findRow)
                {
                    msg += d.Name() + " ";
                }
                MessageBox.Show(msg);
                return null;
            }

            if (findRow.Count == 0)
            {
                return null;
            }

            return findRow[0];
        }

        private void ScrollToFindNext(int offset)
        {
            if (findItems.Count < 1) return;

            ListViewItem target = findItems[findItemCurrent];
            findItemCurrent += offset + findItems.Count;
            findItemCurrent %= findItems.Count;

            listView1.EnsureVisible(target.Index);

            foreach (var f in findItems)
            {
                f.BackColor = Color.White;
            }
            target.BackColor = Color.LightYellow;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ScrollToFindNext(-1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ScrollToFindNext(1);
        }

        Newtonsoft.Json.Linq.JObject jsonConfig = new Newtonsoft.Json.Linq.JObject();
        string configfileName = "MyBmsKeyBind.config";

        private void ConfigLoad()
        {
            if (File.Exists(configfileName))
            {
                string data = File.ReadAllText(configfileName);
                jsonConfig = Newtonsoft.Json.Linq.JObject.Parse(data);

                openFileDialog1.FileName = jsonConfig["lastfile"].ToString();
            }
        }

        private void ConfigSave()
        {
            jsonConfig["lastfile"] = openFileDialog1.FileName;

            string data = jsonConfig.ToString();
            File.WriteAllText(configfileName, data);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfigSave();
        }

        MyDeviceManager dmanager = new MyDeviceManager();

    }
}
