using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBmsKeyBind3
{
    class MyRowDx
    {
        string function;
        int dxkey;
        int invoc = 8;
        int dxtype = -2;
        int press = 0;
        string six = "0x0";
        string soundid = "-1";

        public string Function { get => function; set => function = value; }
        public int Dxkey { get => dxkey; set => dxkey = value; }

        public void Set(string[] data)
        {
            function = data[0];
            Dxkey = int.Parse(data[1]);
            invoc = int.Parse(data[2]);
            dxtype = int.Parse(data[3]);
            press = int.Parse(data[4]);
            six = data[5];
            soundid = data[6];
        }
        
        public override string ToString()
        {
            string temp = function;
            temp += " " + Dxkey.ToString();
            temp += " " + invoc.ToString();
            temp += " " + dxtype.ToString();
            temp += " " + press.ToString();
            temp += " " + six;
            temp += " " + soundid;

            return temp;
        }

        public string HumanString()
        {
            string temp = "";

            if (IsShift())
            {
                temp += "Shift ";
            }

            temp += "Button " + DxCode().ToString();
            return temp;
        }

        public bool IsShift()
        {
            return Dxkey >= 256;
        }

        public int DxCode()
        {
            return IsShift() ? 256 - Dxkey : Dxkey;
        }

        public void SetKey(int code, bool mod)
        {
            Dxkey = mod ? code + 256 : code;
        }
    }
}
