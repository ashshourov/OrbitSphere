# OrbitSphere Scene Setup Guide

## Project Structure Overview
This project uses a scene management system with three main scenes:
1. **Scene 1 (Title)**: Title screen with fade-in effect
2. **Scene 2 (Orbit)**: Three rotating spheres that can be selected
3. **Scene 3 (Detail)**: Selected sphere with additional details

## Setup Instructions

### Step 1: Create the Main Canvas (Timeline Navigator)
1. Create a new Canvas in your main scene
2. Name it "TimelineCanvas"
3. Set it to Screen Space - Overlay
4. Add a horizontal layout group
5. Create three buttons as children:
   - "TitleButton"
   - "OrbitButton"
   - "DetailButton"
6. Create three images (indicators) as siblings to buttons:
   - "TitleIndicator" (size: 20x20, color: Gray initially)
   - "OrbitIndicator" (size: 20x20, color: Gray initially)
   - "DetailIndicator" (size: 20x20, color: Gray initially)
7. Add the TimelineNavigator script to the Canvas
8. Assign the buttons and indicators in the inspector

### Step 2: Create Scene 1 - Title Scene
1. Create a new scene and save it as "Title"
2. Create a Canvas named "TitleCanvas"
3. Add a Text (TextMeshPro) element with your title text
4. Create a new empty GameObject and add SceneTitleModule script to it
5. Assign the Canvas's CanvasGroup component to the canvasGroup field
6. Add the Title scene to Build Settings

### Step 3: Create Scene 2 - Orbit Scene
1. Create a new scene and save it as "Orbit"
2. Create a 3D object as the center point (empty GameObject named "OrbitCenter")
3. Create three Spheres around the center:
   - Position them around the orbit center at equal distances (e.g., 5 units away)
   - Add a Renderer component to each sphere
   - Name them "Sphere1", "Sphere2", "Sphere3"
4. Create a manager GameObject and add SceneOrbitModule script to it
5. Assign:
   - The three Sphere transforms to the "items" list
   - The "OrbitCenter" transform to the "center" field
6. Add OrbitItem script to each sphere
7. Assign the "OrbitCenter" transform to each sphere's "center" field
8. Add the Orbit scene to Build Settings

### Step 4: Create Scene 3 - Detail Scene
1. Create a new scene and save it as "Detail"
2. Create a Canvas for UI
3. Create a display point (empty GameObject named "DisplayPoint" at the center)
4. Create a manager GameObject and add SceneDetailModule script to it
5. Assign the "DisplayPoint" transform to displayPoint field
6. Assign the Canvas's CanvasGroup to extraGroup field
7. Create detail objects (e.g., info text, decorative objects, etc.)
8. Create an empty GameObject named "DetailDisplay" and add SphereDetailDisplay script
9. Assign all detail objects to the "detailObjects" array
10. Assign DetailDisplay to the detailDisplay field in SceneDetailModule
11. Create a Restart Button and hook it to SceneDetailModule.Restart()
12. Add the Detail scene to Build Settings

### Step 5: Setup Scene Flow Controller
1. Create an empty GameObject in your first scene named "SceneFlowController"
2. Add the SceneFlowController script to it
3. Assign all three scene module GameObjects to the "sceneModules" list

### Step 6: Build Settings
Add all scenes to Build Settings in this order:
1. Title (Scene 0)
2. Orbit (Scene 1)
3. Detail (Scene 2)

## How It Works

### Title Scene
- Automatically fades in text over 1 second
- Automatically transitions to Orbit scene after 2 seconds

### Orbit Scene
- Three spheres rotate around the center point
- Click any sphere to select it
- Smooth transition to Detail scene with unselected spheres fading out

### Detail Scene
- Selected sphere moves to the center with smooth animation
- Additional detail objects fade in
- Restart button returns to Title scene

### Timeline Navigator
- Always visible at the top
- Shows current scene as white indicator
- Allows quick navigation between scenes (optional)

## Tips
- Adjust `speed` parameter in OrbitItem for faster/slower rotation
- Adjust `duration` parameters in TransitionUtility for faster/slower transitions
- Use CanvasGroup for UI fading instead of Image alpha for better performance
- The selected sphere maintains its position during transitions
