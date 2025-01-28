using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.Factory;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI
{
    public class UnlockPopup : Popup
    {
        [SerializeField] Button Unlock;
        const string popupName = "UnlockPopup";

        public class Params : IParams
        {
            public Action OnUnlockClick;
            public Params(Action onUnlockClick)
            {
                OnUnlockClick = onUnlockClick;
            }
        }

        public override void OnDestroy()
        {
            Unlock.onClick.RemoveAllListeners();
            base.OnDestroy();
        }

        public void Init(Params p)
        {
            Unlock.onClick.AddListener(delegate()
            {
                p.OnUnlockClick.Invoke();
                Hide();
            });
        }

        protected override string GetPrefabName()
        {
            return "UnlockPopup";
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
