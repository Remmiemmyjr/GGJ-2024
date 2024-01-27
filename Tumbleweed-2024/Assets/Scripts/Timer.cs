using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // UI Timer reference
    public TMPro.TextMeshProUGUI timeText;

    // The elapsed time
    private float elapsedTime;

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Format the timer text
        string formattedTime = FormatTime(elapsedTime);

        // Update the UI text to display the formatted time
        timeText.text = formattedTime;
    }

    string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 1000) % 1000);

        // Format the timer and return
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
