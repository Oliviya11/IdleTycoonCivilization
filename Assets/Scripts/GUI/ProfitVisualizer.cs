using Assets.Scripts.Core.Sources;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GUI
{
    public class ProfitVisualizer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI profitText;
        public SourceUpgrade SourceUpgrade { get; set; }

        void Update()
        {
            if (SourceUpgrade != null)
            {
                profitText.text = SourceUpgrade.CurrentProfit;
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
