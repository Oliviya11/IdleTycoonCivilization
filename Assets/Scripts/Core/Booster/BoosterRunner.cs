using Assets.Scripts.Core.Booster.Service;
using UnityEngine;

namespace Assets.Scripts.Core.Booster
{
    public class BoosterRunner : MonoBehaviour
    {
        IBoosterManager _boosterManager;
        float _duration;

        void Update()
        {

            if (_duration > 0)
            {
                _duration -= Time.deltaTime;
                if (_duration <= 0)
                {
                    _boosterManager.DeactivateBooster();
                }
            }
        }
        private void OnDestroy()
        {
            if (_boosterManager != null)
            {
                _boosterManager.OnBoosterActivated -= OnBoosterActivated;
            }
        }

        public void Construct(IBoosterManager boosterManager)
        {
            _boosterManager = boosterManager;
            _boosterManager.OnBoosterActivated += OnBoosterActivated;
        }

        void OnBoosterActivated(Booster booster) {
            _duration = _boosterManager.boostersStaticData[booster].duration;
            Debug.LogError(_duration);
        }
    }
}
