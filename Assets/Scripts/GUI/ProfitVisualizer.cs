using Assets.Scripts.Core.Sources;
using TMPro;
using UnityEngine;
using static Assets.Scripts.Core.ClientsNPCMechanics.ProducersNPCManager;

namespace Assets.Scripts.GUI
{
    public class ProfitVisualizer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI profitText;
        [SerializeField] TextMeshProUGUI profitModifierText;
        public SourceUpgrade SourceUpgrade { get; set; }
        public ProfitModifier ProfitModifier { get; set; }

        void Update()
        {
            if (SourceUpgrade != null)
            {
                profitText.text = SourceUpgrade.CurrentProfit;
                if (ProfitModifier.modifier == "1")
                {
                    profitModifierText.gameObject.SetActive(false);
                }
                else
                {
                    profitModifierText.text = "x" + ProfitModifier.modifier;
                    profitModifierText.gameObject.SetActive(true);
                }
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
