using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ObooltNet;

namespace MyBmsServer
{
    public partial class Form1 : Form
    {
        NetConnection server;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            server = new NetConnection();
            server.OnConnect += Server_OnConnect;
            server.OnDisconnect += Server_OnDisconnect;
            server.OnDataReceived += Server_OnDataReceived;

            server.Start(55555);

            UpdateProcessList();

            timer1.Start();
        }

        private void Server_OnDataReceived(object sender, NetConnection connection, byte[] e)
        {
            string data = Encoding.UTF8.GetString(e);

            textBox1.AppendText("Message from " + connection.RemoteEndPoint + " : " + data + "\n");
            Send(data);
        }

        private void Server_OnDisconnect(object sender, NetConnection connection)
        {
            textBox1.AppendText("Disconnect fom " + connection.RemoteEndPoint + "\n");
        }

        private void Server_OnConnect(object sender, NetConnection connection)
        {
            textBox1.AppendText("Connect from " + connection.RemoteEndPoint + "\n");
        }

        Process falconBms = null;
        private void UpdateProcessList()
        {
            Process[] list = Process.GetProcessesByName("Falcon BMS");

            if (list.Length == 0)
            {
                return;
            }

            foreach (var p in list)
            {
                listBox1.Items.Add(p.ProcessName);
            }

            falconBms = list[0];
        }

        private void Send(string msg)
        {
            if (falconBms == null) return;

            AU3_Send(msg, 0);
        }

        [DllImport("autoitx3.dll")]
        public static extern void AU3_Send([MarshalAs(UnmanagedType.LPTStr)]string text, int mode);

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (falconBms == null)
            {
                UpdateProcessList();
            }
        }
    }
}
