using Assets.Scripts.Sources;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.StaticData.SourceStaticData;

namespace Assets.Scripts.Core.Sources
{
    public class SourceUpgrade
    {
        const float STEP = 0.1f;
        public Product Product {  get; private set; }
        public float ProductionTime { get; private set; }
        public List<Upgrade> Upgrades { get; private set; }


        string currentPrice;
        public string CurrentPrice { get => currentPrice; private set => currentPrice = value; }

        string currentProfit;
        public string CurrentProfit { get => currentProfit; private set => currentProfit = value; }

        public int CurrentUpgrade { get; private set; }

        int currentLevel;
        public int CurrentLevel { get => currentLevel; private set => currentLevel = value; }
        public int MaxUpgrades => Upgrades.Count;
        float priceTime;
        float profitTime;

        public SourceUpgrade(float profitTime, float priceTime, float productionTime, List<Upgrade> upgrades,
            int currentUpgrade, int currentLevel, Product product)
        {
            ProductionTime = productionTime;
            Upgrades = upgrades;
            CurrentUpgrade = currentUpgrade;
            CurrentLevel = currentLevel;
            Product = product;
            this.priceTime = priceTime;
            this.profitTime = profitTime;
        }

        public void UpgradeTill(int upgrade, int level)
        {
            for (int i = 0; i < Upgrades.Count; ++i)
            {
                if (i <= upgrade)
                {
                    Upgrade u = Upgrades[i];
                    for (int j = 0; j < u.levelsToNextUpgrade; ++j)
                    {
                        ++CurrentLevel;

                        // price
                        CalculateNewBigNumber(u, u.priceCurve, ref currentPrice, ref priceTime);

                        // profit
                        CalculateNewBigNumber(u, u.profitCurve, ref currentProfit, ref profitTime);

                        if (CurrentLevel >= level) return;
                    }
                }
            }
        }

        public void Upgrade()
        {
            if (CurrentUpgrade >= Upgrades.Count) return;

            ++CurrentLevel;
            if (CurrentLevel > GetActualLevel(CurrentUpgrade))
            {
                ++CurrentUpgrade;
            }

            if (CurrentUpgrade >= Upgrades.Count) return;

            Upgrade u = Upgrades[CurrentUpgrade];

            // price
            CalculateNewBigNumber(u, u.priceCurve, ref currentPrice, ref priceTime);

            // profit
            CalculateNewBigNumber(u, u.profitCurve, ref currentProfit, ref profitTime);
        }

        int GetActualLevel(int currentUpgrade)
        {
            int level = 0;
            for (int i = 0; i <= currentUpgrade; ++i) {
                level += Upgrades[i].levelsToNextUpgrade;
            }

            return level;
        }

        public int MaxLevels()
        {
            int maxLevel = 0;
            foreach (Upgrade u in Upgrades)
            {
                maxLevel += u.levelsToNextUpgrade;
            }

            return maxLevel;
        }
        private void CalculateNewBigNumber(Upgrade u, AnimationCurve curve, ref string number, ref float time)
        {
            time += STEP;
            number = CalculateBigNumber(curve, time);
        }

        private static string CalculateBigNumber(AnimationCurve curve, float time)
        {
            string number;
            float nextNumber = curve.Evaluate(time);
            number = BigNumber.FromFloat(nextNumber).ToString();
            return number;
        }
    }
}
