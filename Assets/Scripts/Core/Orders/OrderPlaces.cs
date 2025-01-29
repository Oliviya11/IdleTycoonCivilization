using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Orders
{
    public class OrderPlaces : MonoBehaviour
    {
        [SerializeField] List<Transform> places;
        List<bool> _isOccupied;

        private void Awake()
        {
            _isOccupied = new();
            for (int i = 0; i < places.Count; i++)
            {
                _isOccupied.Add(false);
            }
        }

        public int Occupy()
        {
            int placeIndex = Random.Range(0, places.Count);
            _isOccupied[placeIndex] = true;

            return placeIndex;
        }

        public void Deoccupy(int placeIndex)
        {
            _isOccupied[placeIndex] = false;
        }

        public Transform GetPlace(int index) => places[index];
    }
}
