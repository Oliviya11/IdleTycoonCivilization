using Assets.Scripts.Core.Orders;
using Assets.Scripts.Infrastracture.AssetManagement;
using Assets.Scripts.Sources;
using UnityEngine;

namespace Assets.Scripts.Infrastracture.Factory
{
    public class GameFactory : IGameFactory
    {
        readonly IAssetProvider _assetProvider;

        public GameFactory(IAssetProvider assetProvider) {
            _assetProvider = assetProvider;
        }

        public SourcesCollection CreateSourcesCollection(int level)
        {
            return _assetProvider.Instantiate($"{AssetPath.SourcesCollectionPath} ({level})").GetComponent<SourcesCollection>();
        }
       
        public OrdersCollection CreateOrdersCollection(int level)
        {
            return _assetProvider.Instantiate($"{AssetPath.OrdersCollectionPath} ({level})").GetComponent<OrdersCollection>();
        }

        public GameObject CreateProducer(Vector3 at)
        {
            return _assetProvider.Instantiate(AssetPath.ProducerPath, at);
        } 

        public GameObject CreateHud()
        {
            return _assetProvider.Instantiate(AssetPath.HudPath);
        }

        public GameObject CreatePumpkin(Vector3 at)
        {
            return _assetProvider.Instantiate(AssetPath.PumpkinPath, at);
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
    }
}
