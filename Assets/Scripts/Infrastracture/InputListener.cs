using Assets.Scripts.Services.Inputs;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastracture
{
    public class InputListener : MonoBehaviour
    {
        public void Update()
        {
            if (AllServices.Container.Single<IInputService>() == null) return;
            AllServices.Container.Single<IInputService>().ProcessInput();
        }
    }
}
