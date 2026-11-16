using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TMP_Text timer;

    public void Timer(int _minute,int _second)
    {
        timer.text = $"{_minute:D2}:{_second:D2}"; // D2:Å’á“ñŒ…
    }
}
