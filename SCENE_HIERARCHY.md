# Scene Hierarchy Reference

## SCENE 1: Title

```
Canvas (CanvasGroup required)
├── Text (TextMeshPro)
│   └── Content: "Your Title Here"
└── [Other UI elements as desired]

SceneTitleModule (Script Component)
└── References:
    └── canvasGroup → Canvas.CanvasGroup
```

**Expected Behavior:**
- Text fades in over 1 second
- Auto-transitions to Orbit scene after 2 seconds

---

## SCENE 2: Orbit

```
OrbitCenter (Empty - Position at 0,0,0)
└── [Used as rotation reference point]

Sphere1 (Position: 5, 0, 0)
├── Renderer (with material)
├── Collider (for mouse detection)
└── OrbitItem (Script)
    └── References:
        ├── speed → 40 (adjustable)
        └── center → OrbitCenter Transform

Sphere2 (Position: -2.5, 0, 4.3)
├── Renderer (with material)
├── Collider
└── OrbitItem (Script)
    └── References:
        ├── speed → 40
        └── center → OrbitCenter Transform

Sphere3 (Position: -2.5, 0, -4.3)
├── Renderer (with material)
├── Collider
└── OrbitItem (Script)
    └── References:
        ├── speed → 40
        └── center → OrbitCenter Transform

SceneOrbitModule (Manager GameObject)
├── Script Component
└── References:
    ├── items[] → [Sphere1, Sphere2, Sphere3]
    └── center → OrbitCenter Transform
```

**Expected Behavior:**
- All 3 spheres rotate around OrbitCenter continuously
- Clicking any sphere selects it and transitions to Detail scene
- Unselected spheres fade out during transition

---

## SCENE 3: Detail

```
DisplayPoint (Empty - Position at 0, 0, 0)
└── [Where selected sphere will move to]

Canvas (CanvasGroup required)
├── UI Elements (info, stats, etc.)
│   └── [Will fade in after sphere arrives]
├── RestartButton
│   └── onClick → SceneDetailModule.Restart()
└── [Other UI as desired]

DetailDisplay (Empty - Child of Canvas or root)
└── Script: SphereDetailDisplay
    └── detailObjects[] →
        ├── InfoText GameObject
        ├── DecorativeElement1
        ├── DecorativeElement2
        └── [Any other detail objects]

SceneDetailModule (Manager GameObject)
├── Script Component
└── References:
    ├── displayPoint → DisplayPoint Transform
    ├── extraGroup → Canvas CanvasGroup
    └── detailDisplay → DetailDisplay SphereDetailDisplay
```

**Expected Behavior:**
- Selected sphere moves to DisplayPoint over 1 second (smooth)
- Once positioned, detail objects fade in over 1 second
- Restart button returns to Title scene

---

## TIMELINE UI (Persistent - In Starting Scene)

```
TimelineCanvas (CanvasGroup optional, Screen Space - Overlay)
├── TimelineNavigator (Script)
│   └── References:
│       ├── titleButton → TitleButton
│       ├── orbitButton → OrbitButton
│       ├── detailButton → DetailButton
│       ├── titleIndicator → TitleIndicator Image
│       ├── orbitIndicator → OrbitIndicator Image
│       └── detailIndicator → DetailIndicator Image
│
├── HorizontalLayoutGroup (for organization)
│
├── TitleButton
│   └── onClick → TimelineNavigator.Navigate(AppSceneState.Title)
│
├── TitleIndicator (Image, 20x20, gray)
│
├── OrbitButton
│   └── onClick → TimelineNavigator.Navigate(AppSceneState.Orbit)
│
├── OrbitIndicator (Image, 20x20, gray)
│
├── DetailButton
│   └── onClick → TimelineNavigator.Navigate(AppSceneState.Detail)
│
└── DetailIndicator (Image, 20x20, gray)
```

**Expected Behavior:**
- Timeline is always visible
- Current scene indicator is white, others are gray
- Clicking buttons transitions to that scene
- Automatically updates when scenes transition

---

## MAIN SCENE (Contains Timeline & Flow Control)

```
SceneFlowController (Empty GameObject)
├── Script Component: SceneFlowController
│   └── sceneModules[] →
│       ├── [Reference to SceneTitleModule from Title scene]
│       ├── [Reference to SceneOrbitModule from Orbit scene]
│       └── [Reference to SceneDetailModule from Detail scene]
│
├── Event: OnSceneChanged
│   └── [Subscribed by TimelineNavigator]
│
└── [Manages transitions between scenes]

TimelineCanvas
└── [See Timeline UI section above]
```

**Expected Behavior:**
- Manages all scene transitions
- Broadcasts OnSceneChanged event
- Coordinates entrance and exit of scenes

---

## Position Reference for Orbit Scene

If you want spheres at equal distances:

```
Using distance 5 units from center (0,0,0):

Sphere 1: (5, 0, 0)      - East
Sphere 2: (-2.5, 0, 4.3) - Northwest  
Sphere 3: (-2.5, 0, -4.3)- Southwest
```

Alternative (Distance 4 units):
```
Sphere 1: (4, 0, 0)      - East
Sphere 2: (-2, 0, 3.46)  - Northwest
Sphere 3: (-2, 0, -3.46) - Southwest
```

---

## Material Setup for Fading

For smooth fade effects on spheres:

1. Create a new Material
2. Select a shader that supports transparency:
   - Standard → Rendering Mode: Transparent
   - Or use: Unlit/Transparent
3. Assign to all sphere renderers

For UI fading:
- Use CanvasGroup component (not Image alpha)
- This affects all children efficiently
