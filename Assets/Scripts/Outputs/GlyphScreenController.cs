using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlyphScreenController : MonoBehaviour {
	private const int SCREEN_GLYPH_COUNT = 4;
	private const GlyphTextDirection DIRECTION = GlyphTextDirection.RTL;

	private Image[] screenGlyphs = new Image[SCREEN_GLYPH_COUNT];

	public void DisplayGlyphs(Sprite[] glyphs) 
	{	
		int dir;
		int spriteIndex;
		if (DIRECTION == GlyphTextDirection.RTL)
		{
			dir = -1;
			spriteIndex = glyphs.Length - 1;
		}
		else
		{
			dir = 1;
			spriteIndex = 0;
		}

		for (int i = 0; i < screenGlyphs.Length; i++)
		{
			if (spriteIndex >= 0 && spriteIndex < glyphs.Length)
			{
				screenGlyphs[i].enabled = true;
				screenGlyphs[i].sprite = glyphs[spriteIndex];
				spriteIndex += dir;
			}
			else
			{
				screenGlyphs [i].enabled = false;
			}
		}

	}

	public void ClearScreen()
	{
		for (int i = 0; i < screenGlyphs.Length; i++)
		{
			screenGlyphs [i].enabled = false;
		}
	}

	public void ClearGlyph(ScreenClearSide side)
	{
		if (ScreenClearSide.LEFT == side)
		{
			for (int i = screenGlyphs.Length - 1; i >= 0; i--) 
			{
				if (screenGlyphs[i].enabled)
				{
					screenGlyphs [i].enabled = false;
					break;
				}
			}
		}
		else
		{
			Sprite toDropSprite = screenGlyphs [0].sprite;
			for (int i = 1; i < screenGlyphs.Length; i++)
			{
				screenGlyphs [i - 1].sprite = screenGlyphs [i].sprite;
				screenGlyphs [i - 1].enabled = screenGlyphs [i].enabled;
			}
			screenGlyphs [screenGlyphs.Length - 1].sprite = toDropSprite;
			screenGlyphs [screenGlyphs.Length - 1].enabled = false;
		}
	}

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < screenGlyphs.Length; i++) 
		{
			screenGlyphs [i] = transform.Find ("Glyph" + i).gameObject.GetComponent<Image> ();
		}
	}

}

public enum GlyphTextDirection {
	RTL,
	LTR
}

public enum ScreenClearSide {
	LEFT,
	RIGHT
}
