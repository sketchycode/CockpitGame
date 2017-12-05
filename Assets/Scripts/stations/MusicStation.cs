using UnityEngine;

public class MusicStation : StationBase
{
    public RangeIndicator SongTimeRemainingIndicator;
    public ShipButton PrepareNewSongButton;
    public Switch TrackSwitch;
    public LED TrackOneLED;
    public LED TrackTwoLED;
    public SlideBar BeatMatcherSlider;

    public float MinSongTimeInSeconds = 20f;
    public float MaxSongTimeInSeconds = 35f;

    public float BeatMatchingEpsilon = .1f;

    private bool nextSongPrepared = false;
    private float nextSongBeatValue = 0;
    private float songDuration = 0;
    private float songDurationRemaining = 0;
    private bool songEnded = false;

    void Start()
    {
        songDuration = MinSongTimeInSeconds + (MaxSongTimeInSeconds - MinSongTimeInSeconds) * Random.value;
        songDurationRemaining = songDuration;
        TrackSwitch.IsOn = false;
        TrackOneLED.isOn = true;
        TrackSwitch.IsOn = false;
        BeatMatcherSlider.SliderValue = Random.value;

        PrepareNewSongButton.Interacted += PrepareNewSongButton_Interacted;
        TrackSwitch.Interacted += TrackSwitch_Interacted;
        BeatMatcherSlider.Interacted += BeatMatcherSlider_Interacted;
    }

    void Update()
    {
        songDurationRemaining = songDurationRemaining - Time.deltaTime;
        if(!songEnded && songDurationRemaining <= 0)
        {
            songEnded = true;
            HandleScoringEvent(ScoringEvent.SongEnded);
        }
        SongTimeRemainingIndicator.TrueValue = Mathf.Clamp01(songDurationRemaining / songDuration);
    }

    public override float GetStationRating()
    {
        return SongTimeRemainingIndicator.CurrentStatus == RangeSegmentStatus.Critical ? 0f : 1f;
    }

    public override void ResetToGoodState()
    {
        throw new System.NotImplementedException();
    }

    public override void TriggerFailureCondition()
    {
        throw new System.NotImplementedException();
    }

    private void BeatMatcherSlider_Interacted(object sender, InputActionEventArgs e)
    {
        CheckBeatMatchQuality(nextSongBeatValue, e.InputAction.InputValue, TrackSwitch.IsOn ? TrackOneLED : TrackTwoLED);
    }

    private void TrackSwitch_Interacted(object sender, InputActionEventArgs e)
    {
        var beatMatched = CheckBeatMatchQuality(nextSongBeatValue, BeatMatcherSlider.SliderValue, TrackSwitch.IsOn ? TrackOneLED : TrackTwoLED);
        TrackOneLED.StopBlinking();
        TrackOneLED.isOn = !TrackSwitch.IsOn;
        TrackTwoLED.StopBlinking();
        TrackTwoLED.isOn = TrackSwitch.IsOn;

        if (nextSongPrepared)
        {
            HandleScoringEvent(beatMatched ? ScoringEvent.BeatMatched : ScoringEvent.BeatDropped);

            songDuration = MinSongTimeInSeconds + (MaxSongTimeInSeconds - MinSongTimeInSeconds) * Random.value;
            songDurationRemaining = songDuration;
            songEnded = false;
            nextSongPrepared = false;
        }
        else
        {
            songDurationRemaining = 0f;
        }
    }

    private void PrepareNewSongButton_Interacted(object sender, InputActionEventArgs e)
    {
        nextSongPrepared = true;
        nextSongBeatValue = Random.value;
        CheckBeatMatchQuality(nextSongBeatValue, BeatMatcherSlider.SliderValue, TrackSwitch.IsOn ? TrackOneLED : TrackTwoLED);
    }

    private bool CheckBeatMatchQuality(float currentBeatValue, float nextBeatValue, LED nextSongLED)
    {
        if (!nextSongPrepared) { return false; }

        var beatDif = Mathf.Abs(currentBeatValue - nextBeatValue);
        if (beatDif < BeatMatchingEpsilon)
        {
            nextSongLED.StopBlinking();
            nextSongLED.isOn = true;
            return true;
        }
        else
        {
            nextSongLED.StartBlinking(beatDif * .6f, beatDif * .6f);
            return false;
        }
    }
}