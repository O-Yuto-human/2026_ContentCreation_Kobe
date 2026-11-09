using UnityEngine;

public class WayPointMove : MonoBehaviour
{

    //チャックポイント
    [SerializeField]
    public Transform[] CheckPoints;//チェックポイントの情報を配列で保管(座標、回転、大きさ)
    //今回は4つあるので、インスペクター上で設定する

    //移動速度
    [SerializeField]
    public float PLAYER_SPEED = 2.0f;//親ゴーストの移動速度

    //次のチェックポイント
    private int CurrentPoint = 0;//現在進行しているチェックポイント


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckPoints.Length == 0)//配列が空であれば
            return;

        //現在の目的地へ移動
        transform.position = Vector3.MoveTowards(
            transform.position,//現在位置
            CheckPoints[CurrentPoint].position,//目的地
            PLAYER_SPEED * Time.deltaTime//移動量
            );

        if (Vector3.Distance(transform.position, CheckPoints[CurrentPoint].position) < 0.1f)//2点間の距離を測り、現在の位置と目的地への距離感が0.1より小さかったら
        {
            CurrentPoint++;

            //最終チェックポイントまで到着したら初期チェックポイントへ戻り、再び循環
            if (CurrentPoint >= CheckPoints.Length)
            {
                CurrentPoint = 0;
            }
        }

    }
}
