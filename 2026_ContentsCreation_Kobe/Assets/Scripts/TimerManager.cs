using UnityEngine;

public class TimerManager : MonoBehaviour
{
    float timer = 300.0f;
    int minutes = 0;
    int seconds = 0;

    public (int,int) TimerCalc() // ƒ^ƒvƒ‹(2ŒÂ’l‚ð•Ô‚·)
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
        minutes = Mathf.FloorToInt(timer / 60);
        seconds = Mathf.FloorToInt(timer % 60);

        return (minutes,seconds);
    }
}
