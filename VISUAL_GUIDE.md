# 🎨 Visual Setup Guide - Quick Reference

## Scene 1: Title - Visual Layout

```
┌─────────────────────────────────────┐
│         TITLE CANVAS                │
│  ┌───────────────────────────────┐  │
│  │                               │  │
│  │     "Press Start"             │  │
│  │     (Fades in smoothly)       │  │
│  │                               │  │
│  └───────────────────────────────┘  │
│                                     │
│  Action: Auto-transitions in 2s     │
└─────────────────────────────────────┘
```

**GameObjects Needed:**
- Canvas (CanvasGroup required)
  - TextMeshPro Text child
- SceneTitleModule (script)

---

## Scene 2: Orbit - Visual Layout

```
                    TOP VIEW
     
           Sphere1 (5, 0, 0)
                 *
                / \
               /   \
              /     \
    Sphere2 *       * Sphere3
   (-2.5, 0,        (-2.5, 0,
    4.3)             -4.3)
    
    Center: OrbitCenter (0, 0, 0)
    
    Rotation: Around Y-axis (up)
    Speed: 40 units/sec
    
    ↻ ↻ ↻ (All rotate continuously)
```

**GameObjects Needed:**
- OrbitCenter (empty, reference point)
- Sphere1 (position: 5, 0, 0)
  - OrbitItem script
  - Renderer + Collider
- Sphere2 (position: -2.5, 0, 4.3)
  - OrbitItem script
  - Renderer + Collider
- Sphere3 (position: -2.5, 0, -4.3)
  - OrbitItem script
  - Renderer + Collider
- SceneOrbitModule (manager script)

**Interaction:**
```
Click Sphere1 → Fade out Sphere2 & 3 → Transition to Detail
Click Sphere2 → Fade out Sphere1 & 3 → Transition to Detail
Click Sphere3 → Fade out Sphere1 & 2 → Transition to Detail
```

---

## Scene 3: Detail - Visual Layout

```
┌─────────────────────────────────────┐
│    DETAIL CANVAS                    │
│  ┌───────────────────────────────┐  │
│  │ Details about sphere info...  │  │
│  │ • Point 1                      │  │
│  │ • Point 2                      │  │
│  │ • Point 3                      │  │
│  │                               │  │
│  │          ┌─────────┐          │  │
│  │          │ Restart │          │  │
│  │          └─────────┘          │  │
│  └───────────────────────────────┘  │
│                                     │
│      Sphere at DisplayPoint         │
│         (in 3D, center)             │
└─────────────────────────────────────┘
```

**GameObjects Needed:**
- DisplayPoint (empty, position: 0, 0, 0)
- DetailCanvas (with CanvasGroup)
  - Text elements
  - Restart Button
- DetailDisplay (manager)
  - Detail objects (text, cubes, etc.)
- SceneDetailModule (manager script)

**Animation Sequence:**
```
1. Selected sphere moves to DisplayPoint
   Duration: 1 second
   
2. Detail objects fade in
   Duration: 1 second
   
3. User clicks Restart
   
4. Transition to Title
```

---

## Timeline UI - Visual Layout

```
┌─────────────────────────────────────────────────────┐
│ [Title] ● [Orbit] ● [Detail] ●                      │
│  Button    Indicator Button  Indicator Button Indicator
│                                                      │
│ Top-left corner, always visible                    │
│ Colors: Current scene = White, Others = Gray       │
└─────────────────────────────────────────────────────┘
```

**UI Structure:**
```
TimelineCanvas (Screen Space - Overlay)
├── TitleButton (60x30, pos: 10, -10)
├── TitleIndicator (10x10, pos: 50, -10, color: Gray)
├── OrbitButton (60x30, pos: 80, -10)
├── OrbitIndicator (10x10, pos: 120, -10, color: Gray)
├── DetailButton (60x30, pos: 150, -10)
└── DetailIndicator (10x10, pos: 190, -10, color: Gray)
```

---

## Transition Flow - Visual Timeline

```
TITLE SCENE
┌─────────────────────────────┐
│  Alpha: 0 → 1 (fade in)    │  Duration: 1s
│  "Press Start"             │
│  Wait 2 seconds            │
└────────┬────────────────────┘
         │
         │ Auto-transition
         ▼
ORBIT SCENE (Entry)
┌─────────────────────────────┐
│  Spheres already rotating  │
│  Ready for interaction      │
└────────┬────────────────────┘
         │
         │ User clicks sphere
         ▼
ORBIT SCENE (Exit)
┌─────────────────────────────┐
│  Unselected spheres fade:  │  Duration: 0.5s
│  Alpha: 1 → 0              │
└────────┬────────────────────┘
         │
         ▼
DETAIL SCENE (Entry)
┌─────────────────────────────┐
│  Sphere moves to center    │  Duration: 1s
│  Details fade in           │  Duration: 1s
└────────┬────────────────────┘
         │
         │ User clicks Restart
         ▼
TITLE SCENE (Entry)
┌─────────────────────────────┐
│  Title fades in again      │  Duration: 1s
│  Loop repeats              │
└─────────────────────────────┘
```

---

## Component Checklist - Visual

```
SCENE 1 (Title)
✓ Canvas
  ✓ CanvasGroup
  ✓ TextMeshPro element
✓ SceneTitleModule script

SCENE 2 (Orbit)
✓ OrbitCenter (empty)
✓ Sphere1
  ✓ Renderer
  ✓ Collider
  ✓ OrbitItem script
✓ Sphere2
  ✓ Renderer
  ✓ Collider
  ✓ OrbitItem script
✓ Sphere3
  ✓ Renderer
  ✓ Collider
  ✓ OrbitItem script
✓ SceneOrbitModule script

SCENE 3 (Detail)
✓ DisplayPoint (empty)
✓ DetailCanvas
  ✓ CanvasGroup
  ✓ UI elements (text, images)
  ✓ RestartButton
✓ DetailDisplay
  ✓ SphereDetailDisplay script
  ✓ Detail GameObjects in array
✓ SceneDetailModule script

TIMELINE
✓ TimelineCanvas
  ✓ TimelineNavigator script
  ✓ TitleButton
  ✓ TitleIndicator
  ✓ OrbitButton
  ✓ OrbitIndicator
  ✓ DetailButton
  ✓ DetailIndicator

FLOW CONTROL
✓ SceneFlowController script
  ✓ All 3 scene modules assigned
✓ Build Settings (3 scenes in order)
```

---

## Material Setup - Visual

### For Sphere Fading:
```
Material Properties:
├── Shader: Standard (Transparent)
├── Rendering Mode: Transparent
├── Base Color: Your color
├── Alpha: Supported (1 = visible, 0 = invisible)
└── Use Material instances per sphere
```

### For UI Fading:
```
Canvas Setup:
├── CanvasGroup component (required!)
├── Alpha: Supported (1 = visible, 0 = invisible)
├── Affects all children
└── More efficient than Image alpha
```

---

## Position Reference - Visual (Top-Down View)

```
              North (Z+)
                  |
                  |
     Sphere2      |      Sphere1
     (-2.5,0,   Origin   (5,0,0)
      4.3)     (0,0,0)
         \       |       /
          \      |      /
    West (X-)----+---- (X+) East
            \    |    /
             \   |   /
           Sphere3
        (-2.5,0,-4.3)
              
              South (Z-)


Distance from center: ~5 units each
Angles: ~120° apart
Rotation: Around Y-axis (up)
```

---

## Inspector Field Assignments - Visual Checklist

```
SceneFlowController
├── [?] sceneModules[0] → SceneTitleModule
├── [?] sceneModules[1] → SceneOrbitModule
└── [?] sceneModules[2] → SceneDetailModule

SceneTitleModule
└── [?] canvasGroup → Canvas.CanvasGroup

SceneOrbitModule
├── [?] items[0] → Sphere1
├── [?] items[1] → Sphere2
├── [?] items[2] → Sphere3
└── [?] center → OrbitCenter

OrbitItem (on each Sphere)
├── [?] speed → 40
└── [?] center → OrbitCenter

SceneDetailModule
├── [?] displayPoint → DisplayPoint
├── [?] extraGroup → DetailCanvas.CanvasGroup
└── [?] detailDisplay → DetailDisplay

SphereDetailDisplay (on DetailDisplay)
├── [?] detailObjects[0] → DetailObject1
└── [?] detailObjects[1] → DetailObject2

TimelineNavigator (on TimelineCanvas)
├── [?] titleButton → TitleButton
├── [?] orbitButton → OrbitButton
├── [?] detailButton → DetailButton
├── [?] titleIndicator → TitleIndicator
├── [?] orbitIndicator → OrbitIndicator
└── [?] detailIndicator → DetailIndicator
```

---

## Common Position Reference Values

### Orbit Sphere Positions:
```
Setup 1 (Distance: 5 units)
Sphere1: (5.0,  0, 0.0)
Sphere2: (-2.5, 0, 4.33)
Sphere3: (-2.5, 0, -4.33)

Setup 2 (Distance: 4 units)
Sphere1: (4.0,  0, 0.0)
Sphere2: (-2.0, 0, 3.46)
Sphere3: (-2.0, 0, -3.46)

Setup 3 (Distance: 3 units)
Sphere1: (3.0,  0, 0.0)
Sphere2: (-1.5, 0, 2.60)
Sphere3: (-1.5, 0, -2.60)
```

### Display Point Position:
```
Always: (0, 0, 0)
        (or wherever you want center)
```

### Timeline Canvas Position:
```
Top-left corner
Buttons: 10, 80, 150 (X positions)
         -10 (Y position)
Indicators: 50, 120, 190 (X positions)
           -10 (Y position)
```

---

## Size Reference

```
Canvas: Full screen (RectTransform)

Text: 32-48 pt size (readable)

Buttons: 60x30 pixels (click-friendly)

Indicators: 10x10 pixels (small circles)

Spheres: 0.5x0.5x0.5 scale (relative to 1x1x1 default)

Detail Objects: Variable (visible in scene)
```

---

## Color Reference

```
Normal sphere: White or gray
Indicator (Active): White (full brightness)
Indicator (Inactive): Gray (dimmed)
Canvas Background: Transparent (or color of choice)
Text: White or contrasting color
Buttons: Gray or themed color
```

---

## This Visual Guide Should Help You:

✅ Visualize the layout of each scene
✅ Understand sphere positioning
✅ See the animation flow
✅ Verify all components exist
✅ Confirm all assignments are made
✅ Check UI positioning
✅ Understand the overall structure

**Keep this open in one window while Unity is in another!**
