// required in addition to other 'using necessary
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace AppGui
{
    class FileSystemController
    {

        private Dictionary<string, string> paths = new Dictionary<string, string>()
        {
            { "PICTURES", @"C:\Users\leand\OneDrive\Imagens" },
            { "DOWNLOADS", @"C:\Users\leand\Downloads" },
            { "DESKTOP", @"C:\Users\leand\Desktop" }
        };

        /// <summary>
        /// Attempt to execute a given action.
        /// </summary>
        /// <param name="args">An array with the action parameters.</param>
        /// <returns>
        /// True if the method was able to recognize the action; otherwise, false.
        /// </returns>
        public bool Execute(string[] args)
        {
            switch (args[0])
            {
                case "OPEN":
                    switch (args[1])
                    {
                        case "FILE_EXPLORER":
                            OpenFolder();
                            break;
                        default:
                            OpenFolder(paths[args[1]]);
                            break;
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
                Process.Start(startInfo);
            }   
            else
            {
                Console.WriteLine(string.Format("{0} Directory does not exist!", folderPath));
            }
        }
    }
    
}
