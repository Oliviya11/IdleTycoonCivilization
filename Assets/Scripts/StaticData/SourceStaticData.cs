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
        public Product product;
        public SourceState.State initialState;
        public float productionTime;

        [Serializable]
        public struct Upgrade
        {
            public AnimationCurve priceCurve;
            public AnimationCurve profitCurve;
            public int levelsToNextUpgrade;
        }

        public List<Upgrade> upgrades;
    }
}
