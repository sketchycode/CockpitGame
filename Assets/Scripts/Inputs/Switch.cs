using System;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public event EventHandler<SwitchValueChangedEventArgs> ValueChanged;

    public string Name { get; set; } 

    private void OnValueChanged(bool value)
    {
        if (ValueChanged != null)
        {
            var args = new SwitchValueChangedEventArgs();
            args.newValue = value;
            ValueChanged(this, args);
        }
    }
}

public class SwitchValueChangedEventArgs : EventArgs
{
    public bool newValue { get; set; }
}