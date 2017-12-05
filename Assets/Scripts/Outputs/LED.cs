using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LED : MonoBehaviour
{
    public bool isOn = false;

    public Color onColor = Color.green;
    public Color offColor = Color.gray;
    public Color glowColor = Color.green;

    public Image bulb;
    public Image bulbGlow;

    private bool isBlinking = false;
    private float timeSinceCycle = 0;
    private float onTime = 0;
    private float offTime = 0;

    public void StartBlinking(float onTime, float offTime)
    {
        this.onTime = onTime;
        this.offTime = offTime;
        isBlinking = true;
    }

    public void StopBlinking()
    {
        isBlinking = false;
    }

    void Update()
    {
        if(isBlinking)
        {
            timeSinceCycle += Time.deltaTime;
            isOn = timeSinceCycle < onTime || timeSinceCycle > (onTime + offTime);
            if (timeSinceCycle > (onTime + offTime)) { timeSinceCycle -= onTime + offTime; }
        }

        if(isOn)
        {
            bulb.color = onColor;
            bulbGlow.color = glowColor;
            bulbGlow.enabled = true;
        }
        else
        {
            bulb.color = offColor;
            bulbGlow.enabled = false;
        }
    }
}
