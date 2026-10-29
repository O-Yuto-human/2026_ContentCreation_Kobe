using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject = null;    // 視点移動用のカメラオブジェクト取得用

    const float MOVE_SPEED = 7.0f;   // 基本移動速度

    void Start()
    {
        // 必要に応じて初期化処理をここに書く
    }

    void Update()
    {
        float inputX = 0f;      //各移動方向の指定用
        float inputZ = 0f;

        //X軸移動
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            inputX = 0.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            inputX = 1.0f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputX = -1.0f;
        }
        else
        {
            inputX = 0.0f;
        }

        // Z軸移動
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            inputZ = 0.0f;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            inputZ = 1.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            inputZ = -1.0f;
        }
        else
        {
            inputZ = 0.0f;
        }


            Vector3 moveVec = new Vector3(inputX, 0f, inputZ);

        // 斜め移動量の正規化
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

            //Y軸をカット(上を向いたときに空中浮遊しないようにするため)
            Vector3 cameraForward = Vector3.Scale(cameraObject.transform.forward, new Vector3(1.0f, 0.0f, 1.0f)).normalized;    

            //正面方向(進むべき方向)の計算
            Vector3 moveDir = cameraForward * moveVec.z + cameraObject.transform.right * moveVec.x;

            //最終的な移動の実行(方向＊スピード＊正規化)
            transform.position += moveDir.normalized * MOVE_SPEED * Time.deltaTime;
        }
    }
}