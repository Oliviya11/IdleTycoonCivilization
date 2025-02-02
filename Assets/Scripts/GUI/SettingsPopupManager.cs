using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services.Audio;
using Assets.Scripts.Services.PersistentProgress;
using UnityEngine;

namespace Assets.Scripts.GUI
{
    public class SettingsPopupManager
    {
        readonly IAudioManager _audioManager;
        readonly IPersistentProgressService _persistentProgress;
        readonly IGameFactory _gameFactory;

        public SettingsPopupManager(IAudioManager audioManager, IPersistentProgressService persistentProgress, IGameFactory gameFactory) {
            _audioManager = audioManager;
            _persistentProgress = persistentProgress;
            _gameFactory = gameFactory;
        }

        public void OpenPopup()
        {
            SettingsPopup.Params p = new(
                delegate
                {
                    _audioManager.MuteEffects();
                    _persistentProgress.Progress.sound = false;
                },
                delegate
                {
                    _audioManager.MuteBackgroundMusic();
                    _persistentProgress.Progress.music = false;
                },
                delegate
                {
                    _audioManager.UnmuteEffects();
                    _persistentProgress.Progress.sound = true;
                },
                delegate
                {
                    _audioManager.UnmuteBackgroundMusic();
                    _persistentProgress.Progress.music = true;
                },
                _persistentProgress.Progress.sound,
                _persistentProgress.Progress.music
            );

            SettingsPopup.OpenPopup(p, _gameFactory, Vector3.zero);
        }
    }
}
