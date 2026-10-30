using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int minutes = 0;
    int seconds = 0;

    TimerManager timerManager;

    private void Start()
    {
        timerManager = GetComponent<TimerManager>();
    }

    private void Update()
    {
        
        (minutes,seconds) = timerManager.TimerCalc(); // ƒ^ƒvƒ‹‚Å“ñŒÂ’l‚ª•Ô‚Á‚Ä‚«‚½

        
        if (minutes <= 0 && seconds <= 0)
        {
            SceneManager.LoadScene("ClearScene");
        }
    }
}
