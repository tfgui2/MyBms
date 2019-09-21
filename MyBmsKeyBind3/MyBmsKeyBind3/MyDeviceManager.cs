using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.DirectInput;

namespace MyBmsKeyBind3
{
    class MyDeviceManager
    {
        Dictionary<int, Joystick> joyDic = new Dictionary<int, Joystick>();

        public void DeviceInit()
        {
            joyDic.Clear();

            var directInput = new DirectInput();
            IList<DeviceInstance> devicelist = directInput.GetDevices();

            foreach (var deviceInstance in devicelist)
            {
                int index = DeviceSortIndex(deviceInstance.ProductGuid);
                if (joyDic.ContainsKey(index))
                {
                    continue;
                }

                if (index > -1)
                {
                    Joystick temp = new Joystick(directInput, deviceInstance.InstanceGuid);
                    temp.Properties.BufferSize = 128;
                    temp.Acquire();
                    joyDic.Add(index, temp);
                }
            }
        }

        private int DeviceSortIndex(Guid guid)
        {
            if (guid == Guid.Parse("{22150738-0000-0000-0000-504944564944}")) return 0;
            if (guid == Guid.Parse("{A2150738-0000-0000-0000-504944564944}")) return 1;
            if (guid == Guid.Parse("{076406A3-0000-0000-0000-504944564944}")) return 2;

            return -1;
        }

        public int PollButton()
        {
            foreach (KeyValuePair<int, Joystick> j in joyDic)
            {
                Joystick joystick = j.Value;
                JoystickState joyState = joystick.GetCurrentState();

                for (int i = 0; i < joyState.Buttons.Length; i++)
                {
                    if (joyState.Buttons[i])
                    {
                        return j.Key * 32 + i;
                    }
                }
            }

            return -1;
        }

    }
}
