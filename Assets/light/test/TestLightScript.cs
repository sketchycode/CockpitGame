using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLightScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject light = GameObject.Find ("RedLight");
		BlinkRandom (light);

		light = GameObject.Find ("BlueLight");
		BlinkRandom (light);

		light = GameObject.Find ("GreenLight");
		BlinkRandom (light);
	}

	private void BlinkRandom(GameObject light) {
		LightController controller = light.GetComponent<LightController> ();
		int blinkCount = Random.Range (10, 21);
		float blinkRate = Random.Range (0.1f, 1f);
		controller.StartBlink(blinkRate, blinkCount);
	}

}
