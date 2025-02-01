using Assets.Scripts.StaticData;

namespace Assets.Scripts.Services.StaticData
{
    internal interface IStaticDataService : IService
    {
        void Load();
        LevelStaticData ForLevel(int level);

        int GetMaxLevels();
    }
}
