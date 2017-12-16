using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldsStation : StationBase {
	public ShipButton[] Buttons;
	public Sprite[] ButtonGlyphs;
	public Sprite[] DisplayGlyphs;
	public LED GreenLED;
	public LED RedLED;
	public GlyphScreenController GlyphScreen;

	public bool IsFault { get { return FaultCondition.Count > 0; } }

	private Stack<int> FaultCondition = new Stack<int> ();

	public float FaultsPerMinute;
	private float faultTriggerCounter;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < Buttons.Length; i++)
		{
			Buttons [i].Interacted += Button_Interacted;
		}

		for (int i = 0; i < ButtonGlyphs.Length; i++)
		{
			Image img = transform.Find ("Button" + i + "_Image").gameObject.GetComponent<Image> ();
			img.sprite = ButtonGlyphs [i];
		}
	}

	void Update() {
		if (!IsFault)
		{
			float target = 60f / FaultsPerMinute;
			faultTriggerCounter += Time.deltaTime + (Random.value * Time.deltaTime);
			if (faultTriggerCounter >= target)
			{
				TriggerFailureCondition ();
				faultTriggerCounter = 0f;
			}
		}
	}

	public override float GetStationRating()
	{
		float rating;
		if (FaultCondition.Count > 0)
		{
			rating = 0f;
		}
		else 
		{
			rating = 1f;
		}

		return rating;
	}

	public override void ResetToGoodState()
	{
		
	}

	public override void TriggerFailureCondition()
	{
		if (FaultCondition.Count < 1)
		{
			SetupFaultCondition ();
		}
	}

	private void Button_Interacted(object sender, InputActionEventArgs e)
	{
		if (FaultCondition.Count > 0)
		{
			for (int i = 0; i < Buttons.Length; i++) 
			{				
				if (sender == Buttons[i])
				{
					if (FaultCondition.Peek() == i)
					{
						FaultCondition.Pop ();
						GlyphScreen.ClearGlyph (ScreenClearSide.LEFT);

						if (FaultCondition.Count < 1)
						{
							FaultCleared ();
						}
					}
					break;
				}
			}
		}
	}

	private void FaultCleared()
	{
		GreenLED.TurnOn ();
		RedLED.TurnOff ();
		HandleGlobalEvent (GlobalEvent.ShieldsRemodulated);
	}

	private void SetupFaultCondition()
	{
		FaultCondition.Clear ();
		int numPresses = Random.Range (1, Buttons.Length + 1);
		Sprite[] readout = new Sprite[numPresses];

		for (int i = numPresses - 1; i >= 0; i--)
		{
			int fault = Random.Range (0, Buttons.Length);
			FaultCondition.Push (fault);
			readout[i] = DisplayGlyphs [fault];
		}

		GlyphScreen.DisplayGlyphs (readout);

		GreenLED.TurnOff ();
		RedLED.StartBlinking (0.5f);
	}

}
