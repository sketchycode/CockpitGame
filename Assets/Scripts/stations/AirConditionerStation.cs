using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// A/C works with a temp setting that forces temperate down if Heater is off,
/// and up when Heater is on. Max setting (all the way to the right) has an absolute
/// value of 1, with a direction based on heater switch value. A random "force" is
/// applied to the temperate periodically, with a max magnitude of .8. There is a .1
/// "stickiness" that will balance out when the net effect of temp setting and external
/// temp pressure has a magnitude less than or equal to .1
/// </summary>
public class AirConditionerStation : StationBase
{
    public FailureEvent ACFrozenFailure;
    public FailureEvent PilotLightOutFailure;

    public RangeIndicator TemperatureIndicator;
    public SlideBar TemperateSetting;
    public Switch Heater;
    public ShipButton ReigniteButton;
    public LED ACFrozenIndicator;
    public LED PilotLightOutIndicator;

    public float TimeToNewRandomTempForceInSeconds = 10f;

    private float externalTempForce = .4f;
    private float currentTemp = .6f;
    private float tempSetting = .4f;
    private float timeSinceLastRandomTempForce = 0;

    private bool InFailure { get { return ACFrozenFailure.InFailure || PilotLightOutFailure.InFailure; } }

    void Start()
    {
        TemperateSetting.Interacted += TemperateSetting_Interacted;
        Heater.Interacted += Heater_Interacted;
        ReigniteButton.Interacted += ReigniteButton_Interacted;
        TemperateSetting.SliderValue = tempSetting;
        Heater.IsOn = externalTempForce < 0;
    }

    void Update()
    {
        if (!InFailure)
        {
            if (Heater.IsOn) { PilotLightOutFailure.CheckForFailure(Time.deltaTime); }
            else { ACFrozenFailure.CheckForFailure(Time.deltaTime); }
        }

        PilotLightOutIndicator.isOn = PilotLightOutFailure.InFailure;
        ACFrozenIndicator.isOn = ACFrozenFailure.InFailure;

        CheckForNewRandomTempForce(Time.deltaTime);
        var hvacForce = GetHvacForce(tempSetting, Heater.IsOn, ACFrozenFailure.InFailure, PilotLightOutFailure.InFailure);
        var tempForceDiff = externalTempForce + hvacForce;
        if(Mathf.Abs(tempForceDiff) <= .1f) { tempForceDiff = 0; }

        currentTemp += Time.deltaTime * tempForceDiff;
        currentTemp = Mathf.Clamp01(currentTemp);
        TemperatureIndicator.TrueValue = currentTemp;
    }

    public override float GetStationRating()
    {
        switch(TemperatureIndicator.CurrentStatus)
        {
            case RangeSegmentStatus.Nominal: return 1f;
            case RangeSegmentStatus.Warning: return 0f;
            case RangeSegmentStatus.Critical: return -1f;
            default: return 0f;
        }
    }

    public override void ResetToGoodState()
    {
        ACFrozenFailure.Clear();
        PilotLightOutFailure.Clear();
    }

    public override void TriggerFailureCondition()
    {
        ACFrozenFailure.TriggerEvent();
        PilotLightOutFailure.TriggerEvent();
    }

    private void CheckForNewRandomTempForce(float elapsedTime)
    {
        timeSinceLastRandomTempForce += elapsedTime;
        if(timeSinceLastRandomTempForce >= TimeToNewRandomTempForceInSeconds)
        {
            timeSinceLastRandomTempForce = 0;
            externalTempForce = UnityEngine.Random.value * 1.6f - 0.8f;
        }
    }

    private float GetHvacForce(float tempSetting, bool heaterOn, bool acFailure, bool pilotLightFailure)
    {
        if((heaterOn && pilotLightFailure) || (!heaterOn && acFailure)) { return 0; }
        return tempSetting * (heaterOn ? 1f : -1f);
    }

    private void ReigniteButton_Interacted(object sender, InputActionEventArgs e)
    {
        ApplyInputActionToFailures(e.InputAction);
    }

    private void Heater_Interacted(object sender, InputActionEventArgs e)
    {
        ApplyInputActionToFailures(e.InputAction);
    }

    private void TemperateSetting_Interacted(object sender, InputActionEventArgs e)
    {
        ApplyInputActionToFailures(e.InputAction);
        tempSetting = e.InputAction.InputValue;
    }

    private void ApplyInputActionToFailures(InputAction inputAction)
    {
        if (PilotLightOutFailure.InFailure)
        {
            if(PilotLightOutFailure.ApplyInputActionToFailureReset(inputAction))
            {
                HandleFailureResolved(PilotLightOutFailure);
            }
        }
        if (ACFrozenFailure.InFailure)
        {
            if(ACFrozenFailure.ApplyInputActionToFailureReset(inputAction))
            {
                HandleFailureResolved(ACFrozenFailure);
            }
        }
    }
}
