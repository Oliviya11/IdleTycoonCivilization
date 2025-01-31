
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI
{
    public class ProductVisualizer : MonoBehaviour
    {
        [SerializeField] Image product;

        public void SetSprite(Sprite sprite)
        {
            product.sprite = sprite;
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
