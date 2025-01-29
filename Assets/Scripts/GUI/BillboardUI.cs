using UnityEngine;

namespace Assets.Scripts.GUI
{
    public class BillboardUI : MonoBehaviour
    {
        void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}
