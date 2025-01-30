using Assets.Scripts.Services.Inputs;
using Assets.Scripts.Sources;
using System;
using UnityEngine;

namespace Assets.Scripts.Core.Sources
{
    public class SourceElementClick : MonoBehaviour
    {
        [SerializeField] Source source;
        public event Action<Source> OnSourceElementClick;

        public void OnMouseDown()
        {
            if (InputService.IsPointerOverUI()) return;
            OnSourceElementClick?.Invoke(source);
        }
    }
}
