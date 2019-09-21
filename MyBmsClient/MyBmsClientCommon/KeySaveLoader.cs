using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBmsClient
{
    class KeySaveLoader
    {
        private string pathname = "testkey.txt";

        public void SaveData(List<MyButton> buttons)
        {
            List<string> lines = new List<string>();

            int reIndex = 0;
            foreach (var b in buttons)
            {
                b.Index = reIndex;
                reIndex++;
                string temp = JsonConvert.SerializeObject(b);
                lines.Add(temp);
            }

            System.IO.File.WriteAllLines(pathname, lines.ToArray());
        }

        public void LoadData(List<MyButton> buttons)
        {
            if (System.IO.File.Exists(pathname) == false)
            {
                return;
            }

            string[] lines = System.IO.File.ReadAllLines(pathname);

            int reIndex = 0;
            foreach (var l in lines)
            {
                MyButton b = JsonConvert.DeserializeObject<MyButton>(l);
                b.Index = reIndex;
                reIndex++;
                buttons.Add(b);
            }
        }
    }
}
