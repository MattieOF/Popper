using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerLegs : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject legPos;
    public GameObject legStartPos;

    private void Update()
    {
        lineRenderer.SetPosition(0, legStartPos.transform.position);
        lineRenderer.SetPosition(1, legStartPos.transform.position + Vector3.up);
        lineRenderer.SetPosition(2, legPos.transform.position);
    }
}
