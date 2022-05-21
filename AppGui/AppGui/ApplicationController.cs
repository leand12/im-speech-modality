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

namespace AppGui
{
    class ApplicationController
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
     

        [DllImport("user32")]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

        private Dictionary<string, Process> apps = new Dictionary<string, Process>();

        private Dictionary<string, string> names = new Dictionary<string, string>()
        {
            { "CALC", "CalculatorApp.exe" },
            { "NOTEPAD", "Notepad" },
            { "SPOTIFY", "Spotify" }
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
                    Console.WriteLine("OPEN");
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
            return false;
        }

        private bool CloseAll()
        {
            foreach (KeyValuePair<string, Process> entry in apps)
            {
                entry.Value.Kill();
            }
            apps.Clear();

            return true;
        }

        private bool Open(string target)
        {
            if (names.ContainsKey(target))
            {
                apps.Add(target, Process.Start(names[target]));
                return true;
            }
            return false;
        }

        private bool Close(string target)
        {
            if (apps.ContainsKey(target))
            {
                apps[target].Kill();
                apps.Remove(target);
                return true;
            }
            else if (target == "SELECTED")
            {
                Process.GetCurrentProcess().Kill();
                return true;
            }
            else if (target == "ALL")
                return CloseAll();

            return false;
        }

        private bool Show(string target)
        {
            Process process = null;
            if (apps.ContainsKey(target))
                process = apps[target];
            else
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

        private Process GetProcess(String name)
        {
            switch (name)
            {
                case "CALC":
                    return Process.GetProcessesByName("calculator").FirstOrDefault();
                case "FILE_EXPLORER":
                    return Process.GetProcessesByName("file_explorer").FirstOrDefault();
                case "NOTEPAD":
                    return Process.GetProcessesByName("Notepad").FirstOrDefault();
                case "SPOTIFY":
                    return Process.GetProcessesByName("Spotify").FirstOrDefault();
            }
            //Process process = Process.GetProcessesByName("Notepad").FirstOrDefault();

            return null;
        }

        private bool MoveWindowTo(string target, int position)
        {
            Process process = null;
            if (apps.ContainsKey(target))
            {
                process = apps[target];
            }
            else if (target == "SELECTED")
            {
                process = Process.GetCurrentProcess();
            }

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
            if (apps.ContainsKey(target))
            {
                ShowWindow(apps[target].MainWindowHandle, value);
                return true;
            } else if (target == "SELECTED")
            {
                ShowWindow(Process.GetCurrentProcess().MainWindowHandle, value);
                return true;
            } else if (target == "ALL" && value == SW_SHOWMINIMIZED)
            {
                foreach (KeyValuePair<string, Process> entry in apps)
                {
                    ShowWindow(entry.Value.MainWindowHandle, value);
                }
                return true;
            }

            return false;
        }
    }
}
