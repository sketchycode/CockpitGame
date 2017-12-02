using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour {
	public float BlinkRateSeconds;
	public Light MyLight;

	private bool shouldBlink;

	public void StartBlink(float blinkRateSeconds) {
		BlinkRateSeconds = blinkRateSeconds;
		shouldBlink = true;
		StartCoroutine(DoBlink(blinkRateSeconds));
	}

	public void StopBlink() {
		shouldBlink = false;
	}

	private IEnumerator DoBlink(float blinkRateSeconds) {
		while (shouldBlink) {			
			MyLight.enabled = !MyLight.enabled;
			yield return new WaitForSeconds (blinkRateSeconds);
		}
	}

	// Use this for initialization
	void Start () {
		StartBlink (BlinkRateSeconds);
	}

}
