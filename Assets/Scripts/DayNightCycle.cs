using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
public Light sun;
public float dayLengthInMinutes = 5f;
public float timeOfDay = 8f;

public float dayIntensity = 1.2f;
public float nightIntensity = 0.05f;

public Color dayColor = Color.white;
public Color sunsetColor = new Color(1f, 0.5f, 0.2f);
public Color nightColor = new Color(0.1f, 0.15f, 0.35f);

void Update()
{
    timeOfDay += Time.deltaTime * 24f / (dayLengthInMinutes * 60f);

    if (timeOfDay >= 24f)
    {
        timeOfDay = 0f;
    }

    float sunAngle = (timeOfDay - 6f) * 15f;

    sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

    if (timeOfDay >= 6f && timeOfDay < 17f)
    {
        sun.intensity = dayIntensity;
        sun.color = dayColor;
    }
    else if (timeOfDay >= 17f && timeOfDay < 20f)
    {
        sun.intensity = Mathf.Lerp(dayIntensity, nightIntensity, (timeOfDay - 17f) / 3f);
        sun.color = sunsetColor;
    }
    else
    {
        sun.intensity = nightIntensity;
        sun.color = nightColor;
    }
}

}
