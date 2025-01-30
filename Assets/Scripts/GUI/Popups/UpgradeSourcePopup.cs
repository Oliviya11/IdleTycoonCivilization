using Assets.Scripts.Infrastracture.Factory;
using static Assets.Scripts.GUI.UnlockPopup;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using static Assets.Scripts.GUI.Popups.UpgradeSourcePopup;

namespace Assets.Scripts.GUI.Popups
{
    public class UpgradeSourcePopup : Popup
    {
        [SerializeField] Button upgradeButton;
        [SerializeField] List<StarUI> stars;
        [SerializeField] ProgressBar upgradeBar;
        [SerializeField] TextMeshProUGUI profit;
        [SerializeField] TextMeshProUGUI duration;
        [SerializeField] TextMeshProUGUI level;
        [SerializeField] TextMeshProUGUI buttonText;

        const string popupName = "UpgradeSourcePopup";
        Params @params;

        public class Params : IParams
        {
            public Action<UpgradeSourcePopup> OnUpgradeClick;
            public int _currentLevel;
            public int _maxLevel;
            public string _profit;
            public string _price;
            public float _duration;
            public int _maxUpgrades;
            public int _currentUpgrades;
            public Func<bool> IsUpdateAvailable;

            public Params(Action<UpgradeSourcePopup> onUpgradeClick, int currentLevel, int maxLevel, string profit, string price, float duration, int maxUpgrades, int currentUpgrades, Func<bool> isUpdateAvailable)
            {
                OnUpgradeClick = onUpgradeClick;
                _currentLevel = currentLevel;
                _maxLevel = maxLevel;
                _profit = profit;
                _price = price;
                _duration = duration;
                _maxUpgrades = maxUpgrades;
                _currentUpgrades = currentUpgrades;
                IsUpdateAvailable = isUpdateAvailable;
            }
        }

        private void Update()
        {
            if (@params.IsUpdateAvailable == null) return;

            upgradeButton.enabled = @params.IsUpdateAvailable();
        }

        public override void OnDestroy()
        {
            upgradeButton.onClick.RemoveAllListeners();
            base.OnDestroy();
        }

        public void Init(Params p)
        {
            @params = p;

            upgradeButton.onClick.AddListener(delegate ()
            {
                p.OnUpgradeClick.Invoke(this);
            });

            for (int i = 0; i < stars.Count; ++i)
            {
                stars[i].gameObject.SetActive(i < p._maxUpgrades);
                if (i < p._currentUpgrades)
                {
                    stars[i].ShowFull();
                }
                else
                {
                    stars[i].ShowEmpty();
                }
            }

            upgradeBar.SetValue(p._currentLevel, p._maxLevel);
            profit.text = p._profit;
            duration.text = p._duration.ToString();
            level.text = (p._currentLevel + 1).ToString();
            buttonText.text = p._price;
        }

        protected override string GetPrefabName()
        {
            return popupName;
        }

        public static void OpenLevelPopUp(IParams p, IGameFactory gameFactory, Vector3 at, Action<UpgradeSourcePopup> onPopupCreated)
        {
            Popup.LoadPopUp(gameFactory, UpgradeSourcePopup.popupName, delegate (Popup popUp) {
                UpgradeSourcePopup upgradePopup = popUp as UpgradeSourcePopup;
                upgradePopup.Init((Params)p);
                onPopupCreated(upgradePopup);
            }, false, at);
        }
    }
}
