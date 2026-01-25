# OrbitSphere Project - Implementation Checklist

## Code Implementation ✓ COMPLETE
- [x] SceneFlowController - Main scene manager with event system
- [x] ISceneModule - Interface for all scene modules
- [x] AppSceneState - Enum for scene states (Title, Orbit, Detail)
- [x] SceneTitleModule - Title scene controller with fade-in
- [x] SceneOrbitModule - Orbit scene with sphere selection
- [x] SceneDetailModule - Detail scene with sphere positioning
- [x] OrbitItem - Sphere behavior (rotation + selection)
- [x] SelectionContext - Shared context for selected sphere
- [x] TransitionUtility - Animation utilities (Fade, Move)
- [x] TimelineNavigator - Timeline/navigation UI bar
- [x] SphereDetailDisplay - Detail objects manager
- [x] SceneFadeHelper - Additional animation helpers

## Unity Scene Setup - MANUAL STEPS REQUIRED

### Scene 1: Title
- [ ] Create new scene "Title"
- [ ] Create Canvas with TextMeshPro text element
- [ ] Add CanvasGroup component to Canvas
- [ ] Create GameObject with SceneTitleModule script
- [ ] Assign Canvas.CanvasGroup to SceneTitleModule.canvasGroup
- [ ] Save scene

### Scene 2: Orbit
- [ ] Create new scene "Orbit"
- [ ] Create empty GameObject "OrbitCenter" at origin
- [ ] Create 3 Sphere objects:
  - [ ] Position at distance ~5 units (e.g., (5,0,0), (-2.5,0,4.3), (-2.5,0,-4.3))
  - [ ] Scale to 0.5x0.5x0.5 (optional, for visibility)
  - [ ] Each needs Renderer component
- [ ] Create manager GameObject with:
  - [ ] SceneOrbitModule script
  - [ ] Collider for mouse detection (optional but recommended)
- [ ] Add OrbitItem script to each sphere:
  - [ ] Set speed property (default 40)
  - [ ] Assign OrbitCenter to center field
- [ ] Assign to SceneOrbitModule:
  - [ ] All 3 Sphere transforms to items[] list
  - [ ] OrbitCenter to center field
- [ ] Save scene

### Scene 3: Detail
- [ ] Create new scene "Detail"
- [ ] Create Canvas for UI
- [ ] Add CanvasGroup component to Canvas
- [ ] Create empty GameObject "DisplayPoint" at origin
- [ ] Create manager GameObject with:
  - [ ] SceneDetailModule script
  - [ ] Assign Canvas.CanvasGroup to extraGroup
  - [ ] Assign DisplayPoint to displayPoint
- [ ] Create detail objects (info text, decorations, etc.)
- [ ] Create empty "DetailDisplay" GameObject with SphereDetailDisplay script
- [ ] Assign all detail objects to SphereDetailDisplay.detailObjects[]
- [ ] Assign DetailDisplay GameObject reference to SceneDetailModule.detailDisplay
- [ ] Create Button for restart and hook to SceneDetailModule.Restart()
- [ ] Save scene

### Timeline UI (Persistent)
- [ ] Create Canvas "TimelineCanvas" (Screen Space - Overlay)
- [ ] Add TimelineNavigator script to Canvas
- [ ] Create 3 Buttons:
  - [ ] TitleButton
  - [ ] OrbitButton
  - [ ] DetailButton
- [ ] Create 3 Image indicators (size ~20x20):
  - [ ] TitleIndicator (initial color: Gray)
  - [ ] OrbitIndicator (initial color: Gray)
  - [ ] DetailIndicator (initial color: Gray)
- [ ] Assign all buttons and indicators to TimelineNavigator in inspector

### Build Settings
- [ ] Add scenes in Build Settings in order:
  - [ ] Scene 0: Title
  - [ ] Scene 1: Orbit
  - [ ] Scene 2: Detail

### Main Scene Setup
- [ ] Create empty GameObject "SceneFlowController"
- [ ] Add SceneFlowController script
- [ ] Assign all 3 scene module GameObjects to sceneModules[] list
- [ ] Make sure TimelineCanvas is in the starting scene

## Testing Checklist
- [ ] Title scene fades in text automatically
- [ ] After 2 seconds, transitions to Orbit scene
- [ ] Orbit scene shows 3 rotating spheres
- [ ] Can click any sphere to select it
- [ ] Unselected spheres fade out on selection
- [ ] Selected sphere smoothly moves to center in Detail scene
- [ ] Detail objects fade in after sphere settles
- [ ] Restart button transitions back to Title scene
- [ ] Timeline indicators show current scene
- [ ] Timeline buttons can jump between scenes (if enabled)

## Optional Enhancements
- [ ] Add sounds/music for scene transitions
- [ ] Add particle effects for sphere selection
- [ ] Customize sphere colors
- [ ] Add more detail objects for Scene 3
- [ ] Add keyboard shortcuts for scene navigation
- [ ] Add animations to title text (bounce, rotate, etc.)
- [ ] Add labels to timeline buttons

## Tips
- Use material instances for per-object fade control
- Test transitions with different duration values
- Ensure all GameObjects have proper layers and tags for organization
- Use Debug.Log in OnMouseDown to verify sphere selection works
- Check console for any missing component references
