using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private Transform player; // プレイヤーのオブジェクトを指定用
    [SerializeField] private float distance = 5.0f; // プレイヤーからの距離
    const float MOUSE_SENSITIVITY = 5.0f; // マウス感度

    float yRotation = 0.0f;   // 左右の回転角度

    void Start()
    {
        // マウスカーソルを画面中央に固定し、不可視にする
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // マウスの左右の移動量を取得
        float mouseX = Input.GetAxis("Mouse X") * MOUSE_SENSITIVITY;
        yRotation += mouseX;

        // 回転の角度から、プレイヤーの周りを回る位置（座標）を計算する
        Quaternion rotation = Quaternion.Euler(0.0f, yRotation, 0.0f);
        Vector3 offset = rotation * new Vector3(0.0f, 2.0f, -distance); // 少し高さ(2.0)をつけて後ろから見上げる位置に配置

        // カメラの位置を「プレイヤーの位置 + 計算したオフセット」に更新
        transform.position = player.position + offset;

        // 常にプレイヤーのほうを向かせる
        transform.LookAt(player.position + Vector3.up * 1.5f); // プレイヤーの少し上のあたりを見る
    }
}