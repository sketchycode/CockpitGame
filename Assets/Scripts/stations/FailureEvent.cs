using System;

[Serializable]
public class FailureEvent
{
    public event EventHandler<FailureEventStateChangedEventArgs> StateChanged;
    public string Name;
    public InputAction[] ResetSequence;
    public float AverageTimeToFailureInSeconds;
    public float CooldownTimeInSeconds;

    public bool InFailure
    {
        get { return m_inFailure; }
        private set
        {
            if(value != m_inFailure)
            {
                if(StateChanged != null)
                {
                    var args = new FailureEventStateChangedEventArgs() { NewState = value, OldState = m_inFailure };
                    StateChanged(this, args);
                }
                m_inFailure = value;

                if(m_inFailure)
                {
                    resetSequenceIndex = 0;
                }
            }
        }
    }
    public bool InCooldown { get; private set; }

    private float timeSinceFailure = 0;
    private int resetSequenceIndex = 0; // for tracking how far into the reset sequence we are
    private bool m_inFailure = false;

    public bool CheckForFailure(float elapsedTime)
    {
        if(InFailure) { return true; }

        if (InCooldown)
        {
            timeSinceFailure += elapsedTime;
            if(timeSinceFailure > CooldownTimeInSeconds)
            {
                timeSinceFailure = 0;
                InCooldown = false;
            }

            return false;
        }

        // probability is 1 - 0.5^(1/numIntervals)
        float numIntervals = AverageTimeToFailureInSeconds / elapsedTime;
        if (numIntervals == 0) { return false; }

        bool didFail = UnityEngine.Random.value < (1 - UnityEngine.Mathf.Pow(0.5f, 1f/numIntervals));

        if(didFail) {
            InFailure = true;
        }

        return didFail;
    }

    /// <summary>
    /// Applies the given inputAction to the reset sequence, and returns whether the failure
    /// event has been resolved.
    /// </summary>
    /// <param name="inputAction"></param>
    /// <returns>True if the failure state has been resolved, false otherwise</returns>
    public bool ApplyInputActionToFailureReset(InputAction inputAction)
    {
        if(!InFailure) { return false; }

        var nextActionNeeded = ResetSequence[resetSequenceIndex];

        if(nextActionNeeded.IsSatisfiedBy(inputAction))
        {
            resetSequenceIndex++;

            if(resetSequenceIndex >= ResetSequence.Length)
            {
                InFailure = false;
                InCooldown = true;
                timeSinceFailure = 0;
                return true;
            }
        }

        return false;
    }

    public void Clear()
    {
        InFailure = false;
        InCooldown = false;
        timeSinceFailure = 0;
        resetSequenceIndex = 0;
    }

    public void TriggerEvent(bool ignoreIfInFailState = false, bool ignoreIfInCooldown = false)
    {
        if (InFailure && ignoreIfInFailState) { return; }
        if (InCooldown && ignoreIfInCooldown) { return; }

        Clear();
        InFailure = true;
    }
}

public class FailureEventStateChangedEventArgs : EventArgs
{
    public bool OldState;
    public bool NewState;
}