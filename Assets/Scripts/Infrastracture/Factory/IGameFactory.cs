﻿using Assets.Scripts.Core.Orders;
using Assets.Scripts.GUI;
using Assets.Scripts.Services;
using Assets.Scripts.Sources;
using UnityEngine;

namespace Assets.Scripts.Infrastracture.Factory
{
    public interface IGameFactory : IService
    {
        SourcesCollection CreateSourcesCollection(int level);

        OrdersCollection CreateOrdersCollection(int level);

        GameObject CreateProducer(Vector3 at, float angle);

        GameObject CreateClient(Vector3 at);

        GameObject CreateClientsManager();

        GameObject CreateProducersManager();

        GameObject CreateHud();

        LevelUpgradeItem CreateLevelUpgradeItem(Transform parent);

        GameObject CreatePumpkin(Vector3 at);

        GameObject CreateChicken(Vector3 at, Quaternion rotation);

        GameObject CreateEgg(Vector3 at);

        GameObject CreatePopUp(string name);

        GameObject CreatePopUp(string name, Vector3 at);
    }
}
