using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SceneDetailModule : MonoBehaviour, ISceneModule
{
    public AppSceneState SceneType => AppSceneState.Detail;

    [SerializeField] private Transform displayPoint;
    [SerializeField] private CanvasGroup extraGroup;
    [SerializeField] private SphereDetailDisplay detailDisplay;
    [SerializeField] private Button restartButton;
    [SerializeField] private CanvasGroup orbitsCanvasGroup; // Reference to hide orbits

    void Start()
    {
        // Ensure restart button is wired up
        if (restartButton != null)
        {
            // Remove any existing listeners first
            restartButton.onClick.RemoveAllListeners();
            // Add the restart listener
            restartButton.onClick.AddListener(Restart);
        }
        else
        {
            Debug.LogError("❌ SceneDetailModule: Restart button not assigned! Drag the Restart Button here.");
        }

        if (displayPoint == null)
            Debug.LogError("❌ SceneDetailModule: Display point not assigned!");
        if (extraGroup == null)
            Debug.LogError("❌ SceneDetailModule: Extra group (ExtraObjects) not assigned!");
        if (orbitsCanvasGroup == null)
            Debug.LogError("❌ SceneDetailModule: Orbits canvas group (TimelineUI) not assigned!");
    }

    public IEnumerator Enter()
    {
        // Disable all Orbit2D scripts
        var orbits = FindObjectsOfType<Orbit2D>();
        foreach (var orbit in orbits)
        {
            if (orbit != null)
            {
                orbit.enabled = false;
            }
        }

        // Disable all OrbitVisualizer2D scripts and their LineRenderers
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

        // IMMEDIATELY DISABLE all other sphere renderers
        var allSpheres = FindObjectsOfType<Transform>();
        foreach (var sphere in allSpheres)
        {
            if (sphere.gameObject.name.Contains("Sphere") && sphere != SelectionContext.SelectedTransform)
            {
                var renderer = sphere.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
        }

        // Hide orbits/spheres canvas
        if (orbitsCanvasGroup != null)
        {
            orbitsCanvasGroup.blocksRaycasts = false;
            yield return TransitionUtility.Fade(orbitsCanvasGroup, 1, 0, 0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        // Move selected sphere to display point
        if (SelectionContext.SelectedTransform != null)
        {
            yield return TransitionUtility.Move(
                SelectionContext.SelectedTransform,
                displayPoint.position,
                1f
            );
        }

        // Show detail display (cube, info, etc)
        if (detailDisplay != null)
            detailDisplay.ShowDetails();

        // Fade in the detail UI and ENABLE RAYCASTS
        if (extraGroup != null)
        {
            extraGroup.blocksRaycasts = true;  // CRITICAL: Allow button clicks
            yield return TransitionUtility.Fade(extraGroup, 0, 1, 1f);
        }
        
        if (restartButton != null)
        {
            restartButton.interactable = true;
        }
            
    }

    public IEnumerator Exit()
    {
        if (detailDisplay != null)
            detailDisplay.HideDetails();

        if (extraGroup != null)
        {
            extraGroup.blocksRaycasts = false;  // Disable button interaction
            yield return TransitionUtility.Fade(extraGroup, 1, 0, 0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        // Move sphere back to origin
        if (SelectionContext.SelectedTransform != null)
        {
            yield return TransitionUtility.Move(
                SelectionContext.SelectedTransform,
                Vector3.zero,
                0.5f
            );
        }

        // IMMEDIATELY RE-ENABLE all other sphere renderers
        var allSpheres = FindObjectsOfType<Transform>();
        foreach (var sphere in allSpheres)
        {
            if (sphere.gameObject.name.Contains("Sphere") && sphere != SelectionContext.SelectedTransform)
            {
                var renderer = sphere.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
            }
        }

        // Fade in the orbits canvas
        if (orbitsCanvasGroup != null)
        {
            orbitsCanvasGroup.blocksRaycasts = true;
            yield return TransitionUtility.Fade(orbitsCanvasGroup, 0, 1, 0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        // Re-enable all Orbit2D scripts
        var orbits = FindObjectsOfType<Orbit2D>();
        foreach (var orbit in orbits)
        {
            if (orbit != null)
            {
                orbit.enabled = true;
            }
        }

        // Re-enable all OrbitVisualizer2D scripts
        var visualizers = FindObjectsOfType<OrbitVisualizer2D>();
        foreach (var viz in visualizers)
        {
            if (viz != null)
            {
                viz.enabled = true;
                var lineRenderer = viz.GetComponent<LineRenderer>();
                if (lineRenderer != null)
                    lineRenderer.enabled = true;
            }
        }
    }

    public void Restart()
    {
        if (SceneFlowController.Instance != null)
        {
            SceneFlowController.Instance.ChangeScene(AppSceneState.Title);
        }
        else
        {
            Debug.LogError("❌ SceneDetailModule: SceneFlowController.Instance is null!");
        }
    }
}
