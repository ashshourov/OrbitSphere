// QUICK REFERENCE GUIDE FOR ORBITSPHERE

// ===== CHANGING SCENES PROGRAMMATICALLY =====
SceneFlowController.Instance.ChangeScene(AppSceneState.Title);
SceneFlowController.Instance.ChangeScene(AppSceneState.Orbit);
SceneFlowController.Instance.ChangeScene(AppSceneState.Detail);

// ===== LISTENING TO SCENE CHANGES =====
SceneFlowController.Instance.OnSceneChanged += (scene) => {
    Debug.Log("Scene changed to: " + scene);
};

// ===== FADING UI ELEMENTS =====
CanvasGroup group = GetComponent<CanvasGroup>();
StartCoroutine(TransitionUtility.Fade(group, 0, 1, 1f)); // Fade in over 1 second
StartCoroutine(TransitionUtility.Fade(group, 1, 0, 0.5f)); // Fade out over 0.5 seconds

// ===== MOVING OBJECTS SMOOTHLY =====
Transform obj = GetComponent<Transform>();
StartCoroutine(TransitionUtility.Move(obj, new Vector3(5, 0, 0), 2f)); // Move over 2 seconds

// ===== FADING RENDERERS =====
Renderer renderer = GetComponent<Renderer>();
StartCoroutine(SceneFadeHelper.FadeRenderer(renderer, 1, 0, 1f)); // Fade renderer alpha from 1 to 0

// ===== ROTATING AROUND A POINT =====
// Already implemented in OrbitItem.cs
// To add to any object:
Transform center = GetComponent<Transform>();
transform.RotateAround(center.position, Vector3.up, speed * Time.deltaTime);

// ===== DETECTING SPHERE SELECTION =====
// Already implemented in OrbitItem.cs
// void OnMouseDown() is called when clicking on a collider
void OnMouseDown()
{
    SceneOrbitModule.Instance.Select(this);
}

// ===== ACCESSING SELECTED TRANSFORM =====
Transform selected = SelectionContext.SelectedTransform;

// ===== SHOWING/HIDING DETAIL OBJECTS =====
SphereDetailDisplay display = GetComponent<SphereDetailDisplay>();
display.ShowDetails();  // Activates all detail objects
display.HideDetails();  // Deactivates all detail objects

// ===== TIMING AND DELAYS =====
StartCoroutine(DelayedAction(2f, () => Debug.Log("This runs after 2 seconds")));

IEnumerator DelayedAction(float delay, System.Action action)
{
    yield return new WaitForSeconds(delay);
    action?.Invoke();
}

// ===== MATERIAL COLOR/ALPHA MANIPULATION =====
Color c = renderer.material.color;
c.a = 0.5f; // Set alpha to 50%
renderer.material.color = c;

// ===== CANVAS AND UI SETUP =====
// Make sure Canvas has CanvasGroup component for fading
// Image components don't fade well - use CanvasGroup instead
// Set Canvas to "Screen Space - Overlay" for permanent UI

// ===== DEBUGGING SCENE TRANSITIONS =====
// Add this to any scene module to see transitions in action
public IEnumerator Enter()
{
    Debug.Log("Entering: " + SceneType);
    gameObject.SetActive(true);
    // ... rest of enter logic
    yield break;
}

public IEnumerator Exit()
{
    Debug.Log("Exiting: " + SceneType);
    // ... exit logic
    yield break;
}

// ===== COROUTINE CHAINING =====
public IEnumerator ComplexTransition()
{
    // Do first action
    yield return TransitionUtility.Fade(canvasGroup1, 0, 1, 1f);
    
    // Do second action
    yield return TransitionUtility.Move(sphere, targetPos, 1f);
    
    // Do third action
    yield return TransitionUtility.Fade(canvasGroup2, 0, 1, 1f);
}
