using UnityEngine;
using UnityEngine.InputSystem; // Input Systemを使うために必要

public class PlayerLookingController : MonoBehaviour
{
    [SerializeField] private Transform player; // プレイヤーのオブジェクトを指定
    const float DISTANCE = 5.0f; // プレイヤーからの距離
    const float SENSITIVITY = 0.5f; // 視点感度

    private float yRotation = 0.0f;   // 左右の回転角度

    [Header("カメラの固定アングル設定")]
    [SerializeField] private float fixedPitch = 20.0f; // 見下ろす角度（数字が大きいほど上から見下ろす）

    void Start()
    {
        // マウスカーソルを画面中央に固定し、不可視にする
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (player == null) return;

        float inputX = 0f;

        // ゲームパッドが接続されているか確認
        if (Gamepad.current != null)
        {
            Vector2 stickDelta = Gamepad.current.rightStick.ReadValue();
            inputX += stickDelta.x * SENSITIVITY * 2.0f;

            if (Gamepad.current.leftTrigger.wasPressedThisFrame)    //左トリガーが押されている場合
            {
                // カメラの左右角度を、プレイヤーが向いている方向（Y軸の回転）に強制的に合わせる
                yRotation = player.eulerAngles.y;
                
            }
        }
        // トリガーが押されていない（または右スティックが入力された）場合は通常の回転を反映
        yRotation += inputX;

        // カメラの回転と位置を計算（通常の fixedPitch の高さを維持）
        Quaternion rotation = Quaternion.Euler(fixedPitch, yRotation, 0.0f);
        Vector3 targetPosition = player.position + Vector3.up * 1.0f; // プレイヤーの中心付近

        // プレイヤーを中心としたオフセット位置を計算
        Vector3 offset = rotation * new Vector3(0.0f, 0.0f, -DISTANCE);
        transform.position = targetPosition + offset;

        // 常にプレイヤーのほうを向かせる
        transform.LookAt(targetPosition);
    }
}