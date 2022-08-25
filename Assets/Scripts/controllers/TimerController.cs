using UnityEngine.UIElements;

public class TimerController
{
    private ProgressBar timer;

    public TimerController(ProgressBar progressBarElement)
    {
        timer = progressBarElement;
    }

    public void SetTimerRange(float maxTime, float minTime = 0)
    {
        timer.lowValue = minTime;
        timer.highValue = maxTime;
    }

    public void SetTimerValue(float value)
    {
        timer.value = value;
    }
}
