using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ChangeScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
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