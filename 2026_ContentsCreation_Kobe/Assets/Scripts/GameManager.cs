using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    enum PlayState
    {
        Title,
        Game,
        Finish
    }

    enum HumanStates
    {
        Hidden,
        Patrol,
        Flee,
        Escaped
    }

    PlayState playState;
    HumanStates[] humanStates;

    int minutes = 0;
    int seconds = 0;
    bool[] isFleeing;
    bool[] isPatrolling;

    [SerializeField] HumanAI[] humanAI = null;
    [SerializeField] GameObject cameraObject = null;
    TimerManager timerManager;

    private void Start()
    {
        timerManager = GetComponent<TimerManager>();
        playState = PlayState.Game; // Title
        humanStates = new HumanStates[humanAI.Length];
        isFleeing = new bool[humanAI.Length];
        isPatrolling = new bool[humanAI.Length];
        bool isHidden;

        if (cameraObject == null)
        {
            Debug.Log("カメラオブジェクトが設定されていません");
            QuitGame();
        }
        if (Gamepad.current == null)
        {
            Debug.Log("コントローラーが接続されていません");
            QuitGame();
        }

        for (int i = 0; i < humanAI.Length; i++)
        {
            isHidden = humanAI[i].IsHidden();
            if (isHidden)
            {
                humanStates[i] = HumanStates.Hidden;
            }
            else
            {
                humanStates[i] = HumanStates.Patrol;
            }
        }
    }

    private void Update()
    {
        switch (playState)
        {
            case PlayState.Title:
                // SceneManager.LoadScene("TitleScene");
                // if(当たり判定)
                SceneManager.LoadScene("GameScene");
                break;

            case PlayState.Game:
                for (int i = 0; i < humanAI.Length; i++)
                {
                    switch (humanStates[i])
                    {
                        case HumanStates.Hidden:
                        // if(見つかったら)
                        // humanStates[i] = HumanStates.Flee;
                            break;

                        case HumanStates.Patrol:
                            if (!isPatrolling[i] && humanAI[i].HumanMoveSystem() != -1)
                            {
                                StartCoroutine(PatrolRoutine(i));
                                isPatrolling[i] = true;
                            }                        
                            // if(見つかったら)
                            // humanStates[i] = HumanStates.Flee;
                            break;

                        case HumanStates.Flee:
                            if (!isFleeing[i])
                            {
                                humanAI[i].StartCoroutine(humanAI[i].FleeCoroutine()); // コルーチンを含んだ逃走処理開始
                                isFleeing[i] = true;
                            }
                            if (humanAI[i].IsEscaped())
                            {
                                humanStates[i] = HumanStates.Escaped;
                            }
                            break;

                        case HumanStates.Escaped:
                            break;
                    }
                }

                (minutes, seconds) = timerManager.TimerCalc(); // タプルで二個値が返ってきた

                if (minutes <= 0 && seconds <= 0)
                {
                    SceneManager.LoadScene("ClearScene");
                    playState = PlayState.Finish;
                }
                break;

            case PlayState.Finish:
                break;
        }
    }

    // HumanAI.csで設定したコルーチンを受け取って、終わったかチェック
    IEnumerator PatrolRoutine(int _index)
    {
        yield return StartCoroutine(humanAI[_index].PatrolCoroutine());

        isPatrolling[_index] = false;
    }

    private void QuitGame()
    {
        // アプリを終了
        Application.Quit();

        // Unityエディタ上でのテストプレイ時でも自動で停止させる
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
