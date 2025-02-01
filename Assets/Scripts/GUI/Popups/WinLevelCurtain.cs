using Assets.Scripts.Infrastracture.Factory;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GUI.Popups
{
    public class WinLevelCurtain : MonoBehaviour
    {
        public CanvasGroup Curtain;

        void Awake()
        {
            DontDestroyOnLoad(this);
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            Curtain.alpha = 1;
        }

        public void Hide() => StartCoroutine(DoFadeIn());

        private IEnumerator DoFadeIn()
        {
            while (Curtain.alpha > 0)
            {
                Curtain.alpha -= 0.03f;
                yield return new WaitForSeconds(0.03f);
            }

            gameObject.SetActive(false);
        }
    }
}
