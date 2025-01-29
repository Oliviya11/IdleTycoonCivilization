using Assets.Scripts.Core.Orders;
using Assets.Scripts.Core.Sources.Services;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Sources;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.Core.ClientsNPCMechanics
{
    public class ClientsNPCSpawner : MonoBehaviour
    {
        [SerializeField] int maxClients;
        [SerializeField] float processWithProducer;
        [SerializeField] List<Transform> spawnPoints;
        public int MaxClients
        {
            get => maxClients;
            set => maxClients = value;
        }

        public List<ClientNPC> readyClients = new();

        bool _collectionChecks = true;
        int _maxPoolSize = 10;
        IObjectPool<ClientNPC> _pool;
        OrderPlaces _places;
        IGameFactory _gameFactory;
        ISourcesManager _sourcesManager;
        Dictionary<int, Transform> _clientToPlace = new();
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

            if (!_places.AreFreePlaces()) return;

            Product p = GetRandomProduct();

            if (p == Product.None) return;

            if (_clients.Count < maxClients)
            {
                var client = _pool.Get();
                Transform tr = _clientToPlace[client.gameObject.GetInstanceID()];
                client.Destination = tr.position;
                client.Move(client.Destination);
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

        ClientNPC CreatePooledItem()
        {
            var go = _gameFactory.CreateClient(ChooseRandomStartPosition());
            var returnToPool = go.AddComponent<ReturnToPool>();
            returnToPool.pool = _pool;
            Transform tr = _places.Occupy();
            _clientToPlace[go.GetInstanceID()] = tr;
            return go.GetComponent<ClientNPC>();
        }

        void OnReturnedToPool(ClientNPC client)
        {
            _clients.Remove(client);
            RemoveFromClientToPlace(client);
            client.gameObject.SetActive(false);
        }

        void OnTakeFromPool(ClientNPC client)
        {
            _clients.Remove(client);
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
            Transform tr = _clientToPlace[id];
            _clientToPlace.Remove(id);
            _places.Deoccupy(tr);
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
                if (!client.IsMoving() && !readyClients.Contains(client))
                {
                    client.Rotate();
                    readyClients.Add(client);
                }
            }
        }
    }
}
