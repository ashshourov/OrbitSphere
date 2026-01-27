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
        
        // Make sure all spheres are active but set alpha to 0 (invisible)
        foreach (var sphere in spheres)
        {
            if (sphere == null) continue;
            sphere.gameObject.SetActive(true);
            
            var renderer = sphere.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Set renderer to fully transparent initially
                Color color = renderer.material.color;
                color.a = 0;
                renderer.material.color = color;
                renderer.enabled = true;
                Debug.Log($"👁️ Sphere prepared (invisible): {sphere.gameObject.name}");
            }
        }
        
        // Enable orbits (they will start rotating immediately but spheres invisible)
        foreach (var orbit in discoveredOrbits)
        {
            if (orbit != null)
            {
                orbit.enabled = true;
                Debug.Log($"▶️ Enabled orbit: {orbit.gameObject.name}");
            }
        }

        // Enable orbit visualizers
        var allVisualizers = FindObjectsOfType<OrbitVisualizer2D>();
        foreach (var visualizer in allVisualizers)
        {
            if (visualizer != null)
            {
                visualizer.enabled = true;
                var renderer = visualizer.GetComponent<LineRenderer>();
                if (renderer != null)
                    renderer.enabled = true;
                Debug.Log($"✨ Enabled OrbitVisualizer2D: {visualizer.gameObject.name}");
            }
        }

        // Start all fades in parallel (orbits canvas + sphere renderers)
        List<IEnumerator> allFades = new List<IEnumerator>();
        
        // Add orbits canvas fade
        if (orbitsCanvasGroup != null)
        {
            orbitsCanvasGroup.alpha = 0;
            orbitsCanvasGroup.blocksRaycasts = true;
            Debug.Log($"📈 Starting orbitsCanvasGroup fade");
            allFades.Add(TransitionUtility.Fade(orbitsCanvasGroup, 0, 1, fadeInTime));
        }
        else
        {
            Debug.LogWarning("⚠️ orbitsCanvasGroup not assigned - skipping visual fade");
            allFades.Add(WaitRoutine(fadeInTime));
        }

        // Add sphere renderer fades
        foreach (var sphere in spheres)
        {
            if (sphere == null) continue;
            var renderer = sphere.GetComponent<Renderer>();
            if (renderer != null)
            {
                Debug.Log($"✨ Starting sphere fade for {sphere.gameObject.name}");
                allFades.Add(TransitionUtility.FadeRenderer(renderer, 0, 1, fadeInTime));
            }
        }

        // Run all fades in parallel
        foreach (var fade in allFades)
        {
            StartCoroutine(fade);
        }

        // Wait for all fades to complete
        yield return new WaitForSeconds(fadeInTime);
        
        Debug.Log("✅ Orbit state ready - spheres rotating and visible");
    }

    private IEnumerator WaitRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
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
