using Assets.Scripts.Infrastracture.Factory;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Colors = Assets.Scripts.GUI.Clolors;
using Assets.Scripts.Utils;
using System.Collections;

namespace Assets.Scripts.GUI.Popups
{
    public class UpgradeSourcePopup : Popup
    {
        [SerializeField] HorizontalLayoutGroup group;
        [SerializeField] Button upgradeButton;
        [SerializeField] List<StarUI> stars;
        [SerializeField] ProgressBar upgradeBar;
        [SerializeField] TextMeshProUGUI profit;
        [SerializeField] TextMeshProUGUI duration;
        [SerializeField] TextMeshProUGUI level;
        [SerializeField] TextMeshProUGUI buttonText;
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] GameObject done;

        const string popupName = "UpgradeSourcePopup";
        Params @params;

        public class Params : IParams
        {
            public readonly Action<UpgradeSourcePopup> _onUpgradeClick;
            public readonly int _currentLevel;
            public readonly int _maxLevel;
            public readonly string _profit;
            public readonly string _price;
            public readonly float _duration;
            public readonly int _maxUpgrades;
            public readonly int _currentUpgrades;
            public readonly Func<bool> IsUpdateAvailable;
            public readonly string _title;
            public readonly int _currentUpgradeLevel;
            public readonly int _maxUpgradeLevel;

            public Params(Action<UpgradeSourcePopup> onUpgradeClick, int currentLevel, int maxLevel, string profit, string price,
                float duration, int maxUpgrades, int currentUpgrades, Func<bool> isUpdateAvailable, string title, int currentUpgradeLevel, int maxUpgradeLevel)
            {
                _onUpgradeClick = onUpgradeClick;
                _currentLevel = currentLevel;
                _maxLevel = maxLevel;
                _profit = profit;
                _price = price;
                _duration = duration;
                _maxUpgrades = maxUpgrades;
                _currentUpgrades = currentUpgrades;
                IsUpdateAvailable = isUpdateAvailable;
                _title = title;
                _currentUpgradeLevel = currentUpgradeLevel;
                _maxUpgradeLevel = maxUpgradeLevel;
            }
        }

        void Start()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(group.GetComponent<RectTransform>());
            if (!group.gameObject.activeSelf) return;
            group.gameObject.SetActive(false);
            group.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (@params.IsUpdateAvailable == null) return;

            upgradeButton.enabled = @params.IsUpdateAvailable();
            upgradeButton.UpdateOnState(buttonText);
        }

        public override void OnDestroy()
        {
            upgradeButton.onClick.RemoveAllListeners();
            base.OnDestroy();
        }

        public void Init(Params p)
        {
            @params = p;

            if (p._maxUpgrades == p._currentUpgrades)
            {
                done.SetActive(true);
                upgradeButton.gameObject.SetActive(false);
                upgradeBar.gameObject.SetActive(false);
                UpgradeStars(p);
            }
            else
            {
                done.SetActive(false);
                upgradeButton.gameObject.SetActive(true);
                upgradeBar.gameObject.SetActive(true);

                upgradeButton.onClick.AddListener(delegate ()
                {
                    p._onUpgradeClick.Invoke(this);
                });

                UpgradeStars(p);

                upgradeBar.SetValue(p._currentUpgradeLevel, p._maxUpgradeLevel);
                profit.text = p._profit;
                group.gameObject.SetActive(false);
                group.gameObject.SetActive(true);
                duration.text = $"{p._duration.ToString()} s";
                level.text = $"Level {(p._currentLevel + 1).ToString()}";
                buttonText.text = p._price;
                titleText.text = p._title;
            }
        }

        private void UpgradeStars(Params p)
        {
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
        }

        protected override string GetPrefabName()
        {
            return popupName;
        }

        public static void OpenPopup(IParams p, IGameFactory gameFactory, Vector3 at, Action<UpgradeSourcePopup> onPopupCreated)
        {
            Popup.LoadPopUp(gameFactory, UpgradeSourcePopup.popupName, delegate (Popup popUp) {
                UpgradeSourcePopup upgradePopup = popUp as UpgradeSourcePopup;
                upgradePopup.Init((Params)p);
                onPopupCreated(upgradePopup);
            }, false, at);
        }
    }
}
