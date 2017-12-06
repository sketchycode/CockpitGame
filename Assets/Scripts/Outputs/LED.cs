using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LED : MonoBehaviour
{
    public bool isOn = false;

    public Color onColor = Color.green;
    public Color offColor = Color.gray;
    public Color glowColor = Color.green;

    public Image bulb;
    public Image bulbGlow;

	private IEnumerator blinkCoroutine;

	public void Blink(float blinkFrequencySeconds)
	{		
		StopBlink ();
		blinkCoroutine = BlinkIndefinitely (blinkFrequencySeconds);
		StartCoroutine (blinkCoroutine);
	}

	private void StopBlink()
	{
		if (blinkCoroutine != null)
		{
			StopCoroutine (blinkCoroutine);
		}
	}

	private IEnumerator BlinkIndefinitely(float blinkFrequencySeconds)
	{
		while (true)
		{			
			_Toggle ();
			yield return new WaitForSeconds (blinkFrequencySeconds / 2f);
		}
	}

	public void Toggle()
	{
		StopBlink ();
		_Toggle ();
	}
	private void _Toggle()
	{
		isOn = !isOn;
		UpdateColorImageState (isOn);
	}

	public void TurnOn()
	{
		StopBlink ();
		isOn = true;
		UpdateColorImageState (isOn);
	}

	public void TurnOff()
	{
		StopBlink ();
		isOn = false;
		UpdateColorImageState (isOn);
	}

    void Update()
    {
		UpdateColorImageState (isOn);
    }

	private void UpdateColorImageState(bool enabled)
	{
		if (enabled)
		{
			bulb.color = onColor;
			bulbGlow.color = glowColor;
			bulbGlow.enabled = true;
		}
		else
		{
			bulb.color = offColor;
			bulbGlow.enabled = false;
		}
	}

}
