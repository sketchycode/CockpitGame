using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {
	public float BlinkRateSeconds;
	public Light MyLight;

	private IEnumerator coroutine;

	public void StartBlink() {
		StopBlink ();
		coroutine = BlinkIndefinitely (BlinkRateSeconds);
		StartCoroutine(coroutine);
	}

	public void StartBlink(float blinkRateSeconds) {		
		StopBlink ();
		coroutine = BlinkIndefinitely (blinkRateSeconds);
		StartCoroutine (coroutine);
	}
		

	public void StartBlink(float blinkRateSeconds, int blinkCount) {
		StopBlink ();
		coroutine = BlinkTimes (blinkRateSeconds, blinkCount);
		StartCoroutine (coroutine);
	}

	public void StartBlink(int blinkCount) {
		StopBlink ();
		coroutine = BlinkTimes (BlinkRateSeconds, blinkCount);
		StartCoroutine (coroutine);
	}

	public void StopBlink() {
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
	}

	public void LightOn() {
		StopBlink ();
		MyLight.enabled = true;
	}

	public void LightOff() {
		StopBlink ();
		MyLight.enabled = false;
	}

	public void ToggleLight() {
		StopBlink ();
		MyLight.enabled = !MyLight.enabled;
	}

	private IEnumerator BlinkIndefinitely(float blinkRateSeconds) {
		while (true) {			
			MyLight.enabled = !MyLight.enabled;
			yield return new WaitForSeconds (blinkRateSeconds);
		}
	}

	private IEnumerator BlinkTimes(float blinkRateSeconds, int times) {
		times *= 2;
		while (times > 0) {
			MyLight.enabled = !MyLight.enabled;
			times--;
			yield return new WaitForSeconds (blinkRateSeconds);
		}
	}

	// Use this for initialization
	void Start () {
		MyLight.enabled = false;
	}

}
