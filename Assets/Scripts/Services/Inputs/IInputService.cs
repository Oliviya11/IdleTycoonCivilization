using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services.Inputs
{
    public interface IInputService : IService
    {
        public event Action<Vector2> OnClick;
        public event Action<float> OnDrag;
        public void ProcessInput();
    }
}
