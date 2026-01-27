using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneDetailModule : MonoBehaviour, ISceneModule
{
    public AppSceneState SceneType => AppSceneState.Detail;

    [SerializeField] private Transform displayPoint;
    [SerializeField] private CanvasGroup extraGroup;
    [SerializeField] private SphereDetailDisplay detailDisplay;
    [SerializeField] private Button restartButton;
    [SerializeField] private CanvasGroup orbitsCanvasGroup; // Reference to hide orbits

    private Vector3 fixedDisplayPointPosition;  // Store position at startup to avoid canvas scaling issues

    void Start()
    {
        // CRITICAL: Store displayPoint position at startup (before any canvas resizing)
        // If displayPoint is null or in wrong space, default to world center (0,0,0)
        if (displayPoint != null)
        {
            fixedDisplayPointPosition = displayPoint.position;
            Debug.Log($"📍 Fixed display point position stored: {fixedDisplayPointPosition}");
            
            // WARN if position looks like it's in UI/Canvas space (very large numbers)
            if (Mathf.Abs(fixedDisplayPointPosition.x) > 100 || Mathf.Abs(fixedDisplayPointPosition.y) > 100)
            {
                Debug.LogWarning("⚠️ WARNING: DisplayPoint position seems to be in Canvas/UI space, not world space!");
                Debug.LogWarning("⚠️ Make sure DisplayPoint is a world-space object, NOT a child of Canvas!");
                Debug.LogWarning("⚠️ Setting to world center (0,0,0) instead");
                fixedDisplayPointPosition = Vector3.zero;
            }
        }
        else
        {
            // Default to world center if displayPoint not assigned
            fixedDisplayPointPosition = Vector3.zero;
            Debug.LogWarning("⚠️ DisplayPoint not assigned! Using world center (0,0,0)");
        }
        // Ensure restart button is wired up
        if (restartButton != null)
        {
            // Remove any existing listeners first
            restartButton.onClick.RemoveAllListeners();
            // Add the restart listener
            restartButton.onClick.AddListener(Restart);
            Debug.Log("✅ SceneDetailModule: Restart button listener added");
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
        Debug.Log("🎬 SceneDetailModule: Entering Detail state");
        Debug.Log($"📍 Selected sphere: {(SelectionContext.SelectedTransform != null ? SelectionContext.SelectedTransform.gameObject.name : "NONE")}");
        
        // Disable ALL Orbit2D scripts IMMEDIATELY (stop all rotations)
        var allOrbits = FindObjectsOfType<Orbit2D>();
        foreach (var orbit in allOrbits)
        {
            if (orbit != null)
            {
                orbit.enabled = false;
                Debug.Log($"⏹️ Disabled Orbit2D: {orbit.gameObject.name}");
            }
        }
        
        // Disable ALL OrbitVisualizer2D scripts (stop drawing orbit paths)
        var allVisualizers = FindObjectsOfType<OrbitVisualizer2D>();
        foreach (var visualizer in allVisualizers)
        {
            if (visualizer != null)
            {
                visualizer.enabled = false;
                var renderer = visualizer.GetComponent<LineRenderer>();
                if (renderer != null)
                    renderer.enabled = false;
                Debug.Log($"🚫 Disabled OrbitVisualizer2D: {visualizer.gameObject.name}");
            }
        }
        
        // Fade out and HIDE the orbits canvas group completely
        if (orbitsCanvasGroup != null)
        {
            orbitsCanvasGroup.blocksRaycasts = false;
            orbitsCanvasGroup.interactable = false;
            orbitsCanvasGroup.alpha = 0;  // IMMEDIATE hide
            Debug.Log("🌙 Orbits canvas group hidden immediately");
        }
        else
        {
            Debug.LogWarning("⚠️ orbitsCanvasGroup not assigned - orbit UI may still be visible");
        }
        
        // Hide OTHER sphere renderers (keep selected visible for movement animation)
        var allSpheres = FindObjectsOfType<SphereClickHandler>();
        foreach (var sphereHandler in allSpheres)
        {
            // Skip the selected sphere - we want to see it move!
            if (sphereHandler.transform == SelectionContext.SelectedTransform)
            {
                Debug.Log($"✨ Keeping {sphereHandler.gameObject.name} visible for movement");
                continue;
            }
            
            var renderer = sphereHandler.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
                Debug.Log($"👁️ Hidden renderer: {sphereHandler.gameObject.name}");
            }
        }

        // Move selected sphere to display point - VISIBLE MOVEMENT
        if (SelectionContext.SelectedTransform != null)
        {
            Debug.Log($"🚀 Moving sphere {SelectionContext.SelectedTransform.gameObject.name} from {SelectionContext.SelectedTransform.position} to {fixedDisplayPointPosition}");
            yield return TransitionUtility.Move(
                SelectionContext.SelectedTransform,
                fixedDisplayPointPosition,
                1.2f  // Slightly longer for smooth animation
            );
            Debug.Log($"✅ Sphere reached display point");
        }

        // Show detail display (cube, info, etc)
        if (detailDisplay != null)
            detailDisplay.ShowDetails();

        // Fade in the detail UI and ENABLE RAYCASTS
        if (extraGroup != null)
        {
            extraGroup.blocksRaycasts = true;  // CRITICAL: Allow button clicks
            yield return TransitionUtility.Fade(extraGroup, 0, 1, 0.8f);
            Debug.Log("✅ Detail UI visible and clickable");
        }
        
        if (restartButton != null)
        {
            restartButton.interactable = true;
            Debug.Log("✅ Restart button enabled");
        }
            
        Debug.Log("✅ SceneDetailModule: Detail state ready");
    }

    public IEnumerator Exit()
    {
        Debug.Log("🚪 SceneDetailModule: Exiting Detail state");
        
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
            Debug.Log($"📍 Moving sphere back to origin");
            
            // First, reset position to origin
            SelectionContext.SelectedTransform.position = Vector3.zero;
            Debug.Log($"✓ {SelectionContext.SelectedTransform.gameObject.name} position reset to (0,0,0)");
            
            // Show the selected sphere renderer
            var selectedRenderer = SelectionContext.SelectedTransform.GetComponent<Renderer>();
            if (selectedRenderer != null)
            {
                selectedRenderer.enabled = true;
                Debug.Log($"👁️ Showed renderer: {SelectionContext.SelectedTransform.gameObject.name}");
            }
            
            // Then re-enable Orbit2D so it recalculates angle at correct position
            var orbitScript = SelectionContext.SelectedTransform.GetComponent<Orbit2D>();
            if (orbitScript != null)
            {
                orbitScript.enabled = true;
                Debug.Log($"▶️ Re-enabled Orbit2D on {SelectionContext.SelectedTransform.gameObject.name}");
            }
            
            yield return new WaitForSeconds(0.2f);  // Small delay to ensure Orbit2D recalculates
        }

        // Show all other sphere renderers
        var allSpheres = FindObjectsOfType<SphereClickHandler>();
        foreach (var sphereHandler in allSpheres)
        {
            var renderer = sphereHandler.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = true;
                Debug.Log($"👁️ Showed renderer: {sphereHandler.gameObject.name}");
            }
        }
        
        // Re-enable ALL OrbitVisualizer2D scripts (show orbit paths again)
        var allVisualizers = FindObjectsOfType<OrbitVisualizer2D>();
        foreach (var visualizer in allVisualizers)
        {
            if (visualizer != null)
            {
                visualizer.enabled = true;
                var renderer = visualizer.GetComponent<LineRenderer>();
                if (renderer != null)
                    renderer.enabled = true;
                Debug.Log($"✨ Re-enabled OrbitVisualizer2D: {visualizer.gameObject.name}");
            }
        }
        
        // Re-enable ALL Orbit2D scripts (resume rotations for all spheres)
        var allOrbits = FindObjectsOfType<Orbit2D>();
        foreach (var orbit in allOrbits)
        {
            if (orbit != null)
            {
                orbit.enabled = true;
                Debug.Log($"▶️ Re-enabled Orbit2D: {orbit.gameObject.name}");
            }
        }
        
        // Fade in the orbits canvas group if it exists (for UI elements)
        if (orbitsCanvasGroup != null)
        {
            orbitsCanvasGroup.alpha = 1;  // Immediately show
            orbitsCanvasGroup.blocksRaycasts = true;
            Debug.Log("🌞 Orbits canvas group shown");
        }
        
        Debug.Log("✅ Exited Detail state");
    }

    public void Restart()
    {
        Debug.Log("🔄 SceneDetailModule: Restart button pressed!");
        
        if (SceneFlowController.Instance != null)
        {
            Debug.Log("✅ Transitioning to Title");
            SceneFlowController.Instance.ChangeScene(AppSceneState.Title);
        }
        else
        {
            Debug.LogError("❌ SceneDetailModule: SceneFlowController.Instance is null!");
        }
    }
}
