using Assets.Scripts.Infrastracture.Factory;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GUI.Popups
{
    public abstract class Popup : MonoBehaviour
    {
        private static Dictionary<string, Action<Popup>> cachedCallbacks = new Dictionary<string, Action<Popup>>();
        private static Dictionary<string, bool> nameToIsPoolable = new Dictionary<string, bool>();

        public virtual void Awake()
        {
            CallCallback();
        }

        public virtual void OnDestroy() { 
        }

        public void CallCallback()
        {
            string prefabName = GetPrefabName();
            if (cachedCallbacks.ContainsKey(prefabName))
            {
                cachedCallbacks[GetPrefabName()](this);
                cachedCallbacks.Remove(prefabName);
            }
        }

        public void Hide()
        {
            string name = GetPrefabName();
            bool isPoolable = nameToIsPoolable[name];
            if (isPoolable)
            {
                // TODO: implement remove from pool;
                nameToIsPoolable.Remove(name);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void LoadPopUp(IGameFactory gameFactory, string name, Action<Popup> callback, bool isPoolable, Vector3 at)
        {
            cachedCallbacks[name] = callback;
            nameToIsPoolable[name] = isPoolable;

            if (isPoolable)
            {
                // TODO: implement create from pool
            }
            else
            {
                gameFactory.CreatePopUp(name, at);
                AllServices.Container.Single<IAudioManager>().PlayPopupOpened();
            }
        }

        protected abstract string GetPrefabName();

    }
}
