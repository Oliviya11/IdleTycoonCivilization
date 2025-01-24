using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastracture
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {

        private Game _game;

        public void Awake()
        {
            _game = new Game();

            DontDestroyOnLoad(this);
        }
    }
}
