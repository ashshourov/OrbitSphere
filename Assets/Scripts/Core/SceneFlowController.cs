using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SceneFlowController : MonoBehaviour
{
    public static SceneFlowController Instance;

    [SerializeField] private SceneTitleModule titleModule;
    [SerializeField] private OrbitSceneModule orbitModule;
    [SerializeField] private SceneDetailModule detailModule;

    public event Action<AppSceneState> OnSceneChanged;

    private Dictionary<AppSceneState, ISceneModule> scenes;
    private AppSceneState currentState = AppSceneState.Title;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        scenes = new Dictionary<AppSceneState, ISceneModule>();
        
        // Manually assign the three modules
        if (titleModule != null)
        {
            scenes[AppSceneState.Title] = titleModule;
        }

        if (orbitModule != null)
        {
            scenes[AppSceneState.Orbit] = orbitModule;
        }

        if (detailModule != null)
        {
            scenes[AppSceneState.Detail] = detailModule;
        }
    }

    void Start()
    {
        if (scenes.Count == 3)
        {
            StartCoroutine(SwitchScene(AppSceneState.Title));
        }
    }

    public void ChangeScene(AppSceneState target)
    {
        if (target != currentState)
            StartCoroutine(SwitchScene(target));
    }

    IEnumerator SwitchScene(AppSceneState target)
    {
        if (!scenes.ContainsKey(target))
        {
            yield break;
        }
        
        // Exit current scene
        if (scenes.ContainsKey(currentState))
            yield return scenes[currentState].Exit();

        // Enter new scene
        currentState = target;
        
        OnSceneChanged?.Invoke(target);
        yield return scenes[target].Enter();
    }
}
