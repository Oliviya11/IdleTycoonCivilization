using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Infrastracture
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner) =>
          _coroutineRunner = coroutineRunner;

        public void Load(string name, bool canLoadSame, Action onLoaded = null) =>
          _coroutineRunner.StartCoroutine(LoadScene(name, canLoadSame, onLoaded));

        public IEnumerator LoadScene(string nextScene, bool canLoadSame, Action onLoaded = null)
        {
            if (!canLoadSame && SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;

            onLoaded?.Invoke();
        }
    }
}
