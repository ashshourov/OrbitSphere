# Troubleshooting Guide

## Common Issues & Solutions

### 🔴 Scripts Not Found / Compilation Errors

**Problem:** Red lines in scripts or "Cannot find type" errors

**Solutions:**
1. Make sure all scripts are in the `Assets` folder
2. Check that filenames match class names exactly (case-sensitive)
3. Restart Unity Editor (File → Restart)
4. Check Console for specific error messages

---

### 🔴 App Crashes on Start or Scenes Don't Load

**Problem:** App crashes immediately or scenes are blank

**Solutions:**
1. **Check Build Settings:**
   - Go to File → Build Settings
   - Verify all 3 scenes are added (Title, Orbit, Detail)
   - Title scene should be at index 0 (starts first)

2. **Check SceneFlowController:**
   - In Scene 1, find SceneFlowController GameObject
   - Verify all 3 scene modules are assigned in Inspector
   - Check that references are not null

3. **Check Console for errors:**
   - Look for "NullReferenceException"
   - Check which component/script is failing

---

### 🔴 Spheres Not Rotating

**Problem:** Spheres appear static in Orbit scene

**Solutions:**
1. **Check OrbitItem Script:**
   - Verify each sphere has OrbitItem component
   - Check `speed` value is not 0 (default 40)
   - Verify `center` field is assigned to OrbitCenter

2. **Check OrbitCenter:**
   - Verify OrbitCenter exists in scene
   - Make sure it's at position (0, 0, 0) or consistent position
   - All spheres should reference the same center

3. **Test rotation manually:**
   ```csharp
   // Add to OrbitItem Update() temporarily for debugging
   Debug.Log($"Rotating {name} around {center.name}");
   ```

---

### 🔴 Can't Click/Select Spheres

**Problem:** Clicking spheres doesn't do anything

**Solutions:**
1. **Add Colliders:**
   - Each sphere needs a Collider component
   - Default Sphere already has one, but verify in Inspector

2. **Check Physics Settings:**
   - Edit → Project Settings → Physics
   - Ensure Raycast is enabled

3. **Verify OnMouseDown:**
   - Add debug output:
   ```csharp
   void OnMouseDown()
   {
       Debug.Log($"Clicked: {gameObject.name}");
       SceneOrbitModule.Instance.Select(this);
   }
   ```

4. **Check if SceneOrbitModule exists:**
   - Orbit scene must have GameObject with SceneOrbitModule script
   - It should have `Instance` set in Awake()

---

### 🔴 Fading Not Working Smoothly

**Problem:** Text/Objects appear/disappear instantly instead of fading

**Solutions:**

1. **For UI Text (Use CanvasGroup):**
   - Select Canvas, not the Text element
   - Add CanvasGroup component to Canvas
   - Script fades the CanvasGroup, not individual elements

2. **For 3D Objects (Use Material):**
   - Verify material shader supports transparency
   - Change to: Standard → Rendering Mode: Transparent
   - Check material is using Alpha channel

3. **Test fade timing:**
   ```csharp
   // Slow down fade duration for testing
   yield return TransitionUtility.Fade(group, 0, 1, 3f); // 3 seconds instead of 1
   ```

---

### 🔴 Transitions Skip or Appear Broken

**Problem:** Scene transitions happen instantly or in wrong order

**Solutions:**
1. **Check SceneFlowController timing:**
   ```csharp
   // Add to SwitchScene for debugging
   Debug.Log($"Exiting {currentScene.SceneType}");
   yield return currentScene.Exit();
   Debug.Log($"Entering {target}");
   ```

2. **Verify IEnumerator chains:**
   - Each Exit() should yield return all fade/move operations
   - Each Enter() should yield return all fade/move operations

3. **Check GameObjects active state:**
   - Scenes should set active/inactive at start of Enter/Exit
   - If already active, won't show transitions

---

### 🔴 Title Scene Doesn't Auto-Progress

**Problem:** Title scene stays visible after 2 seconds

**Solutions:**
1. **Check SceneTitleModule.Start():**
   ```csharp
   IEnumerator Start()
   {
       yield return new WaitForSeconds(2f);
       GoNext(); // This should be called
   }
   ```

2. **Verify GoNext() exists:**
   - Should call `SceneFlowController.Instance.ChangeScene(AppSceneState.Orbit);`

3. **Check Console for errors:**
   - Any exceptions will prevent the transition

---

### 🔴 Detail Scene Doesn't Show Detail Objects

**Problem:** Extra details don't fade in after sphere moves

**Solutions:**
1. **Check SphereDetailDisplay:**
   - Verify detailObjects[] array is populated
   - All GameObjects should be assigned

2. **Check SceneDetailModule:**
   - Verify detailDisplay reference is assigned
   - Check that ShowDetails() is called

3. **Verify CanvasGroup:**
   - Detail objects should have CanvasGroup if using fade
   - Check Canvas has CanvasGroup component

4. **Test manually:**
   - In Inspector, select DetailDisplay
   - Look for SphereDetailDisplay component
   - Check that all detail objects show in array

---

### 🔴 Timeline Navigator Not Showing

**Problem:** Timeline UI buttons not visible

**Solutions:**
1. **Check Canvas Settings:**
   - Canvas must be in first scene (before Title)
   - Set Render Mode to "Screen Space - Overlay"
   - Check UI Scale Factor

2. **Check TimelineNavigator Script:**
   - Verify Canvas has TimelineNavigator component
   - All button and indicator references must be assigned

3. **Check Button/Indicator GameObjects:**
   - All must be children of Canvas
   - Use Rect Transform for proper positioning
   - Buttons should be Image + Button component
   - Indicators should be Image component

4. **Visibility test:**
   - Set one indicator color to bright red temporarily
   - If you see it, canvas is working
   - If not, check Canvas visibility/layering

---

### 🔴 Restart Button Doesn't Work

**Problem:** Clicking restart button does nothing

**Solutions:**
1. **Check Button Component:**
   - Select Restart Button in Detail scene
   - Verify Button component exists
   - Check onClick event has handler

2. **Check Event Assignment:**
   - onClick should reference SceneDetailModule.Restart()
   - Or directly call `SceneFlowController.Instance.ChangeScene(AppSceneState.Title);`

3. **Verify Script Reference:**
   - Button must have access to SceneDetailModule or SceneFlowController
   - Both are singletons, so should be accessible

4. **Test with Debug:**
   ```csharp
   public void Restart()
   {
       Debug.Log("Restart clicked!");
       SceneFlowController.Instance.ChangeScene(AppSceneState.Title);
   }
   ```

---

### 🔴 Selected Sphere Not Moving to Center

**Problem:** Sphere stays in place instead of moving to DisplayPoint

**Solutions:**
1. **Check DisplayPoint:**
   - Must be at desired center position
   - Usually at (0, 0, 0)
   - Verify position in Inspector

2. **Check SceneDetailModule:**
   - Verify displayPoint is assigned
   - TransitionUtility.Move() is called with correct target

3. **Check SelectionContext:**
   - Verify selected sphere transform is stored:
   ```csharp
   SelectionContext.SelectedTransform = item.transform;
   ```

4. **Test movement manually:**
   ```csharp
   // In SceneDetailModule.Enter()
   Debug.Log($"Moving {SelectionContext.SelectedTransform.name} to {displayPoint.position}");
   ```

---

### 🔴 Performance Issues / Lag

**Problem:** App is slow or stutters during transitions

**Solutions:**
1. **Check Material Instances:**
   - Ensure using material instances, not shared materials
   - Each object fading should have its own material

2. **Optimize Fade Targets:**
   - Use CanvasGroup for UI (more efficient than individual elements)
   - Limit number of objects fading simultaneously

3. **Check Update() Calls:**
   - OrbitItem Updates every frame - ensure it's optimized
   - No heavy calculations in loops

4. **Profile with Profiler:**
   - Window → Analysis → Profiler
   - Check where time is spent

---

### 🟡 Scene References Keep Resetting

**Problem:** Assigned GameObjects become null after play/stop

**Solutions:**
1. **Use Proper References:**
   - Make scripts public or use [SerializeField]
   - Assign via Inspector, not code (if possible)

2. **Initialize in Awake():**
   ```csharp
   void Awake()
   {
       Instance = this; // For singletons
   }
   ```

3. **Avoid Cross-Scene References:**
   - Don't assign GameObjects across different scenes
   - Use singletons (Instance) instead

---

## Quick Diagnostic Steps

When something isn't working:

1. **Check Console** - Read all error messages
2. **Verify Build Settings** - All scenes present?
3. **Verify Assignments** - All Inspector fields filled?
4. **Check Singletons** - Instance variables initialized?
5. **Test Individually** - Does each scene work alone?
6. **Add Debug Logs** - Trace execution flow
7. **Restart Unity** - Clears cache issues

## Support Script

Add this to a GameObject to help diagnose issues:

```csharp
using UnityEngine;

public class DiagnosticHelper : MonoBehaviour
{
    void Update()
    {
        // Press 'D' to print diagnostic info
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log($"=== DIAGNOSTICS ===");
            Debug.Log($"Current Scenes: {UnityEngine.SceneManagement.SceneManager.sceneCount}");
            Debug.Log($"Active Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            
            if (SceneFlowController.Instance != null)
                Debug.Log("SceneFlowController: OK");
            else
                Debug.Log("SceneFlowController: MISSING!");
        }
    }
}
```
