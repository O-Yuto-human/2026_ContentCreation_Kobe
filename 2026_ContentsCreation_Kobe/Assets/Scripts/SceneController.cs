using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            ChangeScene(currentSceneName);
        }
    }

    // ˆø”‚ğ string Œ^‚É•ÏX
    public void ChangeScene(string currentSceneName)
    {
        switch (currentSceneName)
        {
            case "TitleScene":
                SceneManager.LoadScene("PlayScene");
                break;

            case "PlayScene":
                SceneManager.LoadScene("ResultScene");
                break;

            case "ResultScene":
                SceneManager.LoadScene("TitleScene");
                break;
        }
    }
}