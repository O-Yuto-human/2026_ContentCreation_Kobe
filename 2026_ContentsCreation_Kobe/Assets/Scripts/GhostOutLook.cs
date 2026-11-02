using System;
using UnityEngine;

public class GhostOutLook : MonoBehaviour
{
    [Header("プレイヤー")]
    [SerializeField] private Transform player;

    [Header("視野角")]
    [Range(0, 60)]
    [SerializeField] private float SIGHT_ANGLE = 60f;

    [Header("視野距離")]
    [SerializeField] private float SIGHT_DISTANCE = 8f;

        

    // プレイヤーを見つけたか
    public bool IsPlayerFound { get; private set; }

    void Update()
    {
        CheckSight();
    }

    void CheckSight()
    {
        IsPlayerFound = false;

        if (player == null)
            return;

        // ゴーストからプレイヤーへの方向
        Vector3 direction = player.position - transform.position;

        // 距離判定
        if (direction.magnitude > SIGHT_DISTANCE)
            return;

        // 角度判定
        float angle = Vector3.Angle(transform.forward, direction);

        if (angle > SIGHT_ANGLE * 0.5f)
            return;

        // Rayを飛ばして壁があるか判定
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction.normalized, out hit, SIGHT_DISTANCE))
        {
            if (hit.transform == player)
            {
                IsPlayerFound = true;
                Debug.Log("プレイヤー発見！");
            }
            else
            {
                IsPlayerFound = false;
            }
        }
    }
}