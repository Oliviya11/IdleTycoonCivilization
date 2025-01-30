using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.StaticData.SourceStaticData;

namespace Assets.Scripts.Core.Sources
{
    public class SourceUpgrade
    {
        const float STEP = 1f;
        public string InitialPrice { get; private set; }
        public string InitialProfit { get; private set; }
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

        public SourceUpgrade(string initialPrice, string initialProfit, float productionTime, List<Upgrade> upgrades,
            int currentUpgrade, int currentLevel)
        {
            InitialPrice = initialPrice;
            InitialProfit = initialProfit;
            ProductionTime = productionTime;
            Upgrades = upgrades;
            CurrentUpgrade = currentUpgrade;
            CurrentLevel = currentLevel;
        }

        public void UpgradeTill(int upgrade, int level)
        {
            CurrentPrice = InitialPrice;
            CurrentProfit = InitialProfit;

            if (level == 0) return;

            for (int i = 0; i < Upgrades.Count; ++i)
            {
                if (i <= upgrade)
                {
                    Upgrade u = Upgrades[i];
                    for (int j = 0; j < u.levelsToNextUpgrade; ++j)
                    {
                        ++CurrentLevel;

                        // price
                        CalculateNewBigNumber(u, u.priceCurve, ref currentPrice);

                        // profit
                        CalculateNewBigNumber(u, u.profitCurve, ref currentProfit);

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
            CalculateNewBigNumber(u, u.priceCurve, ref currentPrice);

            // profit
            CalculateNewBigNumber(u, u.profitCurve, ref currentProfit);
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
        private void CalculateNewBigNumber(Upgrade u, AnimationCurve curve, ref string number)
        {
            BigNumber currentNumber = new BigNumber(number);
            float currentNumberF = currentNumber.ToFloat();
            currentNumberF += STEP;
            float nextNumber = curve.Evaluate(currentNumberF);
            number = BigNumber.FromFloat(nextNumber).ToString();
        }
    }
}
