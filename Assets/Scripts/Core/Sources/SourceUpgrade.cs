using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.StaticData.SourceStaticData;

namespace Assets.Scripts.Core.Sources
{
    public class SourceUpgrade
    {
        public string InitialPrice { get; private set; }
        public string InitialProfit { get; private set; }
        public float ProductionTime { get; private set; }
        public List<Upgrade> Upgrades { get; private set; }

        public SourceUpgrade(string initialPrice, string initialProfit, float productionTime, List<Upgrade> upgrades)
        {
            InitialPrice = initialPrice;
            InitialProfit = initialProfit;
            ProductionTime = productionTime;
            Upgrades = upgrades;
        }
    }
}
