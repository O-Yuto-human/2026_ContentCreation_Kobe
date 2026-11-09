using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class HumanAI : MonoBehaviour
{
    [SerializeField] Vector3[] destinations; // 目的地
    [SerializeField] Vector3[] exitPoints;
    [SerializeField] NavMeshAgent navMeshAgent;
    float escapeSpeed = 6.0f;
    float normalSpeed = 1.0f;
    int num = 0;
    int exitNum = 0;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (CompareTag("Human"))
        {
            SetRandomDestination();
        }
    }

    public int HumanMoveSystem()
    {
        // 経路の計算が終わっているかつ、目的地までの残り距離が停止距離以下なら
        if (navMeshAgent.hasPath &&             // 目的地までの経路を持っているかつ
            !navMeshAgent.pathPending &&        // 目的地までの経路を計算中ではないかつ(bool)
            navMeshAgent.remainingDistance <=   // 目的地までの残りの距離
            navMeshAgent.stoppingDistance)      // 目的地に到着したとみなす距離
        {
            num = Random.Range(0, destinations.Length); // 将来性を踏まえて、数字ではなく変数で
            return num;
        }
        else
        {
            return -1;
        }
    }

    // 目的地設定
    public void SetRandomDestination()
    {
        num = Random.Range(0, destinations.Length);
        // NavMeshAgentに、指定した座標まで移動する目的地を設定する
        navMeshAgent.SetDestination(destinations[num]);
    }

    // 巡回中の待機処理
    public IEnumerator PatrolCoroutine()
    {
        navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(5.0f);

        navMeshAgent.isStopped = false;

        num = Random.Range(0, destinations.Length);
        SetRandomDestination();
    }

    // 逃げにつなげる処理
    public IEnumerator FleeCoroutine() // コルーチン 処理の途中で停止ができる。IEnumerator コルーチンを作るときに使う戻り値の型
    {
        navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(1.0f); // yield 処理を一時中断、呼び出し元に制御を返す

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = escapeSpeed;

        exitNum = Random.Range(0, exitPoints.Length);
        navMeshAgent.SetDestination(exitPoints[exitNum]);
    }

    // 発見されたとき
    public bool IsEscaped()
    {
        if (navMeshAgent.hasPath && // 経路を持っているか
            !navMeshAgent.pathPending && // 経路を計算中か
            navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && // 経路が完成しているか
            navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) // 到着とみなしていい距離か
        {
            navMeshAgent.speed = normalSpeed;
            return true;
        }

        return false;
    }
}
