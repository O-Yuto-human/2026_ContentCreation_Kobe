using UnityEngine;

public class MouseController : MonoBehaviour
{
    const float MOUSE_SENSITIVITY = 1000.0f; // マウス感度

    [SerializeField]Transform playerBody;   // プレイヤー全体の体
    float xRotation = 0.0f;   //プレイヤーの視線の現在の上下の傾き


    void Start()
    {
        //マウスカーソルを画面中央に固定し、不可視にする(Unity実行中の場合Escキーで解除可能)
        Cursor.lockState = CursorLockMode.Locked;   
    }

    void Update()
    {
        // マウスの移動量を取得
        float mouseX = Input.GetAxis("Mouse X") * MOUSE_SENSITIVITY * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MOUSE_SENSITIVITY * Time.deltaTime;

        // 上下の首振り
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 真上や真後ろを向けないように制限
        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        // 左右の首振り（体ごと回転）
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
