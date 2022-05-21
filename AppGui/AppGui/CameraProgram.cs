using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace AppGui
{
    class CameraProgram : Program
    {
        public const string ProgramName = "CAMERA";
        private InputSimulator input = new InputSimulator();

        /// <summary>
        /// Attempt to execute a given action.
        /// </summary>
        /// <param name="args">An array with the action parameters.</param>
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
            // 1 .. 2 .. 3 .. cheese

            // press ENTER to take photo
            input.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            return true;
        }
     

    }
}
