using Assets.Scripts.Core.Orders;
using Assets.Scripts.GUI;
using Assets.Scripts.Infrastracture.AssetManagement;
using Assets.Scripts.Services.PersistentProgress;
using Assets.Scripts.Sources;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Infrastracture.Factory
{
    public class GameFactory : IGameFactory
    {
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        readonly IAssetProvider _assetProvider;

        public GameFactory(IAssetProvider assetProvider) {
            _assetProvider = assetProvider;
        }

        public SourcesCollection CreateSourcesCollection(int level)
        {
            GameObject go = _assetProvider.Instantiate($"{AssetPath.SourcesCollectionPath} {level}");
            RegisterProgressWatchers(go);
            return go.GetComponent<SourcesCollection>();
        }
       
        public OrdersCollection CreateOrdersCollection(int level)
        {
            return _assetProvider.Instantiate($"{AssetPath.OrdersCollectionPath} {level}").GetComponent<OrdersCollection>();
        }

        public GameObject CreateProducer(Vector3 at)
        {
            return _assetProvider.Instantiate(AssetPath.ProducerPath, at);
        }

        public GameObject CreateClient(Vector3 at)
        {
            return _assetProvider.Instantiate(AssetPath.ClientPath, at);
        }

        public GameObject CreateClientsManager()
        {
            GameObject go = _assetProvider.Instantiate(AssetPath.ClientsManagerPath);
            RegisterProgressWatchers(go);
            return go;
        }

        public GameObject CreateProducersManager()
        {
            GameObject go = _assetProvider.Instantiate(AssetPath.ProducersManagerPath);
            RegisterProgressWatchers(go);
            return go;
        }

        public GameObject CreateHud()
        {
            return _assetProvider.Instantiate(AssetPath.HudPath);
        }

        public LevelUpgradeItem CreateLevelUpgradeItem(Transform parent)
        {
            return _assetProvider.Instantiate(AssetPath.LevelUpgradeItemPath).GetComponent<LevelUpgradeItem>();
        }

        public GameObject CreatePumpkin(Vector3 at)
        {
            return _assetProvider.Instantiate(AssetPath.PumpkinPath, at);
        }

        public GameObject CreateChicken(Vector3 at, Quaternion rotation)
        {
            return _assetProvider.Instantiate(AssetPath.ChickenPath, at, rotation);
        }

        public GameObject CreateProducer(Vector3 at, float angle)
        {
            return _assetProvider.Instantiate(AssetPath.ProducerPath, at, angle);
        }

        public GameObject CreatePopUp(string name)
        {
            return _assetProvider.Instantiate($"{AssetPath.PopUpPath}{name}");
        }

        public GameObject CreatePopUp(string name, Vector3 at)
        {
            return _assetProvider.Instantiate($"{AssetPath.PopUpPath}{name}", at);
        }

        public GameObject CreateEgg(Vector3 at)
        {
            return _assetProvider.Instantiate(AssetPath.EggPath, at);
        }

        public GameObject CreateTomato1(Vector3 at)
        {
            return _assetProvider.Instantiate(AssetPath.Tomato1Path, at);
        }

        public GameObject CreateTomato2(Vector3 at)
        {
            return _assetProvider.Instantiate(AssetPath.Tomato2Path, at);
        }

        void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }

        void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public BoosterItem CreateBoosterItem(Transform parent)
        {
            return _assetProvider.Instantiate(AssetPath.BoosterItemPath).GetComponent<BoosterItem>();
        }
    }
}
