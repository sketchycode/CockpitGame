using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RangeIndicator : MonoBehaviour
{
    public RangeSegment[] Segments;

    [Range(0, 1f)]
    public float TrueValue; // gauge will animate to this value

    private GameObject background;
    private Slider slider;
    private float currentValue;

    public void Start()
    {
        background = transform.Find("Background").gameObject;
        slider = GetComponent<Slider>();
        currentValue = TrueValue;
    }
    
    public void Update()
    {
        if (Segments.Length > 0)
        {
            AdjustBackgroundSegmentsIfNeeded();
            currentValue = Mathf.Lerp(currentValue, TrueValue, Time.deltaTime * 5);
            slider.value = currentValue;
        }
    }

    /// <summary>
    /// Use this to set the displayed value immediately to the given value.
    /// </summary>
    public void SetImmediateValue(float value)
    {
        TrueValue = value;
        currentValue = value;
    }

    public RangeSegmentStatus CurrentStatus
    {
        get
        {
            var segmentWidthTotal = Segments.Sum(s => s.Length);
            float currentWidth = 0;

            for(int i=0; i<Segments.Length; i++)
            {
                currentWidth += Segments[i].Length / segmentWidthTotal;
                if(currentWidth >= TrueValue) { return Segments[i].Status; }
            }

            return Segments[Segments.Length - 1].Status;
        }
    }

    private void AdjustBackgroundSegmentsIfNeeded()
    {
        while (background.transform.childCount < Segments.Length)
        {
            GameObject panel = new GameObject("BackgroundRangeSegment");
            panel.AddComponent<CanvasRenderer>();
            panel.AddComponent<RectTransform>();
            panel.AddComponent<Image>();
            panel.transform.SetParent(background.transform, false);
        }

        while (background.transform.childCount > Segments.Length)
        {
            var go = background.transform.GetChild(0);
            go.parent = null;
            GameObject.Destroy(go);
        }

        var backgroundRect = background.GetComponent<RectTransform>();
        var segmentWidthTotal = Segments.Sum(s => s.Length);
        float left = 0;
        for (int i=0; i<background.transform.childCount; i++)
        {
            var rt = background.transform.GetChild(i).GetComponent<RectTransform>();
            var width = i == (background.transform.childCount - 1) ?
                backgroundRect.rect.width - left :
                backgroundRect.rect.width * (Segments[i].Length / segmentWidthTotal);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, left, width - backgroundRect.rect.width);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            left += width;

            var image = background.transform.GetChild(i).GetComponent<Image>();
            image.color = Segments[i].Status == RangeSegmentStatus.Critical ? Color.red :
                Segments[i].Status == RangeSegmentStatus.Warning ? Color.yellow :
                Color.green;
        }
    }
}

public enum RangeSegmentStatus
{
    Nominal,
    Warning,
    Critical
}

[System.Serializable]
public class RangeSegment
{
    public float Length;
    public RangeSegmentStatus Status;
}