using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core.ClientsNPCMechanics
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] Image progressBar;
        public float Duration {  
            get => _currentDuration;
            set
            {
                _currentDuration = 0;
                _originalDuration = value;
            } 
        }
        float _currentDuration;
        float _originalDuration;

        public Action Callback { get; set; }

        private void Update()
        {
            if (_currentDuration <= _originalDuration)
            {
                progressBar.fillAmount = _currentDuration / _originalDuration;
                _currentDuration += Time.deltaTime;
            }
            else
            {
                Callback?.Invoke();
                Callback = null;
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
