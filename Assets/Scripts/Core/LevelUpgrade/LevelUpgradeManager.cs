using Assets.Scripts.Core.ClientsNPCMechanics;
using Assets.Scripts.Core.Money.Services;
using Assets.Scripts.Core.Sources.Services;
using Assets.Scripts.GUI;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Sources;
using Assets.Scripts.StaticData;
using System;
using static Assets.Scripts.StaticData.LevelUpgradeStaticData;
using Product = Assets.Scripts.Sources.Product;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using Assets.Scripts.GUI.Popups;
using UnityEngine;
using Assets.Scripts.Services.PersistentProgress;

namespace Assets.Scripts.Core.LevelUpgrade
{
    public class LevelUpgradeManager
    {
        LevelUpgradeStaticData upgradeData;
        ISourcesManager sourcesManager;
        IGameFactory gameFactory;
        IMoneyManager moneyManager;
        ClientsNPCManager clientsNPCManager;
        ProducersNPCManager producersNPCManager;
        SourcesCollectionClick sourcesCollectionClick;
        List<UpgradeItem> items = new List<UpgradeItem>();
        IPersistentProgressService _persistentProgress;

        public LevelUpgradeManager(LevelUpgradeStaticData upgradeData, ISourcesManager sourcesManager, IGameFactory gameFactory,
            IMoneyManager moneyManager, ClientsNPCManager clientsNPCManager, ProducersNPCManager producersNPCManager,
            SourcesCollectionClick sourcesCollectionClick, IPersistentProgressService persistentProgress)
        {
            this.upgradeData = upgradeData;
            this.sourcesManager = sourcesManager;
            this.gameFactory = gameFactory;
            this.moneyManager = moneyManager;
            this.clientsNPCManager = clientsNPCManager;
            this.producersNPCManager = producersNPCManager;
            items = new List<UpgradeItem>(upgradeData.items);
            this.sourcesCollectionClick = sourcesCollectionClick;
            _persistentProgress = persistentProgress;
            for (int i = 0; i < upgradeData.items.Count; i++)
            {
                upgradeData.items[i].id = i;
            }
        }

        public void OpenPopup()
        {
            LevelUpgradePopup.OpenPopup(gameFactory, Vector3.zero, delegate (LevelUpgradePopup p) {
                foreach (UpgradeItem item in items)
                {
                    if (_persistentProgress.Progress.appliedLevelUpgrades != null &&
                    _persistentProgress.Progress.appliedLevelUpgrades.Contains(item.id)) continue;
                    LevelUpgradeItem upgrade = gameFactory.CreateLevelUpgradeItem(p.content);
                    upgrade.transform.SetParent(p.content, false);
                    Func<bool> isUpdateAvailable = delegate () { return IsUpgradeAvailable(item.price, item.product); };
                    LevelUpgradeItem.Params @params = new LevelUpgradeItem.Params(
                        item.description, item.title, item.price, item.sprite, delegate
                        {
                            OnUpgradeClick(item);
                            Object.Destroy(upgrade.gameObject);
                        }, isUpdateAvailable);
                    upgrade.Construct(@params);
                }
            });
        }

        void OnUpgradeClick(UpgradeItem item) {
            if (item.type == LevelUpgradeType.IncreaseClients)
            {
                clientsNPCManager.MaxClients += item.additiver;
            }
            else if (item.type == LevelUpgradeType.IncreaseProducers)
            {
                for (int i = 0; i < item.additiver; ++i) {
                    producersNPCManager.SpawnProducer();
                }
            }
            else 
            {
                Source source = sourcesManager.GetSource(item.product);
                if (item.type == LevelUpgradeType.DecreaseProductionTime)
                {
                    source.upgrade.ProductionTime /= item.multiplier;
                    sourcesCollectionClick.UpdateUpgradeSourcePopup();
                }
                else if (item.type == LevelUpgradeType.IncreaseProfit)
                {
                    BigNumber currentProfitBN = new BigNumber(source.upgrade.CurrentProfit);
                    BigNumber multiplierBN = new BigNumber(item.multiplier.ToString());
                    BigNumber result = currentProfitBN * multiplierBN;
                    source.upgrade.CurrentProfit = result.ToString();
                    sourcesCollectionClick.UpdateUpgradeSourcePopup();
                }
            }

            if (_persistentProgress.Progress.appliedLevelUpgrades == null)
            {
                _persistentProgress.Progress.appliedLevelUpgrades = new();
            }
            _persistentProgress.Progress.appliedLevelUpgrades.Add(item.id); 
        }

        bool IsUpgradeAvailable(string price, Product product)
        {
            if (!moneyManager.IsEnoughMoney(price)) return false;
            if (product != Product.None)
            {
                Source source = sourcesManager.GetSource(product);
     
                if (source != null) return true;
                return false;
            }
            return true;
        }
    }
}
