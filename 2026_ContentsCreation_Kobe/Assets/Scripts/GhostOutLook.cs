using System;

using System.Collections;

using UnityEngine;

public class GhostOutLook : MonoBehaviour

{

    [SerializeField]

    private WayPointMove wayPointMove;//親お化けを停止・再開させるため

    [Header("プレイヤー")]

    [SerializeField] private Transform player;//プレイヤーの位置情報を所得するため

    [Header("視野角")]
    [Range(0, 60)]
    [SerializeField] private float SIGHT_ANGLE = 60f;//親お化けの視野角判定を所得するため

    [Header("視野距離")]
    [SerializeField] private float SIGHT_DISTANCE = 6.5f;//親お化けの視野距離判定を所得するため

    [SerializeField]
    private float StopTime = 5.0f;//初回止まる時間

    private bool hasDetected = false;   // 一度検知したか
    private bool isStopping = false;    // 停止中か
    private float Timer = 0.0f;

    private float originSpeed;//元のスピード

    private int foundCount = 0;

    public bool IsPlayerFound { get; private set; }

    public event Action<int> OnPlayerFound;

    public bool playerVisible = false;

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

        {

            hasDetected = false;

            IsPlayerFound = false;

            return;

        }

        // 角度判定

        float angle = Vector3.Angle(transform.forward, direction);

        if (angle > SIGHT_ANGLE * 0.5f)

        {

            hasDetected = false;

            IsPlayerFound = false;

            return;

        }



        // Rayを飛ばして壁があるか判定

        RaycastHit hit;

        playerVisible = false;

        if (Physics.Raycast(transform.position, direction.normalized, out hit, SIGHT_DISTANCE))

        {

            if (hit.transform == player)

            {
                playerVisible = true;
                IsPlayerFound = true;

                if (!hasDetected)
                {
                    hasDetected = true;

                    //発見回数だけ増やす
                    foundCount++;

                    //GameManagerへ通知
                    OnPlayerFound?.Invoke(foundCount);

                    //親お化け自身の停止処理
                    wayPointMove.PLAYER_SPEED = 0.0f;
                    isStopping = true;
                    Timer = 0.0f;
                }

            }

        }

        if (!playerVisible)

        {

            hasDetected = false;
            IsPlayerFound = false;

        }

    }

}