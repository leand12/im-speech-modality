using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace AppGui
{
    class WorkspaceController
    {
        private InputSimulator inputSimulator = new InputSimulator();
        public bool Execute(string action, string value)
        {
            switch (action)
            {
                case "CREATE":
                    return Create();
                case "DELETE":
                    return Delete();
                case "MOVE":
                    return Move(value);
            }
            return true;
        }

        private bool Create()
        {
            inputSimulator.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.LWIN, VirtualKeyCode.CONTROL }, VirtualKeyCode.VK_D);
            return true;
        }

        private bool Delete()
        {
            return true;
        }

        private bool Move(string value)
        {
            if (value == "NEXT")
                inputSimulator.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.LWIN, VirtualKeyCode.CONTROL }, VirtualKeyCode.RIGHT);
            else if (value == "PREV")
                inputSimulator.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.LWIN, VirtualKeyCode.CONTROL }, VirtualKeyCode.LEFT);
            else
                return false;
            return true;
        }

    }
}
