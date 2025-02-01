using Assets.Scripts.Infrastracture.Factory;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Colors = Assets.Scripts.GUI.Clolors;
using Assets.Scripts.Utils;

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
            public Action<UpgradeSourcePopup> OnUpgradeClick;
            public int currentLevel;
            public int maxLevel;
            public string profit;
            public string price;
            public float duration;
            public int maxUpgrades;
            public int currentUpgrades;
            public Func<bool> IsUpdateAvailable;
            public string title;
            public int currentUpgradeLevel;
            public int maxUpgradeLevel;

            public Params(Action<UpgradeSourcePopup> onUpgradeClick, int currentLevel, int maxLevel, string profit, string price,
                float duration, int maxUpgrades, int currentUpgrades, Func<bool> isUpdateAvailable, string title, int currentUpgradeLevel, int maxUpgradeLevel)
            {
                OnUpgradeClick = onUpgradeClick;
                this.currentLevel = currentLevel;
                this.maxLevel = maxLevel;
                this.profit = profit;
                this.price = price;
                this.duration = duration;
                this.maxUpgrades = maxUpgrades;
                this.currentUpgrades = currentUpgrades;
                IsUpdateAvailable = isUpdateAvailable;
                this.title = title;
                this.currentUpgradeLevel = currentUpgradeLevel;
                this.maxUpgradeLevel = maxUpgradeLevel;
            }
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

            if (p.maxUpgrades == p.currentUpgrades)
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
                    p.OnUpgradeClick.Invoke(this);
                });

                UpgradeStars(p);

                upgradeBar.SetValue(p.currentUpgradeLevel, p.maxUpgradeLevel);
                profit.text = p.profit;
                duration.text = $"{p.duration.ToString()} s";
                level.text = $"Level {(p.currentLevel + 1).ToString()}";
                buttonText.text = p.price;
                titleText.text = p.title;
            }
        }

        private void UpgradeStars(Params p)
        {
            for (int i = 0; i < stars.Count; ++i)
            {
                stars[i].gameObject.SetActive(i < p.maxUpgrades);
                if (i < p.currentUpgrades)
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
