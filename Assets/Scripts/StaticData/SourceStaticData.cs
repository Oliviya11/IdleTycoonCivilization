using Assets.Scripts.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.StaticData
{

    [CreateAssetMenu(fileName = "SourceData", menuName = "Static Data/Source")]
    public class SourceStaticData : ScriptableObject
    {
        public SourceState.State initialState;
        public string initialPrice;

        [Serializable]
        public struct Upgrade
        {
            public string price;
            public string profit;
            public AnimationCurve priceCurve;
            public AnimationCurve profitCurve;
        }

        public List<Upgrade> upgrades;
    }
}
