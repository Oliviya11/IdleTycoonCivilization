using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastracture.AssetManagement
{
    public interface IAssetProvider : IService
    {
        public GameObject Instantiate(string path);

        public GameObject Instantiate(string path, Vector3 at);

        public GameObject Instantiate(string path, Vector3 at, float angle);
    }
}
