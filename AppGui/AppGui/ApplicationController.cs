using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using System.Management;


namespace AppGui
{
    struct ProgramInfo
    {
        public string execName;
        public string killName;
        public bool killAll;
        public Program program;
    }

    class ApplicationController
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
     

        [DllImport("user32")]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

        private Dictionary<string, ProgramInfo> apps = new Dictionary<string, ProgramInfo>()
        {
            { "CALC", new ProgramInfo() { execName = "calc", killName = "CalculatorApp", killAll = false } },
            { "NOTEPAD", new ProgramInfo() { execName =  "Notepad", killName = "Notepad", killAll = false } },
            { "SPOTIFY", new ProgramInfo() { execName = "Spotify", killName = "Spotify", killAll = true } },
            { "CAMERA", new ProgramInfo() { execName = "microsoft.windows.camera:", killName = "WindowsCamera", killAll = false, program = new CameraProgram() } },
        };

        /* Consts */

        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_SHOWMINIMIZED = 6;
        private const int SW_RESTORE = 9;


        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_SHOWWINDOW = 0x0040;

        private const int WP_UP = 0b0001;
        private const int WP_DOWN = 0b0010;
        private const int WP_LEFT = 0b0100;
        private const int WP_RIGHT = 0b1000;


        /// <summary>
        /// Attempt to execute a given action for a given target.
        /// </summary>
        /// <param name="target">The key for the given target.</param>
        /// <param name="action">An array with the action parameters.</param>
        /// <param name="value">A value to be used in some of the actions. May differ depending on the action.</param>
        /// <returns>
        /// True if the method was able to recognize the action; otherwise, false.
        /// </returns>
        public bool Execute(string target, string action, string value)
        {
            switch (action)
            {
                case "OPEN":
                    return Open(target);
                case "CLOSE":
                    return Close(target);
                case "MOVE":
                    return MoveWindowTo(target, GetPosition(value));
                case "MAXIMIZE":
                    return SetWindowShow(target, SW_SHOWMAXIMIZED);
                case "MINIMIZE":
                    return SetWindowShow(target, SW_SHOWMINIMIZED);
                case "SHOW":
                    return Show(target);
            }

            if (apps.ContainsKey(target))
            {
                Program p = apps[target].program;
                if (p == null) return p.Execute(action);
            }

            return false;
        }

        private bool CloseAll()
        {
            foreach (Process process in GetProgramProcesses())
            {
                process.Kill();
            }
            return true;
        }

        private bool Open(string target)
        {
            if (apps.ContainsKey(target))
            {
                Process p = Process.Start(apps[target].execName);
                return true;
            }
            return false;
        }

        private bool Close(string target)
        {
            if (target == "ALL")
                return CloseAll();

            if (!apps.ContainsKey(target) && apps[target].killAll)
            {
                foreach (var p2 in Process.GetProcessesByName(target))
                {
                    p2.Kill();

                }
                return true;
            }

            Process p = GetProcess(target);
            if (p != null)
            {
                p.Kill();
                return true;
            }
            return false;
        }

        private bool Show(string target)
        {
            Process process = GetProcess(target);
            if (process == null)
                return false;

            ShowWindow(process.MainWindowHandle, SW_RESTORE);
            SetForegroundWindow(process.MainWindowHandle);

            return true;
        }

        private int GetPosition(string position)
        {

            string[] directions = position.Split('_');

            int fPosition = 0;
            for (int i = 0; i < directions.Length; i++)
            {
                if (directions[i] == "LEFT")
                    fPosition |= WP_LEFT;
                if (directions[i] == "RIGHT")
                    fPosition |= WP_RIGHT;
                if (directions[i] == "UP")
                    fPosition |= WP_UP;
                if (directions[i] == "DOWN")
                    fPosition |= WP_DOWN;
            }
            return fPosition;

        }

        private Process GetProcess(String target)
        {
            if (target == "SELECTED")
            {
                return Process.GetCurrentProcess();
            }
            if (!apps.ContainsKey(target))
            {
                return null;
            }
            return Process.GetProcessesByName(apps[target].killName).FirstOrDefault();
        }

        private Process[] GetProgramProcesses()
        {
            return Process.GetProcesses()
                .Where(p => (long)p.MainWindowHandle != 0)
                .ToArray();
        }

        private bool MoveWindowTo(string target, int position)
        {
            Process process = GetProcess(target);

            if (process == null)
                return false;

            System.Drawing.Rectangle area = Screen.PrimaryScreen.WorkingArea;
            int x = 0;
            int y = 0;
            int height = area.Height;
            int width = area.Width;

            if ((position & WP_UP) > 0)
            {
                height /= 2;
            }
            else if ((position & WP_DOWN) > 0)
            {
                height /= 2;
                y = height;
            }
            if ((position & WP_LEFT) > 0)
            {
                width /= 2;
            }
            else if ((position & WP_RIGHT) > 0)
            {
                width /= 2;
                x = width;
            }

            IntPtr handle = process.MainWindowHandle;
            Console.WriteLine(process.ProcessName + " " + process.Id + " " + process.MachineName);
            if (handle != IntPtr.Zero)
            {
                SetWindowPos(handle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_SHOWWINDOW);
                return true;
            }
            return false;
        }


        private bool SetWindowShow(string target, int value)
        {
            Process p = GetProcess(target);
            if (p != null)
            {
                ShowWindow(p.MainWindowHandle, value);
                return true;
            } else if (target == "ALL" && value == SW_SHOWMINIMIZED)
            {
                foreach (Process process in GetProgramProcesses())
                {
                    ShowWindow(process.MainWindowHandle, value);
                }
                return true;
            }
            return false;
        }
    }
}
