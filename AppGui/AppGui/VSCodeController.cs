using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppGui
{
    internal class VSCodeController
    {
        public bool Execute(string action, string value, string[] shortcuts)
        {
            if (action == "OPEN")
            {
                if (value.StartsWith("SHORTCUT_"))
                {
                    int shortcutIdx = int.Parse(value.Replace("SHORTCUT_", ""));
                    string path = shortcuts[shortcutIdx - 1];
                    if (Directory.Exists(path))
                    {
                        Open(path);
                        return true;
                    }
                    MainWindow.Send("O caminho para o atalho não é válido.");
                    return false;
                }
                else if (value == "VSCODE")
                {
                    Open();
                    return true;
                }
            }
            else if (action == "CLOSE")
            {
                Process[] processes = Process.GetProcessesByName("Code");
                foreach (Process proc in processes)
                {
                    proc.Kill();
                }
                //foreach (Process proc in Process.GetProcesses())
                //{
                //    if (proc.MainWindowHandle != IntPtr.Zero && proc.MainWindowTitle == @"C:\WINDOWS\system32\cmd.exe")
                //    {
                //        proc.Kill();
                //    }
                //}
            }
            return false;
        }

        public void Open()
        {
            Process.Start("Code");
        }

        public void Open(string filePath)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = "Code";
            process.StartInfo.Arguments = filePath;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            // hack to close cmd that opens vscode
            //Thread.Sleep(2000);
            //foreach (Process proc in Process.GetProcesses())
            //{
            //    if (proc.MainWindowHandle != IntPtr.Zero && proc.MainWindowTitle == @"C:\WINDOWS\system32\cmd.exe")
            //    {
            //        proc.Kill();
            //        break;
            //    }
            //}
        }
    }
}
