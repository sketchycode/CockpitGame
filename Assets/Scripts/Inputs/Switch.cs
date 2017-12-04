using System;
using UnityEngine;

public class Switch : UIInputBase
{
    public void OnValueChanged(bool value)
    {
        Value = value ? 1f : 0f;
        OnInteracted();
    }
}