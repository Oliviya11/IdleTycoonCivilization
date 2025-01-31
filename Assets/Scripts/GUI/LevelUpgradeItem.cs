using Assets.Scripts.Utils;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using Button = UnityEngine.UI.Button;

namespace Assets.Scripts.GUI
{
    public class LevelUpgradeItem : MonoBehaviour
    {
        [SerializeField] RectTransform content;
        [SerializeField] TextMeshProUGUI description;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI buttonText;
        [SerializeField] Button button;
        [SerializeField] Image image;
        Params @params;

        public class Params
        {
            public string description;
            public string title;
            public string price;
            public Sprite sprite;
            public UnityAction onButtonClick;
            public Func<bool> isUpgradeAvaialble;

            public Params(string description, string title, string price, Sprite sprite, UnityAction onButtonClick, Func<bool> isUpgradeAvaialble)
            {
                this.description = description;
                this.title = title;
                this.price = price;
                this.sprite = sprite;
                this.onButtonClick = onButtonClick;
                this.isUpgradeAvaialble = isUpgradeAvaialble;
            }
        }

        public void Update()
        {
            if (@params.isUpgradeAvaialble == null) return;

            button.enabled = @params.isUpgradeAvaialble();
            button.UpdateOnState(buttonText);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        public void Construct(Params p)
        {
            @params = p;
            description.text = p.description;
            title.text = p.title;
            buttonText.text = p.price;
            image.sprite = p.sprite;
            button.onClick.AddListener(p.onButtonClick);
        }
    }
}
