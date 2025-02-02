using Assets.Scripts.Services;
using Assets.Scripts.StaticData;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Booster.Service
{
    public interface IBoosterManager : IService
    {
        Dictionary<Booster, BoosterStaticData> boostersStaticData { get; }
        Dictionary<Booster, int> boosterToNumber { get; }

        event Action<Booster> OnBoosterActivated;
        event Action<Booster> OnBoosterDeactivated;
        void AddBooster();

        void ActivateBooster(Booster booster);
        void DeactivateBooster();

        void Save();

        void Load();
    }
}
