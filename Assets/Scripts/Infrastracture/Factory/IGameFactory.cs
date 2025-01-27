using Assets.Scripts.Services;
using Assets.Scripts.Sources;
using UnityEngine;

namespace Assets.Scripts.Infrastracture.Factory
{
    internal interface IGameFactory : IService
    {
        SourcesCollection CreateSourcesCollection(int level);

        GameObject CreateProducer(Vector3 at, float angle);

        GameObject CreateHud();

        GameObject CreatePumpkin(Vector3 at);

        GameObject CreatePopUp(string name);

        GameObject CreatePopUp(string name, Vector3 at);
    }
}
