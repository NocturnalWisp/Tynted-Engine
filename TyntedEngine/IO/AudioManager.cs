using Tynted.SFML.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted.IO
{
    public static class AudioManager
    {
        private static Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();
        private static Dictionary<string, Music> music = new Dictionary<string, Music>();

        /// <summary>
        /// Loads an audio from a file.
        /// </summary>
        /// <param name="filePath">The file to load from.</param>
        /// <param name="audioName">Name of the audio.</param>
        /// <param name="shortTime">Set to true if you would like to hold it on ram, otherwise it streams from the file.</param>
        /// <param name="loop">Whether to loop or not.</param>
        public static void LoadAudio(string filePath, string audioName, bool shortTime)
        {
            try
            {
                if (shortTime)
                {
                    Sound s = new Sound(new SoundBuffer(filePath));
                    sounds.Add(audioName, s);
                }
                else
                {
                    Music m = new Music(filePath);
                    music.Add(audioName, m);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not play audio " + filePath + " because " + e.ToString());
            }
        }

        public static void PlayAudio(string audioName, bool loop = false)
        {
            if (sounds.ContainsKey(audioName))
            {
                sounds[audioName].Play();
                sounds[audioName].Loop = true;
            }
            else if (music.ContainsKey(audioName))
            {
                music[audioName].Play();
                music[audioName].Loop = true;
            }
            else
            {
                Console.WriteLine("The audio you selected to play does not exist.");
            }
        }

        /// <summary>
        /// Stops playback of an audio.
        /// </summary>
        /// <param name="audioName">The audio to stop.</param>
        public static void StopAudio(string audioName)
        {
            if (sounds.ContainsKey(audioName))
            {
                sounds[audioName].Stop();
            }
            else if (music.ContainsKey(audioName))
            {
                music[audioName].Stop();
            }
            else
            {
                Console.WriteLine("The audio you selected to stop does not exist.");
            }
        }

        /// <summary>
        /// Sets the volume of the specified audio.
        /// </summary>
        /// <param name="audioName">The audio to set.</param>
        /// <param name="volume">Value between 0 and 100.</param>
        public static void SetVolume(string audioName, float volume)
        {
            if (sounds.ContainsKey(audioName))
            {
                sounds[audioName].Volume = volume;
            }
            else if (music.ContainsKey(audioName))
            {
                music[audioName].Volume = volume;
            }
            else
            {
                Console.WriteLine("The audio you selected to stop does not exist.");
            }
        }

        /// <summary>
        /// Returns the current volume of the audio source.
        /// </summary>
        /// <param name="audioName">The audio to grab.</param>
        public static float GetVolume(string audioName)
        {
            if (sounds.ContainsKey(audioName))
            {
                return sounds[audioName].Volume;
            }
            else if (music.ContainsKey(audioName))
            {
                return music[audioName].Volume;
            }
            else
            {
                Console.WriteLine("The audio you selected to stop does not exist.");
                return 0;
            }
        }
    }
}
