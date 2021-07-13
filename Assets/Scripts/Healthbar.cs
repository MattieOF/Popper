using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public float         maxValue   = 1f;
    public RectTransform healthbar;
    public Image         healthbarImage;
    public float         healthbarWidth;
    public Gradient      colour;

    public void SetValue(float value)
    {
        healthbar.SetRight(healthbarWidth * (1 - (value / maxValue)));
        healthbarImage.color = colour.Evaluate(value / maxValue);
    }
}
