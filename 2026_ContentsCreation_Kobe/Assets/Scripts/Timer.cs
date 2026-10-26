using UnityEngine;

public class Timer : MonoBehaviour
{
    float timer = 0;

    float TimerManager()
    {
        timer += Time.deltaTime;

        return timer;
    }
}
