using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitVisualizer2D : MonoBehaviour
{
    public Transform center;
    public float radius = 3f;
    public int segments = 64;

    LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.loop = true;
        line.widthMultiplier = 0.05f;
        line.positionCount = segments;
    }

    void Update()
    {
        if (!center) return;
        DrawCircle();
    }

    void DrawCircle()
    {
        float angleStep = 2f * Mathf.PI / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep;
            Vector3 pos = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );

            line.SetPosition(i, center.position + pos);
        }
    }
}
