using Assets.Scripts.Sources;
using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class SourceData
    {
        public int level;
        public int upgrade;
        public float time;
        public SourceState.State state;
        public int upgradeLevel;
        public string profit;
        public float profitTime;
    }
}
