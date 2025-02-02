using Assets.Scripts.Core.Booster;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public string money;
        public List<SourceData> sources;
        public int clients = 1;
        public int producers;
        public int level;
        public bool sound = true;
        public bool music = true;
        public Booster booster;
        public float remainingBoosterTime;
    }
}
