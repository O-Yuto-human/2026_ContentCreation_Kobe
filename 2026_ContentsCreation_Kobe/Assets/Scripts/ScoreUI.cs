using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private ScoreManager scoreManager;

    void Update()
    {
        if (scoreManager == null)
        {
            Debug.LogError("scoreManager‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
            return;
        }

        if (scoreText == null)
        {
            Debug.LogError("scoreText‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
            return;
        }

        scoreText.text = "Score : " + scoreManager.GetScore();
    }
}
