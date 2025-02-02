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

        void Start()
        {
            _boosterManager.OnBoosterActivated += OnBoosterActivated;
            _boosterManager.OnBoosterDeactivated += OnBoosterDeactivated;
            OnBoosterDeactivated(Booster.None);
        }

        void OnDestroy()
        {
            _boosterManager.OnBoosterActivated -= OnBoosterActivated;
            _boosterManager.OnBoosterDeactivated -= OnBoosterDeactivated;
        }

        public void Construct(IBoosterManager boosterManager)
        {
            _boosterManager = boosterManager;
        }

        void OnBoosterActivated(Booster booster)
        {
            Debug.LogError("Booster icon: " + booster.ToString());
            icon.gameObject.SetActive(true);
            BoosterStaticData data = _boosterManager.boostersStaticData[booster];
            icon.sprite = data.icon;
        }

        void OnBoosterDeactivated(Booster booster)
        {
            Debug.LogError("Deactivate Booster icon: " + booster.ToString());
            icon.gameObject.SetActive(false);
        }
    }
}
