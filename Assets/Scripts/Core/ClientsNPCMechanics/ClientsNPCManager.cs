using Assets.Scripts.Core.Orders;
using Assets.Scripts.Core.Sources.Services;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static Assets.Scripts.Core.Orders.OrderPlaces;
using Product = Assets.Scripts.Sources.Product;

namespace Assets.Scripts.Core.ClientsNPCMechanics
{
    public class ClientsNPCManager : MonoBehaviour
    {
        [SerializeField] int maxClients;
        [SerializeField] float processWithProducer;
        [SerializeField] List<Transform> spawnPoints;

        const float clientTargetAngle = 180f;

        public int MaxClients
        {
            get => maxClients;
            set => maxClients = value;
        }

        public List<ClientNPC> Clients => _clients;

        bool _collectionChecks = true;
        int _maxPoolSize = 10;
        IObjectPool<ClientNPC> _pool;
        OrderPlaces _places;
        IGameFactory _gameFactory;
        ISourcesManager _sourcesManager;
        Dictionary<int, PlacesPair> _clientToPlace = new();
        List<ClientNPC> _clients = new();
        bool _isConstructed;

        void Awake()
        {
            _pool = new ObjectPool<ClientNPC>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, _collectionChecks, 10, _maxPoolSize);
        }

        private void Update()
        {
            if (!_isConstructed) return;

            Rotate();
            Despawn();

            if (!_places.AreFreePlaces()) return;
            SpawnClient();
        }

        private void SpawnClient()
        {
            Product p = GetRandomProduct();

            if (p == Product.None) return;

            if (_clients.Count < maxClients)
            {
                var client = _pool.Get();
                int id = client.gameObject.GetInstanceID();
                PlacesPair pair = _clientToPlace[id];
                client.Move(pair.clientPlace.position);
                client.Product = p;
                _clients.Add(client);
            }
        }

        public void Construct(OrderPlaces places, IGameFactory gameFactory, ISourcesManager sourcesManager)
        {
            _places = places;
            _gameFactory = gameFactory;
            _sourcesManager = sourcesManager;
            _isConstructed = true;
        }

        public Vector3 GetProducerPlace(int clientId)
        {
            return _clientToPlace[clientId].producerPlace.position;
        }

        public void WaitForOrder(ClientNPC clientNPC)
        {
            clientNPC.CurrentState = ClientNPC.State.WaitingForOrder;
        }

        public void Leave(ClientNPC clientNPC)
        {
            clientNPC.CurrentState = ClientNPC.State.Leave;
            clientNPC.Move(ChooseRandomStartPosition());
        }

        void Despawn()
        {
            List<ClientNPC> clientsToIterate = new List<ClientNPC>(Clients);
            foreach (ClientNPC client in clientsToIterate)
            {
                if (!client.IsMoving() && client.CurrentState == ClientNPC.State.Leave)
                {
                    _pool.Release(client);
                }
            }
        }

        ClientNPC CreatePooledItem()
        {
            var go = _gameFactory.CreateClient(ChooseRandomStartPosition());
            OnCreate(go.GetComponent<ClientNPC>());
            return go.GetComponent<ClientNPC>();
        }

        private void OnCreate(ClientNPC client)
        {
            PlacesPair pair = _places.Occupy();
            client.CurrentState = ClientNPC.State.Come;
            _clientToPlace[client.gameObject.GetInstanceID()] = pair;
            client.CurrentState = ClientNPC.State.Come;
        }

        void OnReturnedToPool(ClientNPC client)
        {
            _clients.Remove(client);
            RemoveFromClientToPlace(client);
            client.gameObject.SetActive(false);
        }

        void OnTakeFromPool(ClientNPC client)
        {
            OnCreate(client);
            client.gameObject.SetActive(true);
        }

        void OnDestroyPoolObject(ClientNPC client)
        {
            RemoveFromClientToPlace(client);
            Destroy(client.gameObject);
        }

        Vector3 ChooseRandomStartPosition()
        {
            return spawnPoints.GetRandomElement().position;
        }

        void RemoveFromClientToPlace(ClientNPC client)
        {
            int id = client.gameObject.GetInstanceID();
            PlacesPair pair = _clientToPlace[id];
            _clientToPlace.Remove(id);
            _places.Deoccupy(pair);
        }

        Product GetRandomProduct()
        {
            Product product = Product.None;

            Array products = Enum.GetValues(typeof(Product));
            products.Shuffle();

            foreach (Product p in products)
            {
                if (_sourcesManager.IsProductOpened(p)) { product = p; break; }
            }

            return product;
        }

        void Rotate()
        {
            for (int i = 0; i < _clients.Count; ++i)
            {
                var client = _clients[i];
                if (!client.IsMoving() && client.CurrentState == ClientNPC.State.Come)
                {
                    client.Rotate(clientTargetAngle);
                    client.CurrentState = ClientNPC.State.ReadyToOrder;
                }
            }
        }
    }
}
