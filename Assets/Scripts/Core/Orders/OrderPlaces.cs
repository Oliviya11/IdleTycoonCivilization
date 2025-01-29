using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Orders
{
    public class OrderPlaces : MonoBehaviour
    {
        [SerializeField] List<Transform> places;
        List<Transform> freePlaces = new List<Transform>();

        private void Awake()
        {
            freePlaces = new List<Transform>(places);
        }

        public Transform Occupy()
        {
            int placeIndex = Random.Range(0, freePlaces.Count);

            Transform tr = GetPlace(placeIndex);

            freePlaces.RemoveAt(placeIndex);

            return tr;
        }

        public void Deoccupy(Transform tr)
        {
            freePlaces.Add(tr);
        }

        public Transform GetPlace(int index) => freePlaces[index];

        public bool AreFreePlaces() => freePlaces.Count > 0;
    }
}
