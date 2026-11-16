using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    enum HumanStates
    {
        Hidden,
        Patrol,
        Flee,
        Escaped
    }

    HumanStates[] humanStates;

    int minutes = 0;
    int seconds = 0;
    float[] respawnTime;
    bool[] isFleeing;
    bool[] isPatrolling;
    bool isSpawned; // テスト用
    bool[] isDespawned;

    [SerializeField] HumanAI[] humanAI = null;
    [SerializeField] GameObject cameraObject = null;
    [SerializeField] PlayerController playerController = null;
    [SerializeField] PlayerLookingController playerLookingController = null;
    [SerializeField] TimerUI timerUI = null;
    [SerializeField] ParentOutLook parentOutLook = null;
    [SerializeField] WayPointMove wayPointMove = null;

    TimerManager timerManager;
    SpawnSystem spawnSystem;
    SceneController sceneController;

    private void Start()
    {
        spawnSystem = GetComponent<SpawnSystem>();
        timerManager = GetComponent<TimerManager>();
        humanStates = new HumanStates[humanAI.Length];
        isFleeing = new bool[humanAI.Length];
        isPatrolling = new bool[humanAI.Length];
        respawnTime = new float[humanAI.Length];
        isDespawned = new bool[humanAI.Length];
        parentOutLook.OnPlayerFound += OnPlayerFound; // 関数そのものの登録

        /*if (cameraObject == null)
        {
            Debug.Log("カメラオブジェクトが設定されていません");
            QuitGame();
        }
        if (Gamepad.current == null)
        {
            Debug.Log("コントローラーが接続されていません");
            QuitGame();
        }*/

        for (int i = 0; i < humanAI.Length; i++)
        {
            if (humanAI[i].CompareTag("Human"))
            {
                humanStates[i] = HumanStates.Patrol;
            }
            else if (humanAI[i].CompareTag("HiddenHuman"))
            {
                humanStates[i] = HumanStates.Hidden;
            }
        }
    }

    private void Update()
    {
        playerController.PlayerMovement();
        playerLookingController.PlayerLooking();
        wayPointMove.ParentMove();
        parentOutLook.CheckSight();

        if (isSpawned == false)
        {
            spawnSystem.FirstSpawn();
            isSpawned = true;
        }

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
                        isFleeing[i] = false;
                        humanStates[i] = HumanStates.Escaped;
                    }
                    break;

                case HumanStates.Escaped:
                    if (isDespawned[i] == true)
                    {
                        humanAI[i].gameObject.SetActive(false);
                        respawnTime[i] = spawnSystem.RespawnTimer(i);

                        if (respawnTime[i] <= 0)
                        { 
                            spawnSystem.Respawn(i);
                            humanAI[i].gameObject.SetActive(true);
                            isDespawned[i] = false;

                            if (humanAI[i].CompareTag("Human"))
                            {
                                humanAI[i].SetRandomDestination(); // 再度目的地設定し、コルーチンの影響を無効に
                                humanStates[i] = HumanStates.Patrol;
                            }
                            else if (humanAI[i].CompareTag("HiddenHuman"))
                            {
                                humanStates[i] = HumanStates.Hidden;
                            }
                        }
                    }
                    else
                    {
                        isDespawned[i] = true;
                    }
                    break;
            }
        }

        (minutes, seconds) = timerManager.TimerCalc(); // タプルで二個値が返ってきた

        if (minutes <= 0 && seconds <= 0)
        {
            sceneController.ChangeScene();
        }
        timerUI.Timer(minutes,seconds);
    }

    // HumanAI.csで設定したコルーチンを受け取って、終わったかチェック

    private IEnumerator PatrolRoutine(int _index)
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

    private void OnPlayerFound(int found_count)
    {
        if (found_count == 1 || found_count == 2)
        {
            playerController.WasFound(found_count);
        }
        else if (found_count == 3)
        {
            sceneController.ChangeScene();
        }
    }
}
