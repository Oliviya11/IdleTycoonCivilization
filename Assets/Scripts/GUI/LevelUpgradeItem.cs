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
            public readonly string _description;
            public readonly string _title;
            public readonly string _price;
            public readonly Sprite _sprite;
            public readonly UnityAction _onButtonClick;
            public readonly Func<bool> _isUpgradeAvaialble;

            public Params(string description, string title, string price, Sprite sprite, UnityAction onButtonClick, Func<bool> isUpgradeAvaialble)
            {
                _description = description;
                _title = title;
                _price = price;
                _sprite = sprite;
                _onButtonClick = onButtonClick;
                _isUpgradeAvaialble = isUpgradeAvaialble;
            }
        }

        public void Update()
        {
            if (@params._isUpgradeAvaialble == null) return;

            button.enabled = @params._isUpgradeAvaialble();
            button.UpdateOnState(buttonText);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        public void Construct(Params p)
        {
            @params = p;
            description.text = p._description;
            title.text = p._title;
            buttonText.text = p._price;
            image.sprite = p._sprite;
            button.onClick.AddListener(p._onButtonClick);
        }
    }
}
