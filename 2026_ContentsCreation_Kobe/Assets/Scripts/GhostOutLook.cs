

using System;
using System.Collections;
using UnityEngine;

public class GhostOutLook : MonoBehaviour
{
    [SerializeField]
    private WayPointMove wayPointMove;


    [Header("プレイヤー")]
    [SerializeField] private Transform player;

    [Header("視野角")]
    [Range(0, 60)]
    [SerializeField] private float SIGHT_ANGLE = 60f;

    [Header("視野距離")]
    [SerializeField] private float SIGHT_DISTANCE = 8f;

    [SerializeField]
    private float StopTime = 1.0f;

    private bool hasDetected = false;   // 一度検知したか
    private bool isStopping = false;    // 停止中か

    private float Timer = 0.0f;
    private float originSpeed;
    public bool IsPlayerFound { get; private set; }

    void Start()
    {
        wayPointMove = GetComponent<WayPointMove>();
        originSpeed = wayPointMove.PLAYER_SPEED;

    }

    void Update()
    {
        CheckSight();

        if (isStopping)
        {
            Timer += Time.deltaTime;

            if (Timer >= StopTime)
            {
                wayPointMove.PLAYER_SPEED = originSpeed;
                isStopping = false;
            }
        }
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
            Timer += Time.deltaTime;
            if (hit.transform == player)
            {
                IsPlayerFound = true;

                // 初めて見つけた瞬間だけ
                if (!hasDetected)
                {
                    hasDetected = true;

                    wayPointMove.PLAYER_SPEED = 0.0f;

                    isStopping = true;
                    Timer = 0.0f;
                }
            }
            else
            {
                IsPlayerFound = false;

                // 見失ったら次回また停止できるようにする
                hasDetected = false;
            }
        }
    }
}