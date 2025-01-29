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
    }
}
