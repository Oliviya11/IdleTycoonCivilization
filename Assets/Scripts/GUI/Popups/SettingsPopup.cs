using Assets.Scripts.Infrastracture.Factory;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Popups
{
    public class SettingsPopup : Popup
    {
        [SerializeField] Button soundButton;
        [SerializeField] Button musicButton;
        [SerializeField] Button noSoundButton;
        [SerializeField] Button noMusicButton;
        [SerializeField] List<Button> closeButtons;

        const string popupName = "SettingsPopup";

        public class Params : IParams
        {
            public readonly UnityAction _onSoundClick;
            public readonly UnityAction _onMusicClick;
            public readonly UnityAction _onNoSoundClick;
            public readonly UnityAction _onNoMusicClick;
            public readonly bool _isSound;
            public readonly bool _isMusic;

            public Params(UnityAction onSoundClick, UnityAction onMusicClick, UnityAction onNoSoundClick, UnityAction onNoMusicClick, bool isSound, bool isMusic)
            {
                _onSoundClick = onSoundClick;
                _onMusicClick = onMusicClick;
                _onNoSoundClick = onNoSoundClick;
                _onNoMusicClick = onNoMusicClick;
                _isSound = isSound;
                _isMusic = isMusic;
            }
        }

        public override void OnDestroy()
        {
            foreach (var button in closeButtons)
            {
                button.onClick.RemoveAllListeners();
            }

            soundButton.onClick.RemoveAllListeners();
            musicButton.onClick.RemoveAllListeners();
            noSoundButton.onClick.RemoveAllListeners();
            noMusicButton.onClick.RemoveAllListeners();

            base.OnDestroy();
        }

        public void Init(Params p)
        {
            foreach (var button in closeButtons)
            {
                button.onClick.AddListener(delegate
                {
                    Hide();
                });
            }

            soundButton.onClick.AddListener(delegate {
                p._onSoundClick();
                noSoundButton.gameObject.SetActive(true);
                soundButton.gameObject.SetActive(false);
            });

            noSoundButton.onClick.AddListener(delegate {
                p._onNoSoundClick();
                soundButton.gameObject.SetActive(true);
                noSoundButton.gameObject.SetActive(false);
            });

            if (p._isSound)
            {
                noSoundButton.gameObject.SetActive(false);
                soundButton.gameObject.SetActive(true);
            }
            else
            {
                soundButton.gameObject.SetActive(false);
                noSoundButton.gameObject.SetActive(true);
            }

            musicButton.onClick.AddListener(delegate {
                p._onMusicClick();
                noMusicButton.gameObject.SetActive(true);
                musicButton.gameObject.SetActive(false);
            });
            noMusicButton.onClick.AddListener(delegate
            {
                p._onNoMusicClick();
                musicButton.gameObject.SetActive(true);
                noMusicButton.gameObject.SetActive(false);
            });

            if (p._isMusic)
            {
                noMusicButton.gameObject.SetActive(false);
                musicButton.gameObject.SetActive(true);
            }
            else {
                
                musicButton.gameObject.SetActive(false);
                noMusicButton.gameObject.SetActive(true);
            }
        }

        protected override string GetPrefabName()
        {
            return popupName;
        }

        public static void OpenPopup(IParams p, IGameFactory gameFactory, Vector3 at)
        {
            Popup.LoadPopUp(gameFactory, SettingsPopup.popupName, delegate (Popup popUp) {
                SettingsPopup settingsPopup = popUp as SettingsPopup;
                settingsPopup.Init((Params)p);
            }, false, at);
        }
    }
}
