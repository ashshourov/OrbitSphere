# IMMEDIATE FIX - Scene Modules Assignment

## The Error
```
SceneFlowController: Scene Title not found in dictionary!
```

This means the modules list is empty!

---

## Quick Fix (Do This Now)

### Step 1: Create Missing GameObjects (if needed)

**Under Canvas:**
- [ ] SceneTitle - **Should already exist** with SceneTitleModule script
- [ ] Create new empty GameObject → Rename to "OrbitSceneModule"
  - Add component: **OrbitSceneModule**
- [ ] Create new empty GameObject → Rename to "SceneDetailModule"
  - Add component: **SceneDetailModule**

**Under Managers:**
- [ ] SceneFlowController - **Should already exist**

---

### Step 2: Assign Modules in Inspector

1. **Click** `Managers > SceneFlowController` in Hierarchy
2. **In Inspector**, look for **Scene Modules** list
3. **Set Size to 3**
4. **Assign each slot:**

```
[0] = Canvas > SceneTitle (GameObject with SceneTitleModule)
[1] = Canvas > OrbitSceneModule (GameObject with OrbitSceneModule)
[2] = Canvas > SceneDetailModule (GameObject with SceneDetailModule)
```

**Or if they're elsewhere:**
```
[0] = Drag SceneTitleModule component
[1] = Drag OrbitSceneModule component
[2] = Drag SceneDetailModule component
```

---

### Step 3: Verify Components Exist

Each GameObject must have its corresponding script:
- SceneTitle has **SceneTitleModule** ✓
- OrbitSceneModule has **OrbitSceneModule** ✓
- SceneDetailModule has **SceneDetailModule** ✓

---

## Why This Happened

The `SceneFlowController.Start()` looks for modules in the list. If the list is empty, it can't find `AppSceneState.Title`, causing the error.

---

## After Fixing

The console should show:
```
SceneFlowController: Registered Title module
SceneFlowController: Registered Orbit module
SceneFlowController: Registered Detail module
SceneFlowController: Requesting scene change to Title
```

Then the title should fade in!

