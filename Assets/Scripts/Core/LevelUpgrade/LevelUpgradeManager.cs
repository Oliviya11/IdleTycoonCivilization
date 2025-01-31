﻿using Assets.Scripts.Core.ClientsNPCMechanics;
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
using Assets.Scripts.Services;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using UnityEditorInternal.Profiling.Memory.Experimental;

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
        List<UpgradeItem> items = new List<UpgradeItem>();

        public LevelUpgradeManager(LevelUpgradeStaticData upgradeData, ISourcesManager sourcesManager, IGameFactory gameFactory,
            IMoneyManager moneyManager, ClientsNPCManager clientsNPCManager, ProducersNPCManager producersNPCManager)
        {
            this.upgradeData = upgradeData;
            this.sourcesManager = sourcesManager;
            this.gameFactory = gameFactory;
            this.moneyManager = moneyManager;
            this.clientsNPCManager = clientsNPCManager;
            this.producersNPCManager = producersNPCManager;
            items = new List<UpgradeItem>(upgradeData.items);
        }

        public void OpenPopup()
        {
            List<UpgradeItem> itemsToIterate = new List<UpgradeItem>(items);
            LevelUpgradePopup.OpenLevelPopUp(new LevelUpgradePopup.Params(), gameFactory, Vector3.zero, delegate (LevelUpgradePopup p) {
                foreach (UpgradeItem item in itemsToIterate)
                {
                    LevelUpgradeItem upgrade = gameFactory.CreateLevelUpgradeItem(p.content);
                    upgrade.transform.SetParent(p.content, false);
                    Func<bool> isUpdateAvailable = delegate () { return IsUpgradeAvailable(item.price, item.product); };
                    LevelUpgradeItem.Params @params = new LevelUpgradeItem.Params(
                        item.description, item.title, item.price, item.sprite, delegate
                        {
                            OnUpgradeClick(item);
                            Object.Destroy(upgrade.gameObject);
                            items.Remove(item);
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
                // TODO
            }
            else 
            {
                Source source = sourcesManager.GetSource(item.product);
                if (item.type == LevelUpgradeType.DecreaseProductionTime)
                {
                    source.upgrade.ProductionTime *= item.multiplier;
                }
                else if (item.type == LevelUpgradeType.IncreaseProfit)
                {
                    BigNumber currentProfitBN = new BigNumber(source.upgrade.CurrentProfit);
                    BigNumber multiplierBN = new BigNumber(item.multiplier.ToString());
                    BigNumber result = currentProfitBN * multiplierBN;
                    source.upgrade.CurrentProfit = result.ToString();
                }
            }
        }

        bool IsUpgradeAvailable(string price, Product product)
        {
            if (!moneyManager.IsEnoughMoney(price)) return false;
            if (product != Product.None)
            {
                Source source = sourcesManager.GetSource(product);
                if (source.state.ProduceProduct()) return true;
                return false;
            }
            return true;
        }
    }
}
