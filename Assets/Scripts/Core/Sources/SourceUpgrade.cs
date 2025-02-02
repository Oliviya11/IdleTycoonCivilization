using Assets.Scripts.Core.Money.Services;
using Assets.Scripts.Data;
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
        public float ProductionTime { get; set; }
        public List<Upgrade> Upgrades { get; private set; }


        string currentPrice;
        public string CurrentPrice { get => currentPrice; private set => currentPrice = value; }

        string currentProfit;
        public string CurrentProfit { get => currentProfit; set => currentProfit = value; }

        public int CurrentUpgrade { get; private set; }

        int currentLevel;
        public int CurrentLevel { get => currentLevel; private set => currentLevel = value; }
        public int CurrentUpgradeLevel { get; private set; }
        public int MaxUpgrades => Upgrades.Count;
        float priceTime;
        float profitTime;
        SourceState sourceState;
        IMoneyManager moneyManager;

        public SourceUpgrade(SourceState sourceState, float profitTime, float priceTime, float productionTime, List<Upgrade> upgrades,
            int currentUpgrade, int currentLevel, Product product, IMoneyManager moneyManager)
        {
            ProductionTime = productionTime;
            Upgrades = upgrades;
            CurrentUpgrade = currentUpgrade;
            CurrentLevel = currentLevel;
            Product = product;
            this.priceTime = priceTime;
            this.profitTime = profitTime;
            this.moneyManager = moneyManager;
            this.sourceState = sourceState;
        }

        public void UpgradeTill(int upgrade, int level)
        {
            CurrentLevel = level;

            int l = 0;

            for (int i = 0; i < Upgrades.Count; ++i)
            {
                if (i <= upgrade)
                {
                    Upgrade u = Upgrades[i];
                    for (int j = 0; j < u.levelsToNextUpgrade; ++j)
                    {
                        // price
                        CalculateNewBigNumber(u, u.priceCurve, ref currentPrice, ref priceTime);

                        // profit
                        CalculateNewBigNumber(u, u.profitCurve, ref currentProfit, ref profitTime);

                        if (l >= level) return;
                    }
                }
            }
        }

        public void SubtractMoney()
        {
            moneyManager.SubtractMoney(CurrentPrice);
        }

        public void Upgrade()
        {
            SubtractMoney();

            if (CurrentUpgrade >= Upgrades.Count) return;

            ++CurrentLevel;
            ++CurrentUpgradeLevel;

            if (CurrentLevel >= GetActualLevel(CurrentUpgrade))
            {
                ++CurrentUpgrade;
                sourceState.SetProductPlace(CurrentUpgrade);
                CurrentUpgradeLevel = 0;
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

        public int GetUpgradeLevels()
        {
            for (int i = 0; i < Upgrades.Count; i++)
            {
                if (CurrentUpgrade == i) return Upgrades[i].levelsToNextUpgrade;
            }

            return 0;
        }

        private void CalculateNewBigNumber(Upgrade u, AnimationCurve curve, ref string number, ref float time)
        {
            time += STEP;
            /*
            BigNumber bigNumber = new BigNumber(number);
            float previousTime = bigNumber.ToFloat();
            if (time < previousTime)
            {
                time = previousTime;
            }
            */
            number = CalculateBigNumber(curve, time);
        }

        private static string CalculateBigNumber(AnimationCurve curve, float time)
        {
            string number;
            float nextNumber = curve.Evaluate(time);
            number = BigNumber.FromFloat(nextNumber).ToString();
            return number;
        }

        public void SetProgress(SourceData data)
        {
            UpgradeTill(data.upgrade, data.level);
            ProductionTime = data.time;
            CurrentUpgradeLevel = data.upgradeLevel;
            CurrentUpgrade = data.upgrade;
        }

        public SourceData LoadProgress()
        {
            SourceData data = new SourceData();
            data.level = CurrentLevel;
            data.upgrade = CurrentUpgrade;
            data.time = ProductionTime;
            data.upgradeLevel = CurrentUpgradeLevel;

            return data;
        }
    }
}
