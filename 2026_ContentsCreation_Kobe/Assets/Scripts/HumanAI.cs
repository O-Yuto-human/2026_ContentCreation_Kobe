using UnityEngine;
using UnityEngine.AI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Vector3[] destinations;
    int num = 0;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetRandomDestination(Random.Range(0,destinations.Length));
    }

    void Update()
    {
        // 経路の計算が終わっているかつ、目的地までの残り距離が停止距離以下なら
        if(!navMeshAgent.pathPending &&         // 目的地までの経路を計算中ではないかつ(bool)
            navMeshAgent.remainingDistance <=   // 目的地までの残りの距離
            navMeshAgent.stoppingDistance)      // 目的地に到着したとみなす距離
        {
            num = Random.Range(0, destinations.Length); // 将来性を踏まえて、数字ではなく変数で
            SetRandomDestination(num);
        }
    }

    void SetRandomDestination(int _value)
    {
        // NavMeshAgentに、指定した座標まで移動する目的地を設定する
        navMeshAgent.SetDestination(destinations[_value]);
    }
}
