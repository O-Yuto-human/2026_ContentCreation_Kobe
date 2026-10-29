using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int minutes = 0;
    int seconds = 0;

    // 型名：クラス名
    TimerManager timerManager;

    private void Start()
    {
        timerManager = GetComponent<TimerManager>();
    }

    private void Update()
    {
        // タイマー
        (minutes,seconds) = timerManager.TimerCalc();

        // シーン遷移条件
        if (minutes <= 0 && seconds <= 0)
        {
            SceneManager.LoadScene("ClearScene");
        }
    }
}
