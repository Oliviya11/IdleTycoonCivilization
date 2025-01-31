using Assets.Scripts.Infrastracture.Factory;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Popups
{
    public class LevelUpgradePopup : Popup
    {
        [SerializeField] Button closeButton;
        [SerializeField] RectTransform content;

        const string popupName = "LevelUpgradePopup";

        public class Params : IParams
        {
        }

        public void Init(Params p)
        {
            closeButton.onClick.AddListener(delegate
            {
                Hide();
            });
        }

        protected override string GetPrefabName()
        {
            return popupName;
        }

        public static void OpenLevelPopUp(IParams p, IGameFactory gameFactory, Vector3 at, Action<LevelUpgradePopup> onPopupCreated)
        {
            Popup.LoadPopUp(gameFactory, LevelUpgradePopup.popupName, delegate (Popup popUp) {
                LevelUpgradePopup levelUpgradePopup = popUp as LevelUpgradePopup;
                levelUpgradePopup.Init((Params)p);
                onPopupCreated(levelUpgradePopup);
            }, false, at);
        }
    }
}
