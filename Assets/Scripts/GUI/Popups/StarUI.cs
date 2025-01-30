using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Popups
{
    public class StarUI : MonoBehaviour
    {
        [SerializeField] Image empty;
        [SerializeField] Image full;

        public void ShowEmpty()
        {
            empty.gameObject.SetActive(true);
            full.gameObject.SetActive(false);
        }

        public void ShowFull()
        {
            full.gameObject.SetActive(true);
            empty.gameObject.SetActive(false);
        }
    }
}
