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
            public Action OnUnlockClick;
            public string price;
            public Func<bool> IsUpdateAvailable;
            public Params(Action onUnlockClick, string price, Func<bool> isUpdateAvailable)
            {
                OnUnlockClick = onUnlockClick;
                this.price = price;
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
                p.OnUnlockClick.Invoke();
                Hide();
            });

            price.text = p.price;
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
