using Assets.Scripts.Core.Money.Services;
using Assets.Scripts.Core.Sources.Services;
using Assets.Scripts.Sources;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Core.Orders.OrderPlaces;
using Product = Assets.Scripts.Sources.Product;

namespace Assets.Scripts.Core.ClientsNPCMechanics
{
    public class ProducersNPCManager : MonoBehaviour
    {
        const float producerTargetAngle = 0f;
        List<ProducerNPC> _producers;
        ClientsNPCManager _clientsNPCManager;
        bool _isConstructed;
        Dictionary<int, ClientNPC> _producersToClients = new();
        Dictionary<int, Product> _producersToProduct = new();
        Dictionary<int, Transform> _producerToPlace = new();
        ISourcesManager _sourcesManager;
        IMoneyManager moneyManager;

        public void Construct(ClientsNPCManager clientsNPCManager, List<ProducerNPC> producers, ISourcesManager sourcesManager, IMoneyManager moneyManager)
        {
            _clientsNPCManager = clientsNPCManager;
            _producers = producers;
            _isConstructed = true;
            _sourcesManager = sourcesManager;
            this.moneyManager = moneyManager;
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
                    producerNPC.timer.Callback = delegate() {
                        MoveToSource(producerNPC);
                        _clientsNPCManager.WaitForOrder(client);
                        producerNPC.timer.Hide();
                    };
                    producerNPC.timer.Duration = 0.5f;
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
                    producerNPC.timer.Duration = 0.5f;
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
                    clientNPC.CurrentState = ClientNPC.State.WaitingForProducer;
                    ProducerNPC producer = GetSleepingProducer();
                    producer.sleepingParticles.Hide();
                    Vector3 destination = _clientsNPCManager.GetProducerPlace(clientNPC.gameObject.GetInstanceID());
                    producer.CurrentState = ProducerNPC.State.MoveToClientForOrder;
                    producer.Move(destination);
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
                producerNPC.CurrentState = ProducerNPC.State.SleepWithOrder;
                producerNPC.Stop();
                producerNPC.sleepingParticles.Show();
                return;
            }
            producerNPC.sleepingParticles.Hide();
            Transform tr = source.places.Occupy(source.state.MaxPlacesCount - 1);
            _producerToPlace[id] = tr;
            producerNPC.Move(tr.position);
        }

        void MoveToClient(ProducerNPC producerNPC)
        {
            producerNPC.CurrentState = ProducerNPC.State.MoveToClientWithOrder;
            ClientNPC clientNPC = _producersToClients[producerNPC.gameObject.GetInstanceID()];
            Vector3 place = _clientsNPCManager.GetProducerPlace(clientNPC.gameObject.GetInstanceID());
            producerNPC.Move(place);
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
                    producerNPC.sleepingParticles.Show();

                    Product product = _producersToProduct[id];
                    Source source = _sourcesManager.GetSource(product);

                    moneyManager.AddMoney(source.upgrade.CurrentProfit);

                    source.places.Deoccupy(_producerToPlace[id]);

                    _producersToClients.Remove(id);
                    _producersToProduct.Remove(id);
                    _producerToPlace.Remove(id);
                }
            }
        }

        void CreateProduct(ProducerNPC producer)
        {
            Product product = _producersToProduct[producer.gameObject.GetInstanceID()];
            Source source = _sourcesManager.GetSource(product);
            GameObject go = source.state.ProduceProduct(producer.productPlace.position);
            go.transform.localScale = new Vector3(0.5f, 0.3f, 0.5f);
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
            sleepingProducers.Shuffle();
            return sleepingProducers[0];
        }
    }
}
