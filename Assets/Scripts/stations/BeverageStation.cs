using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeverageStation : StationBase
{
    public RangeIndicator AlchoholLevel;
    public RangeIndicator SodaLevel;
    public Switch FillingIndicator;
    public ShipButton RefillButton;

    public float AverageTimeToAlchoholEmptyInSeconds = 20f;
    public float AverageTimeToSodaEmptyInSeconds = 30f;
    public int MaxDrinksAvailableForFullLevel = 50;

    private float availableAlchohol = 1f;
    private float availableSoda = 1f;

    void Start()
    {
        FillingIndicator.Interacted += FillingIndicator_Interacted;
        RefillButton.Interacted += RefillButton_Interacted;
        AlchoholLevel.SetImmediateValue(availableAlchohol);
        SodaLevel.SetImmediateValue(availableSoda);
    }

    void Update()
    {
        if(CheckDrinkServed(Time.deltaTime, AverageTimeToAlchoholEmptyInSeconds, MaxDrinksAvailableForFullLevel)) {
            availableAlchohol -= 1f / MaxDrinksAvailableForFullLevel;
        }

        if(CheckDrinkServed(Time.deltaTime, AverageTimeToSodaEmptyInSeconds, MaxDrinksAvailableForFullLevel))
        {
            availableSoda -= 1f / MaxDrinksAvailableForFullLevel;
        }

        availableAlchohol = Mathf.Clamp01(availableAlchohol);
        availableSoda = Mathf.Clamp01(availableSoda);

        AlchoholLevel.TrueValue = availableAlchohol;
        SodaLevel.TrueValue = availableSoda;
    }

    public override float GetStationRating()
    {
        return 2f -
            (AlchoholLevel.CurrentStatus == RangeSegmentStatus.Critical ? 2f : 0f) -
            (SodaLevel.CurrentStatus == RangeSegmentStatus.Critical ? 2f : 0f);
    }

    public override void ResetToGoodState()
    {
        availableAlchohol = 1f;
        availableSoda = 1f;
    }

    public override void TriggerFailureCondition()
    {
    }

    private bool CheckDrinkServed(float elapsedTime, float avgTimeToEmpty, float maxNumDrinks)
    {
        var framerate = 1f / elapsedTime;
        var checksPerSecond = framerate * (avgTimeToEmpty / maxNumDrinks);
        var probability = 1 - Mathf.Pow(.5f, (1f / checksPerSecond));
        return UnityEngine.Random.value <= probability;
    }

    private void RefillButton_Interacted(object sender, InputActionEventArgs e)
    {
        if(FillingIndicator.IsOn) { availableSoda = 1f; }
        else { availableAlchohol = 1f; }
    }

    private void FillingIndicator_Interacted(object sender, InputActionEventArgs e)
    {
    }
}
