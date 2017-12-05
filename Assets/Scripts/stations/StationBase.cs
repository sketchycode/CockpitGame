using System;
using UnityEngine;

public abstract partial class StationBase : MonoBehaviour
{
    public event EventHandler<FailureResolvedEventArgs> FailureResolved;
    public event EventHandler<ScoringEventArgs> ScoringEventOccurred;

    protected void HandleFailureResolved(FailureEvent failureEvent)
    {
        if(FailureResolved != null)
        {
            var args = new FailureResolvedEventArgs() { FailureEvent = failureEvent };
            FailureResolved(this, args);
        }
    }

    protected void HandleScoringEvent(ScoringEvent scoringEvent)
    {
        Debug.Log("scoring event occurred: " + scoringEvent.ToString());
        if(ScoringEventOccurred != null)
        {
            var args = new ScoringEventArgs() { Event = scoringEvent };
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

public class ScoringEventArgs : EventArgs
{
    public StationBase.ScoringEvent Event;
}