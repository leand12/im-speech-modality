using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;

public class WindowController
{

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
	

    /* Consts */
    private const int SWP_NOSIZE = 0x0001;
    private const int SWP_NOZORDER = 0x0004;
    private const int SWP_SHOWWINDOW = 0x0040;

    private const int WP_UP = 0b0001;
    private const int WP_DOWN = 0b0010;
    private const int WP_LEFT = 0b0100;
    private const int WP_RIGHT = 0b1000;


    public bool Execute(string[] args)
    {
        if (args.Length < 2)
            return false;

        return MoveWindowTo(GetProcess(args[0]), GetPosition(args[1]));
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
