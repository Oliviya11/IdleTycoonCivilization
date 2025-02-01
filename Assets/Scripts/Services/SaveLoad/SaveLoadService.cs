using Assets.Scripts.Data;
using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.Playables;

namespace Assets.Scripts.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }

        public void SaveProgress()
        {
            foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);

            SaveLoadSystem.SaveGame(_progressService.Progress);
        }

        public PlayerProgress LoadProgress()
        {
            return SaveLoadSystem.LoadGame();
        }
    }
}
