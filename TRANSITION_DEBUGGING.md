# Scene Transition Debugging Checklist

## Quick Test
1. Open the scene in Play Mode
2. Check the **Console** for any red errors
3. Watch the hierarchy - do GameObjects stay visible or disappear?
4. Check **Console logs** - look for transition messages

---

## Main Issues That Stop Transitions

### ❌ Issue 1: Modules Not Assigned to SceneFlowController
**Symptom:** Scene never starts or nothing happens

**Fix:**
1. Select `Managers > SceneFlowController` in Hierarchy
2. In Inspector, find the **Scene Modules** list
3. Verify all 3 are assigned:
   - [0] SceneTitleModule (from SceneTitle GameObject)
   - [1] OrbitSceneModule (new GameObject under OrbitCenter)
   - [2] SceneDetailModule (new GameObject)
4. **If any is empty:** Drag the correct component into the field

---

### ❌ Issue 2: CanvasGroup Blocking Raycasts
**Symptom:** Spheres won't respond to clicks

**Fix:**
1. Select `Canvas > TimelineUI` (orbit visuals)
2. In Inspector, find **CanvasGroup** component
3. Check **Block Raycasts**: 
   - During Orbit state = ✓ Checked (allow clicks)
   - During Title state = ✗ Unchecked
4. Select `Canvas > ExtraObjects` (detail display)
5. Check its CanvasGroup **Block Raycasts**:
   - During Detail state = ✓ Checked
   - During other states = ✗ Unchecked

**Better Option:** OrbitSceneModule handles this automatically - just verify the checkbox defaults

---

### ❌ Issue 3: GameObjects Active State Wrong
**Symptom:** Objects visible when they shouldn't be

**Fix - Keep Everything Active in Hierarchy:**
- ✓ SceneTitle → Always Active
- ✓ TimelineUI → Always Active  
- ✓ ExtraObjects → Always Active
- ✓ Spheres → Always Active

**The modules control visibility with CanvasGroup.alpha, not SetActive()**

---

### ❌ Issue 4: Missing CanvasGroup on TimelineUI
**Symptom:** Orbits don't fade in/out

**Fix:**
1. Select `Canvas > TimelineUI`
2. Inspector → Add Component → **CanvasGroup**
3. Set initial Alpha to **1** (visible)

---

### ❌ Issue 5: SceneFlowController Missing References
**Symptom:** "NullReferenceException"

**Fix - Assign in Inspector:**
1. Select `Managers > SceneFlowController`
2. In SceneFlowController component:
   - **Instance** field → Auto-set (singleton)
   - **Scene Modules** → Must have all 3

---

## Component Visibility Control Reference

### How Transitions Work:
```
Title State
└─ SceneTitleModule.Enter()
   └─ SceneTitle CanvasGroup.alpha 0→1 (fade in)
   └─ After 2s: ChangeScene(Orbit)

Orbit State
└─ SceneTitle Module.Exit()
│  └─ SceneTitle CanvasGroup.alpha 1→0 (fade out)
├─ OrbitSceneModule.Enter()
│  └─ TimelineUI CanvasGroup.alpha 0→1 (fade in)
│  └─ Enable Orbit2D rotation
└─ On Click Sphere:
   └─ ChangeScene(Detail)

Detail State
└─ OrbitSceneModule.Exit()
│  └─ TimelineUI CanvasGroup.alpha 1→0 (fade out)
│  └─ Disable Orbit2D rotation
├─ SceneDetailModule.Enter()
│  └─ Move sphere to DisplayPoint
│  └─ ExtraObjects CanvasGroup.alpha 0→1 (fade in)
└─ On Click Restart:
   └─ ChangeScene(Title)
```

---

## Inspector Checklist (Copy This)

### SceneFlowController
- [ ] Component exists on `Managers > SceneFlowController`
- [ ] Scene Modules [0]: SceneTitleModule assigned
- [ ] Scene Modules [1]: OrbitSceneModule assigned
- [ ] Scene Modules [2]: SceneDetailModule assigned

### SceneTitleModule
- [ ] Component on `SceneTitle` GameObject
- [ ] Canvas Group assigned
- [ ] Flow Controller assigned
- [ ] Fade Time: 1.5

### OrbitSceneModule
- [ ] Component on separate GameObject (OrbitSceneModule)
- [ ] Orbits Canvas Group: TimelineUI
- [ ] Spheres array: [Sphere1, Sphere3]
- [ ] Orbit Scripts: Orbit2D from each sphere
- [ ] TimelineUI has CanvasGroup component

### SceneDetailModule  
- [ ] Component on separate GameObject (SceneDetailModule)
- [ ] Display Point assigned
- [ ] Extra Group assigned (ExtraObjects CanvasGroup)
- [ ] Detail Display assigned (DetailDisplayManager)
- [ ] Restart Button assigned
- [ ] Orbits Canvas Group: TimelineUI

### ExtraObjects
- [ ] Has CanvasGroup component
- [ ] Alpha initially: 1
- [ ] Block Raycasts: Unchecked (during Orbit state)

### TimelineUI
- [ ] Has CanvasGroup component
- [ ] Alpha initially: 1
- [ ] Block Raycasts: Checked

---

## Testing Steps (Do These In Order)

### Step 1: Test Title Fade In
1. Play scene
2. **Expected:** Title text fades in over 1.5 seconds
3. **If not:** Check SceneTitleModule.Canvas Group assignment

### Step 2: Test Auto Transition
1. Continue playing
2. **Expected:** After 2 seconds, title fades out, spheres appear
3. **If not:** Check SceneFlowController.Scene Modules list

### Step 3: Test Sphere Visibility
1. Continue playing
2. **Expected:** Spheres visible and rotating
3. **If not:** Check OrbitSceneModule assignments

### Step 4: Test Sphere Click
1. Click a sphere
2. **Expected:** Sphere moves to center, orbits disappear
3. **If not:** 
   - Check SphereClickHandler exists (auto-added)
   - Check sphere has Collider
   - Check EventSystem exists in Canvas

### Step 5: Test Detail View
1. After step 4
2. **Expected:** Cube/detail objects appear, restart button visible
3. **If not:** Check SceneDetailModule.Detail Display assignment

### Step 6: Test Restart
1. Click restart button
2. **Expected:** Back to title
3. **If not:** Check Restart Button is linked to SceneDetailModule.Restart()

---

## Console Error Solutions

### "NullReferenceException in SceneFlowController"
- [ ] Check Scene Modules array has all 3 components
- [ ] Check each module's references are assigned

### "OnMouseDown not working"
- [ ] Add Collider to spheres (SphereCollider works)
- [ ] Check EventSystem exists (should auto-create)
- [ ] Check Main Camera has AudioListener

### "CanvasGroup Fade not working"
- [ ] TimelineUI must have CanvasGroup component
- [ ] ExtraObjects must have CanvasGroup component
- [ ] Check both are enabled (checkbox next to component name)

### "Sphere doesn't move"
- [ ] DisplayPoint must be assigned
- [ ] Check TransitionUtility.Move code (should work)
- [ ] Verify sphere is world position, not UI

---

## Important: GameObject Visibility During States

```
┌──────────────────────────────────────────────────┐
│ Title State                                      │
├────────────────────────────────────────────────┤
│ SceneTitle      │ Alpha: 1 (visible)            │
│ TimelineUI      │ Alpha: 0 (hidden)             │
│ ExtraObjects    │ Alpha: 0 (hidden)             │
└────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────┐
│ Orbit State                                      │
├────────────────────────────────────────────────┤
│ SceneTitle      │ Alpha: 0 (hidden)             │
│ TimelineUI      │ Alpha: 1 (visible)            │
│ ExtraObjects    │ Alpha: 0 (hidden)             │
└────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────┐
│ Detail State                                     │
├────────────────────────────────────────────────┤
│ SceneTitle      │ Alpha: 0 (hidden)             │
│ TimelineUI      │ Alpha: 0 (hidden)             │
│ ExtraObjects    │ Alpha: 1 (visible)            │
└────────────────────────────────────────────────┘
```

**All GameObjects stay ACTIVE in hierarchy - only alpha changes**

---

## Quick Fix Checklist (Try These)

1. [ ] SceneFlowController has all 3 modules assigned
2. [ ] Each module's required fields are filled
3. [ ] TimelineUI and ExtraObjects have CanvasGroups
4. [ ] Spheres have Colliders
5. [ ] Console shows no red errors
6. [ ] All GameObjects are Active (blue checkbox)
7. [ ] Restart button is linked to SceneDetailModule.Restart()

