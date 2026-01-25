# Step-by-Step Tutorial: Building OrbitSphere from Scratch

## Part 1: Project Setup (5 minutes)

### Step 1.1: Prepare Your Workspace
1. Open your existing Unity project (OrbitSphere)
2. Verify all C# scripts are present in `Assets/` folder
3. Create a new folder: `Assets/Scenes/`

### Step 1.2: Create the First Scene - Title
1. Right-click in Project → Create → Scene
2. Name it "Title.unity" and save to `Assets/Scenes/`
3. In the new scene, create GameObjects:

```
Right-click in Hierarchy:
- 3D Object → Sphere (you'll use this to understand the viewport)
- UI → Canvas
```

4. Delete the test sphere (we don't need it)

### Step 1.3: Setup Canvas for Title
1. Select the Canvas in Hierarchy
2. In Inspector, add component: `CanvasGroup`
3. Right-click Canvas → Create Empty (child)
4. Rename to "TextContent"
5. Right-click TextContent → Create → TextMeshPro
6. Type your title text in the TextMeshPro component

### Step 1.4: Create Scene Module
1. Right-click empty area in Hierarchy → Create Empty
2. Rename to "SceneTitleModule"
3. Drag `SceneTitleModule.cs` script onto this GameObject
4. In Inspector:
   - Drag Canvas into the `canvasGroup` field
   - The script should find the CanvasGroup component automatically

---

## Part 2: Scene 2 - Orbit Scene (10 minutes)

### Step 2.1: Create Orbit Scene
1. File → New Scene
2. Save to `Assets/Scenes/Orbit.unity`

### Step 2.2: Create Orbit Center
1. Right-click Hierarchy → Create Empty
2. Rename to "OrbitCenter"
3. Position at (0, 0, 0)
4. Keep Scale at (1, 1, 1)

### Step 2.3: Create First Sphere
1. Right-click Hierarchy → 3D Object → Sphere
2. Rename to "Sphere1"
3. Set Position to (5, 0, 0)
4. Set Scale to (0.5, 0.5, 0.5)
5. Select the Sphere's Renderer → Change material to something visible

### Step 2.4: Duplicate Spheres
1. Select Sphere1, press Ctrl+D to duplicate
2. Rename to "Sphere2"
3. Set Position to (-2.5, 0, 4.3)
4. Duplicate again → Sphere3
5. Set Position to (-2.5, 0, -4.3)

### Step 2.5: Add Rotation Scripts
1. Select Sphere1 → Add Component → drag `OrbitItem.cs` script
2. In Inspector:
   - Set `speed` to 40
   - Drag OrbitCenter to the `center` field
3. Repeat for Sphere2 and Sphere3

### Step 2.6: Create Scene Manager
1. Create Empty GameObject, name "SceneOrbitModule"
2. Drag `SceneOrbitModule.cs` onto it
3. In Inspector:
   - Click "+" on `items` list size 3
   - Drag Sphere1, Sphere2, Sphere3 into the list
   - Drag OrbitCenter to the `center` field

### Step 2.7: Test It
1. Press Play
2. You should see 3 spheres rotating

---

## Part 3: Scene 3 - Detail Scene (10 minutes)

### Step 3.1: Create Detail Scene
1. File → New Scene
2. Save to `Assets/Scenes/Detail.unity`

### Step 3.2: Create Display Point
1. Create Empty GameObject
2. Rename to "DisplayPoint"
3. Position at (0, 0, 0)
4. This is where the selected sphere will appear

### Step 3.3: Create Canvas
1. Right-click Hierarchy → UI → Canvas
2. Rename to "DetailCanvas"
3. Add component: `CanvasGroup`

### Step 3.4: Add Detail Content
1. Right-click DetailCanvas → UI → Text - TextMeshPro
2. Type something like "Details about the sphere"
3. Right-click DetailCanvas → 3D Object → Cube (for decoration)
4. Rename to "DetailObject1"
5. Position somewhere visible near center
6. Create another cube "DetailObject2"

### Step 3.5: Add Restart Button
1. Right-click DetailCanvas → UI → Button
2. Rename to "RestartButton"
3. Change button text to "Restart"

### Step 3.6: Create Detail Display Manager
1. Right-click DetailCanvas → Create Empty (child)
2. Rename to "DetailDisplay"
3. Drag `SphereDetailDisplay.cs` onto it
4. In Inspector:
   - Set `detailObjects` size to 2
   - Drag DetailObject1 and DetailObject2 into the list

### Step 3.7: Create Scene Manager
1. Create Empty GameObject, name "SceneDetailModule"
2. Drag `SceneDetailModule.cs` onto it
3. In Inspector:
   - Drag DisplayPoint to `displayPoint`
   - Drag DetailCanvas to `extraGroup` (get CanvasGroup)
   - Drag DetailDisplay to `detailDisplay`

### Step 3.8: Connect Restart Button
1. Select RestartButton
2. In Inspector, find Button component
3. Click "+" under `On Click ()`
4. Drag SceneDetailModule GameObject into the object field
5. Select SceneDetailModule → Restart()

---

## Part 4: Timeline UI (5 minutes)

### Step 4.1: Create Timeline Canvas
Go back to your **first scene** (where Title is):
1. Right-click Hierarchy → UI → Canvas
2. Rename to "TimelineCanvas"
3. Set Render Mode to "Screen Space - Overlay"
4. Set anchors to top-left corner

### Step 4.2: Create Timeline Buttons
1. Right-click TimelineCanvas → UI → Button
2. Rename to "TitleButton"
3. Set anchors to top-left
4. Position at (10, -10) with size 60x30
5. Duplicate twice → "OrbitButton" (at 80, -10) and "DetailButton" (at 150, -10)
6. Update button text for each

### Step 4.3: Create Indicators
1. Right-click TimelineCanvas → UI → Image
2. Rename to "TitleIndicator"
3. Set size to 10x10
4. Position at (50, -10)
5. Set color to Gray initially
6. Duplicate twice → OrbitIndicator and DetailIndicator
7. Position at (120, -10) and (190, -10)

### Step 4.4: Add TimelineNavigator Script
1. Select TimelineCanvas
2. Drag `TimelineNavigator.cs` onto it
3. In Inspector, assign:
   - TitleButton, OrbitButton, DetailButton to button fields
   - TitleIndicator, OrbitIndicator, DetailIndicator to indicator fields

---

## Part 5: Wire Everything Together (5 minutes)

### Step 5.1: Setup SceneFlowController
1. Create Empty GameObject, name "SceneFlowController"
2. Drag `SceneFlowController.cs` onto it
3. In Inspector:
   - Set `sceneModules` size to 3
   - Drag SceneTitleModule from Title scene → index 0
   - Drag SceneOrbitModule from Orbit scene → index 1
   - Drag SceneDetailModule from Detail scene → index 2

### Step 5.2: Build Settings
1. File → Build Settings
2. Click "Add Open Scenes" to add current scene (Title)
3. Drag Orbit scene from Project to Scenes list (index 1)
4. Drag Detail scene from Project to Scenes list (index 2)
5. Verify order: Title (0), Orbit (1), Detail (2)

### Step 5.3: Make Canvas DontDestroyOnLoad (Optional)
1. Select TimelineCanvas
2. Drag `TimelineCanvas` to make it persist across scenes (advanced)
   OR just duplicate it in each scene

---

## Part 6: Testing (5 minutes)

### Step 6.1: Test Title Scene
1. Open Title scene
2. Press Play
3. Text should fade in
4. After 2 seconds, should transition to Orbit

### Step 6.2: Test Orbit Scene
1. Open Orbit scene, press Play
2. Spheres should rotate
3. Click a sphere - should fade out others and transition
4. Check console for errors

### Step 6.3: Test Detail Scene
1. Open Detail scene, press Play
2. Sphere should appear, move to center, details fade in
3. Click Restart - should go to Title

### Step 6.4: Full Test
1. Go to Title scene
2. Press Play from here
3. Complete full flow: Title → Orbit → Select → Detail → Restart

---

## Part 7: Troubleshooting Checklist

### If Something Doesn't Work:
- [ ] Check Console for red error messages
- [ ] Verify all Inspector field references are assigned (not empty)
- [ ] Verify all 3 scenes are in Build Settings
- [ ] Check that Colliders exist on spheres (for clicking)
- [ ] Verify CanvasGroup components exist where needed
- [ ] Check material has transparency enabled if using fade

### Quick Fixes:
- Spheres not rotating? → Check OrbitItem.center is assigned
- Can't click spheres? → Add Collider component to spheres
- Fading jerky? → Change material to Transparent shader
- App crashes? → Check SceneFlowController sceneModules list

---

## Customization Ideas (After Getting It Working)

### Easy Changes:
1. Change sphere colors → Select sphere → Renderer → Material → Base Color
2. Change rotation speed → OrbitItem → speed property
3. Change transition time → TransitionUtility calls, change duration
4. Add more detail objects → Drag more GameObjects to SphereDetailDisplay
5. Change button colors → Button → Image component → Color

### Intermediate Changes:
1. Add sound effects → Play audio in Enter()/Exit()
2. Add particle effects → Instantiate particles on selection
3. Custom animations → Use Animation component instead of TransitionUtility
4. Multiple spheres have different rotation speeds → Adjust individual speeds
5. Save selected sphere data → Store in SelectionContext

---

## You Did It! 🎉

Once you see the full flow working (Title fades in, auto-transitions to Orbit, clicking sphere goes to Detail, restart goes back), you're done!

Next steps:
- Add music/sound
- Polish visuals (colors, effects)
- Add more features to Detail scene
- Deploy to build

Refer to QUICK_REFERENCE.cs for code examples of common tasks.
