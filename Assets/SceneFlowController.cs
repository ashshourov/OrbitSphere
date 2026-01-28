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

        // IMMEDIATELY HIDE ALL ELEMENTS AT STARTUP
        HideAllElements();

        scenes = new Dictionary<AppSceneState, ISceneModule>();
        
        // Manually assign the three modules
        if (titleModule != null)
        {
            scenes[AppSceneState.Title] = titleModule;
            Debug.Log("✅ Title module registered");
        }
        else
        {
            Debug.LogError("❌ Title module not assigned! Drag the SceneTitle GameObject here.");
        }

        if (orbitModule != null)
        {
            scenes[AppSceneState.Orbit] = orbitModule;
            Debug.Log("✅ Orbit module registered");
        }
        else
        {
            Debug.LogError("❌ Orbit module not assigned! Drag the OrbitSceneModule GameObject here.");
        }

        if (detailModule != null)
        {
            scenes[AppSceneState.Detail] = detailModule;
            Debug.Log("✅ Detail module registered");
        }
        else
        {
            Debug.LogError("❌ Detail module not assigned! Drag the SceneDetailModule GameObject here.");
        }

        Debug.Log($"📊 SceneFlowController: Registered {scenes.Count}/3 modules");
    }

    void HideAllElements()
    {
        // Hide all spheres
        var allSpheres = FindObjectsOfType<Transform>();
        foreach (var sphere in allSpheres)
        {
            if (sphere.gameObject.name.Contains("Sphere"))
            {
                var renderer = sphere.GetComponent<Renderer>();
                if (renderer != null)
                    renderer.enabled = false;
            }
        }

        // Disable all orbits
        var orbits = FindObjectsOfType<Orbit2D>();
        foreach (var orbit in orbits)
        {
            if (orbit != null)
                orbit.enabled = false;
        }

        // Disable all visualizers
        var visualizers = FindObjectsOfType<OrbitVisualizer2D>();
        foreach (var viz in visualizers)
        {
            if (viz != null)
            {
                viz.enabled = false;
                var lineRenderer = viz.GetComponent<LineRenderer>();
                if (lineRenderer != null)
                    lineRenderer.enabled = false;
            }
        }

        // Hide canvas groups
        var allCanvasGroups = FindObjectsOfType<CanvasGroup>();
        foreach (var cg in allCanvasGroups)
        {
            cg.alpha = 0;
            cg.blocksRaycasts = false;
        }
    }

    void Start()
    {
        if (scenes.Count == 3)
        {
            StartCoroutine(SwitchScene(AppSceneState.Title));
        }
        else
        {
            Debug.LogError("❌ Cannot start - not all 3 modules assigned!");
        }
    }

    public void ChangeScene(AppSceneState target)
    {
        Debug.Log($"🔄 Requesting scene change: {currentState} → {target}");
        if (target != currentState)
            StartCoroutine(SwitchScene(target));
    }

    IEnumerator SwitchScene(AppSceneState target)
    {
        if (!scenes.ContainsKey(target))
        {
            Debug.LogError($"❌ Scene {target} not registered!");
            yield break;
        }

        Debug.Log($"🚪 Exiting {currentState}...");
        
        // Exit current scene
        if (scenes.ContainsKey(currentState))
            yield return scenes[currentState].Exit();

        // Enter new scene
        currentState = target;
        Debug.Log($"➡️ Entering {target}...");
        
        OnSceneChanged?.Invoke(target);
        yield return scenes[target].Enter();
        Debug.Log($"✅ {target} state active");
    }
}
