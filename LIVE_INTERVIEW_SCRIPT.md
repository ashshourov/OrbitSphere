# 🎬 OrbitSphere - Live Interview Script

## Complete Word-by-Word Presentation (10 minutes)

---

## ⏱️ OPENING (0:00-1:00) - 60 SECONDS

**[Confident, clear voice]**

"Thank you for having me. I'm excited to talk about **OrbitSphere**, a project that really helped me understand **professional game architecture**.

OrbitSphere is a **scene management system** built in Unity. What makes it interesting isn't just what it does - it's *how* it's structured. Rather than writing one big monolithic controller, I designed it using **modular architecture** with proper **separation of concerns**.

The app has three scenes: a Title scene with animations, an Orbit scene with interactive rotating spheres, and a Detail scene that moves the selected sphere to the center. But the interesting part is how these scenes communicate.

I'd like to walk you through the architecture first, then dive into the code, and then we can discuss any specific aspects you're curious about. Sound good?"

---

## 🏗️ ARCHITECTURE EXPLANATION (1:00-3:30) - 2.5 MINUTES

**[Pointing to code]**

"Let me start with the core pattern. The system uses **five main design patterns**.

**First: Singleton Pattern**

[Open SceneFlowController.cs]

The `SceneFlowController` is a Singleton. This is the **central orchestrator** for the entire application. There's only ever one instance, and it manages all scene transitions.

```csharp
public class SceneFlowController : MonoBehaviour
{
    public static SceneFlowController Instance;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        Instance = this;
    }
}
```

This means **any code in the application** can access the scene controller through `SceneFlowController.Instance`. It's a **single point of entry** for scene management.

**Second: Module Pattern**

[Show ISceneModule.cs]

Rather than writing custom code for each scene, every scene implements the `ISceneModule` interface. This **interface contract** guarantees that every scene has the same lifecycle methods.

```csharp
public interface ISceneModule
{
    AppSceneState SceneType { get; }
    IEnumerator Enter();
    IEnumerator Exit();
}
```

So when I add a new scene, I don't need to modify the controller. I just implement this interface. It's **open for extension, closed for modification** - a core SOLID principle.

**Third: State Machine Pattern**

[Show AppSceneState.cs]

The `AppSceneState` enum defines exactly **three valid states** - Title, Orbit, and Detail. Why an enum instead of strings? Because enums are **type-safe**. The compiler catches typos. String-based systems can have bugs that only appear at runtime.

```csharp
public enum AppSceneState
{
    Title,
    Orbit,
    Detail
}
```

**Fourth: Observer/Event Pattern**

[Back to SceneFlowController.cs]

When scenes transition, the controller broadcasts an `OnSceneChanged` event.

```csharp
public event Action<AppSceneState> OnSceneChanged;
```

This keeps scenes **loosely coupled**. They don't need to know about each other. Any system can listen to scene changes without being tightly coupled to the controller.

**Fifth: Utility Pattern**

[Show TransitionUtility.cs]

All animations are centralized in `TransitionUtility`. This avoids **code duplication** and makes animations **consistent** across all scenes.

These five patterns together create a **professional, scalable system**."

---

## 🎬 SCENE WALKTHROUGH (3:30-6:00) - 2.5 MINUTES

**[Show hierarchy while talking]**

"Now let me walk you through what actually happens when the app runs.

**The Title Scene:**

[Open SceneTitleModule.cs]

When the Title scene enters, three things happen:

1. The text canvas is set to alpha 0 (invisible)
2. We fade it in over 1.5 seconds
3. We wait for 2 seconds, then automatically transition to the Orbit scene

This creates a smooth, professional intro. No jarring cuts. Just smooth fades.

**The Orbit Scene:**

[Open OrbitSceneModule.cs - Enter() method]

This is where the architecture really shines. Here's what happens:

1. Three spheres are created with their renderers set to alpha 0
2. All Orbit2D scripts are enabled - they START rotating immediately
3. But the spheres are invisible
4. The orbits canvas group fades in
5. **WHILE THAT'S HAPPENING**, all three sphere renderers fade in too

Look at this code:

```csharp
List<IEnumerator> allFades = new List<IEnumerator>();

allFades.Add(TransitionUtility.Fade(orbitsCanvasGroup, 0, 1, fadeInTime));

foreach (var sphere in spheres)
{
    allFades.Add(TransitionUtility.FadeRenderer(renderer, 0, 1, fadeInTime));
}

foreach (var fade in allFades)
    StartCoroutine(fade);
```

This is **parallel animation**. All animations start simultaneously. It creates a smooth, polished feel. If I had done them sequentially, it would feel choppy.

Once the spheres are visible, they're interactive. Each sphere has a `SphereClickHandler` that triggers scene transitions.

**The Detail Scene:**

[Open SceneDetailModule.cs - Enter() method]

When you click a sphere, several things happen immediately:

1. All Orbit2D scripts are disabled (stop rotation)
2. All OrbitVisualizer2D scripts are disabled (orbit paths disappear)
3. The orbits canvas group alpha is set to 0
4. All other sphere renderers are hidden
5. The **selected sphere remains visible**
6. It smoothly moves to the center over 1.2 seconds
7. The detail UI fades in

This is the `Move` utility in action:

```csharp
yield return TransitionUtility.Move(
    SelectionContext.SelectedTransform,
    fixedDisplayPointPosition,
    1.2f
);
```

When you click Restart, the sphere moves back to its original position, all renderers show again, and we return to the Orbit scene."

---

## 💻 TECHNICAL DECISIONS (6:00-7:30) - 1.5 MINUTES

**[Discussing code decisions]**

"Let me talk about some **design decisions** that make this work well.

**Decision 1: Parallel Animations**

Originally, I faded in the orbits canvas, **waited for it to complete**, then faded in the spheres. It looked slow and choppy.

Then I realized - I can start all animations at the same time using coroutines. This is crucial for professional-feeling transitions.

**Decision 2: Separation of Renderer Fading**

I created two different fade functions:
- `Fade()` for CanvasGroup (UI)
- `FadeRenderer()` for 3D renderers (world objects)

Why? Because they work differently. UI uses alpha on the CanvasGroup. 3D objects use material color alpha. By separating them, the code is **clearer** and each function does **one thing well**.

**Decision 3: Clean Lifecycle**

Each scene's `Exit()` method handles cleanup. It disables scripts, hides renderers, removes listeners. This prevents **memory leaks** and **state corruption**.

**Decision 4: SelectionContext**

Rather than passing data between scenes, I use a static `SelectionContext` class. It's simple and works well for this project. In a larger project, I might use **dependency injection**.

**Decision 5: Utility Functions**

Rather than inlining animations, I created reusable `TransitionUtility` functions. Benefits:
- Code reuse
- Consistent behavior
- Easy to modify all animations in one place
- Professional abstraction"

---

## 🎯 CHALLENGES & SOLUTIONS (7:30-9:00) - 1.5 MINUTES

**[Speaking thoughtfully]**

"I ran into a few interesting challenges building this.

**Challenge 1: Smooth Visibility Transitions**

Initially, when the Detail scene started, I would immediately hide non-selected spheres. But there's a visual glitch - you'd see spheres disappear instantly.

The solution was to **fade out the orbits canvas first**, which contains the visual elements, while keeping the sphere renderers set to invisible. This creates a seamless transition.

**Challenge 2: Rotation State**

When you leave the Orbit scene, the spheres are at specific rotation angles. When you return, they need to continue rotating from those angles, not reset to zero.

My solution: The `Orbit2D` script recalculates its angle in `OnEnable()`. So when the script re-enables after exiting the Detail scene, it reads the current world position and calculates the correct angle. Then rotation continues smoothly.

```csharp
void OnEnable()
{
    if (center != null)
    {
        Vector2 offset = transform.position - center.position;
        angle = Mathf.Atan2(offset.y, offset.x);
    }
}
```

**Challenge 3: Position Consistency**

The sphere needs to move to an exact world position. But if that position is specified as a UI element, it might change due to canvas scaling. 

Solution: In `SceneDetailModule.Start()`, I capture the `displayPoint` position once at startup and store it as `fixedDisplayPointPosition`. Then I use that fixed position for all movements."

---

## 🚀 IF ASKED... (9:00-10:00) - BONUS ANSWERS

**[Confidently answering follow-up questions]**

**"What would you improve?"**

"If I had more time, I would add:

1. **ScriptableObjects** - Store sphere data (names, descriptions) in assets
2. **Save/Load System** - Persist which sphere was selected
3. **Animation Curves** - Use easing functions for more polished feel
4. **Unit Tests** - Test scene transitions and state management
5. **Input System** - Use Unity's new Input System for better extensibility"

**"How would you add another scene?"**

"It's trivial:

1. Create `NewSceneModule.cs` implementing `ISceneModule`
2. Implement `Enter()` and `Exit()` coroutines
3. Drag it onto the SceneFlowController in the Inspector
4. Assign it to the scenes dictionary

The controller doesn't need to change. That's the power of the module pattern."

**"How do you test this?"**

"I would write unit tests:

```csharp
[Test]
public void SceneTransitionChangesState()
{
    controller.ChangeScene(AppSceneState.Orbit);
    Assert.AreEqual(controller.CurrentState, AppSceneState.Orbit);
}
```

I'd mock ISceneModule implementations and test state transitions."

---

## 🎓 CLOSING (10:00) - WRAP UP

**[Confident, professional]**

"So to summarize: OrbitSphere demonstrates how to build a **scalable, professional scene management system** using proper **architecture patterns**. The code is **clean**, the system is **maintainable**, and it can be **extended easily** without modifying existing code.

I'm proud of this project because it shows I understand not just how to write code, but how to structure code professionally. That's what separates junior developers from professionals - thinking about **scalability, maintainability, and design patterns**.

Do you have any questions about the architecture or the code?"

---

## 🎯 DELIVERY TIPS

### **Eye Contact & Presence**
- ✅ Look at the interviewer when speaking
- ✅ Point to code when explaining
- ✅ Use pauses for emphasis
- ✅ Speak clearly and confidently

### **Hand Gestures**
- ✅ Use hands to point at code
- ✅ Gesture to illustrate flow
- ✅ Don't fidget or cross arms
- ✅ Keep hands visible

### **Tone of Voice**
- ✅ Vary your tone - don't be monotone
- ✅ Speak louder for important concepts
- ✅ Slow down for complex parts
- ✅ Sound enthusiastic about the project

### **Timing**
- ⏱️ Total: ~10 minutes
- ⏱️ Pause for questions
- ⏱️ Don't rush
- ⏱️ Check if they want more detail

---

## 📋 PRACTICE CHECKLIST

- [ ] Practiced opening statement (45 sec)
- [ ] Can explain Singleton pattern
- [ ] Can explain Module pattern
- [ ] Can explain State Machine pattern
- [ ] Can explain Observer pattern
- [ ] Can explain Utility pattern
- [ ] Know the scene flow by heart
- [ ] Can answer "How would you add a 4th scene?"
- [ ] Can discuss parallel animations
- [ ] Can mention challenges you solved
- [ ] Can discuss improvements
- [ ] Can answer design questions
- [ ] Comfortable with code references
- [ ] Can run the project and show it working

---

**FINAL REMINDER:**

You built this project with **careful thought**. You used **professional patterns**. Your code is **clean**. You can **explain any part**. You can **extend it easily**.

**Go in there and show them what you've got. You've got this! 🚀**
