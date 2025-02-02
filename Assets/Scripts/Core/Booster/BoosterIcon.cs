using Assets.Scripts.Core.Booster.Service;
using Assets.Scripts.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core.Booster
{
    public class BoosterIcon : MonoBehaviour
    {
        [SerializeField] Image icon;
        IBoosterManager _boosterManager;

        void OnDestroy()
        {
            _boosterManager.OnBoosterActivated -= OnBoosterActivated;
            _boosterManager.OnBoosterDeactivated -= OnBoosterDeactivated;
        }

        public void Construct(IBoosterManager boosterManager)
        {
            _boosterManager = boosterManager;
            _boosterManager.OnBoosterActivated += OnBoosterActivated;
            _boosterManager.OnBoosterDeactivated += OnBoosterDeactivated;
            OnBoosterDeactivated(Booster.None);
        }

        public void OnBoosterActivated(Booster booster)
        {
            icon.gameObject.SetActive(true);
            BoosterStaticData data = _boosterManager.boostersStaticData[booster];
            icon.sprite = data.icon;
        }

        void OnBoosterDeactivated(Booster booster)
        {
            icon.gameObject.SetActive(false);
        }
    }
}
