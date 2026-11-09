using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
public class PlayerController : MonoBehaviour
{

    //-------定数宣言-------
    const float MOVE_SPEED = 7.0f;   //移動速度

    const float INTERACT_DISTANCE = 3.0f; //インタラクト可能な距離

    //-------変数宣言-------
    [SerializeField] private GameObject cameraObject = null;    // 視点移動用のカメラオブジェクト取得用
    int addScore = 0; //スコア加算用の変数

    float p2eDistance = 0.0f;            //プレイヤーと敵の距離
    Vector3 enemyPos = Vector3.zero;    //敵の座標
    GameObject currentTarget = null;    //現在最も近いターゲット

    bool isFreeze = false; //プレイヤーの硬直判定を管理する変数(初期値false = 移動可能)


    private void Start()
    {
        if (cameraObject == null)
        {
            Debug.Log("カメラオブジェクトが設定されていません");
            QuitGame(); 
        }
        /*if(Gamepad.current == null)
        {
            Debug.Log("コントローラーが接続されていません");
            QuitGame();
        }*/

    }


    void PlayerMovement() //GameManagerから呼び出し
    {
        if (isFreeze == false)
        {
            Move();
            FindTarget();
        }

        //if (Gamepad.current.aButton.wasPressedThisFrame && currentTarget != null)
        //{
        if (Input.GetKeyDown(KeyCode.Space) && currentTarget != null)
        {
            Surprised();
        }
    }

    private void Update()
    {
        if (isFreeze == false)
        {
            Move();
            FindTarget();
        }

        //if (Gamepad.current.aButton.wasPressedThisFrame && currentTarget != null)
        //{
        if (Input.GetKeyDown(KeyCode.Space) && currentTarget != null)
        {
            Surprised();
        }
    }



    void Move()
    {
        // キーボード(WASD)とゲームパッド(左スティック)の入力をまとめて取得
        float inputX = Input.GetAxisRaw("Horizontal");      //A/Dキー または 左スティックの左右
        float inputZ = Input.GetAxisRaw("Vertical");        //W / Sキー または 左スティックの上下

        Vector3 moveVec = new Vector3(inputX, 0f, inputZ);
        
        // 斜め移動量の正規化（スティックを斜めに倒したときに速くなりすぎないようにする）
        if (moveVec.magnitude > 1f)
        {
            moveVec.Normalize();
        }

        // 入力がある場合のみ移動処理を行う
        if (moveVec.sqrMagnitude > 0.01f)
        {
            if (cameraObject == null)
            {
                Debug.Log("カメラオブジェクトが見つかりませんでした");
                return;
            }
            // Y軸をカット(上を向いたときに空中浮遊しないようにするため)
            Vector3 cameraForward = Vector3.Scale(cameraObject.transform.forward, new Vector3(1.0f, 0.0f, 1.0f)).normalized;
            // 正面方向(進むべき方向)の計算

            Vector3 moveDir = cameraForward * moveVec.z + cameraObject.transform.right * moveVec.x;

            // 最終的な移動の実行(進む方向＊速度＊毎フレーム時間)
            transform.position += moveDir.normalized * MOVE_SPEED * Time.deltaTime;
            transform.forward = moveDir.normalized; // 進行方向に向きを変える
        }
    }

    //最も近いターゲットを探す
    void FindTarget()
    {
        List<GameObject> targets = new List<GameObject>();
        targets.AddRange(GameObject.FindGameObjectsWithTag("Human"));
        targets.AddRange(GameObject.FindGameObjectsWithTag("HiddenHuman"));
        targets.AddRange(GameObject.FindGameObjectsWithTag("Trap"));

        GameObject nearest = null;
        float maxDistance = INTERACT_DISTANCE;

        foreach(var t in targets)
        {
            float distance = Vector3.Distance(transform.position, t.transform.position);

            if(distance < maxDistance) //距離が最大距離より近い場合
            {
                nearest = t; //最も近いオブジェクトを更新
                maxDistance = distance; //最大距離を更新
            }
        }
        currentTarget = nearest; //最も近いオブジェクトを現在のターゲットとして設定
    }

    public void WasFound(int found_count)
    {
        float freezeTime = 0.0f;

        if (found_count == 1)
        {
            freezeTime = 5.0f;
        }
        else if (found_count == 2)
        {
            freezeTime = 10.0f;
        }

        if (freezeTime > 0.0f)
        {
            StartCoroutine(FreezeCoroutine(freezeTime));
        }
    }

    // 一定時間プレイヤーを硬直させるコルーチン
    private IEnumerator FreezeCoroutine(float duration)
    {
        isFreeze = true;
        yield return new WaitForSeconds(duration);
        isFreeze = false;
    }


    //驚かせる処理
    void Surprised()
    {
        // currentTargetがnullの場合は処理を中断する
        if (currentTarget == null)
        {
            return;
        }

        p2eDistance = Vector3.Distance(transform.position, currentTarget.transform.position); //プレイヤーと敵の距離を取得
        if (p2eDistance <= 3.0f) //距離が3m以内なら
        {
            //GetComponent<イベントスクリプト>().イベント呼び出し関数(currentTarget.tag);

            switch (currentTarget.tag)
            {
                case "Human":
                    Debug.Log("人間を驚かせた");
                    addScore = 100; //スコア加算
                    currentTarget.GetComponent<HumanReaction>().GetTag(currentTarget.tag);
                    break;
                case "HiddenHuman":
                    Debug.Log("隠れている人間を驚かせた");
                    addScore = 200; //スコア加算
                    currentTarget.GetComponent<HumanReaction>().GetTag(currentTarget.tag);
                    break;
                case "Trap":
                    Debug.Log("トラップを起動した");
                    addScore = currentTarget.GetComponent<TrapManager>().TrapActivate(); //トラップ起動
                    break;
            }
        }
    }

    public int  AddScore()  //スコア加算時にGameManagerから呼び出す
    {
        return addScore;
    }

    //エラーが発生した場合や、致命的な問題が発生した場合にゲームを終了する関数  
    private void QuitGame()
    {
        //アプリを終了
        Application.Quit();

        // Unityエディタ上でのテストプレイ時でも自動で停止させる
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

}