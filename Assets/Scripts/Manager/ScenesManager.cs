using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Manager
{
    public class ScenesManager : MonoBehaviour
    {
        [Required]
        public string sceneName;

        public void LoadScene()
        {
            StartCoroutine(AsyncLoad());
        }

        private IEnumerator AsyncLoad()
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
            async.allowSceneActivation = false;
            while (!async.isDone)
            {
                if (async.progress >= 0.9f)
                {
                    Timer.Register(3f,() => async.allowSceneActivation = true);
                }
                yield return null;
            }
        }
    }
}