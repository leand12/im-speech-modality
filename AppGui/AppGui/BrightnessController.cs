using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;


namespace AppGui
{
    class BrightnessController
    {

        /**
         * Executes a given action taking into consideration the provided args.
         */
        public bool Execute(string action, string value)
        {

            switch (action)
            {
                case "+":
                    Set(Get() + 10);
                    break;
                case "-":
                    Set(Get() - 10);
                    break;
                default:
                    return false;
            }

            return true;
        }

        /**
         * Returns the current brightness value
         */
        private int Get()
        {
            var mclass = new ManagementClass("WmiMonitorBrightness")
            {
                Scope = new ManagementScope(@"\\.\root\wmi")
            };
            var instances = mclass.GetInstances();
            foreach (ManagementObject instance in instances)
            {
                return (byte)instance.GetPropertyValue("CurrentBrightness");
            }
            return 0;
        }

        /**
         * Sets the screen brightness value. Only works for laptop.
         */
        private void Set(int brightness)
        {
            var mclass = new ManagementClass("WmiMonitorBrightnessMethods")
            {
                Scope = new ManagementScope(@"\\.\root\wmi")
            };
            var instances = mclass.GetInstances();
            var args = new object[] { 1, brightness };
            foreach (ManagementObject instance in instances)
            {
                instance.InvokeMethod("WmiSetBrightness", args);
            }
        }


    }
}
