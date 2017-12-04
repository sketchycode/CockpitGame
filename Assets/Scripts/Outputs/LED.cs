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

    void Update()
    {
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
