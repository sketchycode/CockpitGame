using System;
using UnityEngine;

public class SlideBar : UIInputBase
{
    public void OnValueChanged(float value)
    {
        Value = value;
        OnInteracted();
    }
}
