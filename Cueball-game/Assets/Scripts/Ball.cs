using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject[] ballModels;
    public int ballIndex;


    public void SetRandomBall()
    {
        foreach (var item in ballModels)
        {
            item.SetActive(false);
        }

        ballIndex = Random.Range(0, ballModels.Length);
        ballModels[ballIndex].SetActive(true);
    }
    public void SetColor(int colorIndex )
    {
        foreach (var item in ballModels)
        {
            item.SetActive(false);
        }

        ballIndex = colorIndex;
        ballModels[ballIndex].SetActive(true);
    }



}
