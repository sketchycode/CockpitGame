using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashStation : StationBase
{
    public FailureEvent FailureEvent;

    public RangeIndicator TrashLevelIndicator;
    public ShipButton DumpButton;
    public Switch RollerSwitch;
    public LED FailureIndicator;

    public float MaxTrashCapacity;
    public float TrashBuildRate;

    private float TrashLevel;

    public override float GetStationRating()
    {
        switch (TrashLevelIndicator.CurrentStatus)
        {
            case RangeSegmentStatus.Nominal:
            case RangeSegmentStatus.Warning:
                return 1f;
            case RangeSegmentStatus.Critical:
                return 0f;
            default:
                return 1f;
        }
    }

    public override void ResetToGoodState()
    {
        FailureEvent.Clear();
    }

    public override void TriggerFailureCondition()
    {
        FailureEvent.TriggerEvent(true, true);
    }

    void Start()
    {
        DumpButton.Interacted += DumpButton_Interacted;
        RollerSwitch.Interacted += RollerSwitch_Interacted;
    }

    void Update()
    {
        bool inFailure = FailureEvent.CheckForFailure(Time.deltaTime);
        FailureIndicator.isOn = inFailure;

        TrashLevel = Mathf.Clamp(TrashLevel + Time.deltaTime * TrashBuildRate, 0, MaxTrashCapacity);
        TrashLevelIndicator.TrueValue = TrashLevel / MaxTrashCapacity;
    }

    private void DumpButton_Interacted(object sender, InputActionEventArgs e)
    {
        if (FailureEvent.InFailure)
        {
            if (FailureEvent.ApplyInputActionToFailureReset(e.InputAction))
            {
                HandleFailureResolved(FailureEvent);
            }
        }
        else
        {
            TrashLevel = 0;
        }
    }

    private void RollerSwitch_Interacted(object sender, InputActionEventArgs e)
    {
        if(FailureEvent.InFailure)
        {
            if (FailureEvent.ApplyInputActionToFailureReset(e.InputAction))
            {
                HandleFailureResolved(FailureEvent);
            }
        }
    }
}