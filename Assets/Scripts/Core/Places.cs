using System.Collections.Generic;
using System;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public abstract class Places<T> : MonoBehaviour
    {
        [SerializeField] List<T> places;
        List<T> _freePlaces;
        int _occupiedPlaces;

        private void Awake()
        {
            _freePlaces = new List<T>(places);
        }

        public T Occupy()
        {
            int placeIndex = UnityEngine.Random.Range(0, _freePlaces.Count);

            T pair = GetPlace(placeIndex);

            _freePlaces.RemoveAt(placeIndex);

            ++_occupiedPlaces;

            return pair;
        }

        public T Occupy(int maxPlaces)
        {
            int placeIndex = UnityEngine.Random.Range(0, maxPlaces);

            T pair = GetPlace(placeIndex);

            _freePlaces.RemoveAt(placeIndex);

            ++_occupiedPlaces;

            return pair;
        }

        public void Deoccupy(T pair)
        {
            _freePlaces.Insert(0, pair);
            --_occupiedPlaces;
        }

        public T GetPlace(int index) => _freePlaces[index];

        public bool AreFreePlaces() => _freePlaces.Count > 0;
        public bool AreFreePlaces(int max) => _freePlaces.Count > 0;
        public int OccupiedPlaces => _occupiedPlaces;
    }
}
