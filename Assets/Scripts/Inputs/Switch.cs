using System;
using UnityEngine;
using UnityEngine.UI;

public class Switch : UIInputBase
{
    public Toggle Toggle;

	public Image OffImage;
	public Image OnImage;

    public bool IsOn {
        get { return Toggle.isOn; }
        set { Toggle.isOn = value; }
    }

    public void OnValueChanged(bool value)
    {
        Value = value ? 1f : 0f;
		EnableDisableImages ();
        OnInteracted();
    }

	private void EnableDisableImages() 
	{		
		if (IsOn) {
			OffImage.enabled = false;
			OnImage.enabled = true;
		} else {
			OffImage.enabled = true;
			OnImage.enabled = false;
		}
	}

	void Start() 
	{
		EnableDisableImages ();
	}
		
}