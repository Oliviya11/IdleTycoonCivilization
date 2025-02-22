﻿using Assets.Scripts.Services;
using Assets.Scripts.Sources;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Sources.Services
{
    public class SourcesManager : ISourcesManager
    {
        readonly Dictionary<Product, Source> _sources = new();

        public void OpenSource(Product product, Source source)
        {
            _sources[product] = source;
        }

        public bool IsProductOpened(Product product) => _sources.ContainsKey(product);

        public Source GetSource(Product product) {
            if (!_sources.ContainsKey(product)) return null;
            return _sources[product];
         }
    }
}
