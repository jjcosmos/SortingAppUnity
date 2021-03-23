using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class AsyncSceneLoader : MonoBehaviour
{
    private static AsyncSceneLoader _inst;
    public static UnityEvent OnStartSceneLoad;
    
    private void Awake() {
        _inst = this;
        OnStartSceneLoad = new UnityEvent();
    }
    public static void LoadSceneByName(string sceneName)
    {
        if(_inst == null) return;
        OnStartSceneLoad.Invoke();
        _inst.StartCoroutine(_inst.LoadSceneCoroutine(sceneName));
    }

    public static void ReloadCurrent()
    {
        if(_inst == null) return;
        OnStartSceneLoad.Invoke();
        _inst.StartCoroutine(_inst.ReloadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        var sceneLoadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        sceneLoadOp.allowSceneActivation = true;
        while(!sceneLoadOp.isDone)
        {
            yield return null;
        }
        //sceneLoadOp.allowSceneActivation = true;
    }

    private IEnumerator ReloadSceneCoroutine()
    {
        var sceneLoadOp = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        sceneLoadOp.allowSceneActivation = true;
        while (!sceneLoadOp.isDone)
        {
            yield return null;
        }
        //sceneLoadOp.allowSceneActivation = true;
    }
}
