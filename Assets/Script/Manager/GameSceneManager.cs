using System;
using System.Collections;
using System.Collections.Generic;
using Script.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Script.Manager
{
    public class GameSceneManager : Singleton<GameSceneManager>
    {
        private float _progress;

        public IEnumerator LoadScene(string sceneName, Action<float> progressEvent = null, Action finishAction = null)
        {
            // 비동기적으로 Scene 로드
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // 0에서 1 사이 값으로 정규화
                _progress = progress * 100f;
                progressEvent?.Invoke(_progress);
                yield return null;
            }
            finishAction?.Invoke();
        }
    }
}