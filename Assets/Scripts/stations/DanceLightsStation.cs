using System.Linq;
using UnityEngine;

public class DanceLightsStation : StationBase
{
    public RangeIndicator MoodRangeIndicator;
    public SlideBar PatternSlider;
    public ShipButton MoodShuffleButton;

    public float AverageTimeToAutoMoodShuffleInSeconds = 8f;
    [Range(0, 1)]
    public float AutoShuffleHidesGoodMoodProbability = 0.2f;

    private RangeSegment[] trueMoodRanges;

    void Start()
    {
        PatternSlider.Interacted += PatternSlider_Interacted;
        MoodShuffleButton.Interacted += MoodShuffleButton_Interacted;
        PatternSlider.SliderValue = .5f;
        MoodRangeIndicator.SetImmediateValue(PatternSlider.SliderValue);

        trueMoodRanges = new RangeSegment[MoodRangeIndicator.Segments.Length];
        for(int i=0; i<MoodRangeIndicator.Segments.Length; i++)
        {
            trueMoodRanges[i] = MoodRangeIndicator.Segments[i].Clone();
        }
    }

    void Update()
    {
        CheckForMoodShuffle(Time.deltaTime, AverageTimeToAutoMoodShuffleInSeconds);

        for(int i=0; i<trueMoodRanges.Length; i++)
        {
            var range = MoodRangeIndicator.Segments[i];
            range.Length = Mathf.Lerp(range.Length, trueMoodRanges[i].Length, 5f * Time.deltaTime);
        }
    }

    public override float GetStationRating()
    {
        return MoodRangeIndicator.CurrentStatus == RangeSegmentStatus.Critical ? -1f : 1f;
    }

    public override void ResetToGoodState()
    {
    }

    public override void TriggerFailureCondition()
    {
    }

    private void MoodShuffleButton_Interacted(object sender, InputActionEventArgs e)
    {
        ShuffleMood(true);
    }

    private void PatternSlider_Interacted(object sender, InputActionEventArgs e)
    {
        MoodRangeIndicator.TrueValue = e.InputAction.InputValue;
    }

    private void ShuffleMood(bool forceGoodMoodVisible)
    {
        bool goodMoodVisible = forceGoodMoodVisible || (Random.value > AutoShuffleHidesGoodMoodProbability);

        foreach(var range in trueMoodRanges.Where(s => s.Status != RangeSegmentStatus.Critical))
        {
            range.Length = goodMoodVisible ? 1f : 0f;
        }

        var totalCriticalLength = trueMoodRanges.Sum(s => s.Status == RangeSegmentStatus.Critical ? s.Length : 0);
        var numCriticals = trueMoodRanges.Sum(s => s.Status == RangeSegmentStatus.Critical ? 1 : 0);

        var critsAssigned = 0;
        foreach(var range in trueMoodRanges.Where(s => s.Status == RangeSegmentStatus.Critical))
        {
            critsAssigned++;

            range.Length = totalCriticalLength * (critsAssigned == numCriticals ? 1f : UnityEngine.Random.value);
            totalCriticalLength -= range.Length;
        }
    }

    private void CheckForMoodShuffle(float elapsedTime, float avgTimeToShuffle)
    {
        float numIntervals = avgTimeToShuffle / elapsedTime;
        if (numIntervals == 0) { return; }

        if (UnityEngine.Random.value < (1 - UnityEngine.Mathf.Pow(0.5f, 1f / numIntervals)))
        {
            ShuffleMood(false);
        }
    }
}
