using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace Splicer
{
    class SoundPlayer
    {
        Dictionary<string, SoundEffectInstance> effectBank;
        Dictionary<string, Song> musicBank;
        ContentManager cm;

        public SoundPlayer(ContentManager cm)
        {
            effectBank = new Dictionary<string, SoundEffectInstance>();
            musicBank = new Dictionary<string, Song>();
            this.cm = cm;
        }

        public bool addMusic(string wavName)
        {
            if (!musicBank.Keys.Contains(wavName))
            {
                Song songToAdd = cm.Load<Song>(wavName);
                musicBank.Add(wavName, songToAdd);
                return true;
            }
            return false;
        }

        public bool addEffect(string wavName)
        {
            if (!effectBank.Keys.Contains(wavName))
            {
                SoundEffectInstance effectToAdd = cm.Load<SoundEffect>(wavName).CreateInstance();
                effectToAdd.Pan = 0;
                effectToAdd.Pitch = 0;
                effectToAdd.Volume = 0.9f;
                effectBank.Add(wavName, effectToAdd);
                return true;
            }
            return false;
        }

        public void playMusic(string wavName, bool loop)
        {
            if (musicBank.Keys.Contains(wavName))
            {
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Play(musicBank[wavName]);
            }
        }

        public void pauseMusic()
        {
            MediaPlayer.Pause();
        }

        public void resumeMusic()
        {
            MediaPlayer.Resume();
        }

        public void playEffect(string wavName, bool loop)
        {
            if (effectBank.Keys.Contains(wavName))
            {
                effectBank[wavName].Play();
            }
        }

        public void stopMusic()
        {
            MediaPlayer.Stop();
        }

        public void setMusicVolume(float volume)
        {
            volume = volume < 0 ? 0 : volume;
            volume = volume > 1 ? 1 : volume;
            MediaPlayer.Volume = volume;
        }

        public void setEffectVolume(string wavName, float volume)
        {
            volume = volume < 0 ? 0 : volume;
            volume = volume > 1 ? 1 : volume;
            if (effectBank.Keys.Contains(wavName))
            {
                effectBank[wavName].Volume = volume;
            }
        }
    }
}
