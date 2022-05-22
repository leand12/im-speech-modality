using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGui
{
    internal class TerminalController
    {
        private int lastProcId = 0;

        public bool Execute(string action, string value, string[] shortcuts)
        {
            if (action == "OPEN")
            {
                if (value.StartsWith("SHORTCUT_"))
                {
                    int shortcutIdx = int.Parse(value.Replace("SHORTCUT_", ""));
                    Open(shortcuts[shortcutIdx - 1]);
                    return true;
                }
                else if (value == "TERMINAL")
                {
                    Open();
                    return true;
                }
            }
            else if (action == "CLOSE")
            {
                if (lastProcId != 0) Process.GetProcessById(lastProcId).Kill();
                else Process.GetProcessesByName("cmd").FirstOrDefault().Kill();
                lastProcId = 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Open terminal in the home path.
        /// </summary>
        private void Open()
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Open(home);
        }

        /// <summary>
        /// Attempt to open the terminal in a given path;
        /// </summary>
        /// <param name="folderPath">The starting vscode path.</param>
        public void Open(string path)
        {
            if (Directory.Exists(path))
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.WorkingDirectory = path;
                cmd.StartInfo.UseShellExecute = true;
                cmd.Start();
                lastProcId = cmd.Id;
            }
            else
            {
                Console.WriteLine(string.Format("{0} Directory does not exist!", path));
            }
        }
    }
}
