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
    public class WorkspaceController
    {
        private InputSimulator inputSimulator = new InputSimulator();
        private ApplicationController appController = new ApplicationController();

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

        public bool ExecuteApp(string target, string action, string value)
        {
            Console.WriteLine("Aqui");
            return appController.Execute(target, action, value);
        }


        private bool Create()
        {
            inputSimulator.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.LWIN, VirtualKeyCode.CONTROL }, VirtualKeyCode.VK_D);
            MainWindow.workspaces.Add(new WorkspaceController());
            MainWindow.currentWorkspace = MainWindow.nWorkspaces;
            MainWindow.nWorkspaces += 1;
            return true;
        }

        private bool Delete()
        {
            if (MainWindow.nWorkspaces == 1)
                return false;
            MainWindow.workspaces[MainWindow.currentWorkspace].ExecuteApp("ALL", "CLOSE", "");
            inputSimulator.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.LWIN, VirtualKeyCode.CONTROL }, VirtualKeyCode.F4);
            MainWindow.workspaces.RemoveAt(MainWindow.currentWorkspace);
            MainWindow.currentWorkspace -= 1;
            MainWindow.nWorkspaces -= 1;
            return true;
        }

        private bool Move(string value)
        {
            if (value == "NEXT")
            {
                if (MainWindow.currentWorkspace + 1 == MainWindow.nWorkspaces)
                    return false;
                inputSimulator.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.LWIN, VirtualKeyCode.CONTROL }, VirtualKeyCode.RIGHT);
                MainWindow.currentWorkspace += 1;
            }
            else if (value == "PREV")
            {
                if (MainWindow.currentWorkspace < 1)
                    return false;
                inputSimulator.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.LWIN, VirtualKeyCode.CONTROL }, VirtualKeyCode.LEFT);
                MainWindow.currentWorkspace -= 1;
            }
            
            return true;
        }

    }
}
