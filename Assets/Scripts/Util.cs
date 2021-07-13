using UnityEngine;

public static class Util
{
    public static Vector3 WithX(this Vector3 vec, float newX)
    {
        return new Vector3(newX, vec.y, vec.z);
    }

    public static Vector3 WithY(this Vector3 vec, float newY)
    {
        return new Vector3(vec.x, newY, vec.z);
    }

    public static Vector3 WithZ(this Vector3 vec, float newZ)
    {
        return new Vector3(vec.x, vec.y, newZ);
    }
}

public static class RectTransformExtensions
{
    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}
