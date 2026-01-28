using UnityEngine;

/// <summary>
/// Generic 2D orbit behaviour.
/// Any object can orbit any other object in XY plane.
/// </summary>
public class Orbit2D : MonoBehaviour
{
    [Header("Orbit Target")]
    public Transform center;

    [Header("Orbit Settings")]
    public float radius = 3f;
    public float speed = 90f;

    float angle;

    void OnEnable()
    {
        // Calculate initial angle based on start position
        // OnEnable ensures angle is recalculated when script is re-enabled
        if (center != null)
        {
            Vector2 offset = transform.position - center.position;
            angle = Mathf.Atan2(offset.y, offset.x);
        }
    }

    void Update()
    {
        if (!center) return;

        angle += speed * Mathf.Deg2Rad * Time.deltaTime;

        Vector2 newPos = new Vector2(
            center.position.x + Mathf.Cos(angle) * radius,
            center.position.y + Mathf.Sin(angle) * radius
        );

        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
