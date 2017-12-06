using System;
using UnityEngine;

public abstract partial class StationBase : MonoBehaviour
{
    public event EventHandler<FailureResolvedEventArgs> FailureResolved;
    public event EventHandler<GlobalEventArgs> ScoringEventOccurred;

    protected void HandleFailureResolved(FailureEvent failureEvent)
    {
        if(FailureResolved != null)
        {
            var args = new FailureResolvedEventArgs() { FailureEvent = failureEvent };
            FailureResolved(this, args);
        }
    }

    protected void HandleGlobalEvent(GlobalEvent globalEvent, RangeSegmentStatus eventQuality = RangeSegmentStatus.Nominal)
    {
        Debug.Log("global event occurred: " + globalEvent.ToString());
        if(ScoringEventOccurred != null)
        {
            var args = new GlobalEventArgs() { Event = globalEvent };
            ScoringEventOccurred(this, args);
        }
    }

    /// <summary>
    /// Returns a value from 0-1 inclusive, 1 being the best
    /// </summary>
    /// <returns></returns>
    public abstract float GetStationRating();
    public abstract void ResetToGoodState();
    public abstract void TriggerFailureCondition();
}

public class FailureResolvedEventArgs : EventArgs
{
    public FailureEvent FailureEvent;
}

public class GlobalEventArgs : EventArgs
{
    public StationBase.GlobalEvent Event;
    public RangeSegmentStatus Quality;
}