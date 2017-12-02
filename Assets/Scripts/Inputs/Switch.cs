using System;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public event EventHandler<SwitchValueChangedEventArgs> ValueChanged;

    public string Name { get; set; } 

    public void OnValueChanged(bool value)
    {
        if (ValueChanged != null)
        {
            var args = new SwitchValueChangedEventArgs();
            args.NewValue = value;
            ValueChanged(this, args);
        }
    }
}

public class SwitchValueChangedEventArgs : EventArgs
{
    public bool NewValue { get; set; }
}