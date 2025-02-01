using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Services.Audio
{
    public class AudioManager : IAudioManager
    {
        [Serializable]
        public class Settings
        {
            public AudioClip coinsAdded;
            public AudioClip popupOpened;
            public AudioSource audioSource;
            public AudioMixer audioMixer;
        }

        Settings _settings;
        const string MUSIC = "MusicVolume";
        const string EFFECTS = "EffectsVolume";

        public AudioManager(Settings sounds)
        {
            _settings = sounds;
        }

        public void PlayCoinsSound()
        {
            PlaySound(_settings.coinsAdded);
        }

        public void PlayPopupOpened()
        {
            PlaySound(_settings.popupOpened);
        }

        void PlaySound(AudioClip clip)
        {
            _settings.audioSource.PlayOneShot(clip);
        }

        void Set(string name, float value)
        {
            bool isSet = _settings.audioMixer.SetFloat(name, Mathf.Log10(value) * 20);
            if (!isSet)
            {
                Debug.LogError(name + " was not set (SoundManager AudioMixer)");
            }
        }

        void Mute(string name)
        {
            Set(name, 0.0001f);
        }

        void Unmute(string name)
        {
            Set(name, 1);
        }

        public void MuteBackgroundMusic()
        {
            Mute(MUSIC);
        }

        public void UnmuteBackgroundMusic()
        {
            Unmute(MUSIC);
        }

        public void MuteEffects()
        {
            Mute(EFFECTS);
        }

        public void UnmuteEffects()
        {
            Unmute(EFFECTS);
        }
    }
}
