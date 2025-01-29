using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.Core.ClientsNPCMechanics
{
    [RequireComponent(typeof(ClientNPC))]
    public class ReturnToPool : MonoBehaviour
    {
        public IObjectPool<ClientNPC> pool;
        ClientNPC client;

        void Awake()
        {
            client = GetComponent<ClientNPC>();
        }

        void Update()
        {
            if (IsObjectOutOfView(gameObject))
            {
                //pool.Release(client);
            }
        }

        public static bool IsObjectOutOfView(GameObject obj)
        {
            if (obj == null) return true;

            Camera mainCamera = Camera.main;
            if (mainCamera == null) return true;

            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(obj.transform.position);
            bool isOutside = viewportPoint.x < 0 || viewportPoint.x > 1 ||
                             viewportPoint.y < 0 || viewportPoint.y > 1 ||
                             viewportPoint.z < 0; // Object is behind the camera

            return isOutside;
        }
    }
}
