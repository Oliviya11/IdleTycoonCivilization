using Assets.Scripts.Data;
using Assets.Scripts.Services;
using Assets.Scripts.Services.PersistentProgress;
using Assets.Scripts.StaticData;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Core.Booster.Service
{
    public class BoosterManager : IBoosterManager
    {
        public Dictionary<Booster, BoosterStaticData> boostersStaticData { get; } = new();
        public Dictionary<Booster, int> boosterToNumber { get; } = new();
        public event Action<Booster> OnBoosterActivated;
        public event Action<Booster> OnBoosterDeactivated;
        Booster activatedBooster;
        IPersistentProgressService _persistentProgressService;

        public BoosterManager(List<BoosterStaticData> staticData, IPersistentProgressService persistentProgressService)
        {
            Init(staticData);

            _persistentProgressService = persistentProgressService;
        }

        private void Init(List<BoosterStaticData> staticData)
        {
            for (int i = 0; i < staticData.Count; i++)
            {
                BoosterStaticData s = staticData[i];
                boostersStaticData[s.booster] = s;
                boosterToNumber[s.booster] = 0;
            }
        }

        public void Load()
        {
            if (_persistentProgressService.Progress.boosters != null)
            {
                for (int i = 0; i < _persistentProgressService.Progress.boosters.Count; i++)
                {
                    BoosterData boosterData = _persistentProgressService.Progress.boosters[i];
                    boosterToNumber[boosterData.booster] = boosterData.number;
                }
            }
        }

        public void Save()
        {
            _persistentProgressService.Progress.boosters = new();
            foreach (KeyValuePair<Booster, int> pair in boosterToNumber)
            {
                if (pair.Value > 0)
                {
                    BoosterData data = new BoosterData();
                    data.booster = pair.Key;
                    data.number = pair.Value;
                    _persistentProgressService.Progress.boosters.Add(data);
                }
            }
        }

        public void AddBooster()
        {
            Booster booster = GetRandomBooster();
            if (boosterToNumber.ContainsKey(booster))
            {
                ++boosterToNumber[booster];
            }
            else
            {
                boosterToNumber[booster] = 1;
            }
        }

        public void ActivateBooster(Booster booster)
        {
            activatedBooster = booster;
            --boosterToNumber[booster];
            OnBoosterActivated?.Invoke(booster);
        }

        public void DeactivateBooster()
        {
            OnBoosterDeactivated?.Invoke(activatedBooster);
            activatedBooster = Booster.None;
        }

        Booster GetRandomBooster()
        {
            Array boosters = Enum.GetValues(typeof(Booster));
            boosters.Shuffle();

            int[] noNone = ((int[])boosters).Where(n => n != (int)Booster.None).ToArray();

            return (Booster)noNone[0];
        }
    }
}
