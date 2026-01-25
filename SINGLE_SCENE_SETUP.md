# Single Scene Setup Guide

## Architecture Overview
All content runs in a **single scene** (Main.unity) with three states managed by `SceneFlowController`:

1. **Title** - Initial state, shows title then auto-transitions
2. **Orbit** - Shows spheres rotating in orbit, allows selection
3. **Detail** - Shows selected sphere centered with cube and restart button

---

## Scene Hierarchy Setup

### Required GameObjects Structure:

```
Main Scene
├── Canvas
│   ├── SceneTitle (CanvasGroup)
│   │   └── Title Text (TMP)
│   ├── TimelineUI (for orbit visuals)
│   └── ExtraObjects (CanvasGroup)
│       ├── DisplayPoint (Transform)
│       └── Cube/Detail Objects
├── OrbitCenter
│   ├── Sphere1 (with Collider)
│   ├── Sphere3 (with Collider)
│   └── Orbit2D scripts on each sphere
├── Managers
│   └── SceneFlowController
└── Main Camera
```

---

## Inspector Setup Checklist

### 1. SceneFlowController (Managers > SceneFlowController)
**Add Component:** `SceneFlowController`

**In Inspector:**
- **Scene Modules** list (size = 3):
  - [0]: SceneTitleModule component
  - [1]: OrbitSceneModule component
  - [2]: SceneDetailModule component

---

### 2. SceneTitleModule (Canvas > SceneTitle)
**Add Component:** `SceneTitleModule`

**In Inspector:**
- **Canvas Group:** Assign the SceneTitle GameObject's CanvasGroup
- **Fade Time:** 1.5
- **Flow Controller:** Drag SceneFlowController from Managers

---

### 3. OrbitSceneModule (Create new empty GameObject under OrbitCenter)
**Name:** OrbitSceneModule

**Add Component:** `OrbitSceneModule`

**In Inspector:**
- **Orbits Canvas Group:** Drag TimelineUI CanvasGroup (contains all orbit visuals)
- **Spheres:** Array of sphere transforms (Sphere1, Sphere3)
- **Orbit Scripts:** Array of Orbit2D components from each sphere
- **Fade In Time:** 1.5

---

### 4. SceneDetailModule (Create new empty GameObject)
**Name:** SceneDetailModule

**Add Component:** `SceneDetailModule`

**In Inspector:**
- **Display Point:** DisplayPoint Transform
- **Extra Group:** ExtraObjects CanvasGroup
- **Detail Display:** (See instructions below)
- **Restart Button:** Button component in ExtraObjects
- **Orbits Canvas Group:** TimelineUI CanvasGroup (to hide when showing detail)

---

### 4B. Setting Up Detail Display
The **Detail Display** field needs a `SphereDetailDisplay` component. Here's how to set it up:

**Step 1:** In Canvas > ExtraObjects, create a new empty GameObject
- **Name:** DetailDisplayManager

**Step 2:** Add Component `SphereDetailDisplay` to DetailDisplayManager
- In Inspector, expand **Detail Objects** array
- Add any objects that should show when detail view is active:
  - Example: Cube mesh, info panel, particle effects, etc.
  - (Can leave empty `[0]` for now to test)

**Step 3:** Assign to SceneDetailModule
- Go to Canvas > SceneDetailModule 
- In the **Detail Display** field, drag the **DetailDisplayManager** GameObject
- This auto-detects the SphereDetailDisplay component

---

### 5. Sphere GameObjects (Sphere1, Sphere3)
**Requirements:**
- Must have a **Collider** (SphereCollider or any collider)
- Must have **Orbit2D** script with rotation enabled

---

## Component Dependencies

```
SceneFlowController
├── Manages all 3 modules
└── Controls scene transitions

SceneTitleModule
├── Shows/hides title text
└── Auto-transitions to Orbit after 2 seconds

OrbitSceneModule
├── Fades in orbit visuals
├── Enables sphere rotation
└── Detects sphere clicks via SphereClickHandler

SphereClickHandler (auto-added to spheres)
├── Listens for OnMouseDown
└── Triggers scene transition to Detail

SceneDetailModule
├── Hides orbits
├── Moves sphere to display point
├── Shows detail display (cube, info)
└── Enables restart button
```

---

## Flow Diagram

```
START
  ↓
[Title State] → Fade in title (1.5s) → Wait 2s
  ↓
[Orbit State] → Fade in spheres/orbits → Enable rotation
  ↓
[User clicks sphere]
  ↓
[Detail State] → Hide orbits → Move sphere to center → Show cube + restart
  ↓
[User clicks restart]
  ↓
[Back to Title State]
```

---

## Troubleshooting

### Spheres not clickable
- [ ] Add Collider to sphere GameObjects
- [ ] Check SphereClickHandler is added (auto-added on start)
- [ ] Verify EventSystem exists in Canvas

### Sphere doesn't move to center
- [ ] DisplayPoint Transform must be assigned
- [ ] Check TransitionUtility.Move is working

### Orbits not showing
- [ ] TimelineUI must have CanvasGroup
- [ ] Assign TimelineUI to OrbitSceneModule's Orbits Canvas Group

### Detail display not showing
- [ ] Check if DetailDisplayManager GameObject exists under Canvas > ExtraObjects
- [ ] Verify SphereDetailDisplay component is on DetailDisplayManager
- [ ] Make sure detailObjects array has at least one object assigned
- [ ] Verify ExtraObjects has CanvasGroup

### Detail Display field empty in Inspector
- [ ] Create a new GameObject named "DetailDisplayManager"
- [ ] Add `SphereDetailDisplay` component to it
- [ ] Drag it into the Detail Display field of SceneDetailModule

---

## Testing Steps

1. **Play scene**
2. **Title appears** → Should fade in over 1.5 seconds
3. **Auto-transition after 2 seconds** → Spheres should fade in
4. **Click a sphere** → Sphere should move to center, orbits hide, cube appears
5. **Click restart** → Return to title, repeat

