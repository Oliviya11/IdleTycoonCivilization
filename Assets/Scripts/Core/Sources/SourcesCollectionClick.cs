﻿using Assets.Scripts.Core.Booster.Service;
using Assets.Scripts.Core.Money.Services;
using Assets.Scripts.Core.Sources;
using Assets.Scripts.Core.Sources.Services;
using Assets.Scripts.GUI;
using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Inputs;
using System;
using UnityEngine;
using BigNumber = Assets.Scripts.Core.BigNumber;

namespace Assets.Scripts.Sources
{
    public class SourcesCollectionClick : MonoBehaviour
    {
        [SerializeField] SourcesCollection sourcesCollection;
        int _lastClickedSourceId;
        Popup _lastPopup;
        Source _lastSource;
        AllServices _services;
        ClickId _clickId;

        enum ClickId
        {
            None,
            Unlock,
            Upgrade,
        }

        void Start()
        {
            foreach (var source in sourcesCollection.sources)
            {
                source.click.OnBlankClick += OpenUnlockPopup;
                source.click.OnSourceIconClick += OpenSourceUpgradePopUp;
                foreach (SourceElementClick sourceElementClick in source.sourceElementClicks)
                {
                    sourceElementClick.OnSourceElementClick += OpenSourceUpgradePopUp;
                }
            }

            _services.Single<IInputService>().OnClick += OnMapClick;
        }

        void OnDestroy()
        {
            foreach (var source in sourcesCollection.sources)
            {
                source.click.OnBlankClick -= OpenUnlockPopup;
                source.click.OnSourceIconClick -= OpenSourceUpgradePopUp;
                foreach (SourceElementClick sourceElementClick in source.sourceElementClicks)
                {
                    sourceElementClick.OnSourceElementClick -= OpenSourceUpgradePopUp;
                }
            }

            _services.Single<IInputService>().OnClick -= OnMapClick;
        }

        public void Construct(AllServices services)
        {
            _services = services;
        }

        public void UpdateUpgradeSourcePopup()
        {
            if (_lastPopup == null || _lastSource == null) return;

            UpgradeSourcePopup upgradeSourcePopup = (UpgradeSourcePopup) _lastPopup;
            if (upgradeSourcePopup != null) {
                upgradeSourcePopup.Init(GetUpgradeSourcePopup(_lastSource.upgrade, true));
            }
        }

        void OpenUnlockPopup(Source source)
        {
            HidePopup();

            _lastSource = source;

            if (source.gameObject.GetInstanceID() == _lastClickedSourceId && _clickId == ClickId.Unlock)
            {
                _lastClickedSourceId = 0;
                return;
            }

            _clickId = ClickId.Unlock;
            _lastClickedSourceId = source.gameObject.GetInstanceID();
            Vector3 position = source.transform.position;
            position.y += 2;

            Func<bool> isUpdateAvailable = delegate () { return IsUpdateAvailable(source.upgrade); };

            UnlockPopup.Params @params = new UnlockPopup.Params(delegate ()
            {
                OpenSource(source);
                source.upgrade.SubtractMoney();
            }, source.upgrade.CurrentPrice, isUpdateAvailable);
            UnlockPopup.OpenPopp(@params, _services.Single<IGameFactory>(), position,
            delegate (UnlockPopup p)
            {
                _lastPopup = p;
            });
        }

        void OpenSourceUpgradePopUp(Source source)
        {
            HidePopup();

            _lastSource = source;

            if (source.gameObject.GetInstanceID() == _lastClickedSourceId && _clickId == ClickId.Upgrade)
            {
                _lastClickedSourceId = 0;
                return;
            }

            _clickId = ClickId.Upgrade;

            _lastClickedSourceId = source.gameObject.GetInstanceID();
            Vector3 position = source.transform.position;
            position.y += 2;

            SourceUpgrade u = source.upgrade;

            UpgradeSourcePopup.Params @params = GetUpgradeSourcePopup(u, false);

            UpgradeSourcePopup.OpenPopup(@params, _services.Single<IGameFactory>(), position,
            delegate (UpgradeSourcePopup p)
            {
                _lastPopup = p;
            });
        }

        private UpgradeSourcePopup.Params GetUpgradeSourcePopup(SourceUpgrade u, bool isUpgradeNone)
        {
            Func<bool> isUpdateAvailable = delegate () { return IsUpdateAvailable(u); };

            Action<UpgradeSourcePopup> onUpgrade = (UpgradeSourcePopup popup) =>
            {

            };

            if (!isUpgradeNone)
            {
                onUpgrade = (UpgradeSourcePopup popup) =>
                {
                    u.Upgrade();
                    popup.Init(GetUpgradeSourcePopup(u, true));
                    sourcesCollection.levelProgress.Update();
                    AddBooster(u);
                };
            }

            return new UpgradeSourcePopup.Params(onUpgrade, u.CurrentLevel, u.MaxLevels(), u.CurrentProfit, u.CurrentPrice,
                u.ProductionTime, u.MaxUpgrades, u.CurrentUpgrade, isUpdateAvailable, u.Product.ToString(),
                u.CurrentUpgradeLevel, u.GetUpgradeLevels());
        }

        private void AddBooster(SourceUpgrade u)
        {
            if (u.CurrentUpgradeLevel == 0)
            {
                _services.Single<IBoosterManager>().AddBooster();
            }
        }

        bool IsUpdateAvailable(SourceUpgrade upgradeSource)
        {
            return _services.Single<IMoneyManager>().IsEnoughMoney(upgradeSource.CurrentPrice);
        }

        void OpenSource(Source source)
        {
            SetProductState(source);
            _lastPopup = null;
            _services.Single<ISourcesManager>().OpenSource(source.state.Product, source);
        }

        void HidePopup()
        {
            if (_lastPopup != null)
            {
                _lastPopup.Hide();
                _lastPopup = null;
            }
        }

        void OnMapClick(Vector2 v, string name)
        {
            _lastClickedSourceId = 0;
            HidePopup();
        }

        void SetProductState(Source source)
        {
            source.state.EnableAccordingToState(SourceState.State.ProductPlace1);
        }
    }
}
