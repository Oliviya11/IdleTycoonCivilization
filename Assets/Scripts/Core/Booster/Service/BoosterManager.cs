﻿using Assets.Scripts.Services;
using Assets.Scripts.StaticData;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Core.Booster.Service
{
    public class BoosterManager : IBoosterManager
    {
        public Dictionary<Booster, BoosterStaticData> boostersStaticData { get; }
        public Dictionary<Booster, int> boosterToNumber { get;  }
        public event Action<Booster> OnBoosterActivated;
        public event Action<Booster> OnBoosterDeactivated;
        Booster activatedBooster;

        public BoosterManager(List<BoosterStaticData> staticData)
        {
            for (int i = 0; i < staticData.Count; i++)
            {
                BoosterStaticData s = staticData[i];
                boostersStaticData[s.booster] = s;
                boosterToNumber[s.booster] = 0;
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
                boosterToNumber[booster] = 0;
            }
        }

        public void ActivateBooster(Booster booster)
        {
            activatedBooster = booster;
            OnBoosterActivated?.Invoke(booster);
        }

        public void Deactivte(Booster booster)
        {
            OnBoosterDeactivated?.Invoke(booster);
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
