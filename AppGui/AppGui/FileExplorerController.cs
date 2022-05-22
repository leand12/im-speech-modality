// required in addition to other 'using necessary
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace AppGui
{
    class FileExplorerController
    {

        private Dictionary<string, string> paths = new Dictionary<string, string>()
        {
            { "PICTURES", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) },
            { "DOWNLOADS", Environment.GetFolderPath(Environment.SpecialFolder.Favorites) },
            { "DESKTOP", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) }
        };

        /// <summary>
        /// Attempt to execute a given action.
        /// </summary>
        /// <param name="args">An array with the action parameters.</param>
        /// <returns>
        /// True if the method was able to recognize the action; otherwise, false.
        /// </returns>
        public bool Execute(string action, string value, string[] shortcuts)
        {

            if (action == "OPEN")
            {

                if (value == "FILE_EXPLORER")
                {
                    OpenFolder();
                }
                else if (value.StartsWith("SHORTCUT_"))
                {
                    int shortcutIdx = int.Parse(value.Replace("SHORTCUT_", ""));
                    OpenFolder(shortcuts[shortcutIdx - 1]);
                } 
                else
                {
                    OpenFolder(paths[value]);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Open file explorer.
        /// </summary>
        private void OpenFolder()
        {
            Process.Start("explorer.exe");
        }

        /// <summary>
        /// Attempt to open the file explorer in a given path;
        /// </summary>
        /// <param name="folderPath">The starting file explorer path.</param>
        private void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };
                Process p = Process.Start(startInfo);
            }   
            else
            {
                MainWindow.Send("O caminho para o atalho não é válido.");
            }
        }
    }
    
}
