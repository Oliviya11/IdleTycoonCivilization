using Assets.Scripts.StaticData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        const string LevelsDataPath = "Levels/StaticData";
        const string BoostersDataPath = "Boosters/StaticData";
        LevelStaticData[] _levels;
        BoosterStaticData[] _boosters;

        public void Load()
        {
            _levels = Resources.LoadAll<LevelStaticData>(LevelsDataPath);
            _boosters = Resources.LoadAll<BoosterStaticData>(BoostersDataPath);
        }

        public LevelStaticData ForLevel(int level)
        {
            if (level > _levels.Length) return null;
            return _levels[level - 1];
        }

        public int GetMaxLevels()
        {
            return _levels.Length;
        }

        public List<BoosterStaticData> GetBoosters()
        {
            return _boosters.ToList();
        }
    }
}
