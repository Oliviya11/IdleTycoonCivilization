using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Sources
{
    public class SourceClick : MonoBehaviour
    {
        [SerializeField] Source source;
        [SerializeField] Button blankButton;
        [SerializeField] Button sourceIconButton;
        [SerializeField] Button upgradeButton;

        public event Action<Source> OnBlankClick;
        public event Action OnSourceIconClick;
        public event Action OnUpgradeClick;

        public void Awake()
        {
            blankButton.onClick.AddListener(InvokeOnBlankClick);
            sourceIconButton.onClick.AddListener(InvokeOnSourceIconClick);
            upgradeButton.onClick.AddListener(InvokeOnUpgradeClick);
        }

        public void OnDestroy()
        {
            blankButton.onClick.RemoveListener(InvokeOnBlankClick);
            sourceIconButton.onClick.RemoveListener(InvokeOnSourceIconClick);
            upgradeButton.onClick.RemoveListener(InvokeOnUpgradeClick);
        }

        void InvokeOnBlankClick()
        {
            OnBlankClick.Invoke(source);
        }

        void InvokeOnSourceIconClick()
        {
            OnSourceIconClick.Invoke();
        }

        void InvokeOnUpgradeClick()
        {
            OnUpgradeClick.Invoke();
        }
    }
}
