using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Colors = Assets.Scripts.GUI.Clolors;

namespace Assets.Scripts.GUI
{
    public class UnlockPopup : Popup
    {
        [SerializeField] HorizontalLayoutGroup group;
        [SerializeField] Button unlockButton;
        [SerializeField] TextMeshProUGUI price;
        const string popupName = "UnlockPopup";

        public class Params : IParams
        {
            public readonly Action _onUnlockClick;
            public readonly string _price;
            public readonly Func<bool> IsUpdateAvailable;
            public Params(Action onUnlockClick, string price, Func<bool> isUpdateAvailable)
            {
                _onUnlockClick = onUnlockClick;
                _price = price;
                IsUpdateAvailable = isUpdateAvailable;
            }
        }

        Params @params;

        public override void OnDestroy()
        {
            unlockButton.onClick.RemoveAllListeners();
            base.OnDestroy();
        }

        void Start()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(group.GetComponent<RectTransform>());
            group.gameObject.SetActive(false);
            group.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (@params.IsUpdateAvailable == null) return;

            unlockButton.enabled = @params.IsUpdateAvailable();
            unlockButton.UpdateOnState(price);
        }

        public void Init(Params p)
        {
            @params = p;

            unlockButton.onClick.AddListener(delegate()
            {
                p._onUnlockClick.Invoke();
                Hide();
            });

            price.text = p._price;
        }

        protected override string GetPrefabName()
        {
            return popupName;
        }

        public static void OpenPopp(IParams p, IGameFactory gameFactory, Vector3 at, Action<UnlockPopup> onPopupCreated)
        {
            Popup.LoadPopUp(gameFactory, UnlockPopup.popupName, delegate (Popup popUp) {
                UnlockPopup unlockPopup = popUp as UnlockPopup;
                unlockPopup.Init((Params)p);
                onPopupCreated(unlockPopup);
            }, false, at);
        }
    }
}
