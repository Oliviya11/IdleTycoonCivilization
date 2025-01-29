using Assets.Scripts.Services;
using Assets.Scripts.Sources;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Sources.Services
{
    public class SourcesManager : ISourcesManager
    {
        Dictionary<Product, Vector3> _sources = new();

        public void OpenSource(Product product, Vector3 position)
        {
            _sources[product] = position;
        }

        public bool IsProductOpened(Product product) => _sources.ContainsKey(product);

        public Vector3 GetSourcePosition(Product product) => _sources[product];
    }
}
