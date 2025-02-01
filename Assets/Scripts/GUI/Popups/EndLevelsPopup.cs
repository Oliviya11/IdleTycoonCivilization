using Assets.Scripts.Infrastracture.Factory;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Popups
{
    public class EndLevelsPopup : Popup
    {
        [SerializeField] List<Button> closeButtons;

        const string popupName = "EndLevelsPopup";

        public override void OnDestroy()
        {
            foreach (var button in closeButtons)
            {
                button.onClick.RemoveAllListeners();
            }
            base.OnDestroy();
        }

        public void Init()
        {
            foreach (var button in closeButtons)
            {
                button.onClick.AddListener(delegate ()
                {
                    Hide();
                });
            }
        }

        protected override string GetPrefabName()
        {
            return popupName;
        }

        public static void OpenPopup(IGameFactory gameFactory, Vector3 at)
        {
            Popup.LoadPopUp(gameFactory, EndLevelsPopup.popupName, delegate (Popup popup) {
                ((EndLevelsPopup) popup).Init();
            }, false, at);
        }
    }
}
