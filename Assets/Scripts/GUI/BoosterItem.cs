using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GUI
{
    public class BoosterItem : MonoBehaviour
    {
        [SerializeField] UnityEngine.UI.Button useButton;
        [SerializeField] UnityEngine.UI.Image icon;
        [SerializeField] TextMeshProUGUI description;
        [SerializeField] TextMeshProUGUI number;

        public class Params
        {
            public UnityAction OnUseButtonClick;
            public Sprite _sprite;
            public string _description;
            public int _number;

            public Params(UnityAction onUseButtonClick, Sprite sprite, string description, int number)
            {
                OnUseButtonClick = onUseButtonClick;
                _sprite = sprite;
                _description = description;
                _number = number;
            }
        }

        private void OnDestroy()
        {
            useButton.onClick.RemoveAllListeners();
        }

        public void Construct(Params p)
        {
            description.text = p._description;
            number.text = p._number.ToString();
            icon.sprite = p._sprite;
            useButton.onClick.AddListener(p.OnUseButtonClick);
        }
    }
}
