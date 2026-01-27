using UnityEngine;

public class SphereDetailDisplay : MonoBehaviour
{
    [SerializeField] private Transform displayPoint;
    [SerializeField] private GameObject[] detailObjects;

    void Start()
    {
        // Initialize detail objects as inactive
        foreach (var obj in detailObjects)
        {
            obj.SetActive(false);
        }
    }

    public void ShowDetails()
    {
        foreach (var obj in detailObjects)
        {
            obj.SetActive(true);
        }
    }

    public void HideDetails()
    {
        foreach (var obj in detailObjects)
        {
            obj.SetActive(false);
        }
    }
}
