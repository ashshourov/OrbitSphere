using UnityEngine;
using System;

public class SphereClickHandler : MonoBehaviour
{
    public event Action<Transform> OnSphereSelected;

    void OnMouseDown()
    {
        OnSphereSelected?.Invoke(transform);
    }
}
