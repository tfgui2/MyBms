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

namespace MyBmsKeyBind3
{
    public partial class Form1 : Form
    {
        private void UpdateFormTitle(string text)
        {
            this.Text = "MyBmsKeyBind - " + text;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenFile();
            }
        }

        private void OpenFile()
        {
            DocumentChangeState(false);

            Rows.Clear();

            string[] rawdata = File.ReadAllLines(openFileDialog1.FileName);
            foreach (string line in rawdata)
            {
                ParseRawLine(line);
            }

            findItems.Clear();
            findItemCurrent = 0;
            UpdateListview();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = openFileDialog1.FileName;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openFileDialog1.FileName = saveFileDialog1.FileName;
                DocumentChangeState(false);

                if (File.Exists(saveFileDialog1.FileName))
                {
                    string backupName = saveFileDialog1.FileName + "." + DateTime.Now.ToString("yy-MM-dd-hh-mm-ss");
                    File.Move(saveFileDialog1.FileName, backupName);
                }

                List<string> rawdata = new List<string>();
                List<MyRowDx> dxrows = new List<MyRowDx>();
                foreach (MyRow r in Rows)
                {
                    string temp = r.ToString();
                    rawdata.Add(temp);

                    if (r.Dx != null)
                    {
                        dxrows.Add(r.Dx);
                    }
                }

                foreach (MyRowDx d in dxrows)
                {
                    string temp = d.ToString();
                    rawdata.Add(temp);
                }

                File.WriteAllLines(saveFileDialog1.FileName, rawdata.ToArray());
            }
        }

        private void ParseRawLine(string line)
        {
            // skip comment line
            if (line.StartsWith("#") || line.Length < 4)
            {
                return;
            }

            string[] cols = line.Split(" ".ToCharArray(), 9);

            // if normal key
            if (cols.Length == 9)
            {
                MyRow r = new MyRow();
                r.Set(cols);
                Rows.Add(r);
            }
            // if dx key
            else
            {
                MyRowDx dx = new MyRowDx();
                dx.Set(cols);

                LinkDx(dx);
            }
        }

        private void LinkDx(MyRowDx dx)
        {
            foreach (var r in Rows)
            {
                if (r.Function == dx.Function)
                {
                    r.Dx = dx;
                    break;
                }
            }
        }
    }
}