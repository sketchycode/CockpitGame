using System;
using UnityEngine;

public abstract class StationBase : MonoBehaviour
{
    public event EventHandler FailureResolved;

    protected void HandleFailureResolved(FailureEvent failureEvent)
    {
        if(FailureResolved != null)
        {
            var args = new FailureResolvedEventArgs() { FailureEvent = failureEvent };
            FailureResolved(this, args);
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