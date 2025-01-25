using Assets.Scripts.StaticData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        const string LevelsDataPath = "Levels/StaticData";
        private LevelStaticData[] _levels;

        public void Load()
        {
            _levels = Resources.LoadAll<LevelStaticData>(LevelsDataPath);
        }

        public LevelStaticData ForLevel(int level)
        {
            if (level > _levels.Length) return null;
            return _levels[level - 1];
        }
    }
}
