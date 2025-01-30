using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI
{
    public class ProgressBar : MonoBehaviour
    {
        public Image ImageCurrent;

        public void SetValue(float current, float max)
        {
            ImageCurrent.fillAmount = current / max;
        }
    }
}
