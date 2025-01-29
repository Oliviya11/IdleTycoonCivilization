using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Core.Orders.OrderPlaces;

namespace Assets.Scripts.Core.Orders
{
    public class OrderPlaces : Places<PlacesPair>
    {
        [Serializable]
        public struct PlacesPair
        {
            public Transform clientPlace;
            public Transform producerPlace;
        }
    }
}
