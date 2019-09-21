using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBmsKeyBind3
{
    class MyKeycode
    {
        int code;
        int mod;

        public void Set(string strCode, string strMod)
        {
            code = Convert.ToInt32(strCode, 16);
            mod = int.Parse(strMod);
        }

        public override string ToString()
        {
            string temp = "0";
            if (code != 0)
            {
                temp = string.Format("0X{0:X}", code);
            }
            temp += " " + mod.ToString();

            return temp;
        }

        public string HumanString()
        {
            string temp = ConvertKeycodeToString(code);
            if (temp.Length == 0)
            {
                temp = code.ToString();
            }

            if (mod > 0)
            {
                temp = modToString() + " " + temp;
            }

            return temp;
        }

        string modToString()
        {
            switch(mod)
            {
                case 1: return "Shift";

                case 2: return "Ctrl";
                case 3: return "Ctrl Shift";

                case 4: return "Alt";
                case 5: return "Alt Shift";

                case 6: return "Ctrl Alt";
                case 7: return "Ctrl Alt Shift";
            }

            return "";
        }

        public bool IsAssigned()
        {
            if (code == -1)
            {
                return false;
            }

            if (code == 0)
            {
                return false;
            }

            return true;
        }

        public string ConvertKeycodeToString(int keycode)
        {
            if (Enum.IsDefined(typeof(SharpDX.DirectInput.Key), keycode))
            {
                SharpDX.DirectInput.Key k = (SharpDX.DirectInput.Key)keycode;
                return k.ToString();
            }

            return "";
        }
    }
}
