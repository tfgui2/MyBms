using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBmsKeyBind3
{
    class MyRow
    {
        string function;
        string soundid;
        string mid;
        MyKeycode mainKey = new MyKeycode();
        MyKeycode addKey = new MyKeycode();
        int uiType;
        string desc;

        public string Function { get => function; set => function = value; }

        internal MyRowDx Dx { get; set; } = null;

        public void Set(string[] data)
        {
            if (data.Length != 9)
            {
                return;
            }

            function = data[0];
            soundid = data[1];
            mid = data[2];
            mainKey.Set(data[3], data[4]);
            addKey.Set(data[5], data[6]);
            uiType = int.Parse(data[7]);
            desc = data[8];
        }

        public override string ToString()
        {
            string temp = function;
            temp += " " + soundid;
            temp += " " + mid;
            temp += " " + mainKey.ToString();
            temp += " " + addKey.ToString();
            temp += " " + uiType.ToString();
            temp += " " + desc;

            return temp;
        }

        public string Name()
        {
            return desc;
        }

        public string KeyToHString()
        {
            if (mainKey.IsAssigned() == false)
            {
                return "";
            }

            string temp = mainKey.HumanString();
            if (addKey.IsAssigned())
            {
                temp = addKey.HumanString() + ", " + temp;
            }

            return temp;
        }

        public string DxToHString()
        {
            if (Dx != null)
            {
                return Dx.HumanString();
            }

            return "";
        }

        public bool IsTitle()
        {
            return uiType == -1;
        }

        public bool IsChangable()
        {
            return uiType == 1;
        }
    }
}
