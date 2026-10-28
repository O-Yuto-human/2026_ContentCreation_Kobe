using UnityEngine;
using UnityEngine.AI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetRandomDestination();
    }

    void Update()
    {
        // 経路の計算が終わっているかつ、目的地までの残り距離が停止距離以下なら
        if(!navMeshAgent.pathPending &&         // 目的地までの経路を計算中ではないかつ
            navMeshAgent.remainingDistance <=   // 目的地までの残りの距離
            navMeshAgent.stoppingDistance)      // 目的地に到着したとみなす距離
        {
            SetRandomDestination();
        }
    }

    void SetRandomDestination() // ランダムに目標座標を決める
    {
        float randomX = Random.Range(-10f, 10f);
        float randomZ = Random.Range(-10f, 10f);

        // 移動先として実際に指定
        navMeshAgent.SetDestination(new Vector3(randomX, 0f, randomZ));
    }
}
