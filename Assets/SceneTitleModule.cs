using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SceneTitleModule : MonoBehaviour, ISceneModule
{
    public AppSceneState SceneType => AppSceneState.Title;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup orbitsCanvasGroup;
    [SerializeField] private CanvasGroup detailCanvasGroup;
    [SerializeField] private float fadeTime = 1.5f;

    void OnEnable()
    {
        Debug.Log($"✓ SceneTitleModule enabled on {gameObject.name}");
        if (canvasGroup == null)
            Debug.LogWarning("⚠️ SceneTitleModule: title canvasGroup not assigned!");
    }

    public IEnumerator Enter()
    {
        Debug.Log("🎬 SceneTitleModule: Entering Title state");
        
        // HIDE ALL SPHERES for Scene 1
        var allSpheres = FindObjectsOfType<Transform>();
        foreach (var sphere in allSpheres)
        {
            if (sphere.gameObject.name.Contains("Sphere"))
            {
                var renderer = sphere.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
        }

        // Disable all orbits
        var orbits = FindObjectsOfType<Orbit2D>();
        foreach (var orbit in orbits)
        {
            if (orbit != null)
                orbit.enabled = false;
        }

        // Disable all visualizers and their line renderers
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
        if (orbitsCanvasGroup != null)
        {
            orbitsCanvasGroup.alpha = 0;
            orbitsCanvasGroup.blocksRaycasts = false;
        }

        if (detailCanvasGroup != null)
        {
            detailCanvasGroup.alpha = 0;
            detailCanvasGroup.blocksRaycasts = false;
        }
        
        if (canvasGroup == null)
        {
            Debug.LogError("❌ SceneTitleModule: canvasGroup is null! Cannot fade.");
            yield break;
        }

        // FADE IN ONLY TITLE TEXT
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = true;
        
        float t = 0f;
        while(t < fadeTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }
        canvasGroup.alpha = 1;
        Debug.Log("✅ Title fade in complete - only title visible");

        // Wait 2 seconds then move to orbit scene automatically
        yield return new WaitForSeconds(2f);
        
        if (SceneFlowController.Instance != null)
        {
            Debug.Log("🔄 SceneTitleModule: Auto-transitioning to Orbit");
            SceneFlowController.Instance.ChangeScene(AppSceneState.Orbit);
        }
        else
        {
            Debug.LogError("❌ SceneTitleModule: SceneFlowController.Instance is null!");
        }
    }

    public IEnumerator Exit()
    {
        Debug.Log("📉 SceneTitleModule: Exiting Title state");
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
            yield return TransitionUtility.Fade(canvasGroup, 1, 0, 0.5f);
        }
        else
            yield return new WaitForSeconds(0.5f);
    }
}
