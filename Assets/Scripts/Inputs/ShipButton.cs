using System;
using UnityEngine;

public class ShipButton : MonoBehaviour
{
    public event EventHandler Clicked;
    public string Name { get; set; }

    public void OnClick()
    {
        if(Clicked != null)
        {
            Clicked(this, EventArgs.Empty);
        }
    }
}
