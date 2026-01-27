using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SceneTitleModule : MonoBehaviour, ISceneModule
{
    public AppSceneState SceneType => AppSceneState.Title;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup orbitsCanvasGroup;
    [SerializeField] private CanvasGroup detailCanvasGroup;
    [SerializeField] private float fadeTime = 1.5f;

    void OnEnable()
    {
        Debug.Log($"✓ SceneTitleModule enabled on {gameObject.name}");
        if (canvasGroup == null)
            Debug.LogWarning("⚠️ SceneTitleModule: title canvasGroup not assigned!");
    }

    public IEnumerator Enter()
    {
        Debug.Log("🎬 SceneTitleModule: Entering Title state");
        
        // Hide orbits and detail UI
        if (orbitsCanvasGroup != null)
        {
            Debug.Log("📉 Hiding orbits");
            orbitsCanvasGroup.alpha = 0;
            orbitsCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            Debug.LogWarning("⚠️ orbitsCanvasGroup not assigned!");
        }

        if (detailCanvasGroup != null)
        {
            Debug.Log("📉 Hiding detail UI");
            detailCanvasGroup.alpha = 0;
            detailCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            Debug.LogWarning("⚠️ detailCanvasGroup not assigned!");
        }
        
        if (canvasGroup == null)
        {
            Debug.LogError("❌ SceneTitleModule: canvasGroup is null! Cannot fade.");
            yield break;
        }

        // Fade in title
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = true;
        
        float t = 0f;
        while(t < fadeTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }
        canvasGroup.alpha = 1;
        Debug.Log("✅ Title fade in complete - only title visible");

        // Wait 2 seconds then move to orbit scene automatically
        yield return new WaitForSeconds(2f);
        
        if (SceneFlowController.Instance != null)
        {
            Debug.Log("🔄 SceneTitleModule: Auto-transitioning to Orbit");
            SceneFlowController.Instance.ChangeScene(AppSceneState.Orbit);
        }
        else
        {
            Debug.LogError("❌ SceneTitleModule: SceneFlowController.Instance is null!");
        }
    }

    public IEnumerator Exit()
    {
        Debug.Log("📉 SceneTitleModule: Exiting Title state");
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
            yield return TransitionUtility.Fade(canvasGroup, 1, 0, 0.5f);
        }
        else
            yield return new WaitForSeconds(0.5f);
    }
}
