using System;
using UnityEngine;

public class SlideBar : MonoBehaviour
{
    public string Name { get; set; }

    public event EventHandler<SlideBarValueChangedEventArgs> ValueChanged;

    public void OnValueChanged(float value)
    {
        if(ValueChanged != null)
        {
            var args = new SlideBarValueChangedEventArgs();
            args.NewValue = value;
            ValueChanged(this, args);
        }
    }
}

public class SlideBarValueChangedEventArgs : EventArgs
{
    public float NewValue { get; set; }
}
