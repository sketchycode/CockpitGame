using System;
using UnityEngine;
using UnityEngine.UI;

public class Switch : UIInputBase
{
    public Toggle Toggle;

    public bool IsOn {
        get { return Toggle.isOn; }
        set { Toggle.isOn = value; }
    }

    public void OnValueChanged(bool value)
    {
        Value = value ? 1f : 0f;
        OnInteracted();
    }
}