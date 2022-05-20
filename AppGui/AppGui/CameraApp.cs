using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppGui
{
    class CameraApp
    {
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
                case "PHOTO":
                    Console.WriteLine("PHOTO");
                    return TakePhoto();
            }
            return false;
        }

        /// <summary>
        /// Open camara application.
        /// </summary>
      

        private bool TakePhoto()
        {
            // 1 .. 2 .. 3 .. cheese

            // press ENTER to take photo
            SendKeys.SendWait("~");
            return true;
        }
     

    }
}
