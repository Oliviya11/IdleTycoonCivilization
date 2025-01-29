using Assets.Scripts.Core.Orders;
using Assets.Scripts.Infrastracture.Factory;
using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Core.ClientsNPCMechanics
{
    public class ClientsNPCSpawner : MonoBehaviour
    {
        [SerializeField] int maxClients;
        [SerializeField] float processWithProducer;
        [SerializeField] List<Transform> spawnPoints;
        const float SlightlyOutside = 0.1f;
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
        Dictionary<int, int> _clientToPlace = new();
        List<ClientNPC> _clients = new();
        int _clientsOnScene;
        bool _isConstructed;

        void Awake()
        {
            _pool = new ObjectPool<ClientNPC>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, _collectionChecks, 10, _maxPoolSize);
        }

        private void Update()
        {
            if (!_isConstructed) return;

            if (_clientsOnScene < maxClients)
            {
                var client = _pool.Get();
                int index = _clientToPlace[client.gameObject.GetInstanceID()];
                client.Destination = _places.GetPlace(index).position;
                client.Move(client.Destination);
                _clients.Add(client);
            }

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

        public void Construct(OrderPlaces places, IGameFactory gameFactory)
        {
            _places = places;
            _gameFactory = gameFactory;
            _isConstructed = true;
        }

        ClientNPC CreatePooledItem()
        {
            var go = _gameFactory.CreateClient(ChooseRandomStartPosition());
            var returnToPool = go.AddComponent<ReturnToPool>();
            returnToPool.pool = _pool;
            int index = _places.Occupy();
            _clientToPlace[go.GetInstanceID()] = index;
            ++_clientsOnScene;
            return go.GetComponent<ClientNPC>();
        }

        void OnReturnedToPool(ClientNPC client)
        {
            --_clientsOnScene;
            _clients.Remove(client);
            RemoveFromClientToPlace(client);
            client.gameObject.SetActive(false);
        }

        void OnTakeFromPool(ClientNPC client)
        {
            ++_clientsOnScene;
            _clients.Remove(client);
            client.gameObject.SetActive(true);
        }

        void OnDestroyPoolObject(ClientNPC client)
        {
            --_clientsOnScene;
            RemoveFromClientToPlace(client);
            Destroy(client.gameObject);
        }

        Vector3 ChooseRandomStartPosition()
        {
            int index = Random.Range(0, spawnPoints.Count);
            return spawnPoints[index].position;
        }

        public static Vector3 GetRandomPositionOutOfView()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null) return Vector3.zero;

            float x, z;
            float y = 1;

            int edge = Random.Range(0, 3);
            if (edge == 0) // Left
            {
                x = -0.1f;
                z = Random.Range(0f, 1f);
            }
            else if (edge == 1) // Right
            {
                x = 1.1f;
                z = Random.Range(0f, 1f);
            }
            else // Top
            {
                x = Random.Range(0f, 1f);
                z = 1.1f;
            }

            Vector3 viewportPosition = new Vector3(x, y, z);
            return mainCamera.ViewportToWorldPoint(viewportPosition);
        }

        void RemoveFromClientToPlace(ClientNPC client)
        {
            int id = client.gameObject.GetInstanceID();
            int index = _clientToPlace[id];
            _clientToPlace.Remove(id);
            _places.Deoccupy(index);
        }
    }
}
