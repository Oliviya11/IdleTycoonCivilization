﻿using Assets.Scripts.Infrastracture.Factory;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Popups
{
    public class LevelUpgradePopup : Popup
    {
        [SerializeField] List<Button> closeButtons;
        public RectTransform content;

        const string popupName = "LevelUpgradePopup";

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

        public static void OpenPopup(IGameFactory gameFactory, Vector3 at, Action<LevelUpgradePopup> onPopupCreated)
        {
            Popup.LoadPopUp(gameFactory, LevelUpgradePopup.popupName, delegate (Popup popUp) {
                LevelUpgradePopup levelUpgradePopup = popUp as LevelUpgradePopup;
                levelUpgradePopup.Init();
                onPopupCreated(levelUpgradePopup);
            }, false, at);
        }
    }
}
