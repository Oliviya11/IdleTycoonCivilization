using UnityEngine.UI;
using UnityEngine.UIElements;
using Colors = Assets.Scripts.GUI.Clolors;
using Button = UnityEngine.UI.Button;
using TMPro;

namespace Assets.Scripts.Utils
{
    public static class ButtonExtension
    {
        public static void UpdateOnState(this Button button, TextMeshProUGUI text)
        {
            if (button.enabled)
            {
                button.image.color = Colors.enabledButtonColor;
                text.color = Colors.enabledButtonTextColor;
            }
            else
            {
                button.image.color = Colors.disabledButtonColor;
                text.color = Colors.disabledButtonTextColor;
            }
        }
    }
}
