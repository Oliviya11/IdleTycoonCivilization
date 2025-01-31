using Assets.Scripts.Core.LevelUpgrade;
using Assets.Scripts.Sources;
using System;
using System.Collections.Generic;
using UnityEngine;
using Product = Assets.Scripts.Sources.Product;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "LevelUpgradeData", menuName = "Static Data/LevelUpgrade")]
    public class LevelUpgradeStaticData : ScriptableObject
    {
        [Serializable]
        public struct UpgradeItem
        {
            public LevelUpgradeType type;
            public Product product;
            public string price;
            public string title;
            public string description;
            public Sprite sprite;
            public int multiplier;
            public int additiver;
        }

        public List<UpgradeItem> items;
    }
}
