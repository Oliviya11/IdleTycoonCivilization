using Assets.Scripts.Services.SaveLoad;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Infrastracture
{
    public class AutoSave : MonoBehaviour
    {
        const float START_AUTO_SAVE = 1f;
        const float AUTO_SAVE_INTERVAL = 10f;

        public void Start()
        {
            InvokeRepeating(nameof(SaveProgress), START_AUTO_SAVE, AUTO_SAVE_INTERVAL);
        }

        void OnApplicationQuit()
        {
            SaveProgress();
        }

        void OnApplicationPause()
        {
            SaveProgress();
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus) SaveProgress();
        }

        void SaveProgress()
        {
            if (AllServices.Container.Single<ISaveLoadService>() == null) return;
            AllServices.Container.Single<ISaveLoadService>().SaveProgress();
        }
    }
}
