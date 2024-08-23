using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateUsPanel : MonoBehaviour
{
    public GameObject[] fiveStars; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RateUsButtonClicked(int num)
    {
        PrefHandlre.RateUsPanelpref = 1;
        for (int i = 0; i < fiveStars.Length; i++)
        {
            fiveStars[i].SetActive(i < num);
        }
        
        string playStoreURL = "https://play.google.com/store/apps/details?id=com.companyname.gamename";
        Application.OpenURL(playStoreURL);
        CloseButton();
    }
    public void CloseButton()
    {
        LevelHandler.instance.ShowVictoryPanel();
        gameObject.SetActive(false);
    }
}
