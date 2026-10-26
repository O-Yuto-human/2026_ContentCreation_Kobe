using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int foundCount = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (foundCount == 3)
        {
            SceneManager.LoadScene("FailScene");
        }
    }
}
