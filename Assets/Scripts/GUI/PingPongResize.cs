using UnityEngine;

namespace Assets.Scripts.GUI
{
    public class PingPongResize : MonoBehaviour
    {
        [SerializeField] RectTransform targetRectTransform;
        [SerializeField] Vector2 minSize = new Vector2(100, 100);
        [SerializeField] Vector2 maxSize = new Vector2(200, 200);
        [SerializeField] float speed = 2f;

        private void Update()
        {
            float pingPong = Mathf.PingPong(Time.time * speed, 1);
            float width = Mathf.Lerp(minSize.x, maxSize.x, pingPong);
            float height = Mathf.Lerp(minSize.y, maxSize.y, pingPong);

            targetRectTransform.sizeDelta = new Vector2(width, height);
        }
    }
}
