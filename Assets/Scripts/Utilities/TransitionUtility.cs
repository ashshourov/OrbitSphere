using UnityEngine;
using System.Collections;

public static class TransitionUtility
{
    public static IEnumerator Fade(CanvasGroup group, float from, float to, float duration)
    {
        if (group == null)
            yield break;

        float t = 0;
        group.alpha = from;

        while (t < duration)
        {
            group.alpha = Mathf.Lerp(from, to, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        group.alpha = to;
    }

    public static IEnumerator Move(Transform obj, Vector3 target, float duration)
    {
        if (obj == null)
            yield break;

        Vector3 start = obj.position;
        float t = 0;

        while (t < duration)
        {
            obj.position = Vector3.Lerp(start, target, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        obj.position = target;
    }

    public static IEnumerator FadeRenderer(Renderer renderer, float from, float to, float duration)
    {
        if (renderer == null)
            yield break;

        float t = 0;
        Material mat = renderer.material;
        Color startColor = mat.color;
        Color endColor = mat.color;
        endColor.a = to;
        startColor.a = from;
        mat.color = startColor;

        while (t < duration)
        {
            Color currentColor = mat.color;
            currentColor.a = Mathf.Lerp(from, to, t / duration);
            mat.color = currentColor;
            t += Time.deltaTime;
            yield return null;
        }

        Color finalColor = mat.color;
        finalColor.a = to;
        mat.color = finalColor;
    }
}
