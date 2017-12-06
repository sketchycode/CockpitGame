using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumStation : StationBase
{
    public RangeIndicator EffectStengthIndicator;
    public ShipButton LockInButton;
    public ShipButton UseChargeButton;
    public ShipButton DetangleButton;
    public LED ChargeOneAvailableLED;
    public LED ChargeTwoAvailableLED;
    public LED ChargeThreeAvailableLED;
    public LED LockedInRedLED;
    public LED LockedInYellowLED;
    public LED LockedInGreenLED;

    public float TimeFactorForEffectChanges = .2f;
    public float CooldownAfterUse = 15f;

    private bool isLockedIn = false;
    private RangeSegmentStatus lockedInEffect;
    private float timeSinceUse = 0; // this is time since all charges used up or detangled
    private int numCharges = 0;
    private bool isInCooldown = false;

    void Start()
    {
        LockInButton.Interacted += LockInButton_Interacted;
        UseChargeButton.Interacted += UseChargeButton_Interacted;
        DetangleButton.Interacted += DetangleButton_Interacted;
    }

    void Update()
    {
        if (!isLockedIn && !isInCooldown)
        {
            EffectStengthIndicator.TrueValue = (float)Perlin.perlin(.5, .5, Time.timeSinceLevelLoad * TimeFactorForEffectChanges);
        }
        else if(isInCooldown)
        {
            timeSinceUse += Time.deltaTime;
            if(timeSinceUse > CooldownAfterUse)
            {
                isInCooldown = false;
                timeSinceUse = 0;
            }
        }

        ChargeOneAvailableLED.isOn = numCharges > 0;
        ChargeTwoAvailableLED.isOn = numCharges > 1;
        ChargeThreeAvailableLED.isOn = numCharges > 2;
    }

    private void LockInButton_Interacted(object sender, InputActionEventArgs e)
    {
        if(isLockedIn || isInCooldown) { return; }

        isLockedIn = true;
        EffectStengthIndicator.TrueValue = EffectStengthIndicator.CurrentValue;
        lockedInEffect = EffectStengthIndicator.CurrentStatus;
        LockedInRedLED.isOn = lockedInEffect == RangeSegmentStatus.Critical;
        LockedInYellowLED.isOn = lockedInEffect == RangeSegmentStatus.Warning;
        LockedInGreenLED.isOn = lockedInEffect == RangeSegmentStatus.Nominal;

        numCharges = 3;
    }

    private void UseChargeButton_Interacted(object sender, InputActionEventArgs e)
    {
        if(isLockedIn && numCharges > 0)
        {
            HandleGlobalEvent(GlobalEvent.QuantumChargeUsed, lockedInEffect);
            numCharges--;

            if(numCharges == 0)
            {
                EnterCooldown();
            }
        }
    }

    private void DetangleButton_Interacted(object sender, InputActionEventArgs e)
    {
        if(isLockedIn && !isInCooldown)
        {
            EnterCooldown();
        }
    }

    private void EnterCooldown()
    {
        isInCooldown = true;
        isLockedIn = false;
        numCharges = 0;
        timeSinceUse = 0;
        EffectStengthIndicator.TrueValue = 0;
        LockedInRedLED.isOn = false;
        LockedInYellowLED.isOn = false;
        LockedInGreenLED.isOn = false;
    }

    public override float GetStationRating()
    {
        return 0;
    }

    public override void ResetToGoodState()
    {
    }

    public override void TriggerFailureCondition()
    {
    }
}
