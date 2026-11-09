using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    const float TRAP_COOLTIME = 10.0f; // クールタイム（秒）
    const float EFFECTIVE_RANGE = 5.0f;     //効果範囲

    [SerializeField] private int addScore = 50;

    bool isCoolingDown = false; // クールタイム中かどうかのフラグ

    void Start()
    {
    }

    // トラップを発動する関数
    public int TrapActivate(string trap_type)
    {
        if (isCoolingDown)
        {
            Debug.Log("トラップは現在クールタイム中です");
            return 0; // スコア加算なし
        }

        switch(trap_type){
            case "Phone":
                Debug.Log("電話を鳴らした");
                break;
            case "LightDown":
                Debug.Log("照明を消した");
                break;
        }

        List<GameObject> targets = new List<GameObject>();  // ターゲットを格納するリスト
        targets.AddRange(GameObject.FindGameObjectsWithTag("Human"));   // Humanタグのオブジェクトをリストに追加

        List<GameObject> affectedHumans = new List<GameObject>();   // 範囲内にいた人間を格納するリスト

        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= EFFECTIVE_RANGE)
            {
                // 人間に対して驚かせる処理を呼び出す
                HumanReaction humanReaction = target.GetComponent<HumanReaction>();
                if (humanReaction != null)
                {
                    humanReaction.GetTag(target.tag);
                    affectedHumans.Add(target); // 範囲内だった人間をリストに追加
                }
            }
        }

        // クールタイムを開始
        StartCoroutine(CooltimeRoutine());
        return addScore * affectedHumans.Count;
    }

    // クールタイムを管理するコルーチン
    private IEnumerator CooltimeRoutine()
    {
        isCoolingDown = true;

        // クールタイム時間待機
        yield return new WaitForSeconds(TRAP_COOLTIME);

        isCoolingDown = false;
    }
}