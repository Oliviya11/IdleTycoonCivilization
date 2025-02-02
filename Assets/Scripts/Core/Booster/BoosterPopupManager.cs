using Assets.Scripts.Core.Booster.Service;
using Assets.Scripts.Core.Money.Services;
using Assets.Scripts.GUI;
using Assets.Scripts.GUI.Popups;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.StaticData;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Booster
{
    public class BoosterPopupManager
    {
        readonly IBoosterManager _boosterManager;
        readonly IGameFactory _gameFactory;

        public BoosterPopupManager(IBoosterManager boosterManager, IGameFactory gameFactory) {
            _boosterManager = boosterManager;
            _gameFactory = gameFactory;
    }

        public void OpenPoup()
        {
            BoosterPopup.OpenPopup(_gameFactory, Vector3.zero, delegate (BoosterPopup popup)
            {
                foreach(KeyValuePair<Booster, int> pair in _boosterManager.boosterToNumber)
                {
                    if (pair.Value > 0)
                    {
                        BoosterItem boosterItem = _gameFactory.CreateBoosterItem(popup.content);
                        boosterItem.transform.SetParent(popup.content, false);
                        BoosterStaticData boosterStaticData = _boosterManager.boostersStaticData[pair.Key];
                        BoosterItem.Params @params = new BoosterItem.Params(delegate ()
                        {
                            if (pair.Value == 1) Object.Destroy(boosterItem);
                            _boosterManager.ActivateBooster(pair.Key);  
                        }, boosterStaticData.icon, boosterStaticData.description, pair.Value);
                        boosterItem.Construct(@params);
                    }
                }
            });
        }
    }
}
