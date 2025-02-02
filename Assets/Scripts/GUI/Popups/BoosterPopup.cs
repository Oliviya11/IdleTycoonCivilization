using Assets.Scripts.Infrastracture.Factory;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Popups
{
    public class BoosterPopup : Popup
    {
        [SerializeField] List<Button> closeButtons;
        public RectTransform content;

        const string popupName = "BoosterPopup";

        public override void OnDestroy()
        {
            foreach (Button closeButton in closeButtons)
            {
                closeButton.onClick.RemoveAllListeners();
            }

            base.OnDestroy();
        }

        public void Init()
        {
            foreach (Button closeButton in closeButtons)
            {
                closeButton.onClick.AddListener(delegate
                {
                    Hide();
                });
            }
        }

        protected override string GetPrefabName()
        {
            return popupName;
        }

        public static void OpenPopup(IGameFactory gameFactory, Vector3 at, Action<BoosterPopup> onPopupCreated)
        {
            Popup.LoadPopUp(gameFactory, BoosterPopup.popupName, delegate(Popup popUp) {
                BoosterPopup boosterPopup = popUp as BoosterPopup;
                boosterPopup.Init();
                onPopupCreated(boosterPopup);
            }, false, at);
        }
    }
}
