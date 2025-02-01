using Assets.Scripts.Infrastracture.Factory;
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

        public class Params : IParams
        {
            
        }

        public override void OnDestroy()
        {
            foreach (Button closeButton in closeButtons)
            {
                closeButton.onClick.RemoveAllListeners();
            }
 
            base.OnDestroy();
        }

        public void Init(Params p)
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

        public static void OpenPopup(IParams p, IGameFactory gameFactory, Vector3 at, Action<LevelUpgradePopup> onPopupCreated)
        {
            Popup.LoadPopUp(gameFactory, LevelUpgradePopup.popupName, delegate (Popup popUp) {
                LevelUpgradePopup levelUpgradePopup = popUp as LevelUpgradePopup;
                levelUpgradePopup.Init((Params)p);
                onPopupCreated(levelUpgradePopup);
            }, false, at);
        }
    }
}
