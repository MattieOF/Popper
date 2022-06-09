using UnityEngine;

public class PlayerEye : MonoBehaviour
{
    public float factor = 0.25f;
    public float limit  = 0.08f;

    private Vector3 _center;

    void Update()
    {
        _center = transform.parent.position;

        // Convert mouse position into a local space vector3
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0.0f;
        pos = transform.InverseTransformPoint(pos);

        Vector3 dir = pos * factor;
        dir.x = Mathf.Clamp(dir.x, -limit, limit);
        dir.y = Mathf.Clamp(dir.y, -limit, limit);

        transform.position = _center + dir;
    }
}
