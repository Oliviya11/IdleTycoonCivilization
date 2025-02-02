using Assets.Scripts.Core.Money.Services;
using Assets.Scripts.Core.Sources.Services;
using Assets.Scripts.Data;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services;
using Assets.Scripts.Services.PersistentProgress;
using Assets.Scripts.Services.Audio;
using Assets.Scripts.Sources;
using Assets.Scripts.StaticData;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;
using Product = Assets.Scripts.Sources.Product;
using Assets.Scripts.Core.Booster.Service;
using BoosterType = Assets.Scripts.Core.Booster.Booster;

namespace Assets.Scripts.Core.ClientsNPCMechanics
{
    public class ProducersNPCManager : MonoBehaviour, ISavedProgress
    {
        float _producersSpeed = 5;
        const float producerTargetAngle = 0f;
        private const float OrderProcessingTime = 0.5f;
        List<ProducerNPC> _producers;
        ClientsNPCManager _clientsNPCManager;
        bool _isConstructed;
        Dictionary<int, ClientNPC> _producersToClients = new();
        Dictionary<int, Product> _producersToProduct = new();
        Dictionary<int, Transform> _producerToPlace = new();
        ISourcesManager _sourcesManager;
        IMoneyManager _moneyManager;
        IGameFactory _gameFactory;
        IAudioManager _soundManager;
        IBoosterManager _boosterManager;
        Vector3 _producerSpawnPoint;
        float _producerRotationAngle;
        string _profitModifier = "1";
        float _producerSpeedModifier = 1;

        public void Construct(ClientsNPCManager clientsNPCManager, List<ProducerNPC> producers, ISourcesManager sourcesManager,
            IMoneyManager moneyManager, IGameFactory gameFactory, Vector3 producerSpawnPoint, float producerRotationAngle,
            IAudioManager soundManager, IBoosterManager boosterManager)
        {
            _clientsNPCManager = clientsNPCManager;
            _producers = producers;
            _isConstructed = true;
            _sourcesManager = sourcesManager;
            _moneyManager = moneyManager;
            _gameFactory = gameFactory;
            _producerSpawnPoint = producerSpawnPoint;
            _producerRotationAngle = producerRotationAngle;
            _soundManager = soundManager;
            _boosterManager = boosterManager;
            _boosterManager.OnBoosterActivated += OnBoosterActivated;
            _boosterManager.OnBoosterDeactivated += OnBoosterDeactivated;
        }

        void Update()
        {
            if (!_isConstructed) return;
            MoveForOrder();
            ProcessOrder();
            CheckSleepingWithOrderProducers();
            ProduceProduct();
            GiveProduct();
        }

        void OnDestroy()
        {
            _boosterManager.OnBoosterActivated -= OnBoosterActivated;
            _boosterManager.OnBoosterDeactivated -= OnBoosterDeactivated;
        }

        void OnBoosterActivated(BoosterType booster)
        {
            Debug.LogError("Producers: " + booster.ToString());

            if (booster == BoosterType.BoostProfit_x2)
            {
                _profitModifier = "2";
            }
            else if (booster == BoosterType.BoostProducerSpeed_x1_5)
            {
                _producerSpeedModifier = 1.5f;
            }
        }

        void OnBoosterDeactivated(BoosterType booster)
        {
            Debug.LogError("Deactivate Producers: " + booster.ToString());
            if (booster == BoosterType.BoostProfit_x2)
            {
                _profitModifier = "1";
            }
            else if (booster == BoosterType.BoostProducerSpeed_x1_5)
            {
                _producerSpeedModifier = 1;
            }
        }

        public void SpawnProducer()
        {
            ProducerNPC producerNPC = _gameFactory.CreateProducer(_producerSpawnPoint, _producerRotationAngle).GetComponent<ProducerNPC>();
            _producers.Add(producerNPC);
        }

        void ProcessOrder()
        {
            foreach (ProducerNPC producerNPC in _producers)
            {
                if (!producerNPC.IsMoving() && producerNPC.CurrentState == ProducerNPC.State.MoveToClientForOrder)
                {
                    producerNPC.Rotate(producerTargetAngle);
                    producerNPC.CurrentState = ProducerNPC.State.ProcessOrder;
                    int id = producerNPC.gameObject.GetInstanceID();
                    ClientNPC client = _producersToClients[id];
                    _producersToProduct[id] = client.Product;
                    client.CurrentState = ClientNPC.State.ProcessOrder;
                    Source source = _sourcesManager.GetSource(client.Product);
                    producerNPC.timer.Callback = delegate() {
                        MoveToSource(producerNPC);
                        _clientsNPCManager.WaitForOrder(client, source.state.GetIcon());
                        producerNPC.timer.Hide();
                    };
                    producerNPC.timer.Duration = OrderProcessingTime;
                    producerNPC.timer.Show();
                }
            }
        }

        void ProduceProduct()
        {
            foreach (ProducerNPC producerNPC in _producers)
            {
                if (!producerNPC.IsMoving() && producerNPC.CurrentState == ProducerNPC.State.MoveToSource)
                {
                    producerNPC.CurrentState = ProducerNPC.State.ProduceProduct;
                    producerNPC.timer.Callback = delegate () {
                        MoveToClient(producerNPC);
                        producerNPC.timer.Hide();
                        CreateProduct(producerNPC);
                    };

                    Product product = _producersToProduct[producerNPC.gameObject.GetInstanceID()];
                    Source source = _sourcesManager.GetSource(product);

                    producerNPC.timer.Duration = source.upgrade.ProductionTime;
                    producerNPC.timer.Show();
                }
            }
        }

        void MoveForOrder()
        {
            _clientsNPCManager.Clients.Shuffle();
            foreach (ClientNPC clientNPC in _clientsNPCManager.Clients)
            {
                if (clientNPC.CurrentState == ClientNPC.State.ReadyToOrder)
                {
                    ProducerNPC producer = GetSleepingProducer();
                    if (producer == null)
                    {
                        return;
                    }

                    clientNPC.CurrentState = ClientNPC.State.WaitingForProducer;
                    producer.sleepingParticles.Hide();
                    Vector3 destination = _clientsNPCManager.GetProducerPlace(clientNPC.gameObject.GetInstanceID());
                    producer.CurrentState = ProducerNPC.State.MoveToClientForOrder;
                    producer.Move(destination, _producersSpeed * _producerSpeedModifier);
                    _producersToClients[producer.gameObject.GetInstanceID()] = clientNPC;
                }
            }
        }

        void MoveToSource(ProducerNPC producerNPC)
        {
            producerNPC.CurrentState = ProducerNPC.State.MoveToSource;
            int id = producerNPC.gameObject.GetInstanceID();
            Product product = _producersToProduct[id];
            Source source = _sourcesManager.GetSource(product);
            if (source.places.OccupiedPlaces >= source.state.MaxPlacesCount)
            {
                MoveToSleepWithOrder(producerNPC);
                return;
            }

            Transform tr = source.places.Occupy(source.state.MaxPlacesCount);
            if (tr == null)
            {
                MoveToSleepWithOrder(producerNPC);
                return;
            }

            producerNPC.sleepingParticles.Hide();
            _producerToPlace[id] = tr;
            producerNPC.Move(tr.position, _producersSpeed * _producerSpeedModifier);
        }

        private static void MoveToSleepWithOrder(ProducerNPC producerNPC)
        {
            producerNPC.CurrentState = ProducerNPC.State.SleepWithOrder;
            producerNPC.Stop();
            producerNPC.sleepingParticles.Show();
        }

        void MoveToClient(ProducerNPC producerNPC)
        {
            producerNPC.CurrentState = ProducerNPC.State.MoveToClientWithOrder;
            ClientNPC clientNPC = _producersToClients[producerNPC.gameObject.GetInstanceID()];
            Vector3 place = _clientsNPCManager.GetProducerPlace(clientNPC.gameObject.GetInstanceID());
            producerNPC.Move(place, _producersSpeed * _producerSpeedModifier);
            Source source = GetSource(producerNPC.gameObject.GetInstanceID());
            producerNPC.profitVisualizer.SourceUpgrade = source.upgrade;
            producerNPC.profitVisualizer.Show();
        }

        void GiveProduct()
        {
            foreach (ProducerNPC producerNPC in _producers)
            {
                if (!producerNPC.IsMoving() && producerNPC.CurrentState == ProducerNPC.State.MoveToClientWithOrder)
                {
                    producerNPC.CurrentState = ProducerNPC.State.GiveOrder;
                    Object.Destroy(producerNPC.productPlace.transform.GetChild(0).gameObject);
                    int id = producerNPC.gameObject.GetInstanceID();
                    ClientNPC clientNPC = _producersToClients[id];
                    _clientsNPCManager.Leave(clientNPC);
                    producerNPC.CurrentState = ProducerNPC.State.Sleep;
                    producerNPC.profitVisualizer.Hide();
                    producerNPC.profitVisualizer.SourceUpgrade = null;
                    producerNPC.sleepingParticles.Show();
                    Source source = GetSource(id);
                    AddProfit(source);

                    source.places.Deoccupy(_producerToPlace[id]);

                    _producersToClients.Remove(id);
                    _producersToProduct.Remove(id);
                    _producerToPlace.Remove(id);

                    _soundManager.PlayCoinsSound();
                }
            }
        }

        private void AddProfit(Source source)
        {
            BigNumber profit = new BigNumber(source.upgrade.CurrentProfit);
            BigNumber modifier = new BigNumber(_profitModifier);
            BigNumber result = profit * modifier;
            _moneyManager.AddMoney(result.ToString());
        }

        private Source GetSource(int id)
        {
            Product product = _producersToProduct[id];
            Source source = _sourcesManager.GetSource(product);
            return source;
        }

        void CreateProduct(ProducerNPC producer)
        {
            int id = producer.gameObject.GetInstanceID();
            Source source = GetSource(id);

            Product product = _producersToProduct[producer.gameObject.GetInstanceID()];

            GameObject go = null;
            if (product == Product.Pumpkin) {
                go = source.state.ProduceProduct(producer.productPlace.position, new Vector3(0.5f, 0.3f, 0.5f));
            }
            else if (product == Product.Egg)
            {
                go = source.state.ProduceProduct(producer.productPlace.position, new Vector3(0.44f, 0.6f, 0.44f));
            }
            else if (product == Product.Tomato)
            {
                go = source.state.ProduceProduct(producer.productPlace.position, Vector3.one);
            }
            go.transform.SetParent(producer.productPlace);
        }

        void CheckSleepingWithOrderProducers()
        {
            foreach(ProducerNPC p in _producers)
            {
                if (p.CurrentState == ProducerNPC.State.SleepWithOrder)
                {
                    MoveToSource(p);
                }
            }
        }

        ProducerNPC GetSleepingProducer()
        {
            List<ProducerNPC> sleepingProducers = new();
            foreach (ProducerNPC producer in _producers)
            {
                if (producer.CurrentState == ProducerNPC.State.Sleep)
                {
                    sleepingProducers.Add(producer);
                }
            }

            if (sleepingProducers.Count == 0) return null;

            sleepingProducers.Shuffle();

            return sleepingProducers[0];
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.producers = _producers.Count - 1;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            for (int i = 0; i < progress.producers; ++i)
            {
                SpawnProducer();
            }
        }
    }
}
