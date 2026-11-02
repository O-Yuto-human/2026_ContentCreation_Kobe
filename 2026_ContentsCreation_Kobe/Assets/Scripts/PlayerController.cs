using UnityEngine;



public class PlayerController : MonoBehaviour

{

    [SerializeField] private GameObject cameraObject = null;    // 視点移動用のカメラオブジェクト取得用



    const float MOVE_SPEED = 7.0f;   //移動速度



    void Update()

    {

        Move();



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

}