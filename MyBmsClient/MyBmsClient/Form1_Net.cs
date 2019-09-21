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
        bool connected = false;

        NetConnection client = new NetConnection();

        private void Client_init()
        {
            client.OnConnect += Client_OnConnect;
            try
            {
                client.Connect("192.168.10.6", 55555);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Client_OnConnect(object sender, NetConnection connection)
        {
            connected = true;
            UpdateLabel();
        }

        private void Send(string msg)
        {
            if (connected == false)
            {
                return;
            }

            client.Send(Encoding.UTF8.GetBytes(msg));
        }
    }
}