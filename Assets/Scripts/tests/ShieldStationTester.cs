using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldStationTester : MonoBehaviour {
	public GlyphScreenController GlyphController;
	public ShieldsStation ShieldsStation;
	public Sprite[] Sprites;

	private bool doOnce = true;

	// Use this for initialization
	void Start () {
		
	}

	void Update() {
		if (doOnce == true) {
			doOnce = false;

			StartCoroutine (PerformTest ());
		}
	}

	private IEnumerator PerformTest()
	{
		ShieldsStation.TriggerFailureCondition ();
		yield return new WaitForSeconds (1f);

		/*
		GlyphController.DisplayGlyphs (Sprites);
		yield return new WaitForSeconds (3f);

		GlyphController.ClearScreen ();
		yield return new WaitForSeconds (2f);

		GlyphController.DisplayGlyphs (Sprites);
		yield return new WaitForSeconds (2f);

		GlyphController.ClearGlyph (ScreenClearSide.RIGHT);
		yield return new WaitForSeconds (1f);
		GlyphController.ClearGlyph (ScreenClearSide.RIGHT);
		yield return new WaitForSeconds (1f);
		GlyphController.ClearGlyph (ScreenClearSide.RIGHT);
		yield return new WaitForSeconds (1f);

		GlyphController.DisplayGlyphs (Sprites);
		yield return new WaitForSeconds (3f);

		GlyphController.ClearGlyph (ScreenClearSide.LEFT);
		yield return new WaitForSeconds (1f);
		GlyphController.ClearGlyph (ScreenClearSide.LEFT);
		yield return new WaitForSeconds (1f);
		GlyphController.ClearGlyph (ScreenClearSide.LEFT);
		yield return new WaitForSeconds (1f);
		*/
	}
}
