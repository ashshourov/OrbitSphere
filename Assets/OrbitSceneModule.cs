using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbitSceneModule : MonoBehaviour, ISceneModule
{
    public AppSceneState SceneType => AppSceneState.Orbit;

    [SerializeField] private CanvasGroup orbitsCanvasGroup;
    [SerializeField] private Transform[] spheres;
    [SerializeField] private Orbit2D[] orbitScripts;
    [SerializeField] public float fadeInTime = 1.5f;

    private List<Orbit2D> discoveredOrbits = new List<Orbit2D>();

    void Start()
    {
        Debug.Log("🌍 OrbitSceneModule: Initializing");
        
        // Discover Orbit2D scripts if not assigned
        if (orbitScripts == null || orbitScripts.Length == 0)
        {
            discoveredOrbits.Clear();
            discoveredOrbits.AddRange(FindObjectsOfType<Orbit2D>());
            Debug.Log($"🔍 OrbitSceneModule: Auto-discovered {discoveredOrbits.Count} Orbit2D scripts");
        }
        else
        {
            discoveredOrbits.AddRange(orbitScripts);
            Debug.Log($"📋 OrbitSceneModule: Using assigned {orbitScripts.Length} Orbit2D scripts");
        }

        // Discover spheres if not assigned
        if (spheres == null || spheres.Length == 0)
        {
            var sphereList = new List<Transform>();
            var allTransforms = FindObjectsOfType<Transform>();
            foreach (var t in allTransforms)
            {
                if (t.gameObject.name.Contains("Sphere"))
                    sphereList.Add(t);
            }
            spheres = sphereList.ToArray();
            Debug.Log($"🔍 OrbitSceneModule: Auto-discovered {spheres.Length} spheres");
        }

        // Add click handlers to spheres
        foreach (var sphere in spheres)
        {
            if (sphere == null) continue;
            
            var handler = sphere.GetComponent<SphereClickHandler>();
            if (handler == null)
                handler = sphere.gameObject.AddComponent<SphereClickHandler>();
            
            handler.OnSphereSelected += HandleSphereSelected;
            Debug.Log($"✓ Added click handler to {sphere.gameObject.name}");
        }
    }

    public IEnumerator Enter()
    {
        Debug.Log("🎬 OrbitSceneModule: Entering Orbit state");
        
        // Enable orbits
        foreach (var orbit in discoveredOrbits)
        {
            if (orbit != null)
            {
                orbit.enabled = true;
                Debug.Log($"▶️ Enabled orbit: {orbit.gameObject.name}");
            }
        }

        // Enable orbit visualizers
        var visualizers = FindObjectsOfType<OrbitVisualizer2D>();
        foreach (var viz in visualizers)
        {
            if (viz != null)
            {
                viz.enabled = true;
                var lineRenderer = viz.GetComponent<LineRenderer>();
                if (lineRenderer != null)
                    lineRenderer.enabled = true;
                Debug.Log($"▶️ Enabled visualizer: {viz.gameObject.name}");
            }
        }

        // Re-enable all sphere renderers (they were disabled in Title scene)
        var allSpheres = FindObjectsOfType<Transform>();
        foreach (var sphere in allSpheres)
        {
            if (sphere.gameObject.name.Contains("Sphere"))
            {
                var renderer = sphere.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
            }
        }

        // Set up parallel fade animations
        if (orbitsCanvasGroup != null)
            orbitsCanvasGroup.alpha = 0;

        foreach (var sphere in allSpheres)
        {
            if (sphere.gameObject.name.Contains("Sphere"))
            {
                var renderer = sphere.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Color col = renderer.material.color;
                    col.a = 0;
                    renderer.material.color = col;
                }
            }
        }

        // Start all fade animations in parallel
        var fadeCoroutines = new List<IEnumerator>();

        if (orbitsCanvasGroup != null)
        {
            orbitsCanvasGroup.blocksRaycasts = true;
            fadeCoroutines.Add(TransitionUtility.Fade(orbitsCanvasGroup, 0, 1, fadeInTime));
        }

        foreach (var sphere in allSpheres)
        {
            if (sphere.gameObject.name.Contains("Sphere"))
            {
                var renderer = sphere.GetComponent<Renderer>();
                if (renderer != null)
                {
                    fadeCoroutines.Add(TransitionUtility.FadeRenderer(renderer, 0, 1, fadeInTime));
                }
            }
        }

        // Start all coroutines simultaneously
        foreach (var fade in fadeCoroutines)
        {
            StartCoroutine(fade);
        }

        yield return new WaitForSeconds(fadeInTime);
        Debug.Log("✅ Orbit state ready - spheres rotating");
    }

    public IEnumerator Exit()
    {
        Debug.Log("🚪 OrbitSceneModule: Exiting Orbit state");
        
        // Disable orbits IMMEDIATELY (don't wait)
        foreach (var orbit in discoveredOrbits)
        {
            if (orbit != null)
            {
                orbit.enabled = false;
                Debug.Log($"⏹️ Disabled orbit: {orbit.gameObject.name}");
            }
        }

        // Fade out orbits visually
        if (orbitsCanvasGroup != null)
        {
            orbitsCanvasGroup.blocksRaycasts = false;
            yield return TransitionUtility.Fade(orbitsCanvasGroup, 1, 0, 0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        Debug.Log("✅ Orbit state exited - orbits disabled");
    }

    private void HandleSphereSelected(Transform selectedSphere)
    {
        Debug.Log($"🎯 Sphere selected: {selectedSphere.gameObject.name}");
        SelectionContext.SelectedTransform = selectedSphere;
        SceneFlowController.Instance.ChangeScene(AppSceneState.Detail);
    }

    void OnDestroy()
    {
        // Clean up handlers
        if (spheres != null)
        {
            foreach (var sphere in spheres)
            {
                if (sphere != null)
                {
                    var handler = sphere.GetComponent<SphereClickHandler>();
                    if (handler != null)
                        handler.OnSphereSelected -= HandleSphereSelected;
                }
            }
        }
    }
}
