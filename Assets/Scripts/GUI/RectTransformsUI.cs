using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GUI
{
    public class RectTransformsUI : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> rectTransforms;

        public void ShowOnly(int max)
        {
            for (int i = 0; i < rectTransforms.Count; ++i)
            {
                rectTransforms[i].gameObject.SetActive(i >= max);
            }
        }
    }
}
