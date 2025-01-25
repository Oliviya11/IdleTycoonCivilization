using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Infrastracture
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}
