using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Infrastracture.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public GameObject Instantiate(string path, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public GameObject Instantiate(string path, Vector3 at, float angle)
        {
            var prefab = Resources.Load<GameObject>(path);
            var gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            gameObject.transform.rotation = Quaternion.Euler(0, angle, 0);
            return gameObject;
        }

        public GameObject Instantiate(string path, Transform parent)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, parent);
        }

        public Sprite LoadProductIcon(string name)
        {
            return Resources.Load<Sprite>($"{AssetPath.ProductIcon}{name}");
        }
    }
}
