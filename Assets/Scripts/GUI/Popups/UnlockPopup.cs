using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.Factory;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI
{
    public class UnlockPopup : Popup
    {
        [SerializeField] Button UnlockButton;
        [SerializeField] TextMeshProUGUI price;
        const string popupName = "UnlockPopup";

        public class Params : IParams
        {
            public Action OnUnlockClick;
            public string _price;
            public Params(Action onUnlockClick, string price)
            {
                OnUnlockClick = onUnlockClick;
                _price = price;
            }
        }

        public override void OnDestroy()
        {
            UnlockButton.onClick.RemoveAllListeners();
            base.OnDestroy();
        }

        public void Init(Params p)
        {
            UnlockButton.onClick.AddListener(delegate()
            {
                p.OnUnlockClick.Invoke();
                Hide();
            });

            price.text = p._price;
        }

        protected override string GetPrefabName()
        {
            return popupName;
        }

        public static void OpenLevelPopUp(IParams p, IGameFactory gameFactory, Vector3 at, Action<UnlockPopup> onPopupCreated)
        {
            Popup.LoadPopUp(gameFactory, UnlockPopup.popupName, delegate (Popup popUp) {
                UnlockPopup unlockPopup = popUp as UnlockPopup;
                unlockPopup.Init((Params)p);
                onPopupCreated(unlockPopup);
            }, false, at);
        }
    }
}
