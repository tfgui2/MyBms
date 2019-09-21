using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyBmsClient
{
    class MyButton
    {
        private int index;
        private Rectangle r;
        private string key;

        public Rectangle R { get => r; set => r = value; }
        public string Key { get => key; set => key = value; }
        public int Index { get => index; set => index = value; }

        public MyButton()
        {
            index = 0;
            Key = "not assigned";
        }
    }
}
