using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;


namespace AppGui
{
    class SoundController
    {
        private static CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;

        /** 
         * Sets the current volume to be equal to the provided value
         */
        public static void Set(int volume)
        {
            defaultPlaybackDevice.Volume = volume;
        }

        /** 
         * Returns the current volume value
         */
        public static double Get()
        {
            return defaultPlaybackDevice.Volume;
        }

    }
}
