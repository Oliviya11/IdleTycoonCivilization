using Assets.Scripts.Infrastracture.States;
using Assets.Scripts.Services;
using Assets.Scripts.Sources;
using UnityEngine;

namespace Assets.Scripts.Core.Sources.Services
{
    public interface ISourcesManager : IService
    {
        void OpenSource(Product product, Source position);

        bool IsProductOpened(Product product);

        Source GetSource(Product product);
    }
}
