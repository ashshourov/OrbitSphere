using UnityEngine;
using System.Collections;

public class SceneFadeHelper : MonoBehaviour
{
    /// <summary>
    /// Fades a CanvasGroup to target alpha over specified duration
    /// </summary>
    public static IEnumerator FadeInUI(CanvasGroup group, float duration = 1f)
    {
        return TransitionUtility.Fade(group, 0, 1, duration);
    }

    /// <summary>
    /// Fades out a CanvasGroup over specified duration
    /// </summary>
    public static IEnumerator FadeOutUI(CanvasGroup group, float duration = 1f)
    {
        return TransitionUtility.Fade(group, 1, 0, duration);
    }

    /// <summary>
    /// Fades a Renderer's alpha
    /// </summary>
    public static IEnumerator FadeRenderer(Renderer renderer, float from, float to, float duration)
    {
        float t = 0;
        Color c = renderer.material.color;
        c.a = from;
        renderer.material.color = c;

        while (t < duration)
        {
            c.a = Mathf.Lerp(from, to, t / duration);
            renderer.material.color = c;
            t += Time.deltaTime;
            yield return null;
        }

        c.a = to;
        renderer.material.color = c;
    }

    /// <summary>
    /// Smoothly rotates an object to face target
    /// </summary>
    public static IEnumerator LookAtSmooth(Transform obj, Vector3 target, float duration)
    {
        float t = 0;
        Quaternion startRot = obj.rotation;
        Vector3 direction = (target - obj.position).normalized;
        Quaternion endRot = Quaternion.LookRotation(direction);

        while (t < duration)
        {
            obj.rotation = Quaternion.Slerp(startRot, endRot, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        obj.rotation = endRot;
    }
}
