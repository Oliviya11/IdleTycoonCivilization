using Assets.Scripts.Core.Booster.Service;
using Assets.Scripts.Services.PersistentProgress;
using UnityEngine;

namespace Assets.Scripts.Core.Booster
{
    public class BoosterRunner : MonoBehaviour
    {
        IBoosterManager _boosterManager;
        IPersistentProgressService _persistentProgress;
        float _duration;

        void Update()
        {

            if (_duration > 0)
            {
                _duration -= Time.deltaTime;
                
                if (_duration <= 0)
                {
                    _boosterManager.DeactivateBooster();
                    _duration = 0;
                }

                _persistentProgress.Progress.remainingBoosterTime = _duration;
            }
        }
        private void OnDestroy()
        {
            if (_boosterManager != null)
            {
                _boosterManager.OnBoosterActivated -= OnBoosterActivated;
            }
        }

        public void Construct(IBoosterManager boosterManager, IPersistentProgressService persistentProgress)
        {
            _boosterManager = boosterManager;
            _persistentProgress = persistentProgress;
            _boosterManager.OnBoosterActivated += OnBoosterActivated;
        }

        public void OnBoosterActivated(Booster booster) {
            if (_persistentProgress.Progress.remainingBoosterTime > 0)
            {
                _duration = _persistentProgress.Progress.remainingBoosterTime;
            }
            else
            {
                _duration = _boosterManager.boostersStaticData[booster].duration;
            }
        }
    }
}
