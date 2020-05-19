using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
        slider.value = value;
        fill.color = gradient.Evaluate(1.0f);
    }

    public void SetSize(float size)
    {
        slider.value = size;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
