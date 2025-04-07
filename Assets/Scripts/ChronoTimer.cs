using UnityEngine;

public class ChronoTimer
{
    private float startTime;
    public bool isRunning = false;

    public float elapsedTime = 0;

    public void StartChrono()
    {
        startTime = Time.time;
        isRunning = true;
    }

    public void StopChrono()
    {
        isRunning = false;
        elapsedTime = Time.time - startTime;
    }
}
