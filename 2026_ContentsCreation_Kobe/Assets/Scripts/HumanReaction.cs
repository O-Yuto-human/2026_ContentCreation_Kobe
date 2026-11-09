using UnityEngine;

public class HumanReaction : MonoBehaviour
{
    public void GetTag(string tag)
    {
        if (tag == "Human")
        {
            HumanAnimation();
        }
        else if (tag == "HiddenHuman")
        {
            HiddenHumanAnimation();
        }
    }

    void HumanAnimation()
    {
        Debug.Log("人間アニメーション");
    }

    void HiddenHumanAnimation()
    {
        Debug.Log("隠れている人間アニメーション");
    }
}
