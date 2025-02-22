﻿using Assets.Scripts.Data;

namespace Assets.Scripts.Services.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        void SaveProgress();
        PlayerProgress LoadProgress();
    }
}
