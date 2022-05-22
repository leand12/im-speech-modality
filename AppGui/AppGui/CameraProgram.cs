using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace AppGui
{
    class CameraProgram : Program
    {
        private InputSimulator input = new InputSimulator();

        /// <summary>
        /// Attempt to execute a given action.
        /// </summary>
        /// <param name="action">A string with the action parameters.</param>
        /// <returns>
        /// True if the method was able to recognize the action; otherwise, false.
        /// </returns>
        public override bool Execute(string action)
        {
            switch (action)
            {
                case "PHOTO":
                    Console.WriteLine("EXEC PHOTO");
                    return TakePhoto();
            }
            return false;
        }

        /// <summary>
        /// Open camara application.
        /// </summary>
        private bool TakePhoto()
        {
            string[] countdown = new string[] { "três", "dois", "um", "banana" };
            for (int i = 0; i < 4; i++)
            {
                MainWindow.Send(countdown[i]);
                Thread.Sleep(1000);
            }
            // press ENTER to take photo
            input.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            return true;
        }
     

    }
}
