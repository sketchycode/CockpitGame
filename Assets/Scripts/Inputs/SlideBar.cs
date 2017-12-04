using System;
using UnityEngine;
using UnityEngine.UI;

public class SlideBar : UIInputBase
{
    public Slider Slider;
    public float SliderValue
    {
        get { return Slider.value; }
        set { Slider.value = value; }
    }

    public void OnValueChanged(float value)
    {
        Value = value;
        OnInteracted();
    }
}
