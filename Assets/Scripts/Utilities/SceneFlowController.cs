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

        HideAllElements();

        scenes = new Dictionary<AppSceneState, ISceneModule>();

        if (titleModule != null)
            scenes[AppSceneState.Title] = titleModule;

        if (orbitModule != null)
            scenes[AppSceneState.Orbit] = orbitModule;

        if (detailModule != null)
            scenes[AppSceneState.Detail] = detailModule;
    }

    void Start()
    {
        if (scenes.Count == 3)
            StartCoroutine(SwitchScene(AppSceneState.Title));
    }

    public void ChangeScene(AppSceneState target)
    {
        if (target != currentState)
            StartCoroutine(SwitchScene(target));
    }

    IEnumerator SwitchScene(AppSceneState target)
    {
        if (!scenes.ContainsKey(target))
            yield break;

        if (scenes.ContainsKey(currentState))
            yield return scenes[currentState].Exit();

        currentState = target;
        OnSceneChanged?.Invoke(target);

        yield return scenes[target].Enter();
    }

    private void HideAllElements()
    {
        // Hide all spheres
        var allTransforms = FindObjectsOfType<Transform>();
        foreach (var t in allTransforms)
        {
            if (t.name.Contains("Sphere"))
            {
                var r = t.GetComponent<Renderer>();
                if (r != null) r.enabled = false;
            }
        }

        // Disable all orbits
        foreach (var orbit in FindObjectsOfType<Orbit2D>())
            orbit.enabled = false;

        // Disable all visualizers
        foreach (var viz in FindObjectsOfType<OrbitVisualizer2D>())
        {
            viz.enabled = false;
            var lr = viz.GetComponent<LineRenderer>();
            if (lr != null) lr.enabled = false;
        }

        // Hide UI
        foreach (var cg in FindObjectsOfType<CanvasGroup>())
        {
            cg.alpha = 0;
            cg.blocksRaycasts = false;
        }
    }
}
