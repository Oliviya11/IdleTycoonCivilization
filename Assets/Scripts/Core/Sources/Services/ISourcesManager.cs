using Assets.Scripts.Infrastracture.States;
using Assets.Scripts.Services;
using Assets.Scripts.Sources;
using UnityEngine;

namespace Assets.Scripts.Core.Sources.Services
{
    public interface ISourcesManager : IService
    {
        void OpenSource(Product product, Vector3 position);

        bool IsProductOpened(Product product);

        Vector3 GetSourcePosition(Product product);
    }
}
