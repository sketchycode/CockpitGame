using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldsStation : StationBase {
	public ShipButton[] Buttons;
	public Sprite[] ButtonGlyphs;
	public LED GreenLED;
	public LED RedLED;
	public GlyphScreenController GlyphScreen;

	private int[] FaultCondition;

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

	public override float GetStationRating()
	{
		return 1f;
	}

	public override void ResetToGoodState()
	{
		
	}

	public override void TriggerFailureCondition()
	{
		if (FaultCondition == null)
		{
			SetupFaultCondition ();
		}
	}

	private void Button_Interacted(object sender, InputActionEventArgs e)
	{
		for (int i = 0; i < Buttons.Length; i++) 
		{
			if (sender == Buttons[i])
			{
				Debug.Log ("Button " + i + " pressed");
				break;
			}
		}
	}

	private void SetupFaultCondition()
	{
		FaultCondition = new int[Random.Range (1, Buttons.Length)];
		Sprite[] readout = new Sprite[FaultCondition.Length];

		for (int i = 0; i < FaultCondition.Length; i++)
		{
			FaultCondition [i] = Random.Range (0, Buttons.Length);
			readout[i] = ButtonGlyphs [FaultCondition [i]];
		}

		GlyphScreen.DisplayGlyphs (readout);
	}
}
