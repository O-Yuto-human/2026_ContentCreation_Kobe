using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int score = 0;

    //Œ»İ‚ÌƒXƒRƒA‚ğæ“¾
    public int GetScore()
    {
        return score;
    }

    public void addScore(int addScore)
    {
        score += addScore;
    }

    public void ResetScore()
    {
        score = 0;
    }
}
