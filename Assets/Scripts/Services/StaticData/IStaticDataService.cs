using Assets.Scripts.StaticData;
using System.Collections.Generic;

namespace Assets.Scripts.Services.StaticData
{
    internal interface IStaticDataService : IService
    {
        void Load();
        LevelStaticData ForLevel(int level);

        List<BoosterStaticData> GetBoosters();

        int GetMaxLevels();
    }
}
