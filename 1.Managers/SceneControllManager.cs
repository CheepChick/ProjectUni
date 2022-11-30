using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneControllManager : TSingleton<SceneControllManager>
{
    DefineEnum.eSceneIndex _preScene;
    DefineEnum.eSceneIndex _currScene;

    AsyncOperation _aoper;
    private void Awake()
    {
        Init();
    }
    protected override void Init()
    {
        base.Init();

    }
    public void StartIngameScene()
    {
        _preScene = _currScene;
        _currScene = DefineEnum.eSceneIndex.IngameScene;
        StartCoroutine(LoadingScene(DefineEnum.eSceneIndex.IngameScene.ToString()));
    }
    IEnumerator LoadingScene(string SceneName)
    {
        yield return null;
        _aoper = SceneManager.LoadSceneAsync(SceneName);

    }
}
