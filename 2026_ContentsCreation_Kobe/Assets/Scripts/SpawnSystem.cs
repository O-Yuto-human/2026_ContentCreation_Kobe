using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] Vector3[] spawnPos4H;
    [SerializeField] Vector3[] spawnPos4HH;
    [SerializeField] GameObject[] human;
    bool[] isUsed4H;
    bool[] isUsed4HH;
    int[] spawnNum;
    int peopleNum = 6;

    private void Start()
    {
        isUsed4H = new bool[spawnPos4H.Length];
        isUsed4HH = new bool[spawnPos4HH.Length];
        spawnNum = new int[peopleNum];
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
                while (isUsed4H[spawnNum[i]] == true);
                isUsed4H[spawnNum[i]] = true;
                human[i].transform.position = spawnPos4H[spawnNum[i]];
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

    void Respawn()
    {

    }

    void Despawn()
    {

    }
}
