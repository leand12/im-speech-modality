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
        private CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;

        /**
         * Executes a given action taking into consideration the provided args.
         */
        public bool Execute(string action, string value)
        {
            switch (action)
            {
                case "+":
                    Set((int)Get() + 10);
                    break;
                case "-":
                    Set((int)Get() - 10);
                    break;
                case ".":
                    Set(int.Parse(value));
                    break;
                default:
                    return false;
            }
            return true;
        }

        /** 
         * Sets the current volume to be equal to the provided value
         */
        private void Set(int volume)
        {
            defaultPlaybackDevice.Volume = volume;
            // if the volume is increased but the speaker is muted, unmute
            if (volume > 0 && defaultPlaybackDevice.IsMuted)
                defaultPlaybackDevice.ToggleMute();
        }

        /** 
         * Returns the current volume value
         */
        private double Get()
        {
            return defaultPlaybackDevice.Volume;
        }

    }
}
