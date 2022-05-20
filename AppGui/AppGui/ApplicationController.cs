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

        private Dictionary<string, Process> apps = new Dictionary<string, Process>();

        /* Consts */
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
                case "Move":
                    return MoveWindowTo(GetProcess(target), GetPosition(action));
            }
            return false;
        }

        private bool Open(string target)
        {
            switch (target)
            {
                case "CALC":
                    apps.Add(target, Process.Start("calculator"));
                    break;
                case "FILE_EXPLORER":
                    apps.Add(target, Process.Start("file_explorer"));
                    break;
                case "NOTEPAD":
                    apps.Add(target, Process.Start("Notepad"));
                    break;
                case "SPOTIFY":
                    apps.Add(target, Process.Start("Spotify"));
                    break;
                case "CAMERA":
                    apps.Add(target, Process.Start("microsoft.windows.camera:"));
                    break;
                default:
                    return false;
            }
            return true;
        }

        private bool Close(string target)
        {
            Process p = null;
            switch (target)
            {
                case "LAST":
                    p = Process.GetCurrentProcess();
:                   break;
                case "CALC":
                    p = FindProcess("calculator");
                    break;
                case "FILE_EXPLORER":
                    p = FindProcess("file_explorer");
                    break;
                case "NOTEPAD":
                    p = FindProcess("Notepad");
                    break;
                case "SPOTIFY":
                    p = FindProcess("Spotify");
                    break;
                case "CAMERA":
                    p = FindProcess("microsoft.windows.camera:");
                    break;
                default:
                    return false;
            }
            if (p == null)
            {
                return false;
            }
            p.Kill();
            return true;
        }

        private Process FindProcess(string target)
        {
            if (apps.ContainsKey(target))
                return apps.TryGetValue(target);
            else:


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

        private bool MoveWindowTo(Process process, int position)
        {

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

    }
}
