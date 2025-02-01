using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Infrastracture.AssetManagement
{
    public interface IAssetProvider : IService
    {
        public GameObject Instantiate(string path);

        public GameObject Instantiate(string path, Transform parent);

        public GameObject Instantiate(string path, Vector3 at);
        public GameObject Instantiate(string path, Vector3 at, Quaternion rotation);

        public GameObject Instantiate(string path, Vector3 at, float angle);

        Sprite LoadProductIcon(string name);
    }
}
