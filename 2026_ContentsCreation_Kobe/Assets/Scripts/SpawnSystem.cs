using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] Vector3[] spawnPos4H;
    [SerializeField] Vector3[] spawnPos4HH;
    [SerializeField] Vector3[] respawnPos;
    [SerializeField] GameObject[] human;
    bool[] isUsed4H;
    bool[] isUsed4HH;
    int[] spawnNum;
    int peopleNum = 6;
    float[] respawnTimer;

    private void Start()
    {
        isUsed4H = new bool[spawnPos4H.Length];
        isUsed4HH = new bool[spawnPos4HH.Length];
        spawnNum = new int[peopleNum];
        respawnTimer = new float[peopleNum];
        for (int i = 0; i < peopleNum; i++)
        {
            respawnTimer[i] = 3.0f;
        }
    }

    public void FirstSpawn()
    {
        for (int i = 0; i < spawnNum.Length; i++)
        {
            if (human[i].CompareTag("Human"))
            {
                do
                {
                    spawnNum[i] = Random.Range(0, spawnPos4H.Length);
                }
                while (isUsed4H[spawnNum[i]] == true); // falseのところに値が入れれるまで繰り返す
                isUsed4H[spawnNum[i]] = true;
                human[i].transform.position = spawnPos4H[spawnNum[i]]; // 位置更新
            }
            else if(human[i].CompareTag("HiddenHuman"))
            {
                do
                {
                    spawnNum[i] = Random.Range(0, spawnPos4HH.Length);
                }
                while (isUsed4HH[spawnNum[i]] == true);
                isUsed4HH[spawnNum[i]] = true;
                human[i].transform.position = spawnPos4HH[spawnNum[i]];
            }
        }
    }

    public float RespawnTimer(int _num)
    {
        respawnTimer[_num] -= Time.deltaTime;
        return respawnTimer[_num];
    }

    public void Respawn(int _num)
    {
        if (human[_num].CompareTag("Human"))
        {
            human[_num].transform.position = respawnPos[Random.Range(0, respawnPos.Length)];
        }
        else if (human[_num].CompareTag("HiddenHuman"))
        {
            human[_num].transform.position = spawnPos4HH[Random.Range(0,spawnPos4HH.Length)];
        }
        respawnTimer[_num] = 3.0f;
    }

}
